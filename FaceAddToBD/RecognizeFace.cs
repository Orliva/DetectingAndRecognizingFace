using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.XObjdetect;
using Emgu.CV.XPhoto;
using Emgu.CV.XImgproc;
using Emgu.CV.Face;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.Util.TypeEnum;
using Config;

namespace FaceAddToBD
{
    class RecognizeFace
    {
        internal static Emgu.CV.Face.EigenFaceRecognizer recognizer;
        internal static Emgu.CV.Face.LBPHFaceRecognizer rec2;
        internal static Emgu.CV.Face.FisherFaceRecognizer rec3;

        internal static void TrainRecognizer(int countComponents)
        {
            recognizer = new EigenFaceRecognizer(countComponents, 1.8 * Math.Pow(10.0, 10.0));
            recognizer.Train(Database.imageList, Database.labelList);
            recognizer.Write(Config.Config.EigenFaceRecognizerPath);
        }

    }
}
