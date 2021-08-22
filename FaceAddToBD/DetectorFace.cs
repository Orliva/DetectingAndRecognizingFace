using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Config;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace FaceAddToBD
{
    class DetectorFace
    {
        ///Дописать dispose к объектам, которые нужно освобождать (using () {}) или .Dispose();
        #region Field
        //public Emgu.CV.UI.HistogramBox histogramBox;
        public TrackBar trackBar;
        public Emgu.CV.UI.ImageBox imageBox1;
        public Emgu.CV.UI.ImageBox imageBox2;
        public Form1 form;
        private static VideoCapture capture;
        private Bitmap bm;
        private Bitmap bm2;
        private Graphics graphics;
        private Rectangle[] faces;
        private readonly Pen pen;
        private static readonly CascadeClassifier classifier =
            new CascadeClassifier(Config.HaarCascadePath.GetHaarCascadePath(HaarCascade.Haarcascade_frontalface_alt_tree));
        //private readonly Image<Hsv, byte> im;
        private readonly Image<Bgr, byte> im;
        private Image<Gray, byte> detectedIm;
        private Image<Gray, byte> grayIm;

        //private Image<Hsv, byte> HsvIm;
        //private DenseHistogram histogram = new DenseHistogram(255, new RangeF(0, 255)); //Histogram
        //private float[] GrayHist = new float[255]; //Histogram
        #endregion
        #region Constructor
        public DetectorFace(Form1 form)
        {
            
            this.form = form;
            imageBox1 = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(12, 12),
                Size = new Size(form.Width / 2 - 12, form.Height - 80), //просчитать размеры для оптимального детектирования картинки в зависимости от скалирования каскада
                Name = "imageBox1",
                TabIndex = 2,
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        };
            imageBox2 = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(form.Width / 2 - 12, 12),
                Size = new Size(form.Width / 2 - 12, form.Height - 80), //просчитать размеры для оптимального детектирования картинки в зависимости от скалирования каскада
                Name = "imageBox2",
                TabIndex = 2,
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile
            };
            //trackBar
            trackBar = new TrackBar()
            {
                Location = new Point(391, 369),
                Size = new Size(392, 56),
                Name = "trackBar",
                TabIndex = 2,
                TabStop = false,
                TickFrequency = 1,
                Maximum = 255,
                Minimum = 0,
                Value = 50,
            };
            /*///HistogramBox
            histogramBox = new Emgu.CV.UI.HistogramBox
            {
                Location = new Point(form.Width / 2 - 12, 12),
                Size = new Size(form.Width / 2 - 12, form.Height - 80), //просчитать размеры для оптимального детектирования картинки в зависимости от скалирования каскада
                Name = "histogramBox",
                TabIndex = 2,
                TabStop = false,
                FunctionalMode =  Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile,
            };*/
        //    this.form.Controls.Add(trackBar);
            this.form.Controls.Add(imageBox1);
            this.form.Controls.Add(imageBox2);
            // this.form.Controls.Add(histogramBox);

            capture = new VideoCapture(Config.Config.DefaultCameraIndex, VideoCapture.API.Any);
         /*   form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Zoom));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.WhiteBalanceRedV));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.WhiteBalanceBlueU));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.WbTemperature));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.TriggerDelay));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Trigger));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Tilt));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Temperature));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Sharpness));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Settings));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Saturation));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.SarNum));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.SarDen));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Roll));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Rectification));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.PvapiPixelFormat));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.PvapiFrameStartTriggerMode));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Pan));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Monochrome));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Mode));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.MaxDC1394));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.IsoSpeed));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Iris));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Hue));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Gamma));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Gain));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Fps));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.FourCC));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Format));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Focus));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Exposure));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.ConvertRgb));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Contrast));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Channel));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Buffersize));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Brightness));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Backlight));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Backend));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.AutoWb));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.Autofocus));
            form.listBox1.Items.Add(capture.GetCaptureProperty(CapProp.AutoExposure));
         */
            capture.SetCaptureProperty(CapProp.FrameWidth, imageBox1.Width);
            capture.SetCaptureProperty(CapProp.FrameHeight, imageBox1.Height);
            capture.SetCaptureProperty(CapProp.Fps, 30.0);

            pen = new Pen(Color.Red, 3);

            // histogramBox.GenerateHistogram("hist1", Color.Red, HsvIm.Mat, 256, HsvIm.Mat. );
            // histogramBox.GenerateHistograms(HsvIm, 250);
            // HsvIm = capture.QueryFrame().ToImage<Hsv, byte>();
            Database.TryRestoreDB();
            trackBar.Scroll += TrackBar_Scroll; ///trackBar
            im = capture.QueryFrame().ToImage<Bgr, byte>();
            detectedIm = null;
            grayIm = null;
            capture.ImageGrabbed += Capture_ImageGrabbed;
         //   capture.Grab();
           // capture.Start();
        }
        #endregion
        #region MethodForTrackBar
        private void HandBinaryForTrackBar()
        {
            int trackbar = trackBar.Value;
            form.textBox1.Text = trackbar.ToString();
  //              using (Image<Gray, Byte> Gray = detectedIm.ThresholdBinary(new Gray(trackbar), new Gray(255)))
                {
    //                imageBox1.BackgroundImage = Gray.ToBitmap();
      //              detectedIm = detectedIm.ThresholdBinary(new Gray(trackbar), new Gray(255));
                }
            detectedIm._EqualizeHist();
            faces = classifier.DetectMultiScale(detectedIm, 1.05, 1);
            bm = detectedIm.Convert<Bgr, byte>().ToBitmap();
            graphics = Graphics.FromImage(bm);
            foreach (Rectangle face in faces)
            {
                graphics.DrawRectangle(pen, face);
            }
            imageBox1.BackgroundImage = bm;
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (detectedIm != null)
            {
                capture.Stop();
                //detectedIm = detectedIm.Canny(100, 0);
                detectedIm.Laplace(1);
              //  BackgroundRemovalFilter.GetGradient(ref detectedIm, ref imageBox1);
                HandBinaryForTrackBar();
                capture.Start();
            }
        }
        #endregion
        #region FailKMeanAlgoritm //Алгоритм k-средних, самописный не работающий
        /*
                double KMeanAlg(float[] arr)
                {
                    float maxVal = arr[0];
                    (float, float) m;
                    float T = GetMiddleStart(arr, ref maxVal);
                    float oldT = 0;
                    float delta = T - oldT;
                    while (delta > 0.01)
                    {
                        m = GetMiddle(arr, T, maxVal);
                        oldT = T;
                        T = (m.Item1 + m.Item2) / 2;
                        delta = Math.Abs(T - oldT);
                    }
                    return (double)T;

                }

                (float, float) GetMiddle(float[] arr, float threshold, float max)
                {
                    (float, float) res = (0, 0);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] > threshold)
                            res.Item1 += arr[i];
                        if (arr[i] < threshold)
                            res.Item2 += arr[i];
                    }
                    res.Item1 = (res.Item1 / (arr.Length));
                    res.Item2 = (res.Item2 / (arr.Length));
                    return res; 
                }

                float GetMiddleStart(float[] arr, ref float max)
                {
                    float sum = 0;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        sum += arr[i];
                        if (arr[i] > max)
                            max = arr[i];
                    }
                    return ((sum / (arr.Length)));
                }
                float GetMiddleStart(float[] arr)
                {
                    float max = arr[0];
                    float sum = 0;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        sum += arr[i];
                        if (arr[i] > max)
                            max = arr[i];
                    }
                   return ((sum / (arr.Length)));
                }
                float GetMax(float[] arr)
                {
                    float max = arr[0];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] > max)
                            max = arr[i];
                    }
                    return max;
                }*/
        #endregion ///
        #region Method
        public void StartDetector()
        {
            capture.Grab();
            capture.Start();
        }
        public void StopDetector()
        {
            capture.Stop(); 
            using (StreamWriter sw = new StreamWriter(Config.Config.TESTPATH, true))
            {
      //          sw.WriteLine((((double)countSucces / (double)count) * (double)100).ToString() + "%");
        //        sw.WriteLine("CountTest = " + count.ToString());
          //      sw.WriteLine("NameTested = " + "Ivan");
            }
            count = 0;
            countSucces = 0;
        }

        public void PauseDetector() => capture.Pause();

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            capture.Retrieve(im);
            //faces = classifier.DetectMultiScale(im, 1.1, 0);
            grayIm = im.Convert<Gray, byte>();
            faces = classifier.DetectMultiScale(grayIm, 1.1, 0);
            bm2 = im.ToBitmap();
           // bm2 = detectedIm.ToBitmap();
            if (faces.Length != 0)
            {
                graphics = Graphics.FromImage(bm2);
                detectedIm = grayIm.GetSubRect(faces[0]);///Самый маленький квадрат
                foreach (Rectangle face in faces)
                {
                    graphics.DrawRectangle(pen, face);
                }
                //if (detectedIm != null)
                 //   imageBox1.BackgroundImage = detectedIm.ToBitmap();
            }
            imageBox2.BackgroundImage = bm2;
         //   TEST();
        }

        public void AddFace()
        {
            capture.Pause();
            if (detectedIm == null)
            {
                 MessageBox.Show("No face detected!", "No face detected error", MessageBoxButtons.OK);
                capture.Start();
                return;
            }
            Database.ValidSource();
            //Save detected face

            if (new PersonForm(ref Database.faceList, ref detectedIm).ShowDialog() == DialogResult.Cancel)
                return;
            else
            {
                ///IsPersonInclude пробуем определить лицо и сравнить с имеющейся базой
                ///если нашли выдать юзеру сообщение, что такой уже есть и спросить добавить ли нового или добавить фото к старому
               // if (IsPersonInclude())
               // {
               //     MessageBox.Show("This person right now located in database!");
               //     ///Заменить return на метод добавления новых фотографий для существующего человека из базы
               //     ///по согласованию с юзером
               //     return; 
               // }
            }
           // Database.faceList[^1].FaceImage.Save(Config.Config.FacePhotosPath + "Oly" + (^1) + Config.Config.ImageFileExtension);
            using (StreamWriter writer = new StreamWriter(Config.Config.FaceListTextFile, true))
            {
                static bool isNameInclude()
                {
                    foreach (string str in Database.nameList)
                    {
                        if (str == Database.faceList[^1].PersonName)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                if (!isNameInclude())
                {
                    Database.nameList.Add(Database.faceList[^1].PersonName);
                    writer.WriteLine(String.Format("face{0}:{1}", Database.faceList[^1].Id, Database.faceList[^1].PersonName));
                }
                writer.Close();
            }

            if (!Database.AddToBasePerson(classifier)) ///Не тестирован, возможны косяки
            {
                MessageBox.Show("Something went wrong! Retry please!");
                return;
            }
            MessageBox.Show("Person added to database! Press \"OK\" and waiting \"successful message\"!(Retrain Recoginazer...)");
            RecognizeFace.TrainRecognizer(Database.nameList.Count); //Переписать на вызов в отдельный поток Task
            ///Функция тренировки
            MessageBox.Show("Successful.");
            detectedIm = null;
            capture.Start();
        }


        public void AddFaceINFOTO()
        {
            FaceData faceData = new FaceData();
            faceData.PersonName = "George Bush";
            Database.faceList.Add(faceData);
            Database.nameList.Add(Database.faceList[^1].PersonName);
            //531
            Image<Gray, byte> tmpIm;
            Bitmap tmpBitmap;
       //     Image asdf;
            //  for (int i = 1; i < 531; i++)
            //{
         //   int j = 1;
           //     foreach (string fileName in Directory.EnumerateFiles(@"lfw-bush\lfw\George_W_Bush\"))
             //   {
               //     asdf = Image.FromFile(fileName);
                 //   
                   //     asdf.Save(@"lfw-bush\lfw\George_W_Bush\George_W_Bush_" + HelpGetPhoto(j++) + ".bmp");
                    
                    //System.IO.File.Delete(fileName);
                //}
            //}
            for (int i = 1; i < 10; i++)
            {
                tmpBitmap = Bitmap.FromFile(@"lfw-bush\lfw\George_W_Bush\George_W_Bush_" + HelpGetPhoto(i) + ".bmp") as Bitmap;
                tmpIm = tmpBitmap.ToImage<Gray, byte>();
                TryGetFacePhoto(ref tmpIm, ref tmpBitmap);
              //  Thread.Sleep(50);
            }
            RecognizeFace.TrainRecognizer(Database.faceList.Count); //Переписать на вызов в отдельный поток Task
            MessageBox.Show("Successful.");

        }

        private void TryGetFacePhoto(ref Image<Gray, byte> im, ref Bitmap bm)
        {
            ///Обязательно остановить камеру!!!
            // im = im.Resize(100, 100, Inter.Cubic);
            Image<Gray, byte> tmpIm;
            tmpIm = null;
            Rectangle[] tmpRect = null;

            tmpRect = classifier.DetectMultiScale(im, 1.1, 0);
            if (im != null)
            {
                graphics = Graphics.FromImage(bm);
                foreach (Rectangle face in tmpRect)
                {
                    graphics.DrawRectangle(pen, face);
                    tmpIm = im.GetSubRect(face).Resize(100, 100, Inter.Cubic);
                }
                FaceData fd = new FaceData();
            //    fd.FaceImage = tmpIm;
                fd.PersonName = Database.faceList[^1].PersonName;
                Database.faceList.Remove(Database.faceList[^1]);
                Database.faceList.Add(fd);
                if (!Database.AddToBasePerson(classifier)) 
                {
          //          MessageBox.Show("Something went wrong! Retry please!");
                    return;
                }
            //    MessageBox.Show("Person added to database! Press \"OK\" and waiting \"successful message\"!(Retrain Recoginazer...)");
                ///Функция тренировки
              //  MessageBox.Show("Successful.");
                tmpIm = null;
                //if (detectedIm != null)
                //   imageBox1.BackgroundImage = detectedIm.ToBitmap();
            }
            imageBox1.Invoke(new Func<Bitmap, bool>(qwer), new object[] { bm });

        }
        bool qwer(Bitmap bm)
        {
            imageBox1.BackgroundImage = bm;
            return true;
        }
        private string HelpGetPhoto(int i)
        {
            if (i < 10)
                return "000" + i;
            else if (i < 100) return "00" + i;
            else if (i < 1000) return "0" + i;
            return "";
        }

        public void ValidFace()
        {
            form.textBox1.Text = "";
            if (Database.nameList.Count > 0)
            {
                //Eigen Face Algorithm
                FaceRecognizer.PredictionResult result = RecognizeFace.recognizer.Predict(detectedIm.Resize(100, 100, Inter.Cubic));
                if (result.Label == -1)
                    form.textBox1.Text = "No face in database!";
                else
                    form.textBox1.Text = Database.nameList[result.Label];
            }
        }
        public string ValidFace(double asdf)
        {

            if (Database.nameList.Count > 0)
            {
                //Eigen Face Algorithm
                FaceRecognizer.PredictionResult result = RecognizeFace.recognizer.Predict(detectedIm.Resize(100, 100, Inter.Cubic));
                if (result.Label == -1)
                    return "No face in database!";
                else
                     return Database.nameList[result.Label];
            }
            return "";
        }

        public void TEST()
        {
            TEST_("qwe");
        }
        double count = 0;
        double countSucces = 0;
        private void TEST_(string NAME)
        {
            count++;
            if (ValidFace(count) == NAME)
                countSucces++;
            if (count == 300)
                StopDetector();
          //  asdf(countSucces, count);
        }
        bool asdf(long countSucces, long count)
        {
            using (StreamWriter sw = new StreamWriter(Config.Config.TESTPATH, true))
            {
                sw.WriteLine(((countSucces / count) * 100).ToString() + "%");
            }
            return true;
        }

        #endregion
    }
}
