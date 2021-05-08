using System;
using System.Collections.Generic;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

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

        internal static bool AddToBasePerson(CascadeClassifier classifier)
        {
            int countPersonPhoto;
            countPersonPhoto = SaveToDatabasePhoto(faceList[^1].FaceImage, ref classifier);
            if (countPersonPhoto == 0)
                return false;
            ValidSource();
            imageList.Push(GetArrayMat(countPersonPhoto));
            nameList.Add(faceList[^1].PersonName);
            labelList.Push(GetArrayLabel(countPersonPhoto));
            return true;

        }

        private static Mat[] GetArrayMat(int count)
        {
            Mat[] m = new Mat[count * 2];
            for (int i = 0; i < count * 2; i++)
            {
                m[i] = new Mat(Config.Config.FacePhotosPath + "Face" + nameList[^1] + i + Config.Config.ImageFileExtension, ImreadModes.Grayscale);
                m[count + i] = m[i].ToImage<Gray, byte>().Flip(FlipType.Horizontal).Mat;
            }
            return m;
        }

        private static int[] GetArrayLabel(int count)
        {
            int[] label = new int[count * 2];
            for (int i = 0; i < count * 2; i++)
            {
                label[i] = faceList.Count - 1;
            }
            return label;
        }

        private static int SaveToDatabasePhoto(Image<Gray, byte> detectedIm, ref CascadeClassifier classifier)
        {
            int count = 0;

            if (detectedIm != null)
            {
                detectedIm.Laplace(1);
                // detectedIm = detectedIm.Canny(100, 0);
                // BackgroundRemovalFilter.GetGradient(ref detectedIm, ref imageBox1);
                for (int i = 0; i < 255; i++)
                {
                    using (Image<Gray, Byte> Gray = detectedIm.ThresholdBinary(new Gray(i), new Gray(255)))
                    {
                        Gray._EqualizeHist();
                        Rectangle[] faces = classifier.DetectMultiScale(Gray, 1.05, 1);
                        if (faces != null)
                        {
                            foreach (Rectangle face in faces)
                            {
                                Gray.GetSubRect(face).Save(Config.Config.FacePhotosPath + "Face" + nameList[^1] + count++ + Config.Config.ImageFileExtension);
                                Gray.GetSubRect(face).Flip(FlipType.Horizontal).Save(Config.Config.FacePhotosPath + "Face" + nameList[^1] +
                                    count++ + Config.Config.ImageFileExtension);
                            }
                        }
                    }
                }
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
