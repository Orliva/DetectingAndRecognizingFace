using Emgu.CV.UI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.Util;
using Emgu.CV.Superres;
using System.Diagnostics;
using Emgu.CV;
using System.Threading;
using UltraFaceDotNet;
using NcnnDotNet.OpenCV;
using System.Linq;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace FaceAddToBD
{
    partial class MainForm : Form
    {
        private DetectorFace detectorFace;
        public static VideoCapture capture = new VideoCapture(0);
        private static Image<Bgr, byte> im;
        private static UltraFaceParameter param;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //capture.ImageGrabbed += Capture_ImageGrabbed;
            //im = capture.QueryFrame().ToImage<Bgr, byte>();
            detectorFace = new DetectorFace(this);
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
        }


        public static void TestSuperres()
        {
            capture.Grab();
            capture.Start();

            //TODO: UltraFaceDotNet 
            param = new UltraFaceDotNet.UltraFaceParameter()
            {
                BinFilePath = "RFB-320.bin",
                ParamFilePath = "RFB-320.param",
                IouThreshold = 0.3f,
                InputLength = 100,
                InputWidth = 100,
                NumThread = 1,
                ScoreThreshold = 0.7f
            };
            var a = UltraFaceDotNet.UltraFace.Create(param);

            //ImageViewer viewer = new ImageViewer();
            ////using (Capture capture = new Capture("car.avi"))
            //using (Emgu.CV.Superres.FrameSource frameSource = new Emgu.CV.Superres.FrameSource(0))//"flv1.avi", false))
            //using (SuperResolution sr = new SuperResolution(SuperResolution.OpticalFlowType.Btvl, frameSource))
            ////using (Superres.SuperResolution sr = new Superres.SuperResolution(Superres.SuperResolution.OpticalFlowType.BTVL1_OCL, frameSource))
            //{
            //    Stopwatch watch = new Stopwatch();
            //    int counter = 0;
            //    //Application.Idle += delegate (object sender, EventArgs e)
            //    //{
            //    watch.Reset();
            //    watch.Start();

            //    //Image<Bgr, byte> frame = frameSource.NextFrame();
            //    Mat frame = new Mat();
            //    sr.NextFrame(frame);
            //    //Image<Gray, Byte> frame = capture.QueryGrayFrame();
            //    watch.Stop();
            //    if (watch.ElapsedMilliseconds < 200)
            //    {
            //        Thread.Sleep(200 - (int)watch.ElapsedMilliseconds);
            //    }
            //    if (!frame.IsEmpty)
            //    {
            //        viewer.Image = frame;
            //        viewer.Text = String.Format("Frame {0}: {1} milliseconds.", counter++, watch.ElapsedMilliseconds);
            //    }
            //    else
            //    {
            //        viewer.Text = String.Format("{0} frames processed", counter);
            //    }
            //    //  };
            //    viewer.ShowDialog();
            //}
        }

        string filepath = "tmpImage.png";
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Image<Bgr, byte> minImage = null;
                if (capture.Retrieve(im))
                {
                    minImage = im.Resize(100, 100, Inter.Cubic);
                }

                using (var ultraFace = UltraFace.Create(param))// config model input
                {
                    using var inMat = NcnnDotNet.Mat.FromPixels(minImage.Mat.DataPointer, NcnnDotNet.PixelType.Bgr2Rgb, 100, 100); //frame.Cols, frame.Rows);

                    var faceInfos = ultraFace.Detect(inMat).ToArray();

                    for (var j = 0; j < faceInfos.Length; j++)
                    {
                        var face = faceInfos[j];
                        var pt1 = new Point<float>(face.X1, face.Y1);
                        var pt2 = new Point<float>(face.X2, face.Y2);
                        minImage.Draw(new System.Drawing.Rectangle((int)face.X1, (int)face.Y1, (int)(face.X2 - face.X1), (int)(face.Y2 - face.Y1)), new Bgr(0, 255, 0));
                    }
                    //imageBox1.Image = minImage;
                }

            }
            catch (Exception ex)
            {

            }

        }


        private void StartBtn_Click(object sender, EventArgs e) => detectorFace.StartDetector();
        private void PauseBtn_Click(object sender, EventArgs e) => detectorFace.PauseDetector();
        private void StopBtn_Click(object sender, EventArgs e) => detectorFace.StopDetector();
        private void AddFaceBtn_Click(object sender, EventArgs e) => detectorFace.AddFace();
        private void ValidFaceBtn_Click(object sender, EventArgs e) => detectorFace.ValidFace();

        private void AddFaceInPhotoBtn_Click(object sender, EventArgs e)
        {
            ChangeEnablePropBtns(false);

            detectorFace.AddFaceINFOTO();
            
            ChangeEnablePropBtns(true);
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            ChangeEnablePropBtns(false);

            detectorFace.StartDetector();
            Task.Run((Action)(()=> detectorFace.Test())).Wait();

            ChangeEnablePropBtns(true);
        }

        private void ChangeEnablePropBtns(bool val)
        {
          //  TestBtn.Enabled = val;
            StartBtn.Enabled = val;
            PauseBtn.Enabled = val;
            StopBtn.Enabled = val;
            AddFaceBtn.Enabled = val;
            ValidFaceBtn.Enabled = val;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            
            string filename = openFileDialog1.FileName;

            try
            {
                detectorFace.DetectCNN(filename);
                detectorFace.DetectHaar(filename, false);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
