using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Alphamat;
using Emgu.CV.Aruco;
using Emgu.CV.BgSegm;
using Emgu.CV.Bioinspired;
using Emgu.CV.Cuda;
using Emgu.CV.Dnn;
using Emgu.CV.Dpm;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Freetype;
using Emgu.CV.Fuzzy;
using Emgu.CV.Geodetic;
using Emgu.CV.Hdf;
using Emgu.CV.Hfs;
using Emgu.CV.ImgHash;
using Emgu.CV.IntensityTransform;
using Emgu.CV.Legacy;
using Emgu.CV.LineDescriptor;
using Emgu.CV.Mcc;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.ML;
using Emgu.CV.Ocl;
using Emgu.CV.OCR;
using Emgu.CV.PhaseUnwrapping;
using Emgu.CV.Plot;
using Emgu.CV.PpfMatch3d;
using Emgu.CV.Quality;
using Emgu.CV.Rapid;
using Emgu.CV.Reflection;
using Emgu.CV.Saliency;
using Emgu.CV.Shape;
using Emgu.CV.Stereo;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Superres;
using Emgu.CV.Text;
using Emgu.CV.Tiff;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.CV.XObjdetect;
using Emgu.CV.XPhoto;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.Util.TypeEnum;
using Emgu.Util;
using Config;
using BackgroundRemovalSample.App;
using BackgroundRemovalSample;


namespace FaceAddToBD
{
    class BackgroundRemovalFilter
    {
        internal static void GetGradient(ref Image<Gray, byte> grayImage, ref ImageBox imageBox)
        {
            Image<Gray, float> dest = grayImage.Convert<Gray, float>();
                using (var gradX = dest.Sobel(xorder: 0, yorder: 1, 3))
                {
                    using (var gradY = dest.Sobel(xorder: 1, yorder: 0, 3))
                    {
                    //imageBox.BackgroundImage = gradY.ToBitmap();
                    Image<Gray, float> asdf = gradX.AbsDiff(gradY);
                    asdf.FillConvexPoly(new System.Drawing.Point[] 
                    {
                        new System.Drawing.Point((Int32) (0.01 * dest.Width)),
                        new System.Drawing.Point((Int32) (0.01 * dest.Height)),
                    }, 
                    new Gray(0.01), LineType.EightConnected);

                    imageBox.BackgroundImage = asdf.ToBitmap();
                    asdf.Save(Config.Config.FacePhotosPath + "asdgb" + Config.Config.ImageFileExtension);
                    
                     //   imageBox.BackgroundImage = gradY.(gradX).ToBitmap();
                        // var result = new Mat();
                        // dest.
                        // Cv2.Magnitude(gradX, gradY, result);

                        //return result;
                    }
                }

        }
    }
}
