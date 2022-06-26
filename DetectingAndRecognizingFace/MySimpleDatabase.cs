using System;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Face;
using System.Text.Json;

namespace FaceAddToBD
{
    class MySimpleDatabase : IDisposable
    {
        public List<FaceData> FaceList { get; set; }
        public VectorOfMat ImageList { get; set; }
        public List<string> NameList { get; set; }
        public VectorOfInt LabelList { get; set; }
        public RecognizeFace Recognizer { get; set; }

        private int countPhotosBefore;

        public MySimpleDatabase()
        {
            FaceList = new List<FaceData>();
            ImageList = new VectorOfMat();
            NameList = new List<string>();
            LabelList = new VectorOfInt();
        }

        #region Class Interface

        /// <summary>
        /// Пробуем восстановить базу данных после закрытия приложения
        /// </summary>
        /// <returns></returns>
        public bool TryRestoreDB()
        {
            if (TryRestoreRecognizer(Recognizer.recognizer,
                Configuration.Config.EigenFaceRecognizerPath) && TryRestoreFaceList())
                return true;
            return false;
        }

        /// <summary>
        /// Добавляем юзера в базу (файлик в случае нативных систем, ну или тестовых, как у меня)
        /// </summary>
        /// <param name="faceData"></param>
        /// <returns></returns>
        public bool AddToBasePerson(in FaceData faceData)
        {
            int countPersonPhoto;
            if (!Directory.Exists(Configuration.Config.FacePhotosPath + @"\" + faceData.PersonName))
                Directory.CreateDirectory(Configuration.Config.FacePhotosPath + @"\" + faceData.PersonName);
            countPersonPhoto = SaveToDatabasePhoto(faceData.FaceImage, in faceData);
            if (countPersonPhoto == 0)
                return false;
            ImageList.Push(GetArrayMat(countPersonPhoto, in faceData));
            LabelList.Push(GetArrayLabel(countPersonPhoto, in faceData));
            return true;
        }

        /// <summary>
        /// Получаем FaceData по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FaceData GetUser(string name)
        {
            foreach (FaceData fd in this.FaceList)
            {
                if (fd.PersonName == name)
                    return fd;
            }
            return default(FaceData);
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Пробуем восстанавливать FaceRecognizer
        /// </summary>
        /// <param name="recognizer"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool TryRestoreRecognizer(FaceRecognizer recognizer, string path)
        {
            if (!File.Exists(path))
                return false;
            recognizer.Read(path);
            return true;
        }

        /// <summary>
        /// Пробуем восстановить faceList и nameList
        /// </summary>
        /// <returns></returns>
        private bool TryRestoreFaceList()
        {
            if (!File.Exists(Configuration.Config.FaceListTextFile))
                return false;
            FaceList = JsonSerializer.Deserialize<List<FaceData>>(File.ReadAllText(Configuration.Config.FaceListTextFile));
            foreach (FaceData fd in FaceList)
                NameList.Add(fd.PersonName);
            return true;
        }

        /// <summary>
        /// Заполняем ImageList для дальнейшего обучения алгоритма распознавания
        /// </summary>
        /// <param name="count"></param>
        /// <param name="faceData"></param>
        /// <returns></returns>
        private Mat[] GetArrayMat(int count, in FaceData faceData)
        {
            int j = countPhotosBefore;
            Mat[] m = new Mat[count];
            for (int i = 0; i < count; i += 2)
            {
                m[i] = new Mat(Configuration.Config.FacePhotosPath + faceData.PersonName + @"\" + "Face" + 
                              faceData.PersonName + (j + i) + Configuration.Config.ImageFileExtension, ImreadModes.Grayscale);
                m[i + 1] = new Mat(Configuration.Config.FacePhotosPath + faceData.PersonName + @"\" + "Face" +
                                   faceData.PersonName + (j + i + 1) + Configuration.Config.ImageFileExtension, ImreadModes.Grayscale);
            }
            return m;
        }

        /// <summary>
        /// Считаем количество фотографий в папке
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int GetCountPhotos(string path)
        {
            int count = 0;
            foreach (string str in Directory.EnumerateFiles(path))
                count++;
            return count;
        }

        /// <summary>
        /// Заполняем LabelList для дальнейшего обучения алгоритма распознавания
        /// </summary>
        /// <param name="count"></param>
        /// <param name="faceData"></param>
        /// <returns></returns>
        private int[] GetArrayLabel(int count, in FaceData faceData)
        {
            int[] label = new int[count];
            for (int i = 0; i < count; i++)
                label[i] = faceData.Id;
            return label;
        }

        /// <summary>
        /// Сохраняем фотографию + зеркальную копию
        /// </summary>
        /// <param name="detectedIm"></param>
        /// <param name="faceData"></param>
        /// <returns></returns>
        private int SaveToDatabasePhoto(Image<Gray, byte> detectedIm, in FaceData faceData)
        {
            int count = 0;
            countPhotosBefore = GetCountPhotos(Configuration.Config.FacePhotosPath + faceData.PersonName);

            if (detectedIm != null)
            {
                detectedIm.Laplace(1);
                // detectedIm = detectedIm.Canny(100, 0);
                detectedIm.Save(Configuration.Config.FacePhotosPath + faceData.PersonName + @"\" + "Face" + 
                                faceData.PersonName + (countPhotosBefore + count) + Configuration.Config.ImageFileExtension);
                count++;
                detectedIm.Flip(FlipType.Horizontal).Save(Configuration.Config.FacePhotosPath + faceData.PersonName + 
                                                          @"\" + "Face" + faceData.PersonName + (countPhotosBefore + count) +
                                                          Configuration.Config.ImageFileExtension);
                count++;
            }
            return count;
        }
        #endregion

        //Освобождаем память
        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            File.WriteAllText(Configuration.Config.FaceListTextFile, JsonSerializer.Serialize(FaceList));

            if (!disposedValue)
            {
                if (disposing)
                {
                    FaceList = null;
                    NameList = null;
                }

                ImageList?.Dispose();
                LabelList?.Dispose();
                disposedValue = true;
            }
        }

         ~MySimpleDatabase()
         {
             Dispose(disposing: false);
         }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
