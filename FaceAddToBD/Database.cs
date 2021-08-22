using System;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using Emgu.CV.Face;

namespace FaceAddToBD
{
    class Database
    {
        ///Рассмотреть Span вместо List
        internal static List<FaceData> faceList = new List<FaceData>(3);
        internal static VectorOfMat imageList = new VectorOfMat();
        internal static List<string> nameList = new List<string>();
        internal static VectorOfInt labelList = new VectorOfInt();

        internal static void RestoreAllList(ref List<FaceData> faceList)
        {
            //Дописать восстановление списков при запуске из ресурсов (парсинг)
        }
        internal static void ValidSource()
        {
            if (!Directory.Exists(Config.Config.FacePhotosPath))
                Directory.CreateDirectory(Config.Config.FacePhotosPath);
        }

        public static bool TryRestoreDB()
        {
            RecognizeFace.recognizer = new EigenFaceRecognizer(2, 1.8 * Math.Pow(10.0, 10.0));
            if (TryRestoreRecognizer(RecognizeFace.recognizer, Config.Config.EigenFaceRecognizerPath) &&
                    TryRestoreNameList())
                return true;
            return false;
        }

        private static bool TryRestoreRecognizer(FaceRecognizer recognizer, string path)
        {
            if (!File.Exists(path))
                return false;
            recognizer.Read(path);
            return true;
        }

        public static bool TryRestoreNameList()
        {
            if (!TryGetNameList(Config.Config.FaceListTextFile, ref nameList))
                return false;
            return true;
        }

        private static bool TryGetNameList(string path, ref List<string> nameList)
        {
            if (!File.Exists(path))
                return false;
            using (StreamReader fs = File.OpenText(path))
            {
                string tmp;
                while ((tmp = fs.ReadLine()) != null)
                {
                    nameList.Add(tmp.Remove(0, 6));
                }
            }
            if (nameList.Count < 1)
                return false;
            return true;
        }

        internal static bool AddToBasePerson(CascadeClassifier classifier)
        {
            int countPersonPhoto;
            if (!Directory.Exists(Config.Config.FacePhotosPath + @"\" + faceList[^1].PersonName))
            {
                Directory.CreateDirectory(Config.Config.FacePhotosPath + @"\" + faceList[^1].PersonName);
            }
            countPersonPhoto = SaveToDatabasePhoto(faceList[^1].FaceImage, ref classifier);
            if (countPersonPhoto == 0)
                return false;
            ValidSource();
            imageList.Push(GetArrayMat(countPersonPhoto));
            if (nameList[^1] != faceList[^1].PersonName)
                nameList.Add(faceList[^1].PersonName);
            labelList.Push(GetArrayLabel(countPersonPhoto));
            return true;

        }

        static int countPhotosBefore;
        private static Mat[] GetArrayMat(int count)
        {
            int j = countPhotosBefore;
            Mat[] m = new Mat[count];
            for (int i = 0; i < count; i+=2)
            {
                m[i] = new Mat(Config.Config.FacePhotosPath + faceList[^1].PersonName + @"\" + "Face" + faceList[^1].PersonName + (j + i) + Config.Config.ImageFileExtension, ImreadModes.Grayscale);
                m[i + 1] = new Mat(Config.Config.FacePhotosPath + faceList[^1].PersonName + @"\" + "Face" + faceList[^1].PersonName + (j + i + 1) + Config.Config.ImageFileExtension, ImreadModes.Grayscale);
            }
            return m;
        }

        private static int GetCountPhotos(string path)
        {
            int count = 0;
            foreach (string str in Directory.EnumerateFiles(path))
            {
                count++;
            }
            return count;
        }

        private static int[] GetArrayLabel(int count)
        {
            int[] label = new int[count];
            for (int i = 0; i < count; i++)
            {
                label[i] = faceList[^1].Id;
            }
            return label;
        }

        private static int SaveToDatabasePhoto(Image<Gray, byte> detectedIm, ref CascadeClassifier classifier)
        {
            int count = 0;
            countPhotosBefore = GetCountPhotos(Config.Config.FacePhotosPath + faceList[^1].PersonName);

            if (detectedIm != null)
            {
                detectedIm.Laplace(1);
                // detectedIm = detectedIm.Canny(100, 0);
                // BackgroundRemovalFilter.GetGradient(ref detectedIm, ref imageBox1);
               // for (int i = 0; i < 70; i++)
                //{
               //     using (Image<Gray, Byte> Gray = detectedIm.ThresholdBinary(new Gray(i + 50), new Gray(255)))
                 //   {
                    //    detectedIm._EqualizeHist();
                    //    Rectangle[] faces = classifier.DetectMultiScale(Gray, 1.05, 1);
                      //  if (faces != null)
                        //{
                          //  foreach (Rectangle face in faces)
                            //{
                            //Добавить папку под человека
                                detectedIm.Save(Config.Config.FacePhotosPath + faceList[^1].PersonName + @"\" + "Face" + faceList[^1].PersonName + (countPhotosBefore + count) + Config.Config.ImageFileExtension);
                        count++;
                                detectedIm.Flip(FlipType.Horizontal).Save(Config.Config.FacePhotosPath + faceList[^1].PersonName + @"\" + "Face" + faceList[^1].PersonName +
                                    (countPhotosBefore + count) + Config.Config.ImageFileExtension);
                        count++;
                            //}
                       // }
                   // }
                //}
            }
            return count;
        }

        //Удалить
        internal static void DeleteAllSource()
        {
            try
            {
                Directory.Delete(Config.Config.FacePhotosPath);
                File.Delete(Config.Config.FaceListTextFile);
                File.Delete(Config.Config.EigenFaceRecognizerPath);
            }
            catch
            {

            }
        }

    }
}
