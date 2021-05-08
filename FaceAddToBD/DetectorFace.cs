using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Config;
using System.Threading.Tasks;

namespace FaceAddToBD
{
    class DetectorFace
    {
        ///Дописать dispose к объектам, которые нужно освобождать (using () {}) или .Dispose();
        #region Field
        //public Emgu.CV.UI.HistogramBox histogramBox;
        //public TrackBar trackBar;
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
        private readonly Image<Hsv, byte> im;
        private Image<Gray, byte> detectedIm;

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
            /*//trackBar
            trackBar = new TrackBar()
            {
                Location = new Point(),
                Size = new Size(),
                Name = "trackBar",
                TabIndex = 2,
                TabStop = false,
                TickFrequency = 1,
                Maximum = 255,
                Minimum = 0,
                Value = 50
            };*/
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
            this.form.Controls.Add(imageBox1);
            this.form.Controls.Add(imageBox2);
            // this.form.Controls.Add(histogramBox);

            capture = new VideoCapture(Config.Config.DefaultCameraIndex, VideoCapture.API.Any);
            capture.SetCaptureProperty(CapProp.FrameWidth, imageBox1.Width);
            capture.SetCaptureProperty(CapProp.FrameHeight, imageBox1.Height);

            pen = new Pen(Color.Red, 3);

            // histogramBox.GenerateHistogram("hist1", Color.Red, HsvIm.Mat, 256, HsvIm.Mat. );
            // histogramBox.GenerateHistograms(HsvIm, 250);
            // HsvIm = capture.QueryFrame().ToImage<Hsv, byte>();
            // TrackBar.Scroll += trackBar_Scroll; ///trackBar
            im = capture.QueryFrame().ToImage<Hsv, byte>();
            capture.ImageGrabbed += Capture_ImageGrabbed;
            pen = new Pen(Color.Green, 4);
        }
        #endregion
        #region MethodForTrackBar
        private void HandBinaryForTrackBar()
        {
            int trackbar = form.trackBar1.Value;
            form.textBox1.Text = trackbar.ToString();
                using (Image<Gray, Byte> Gray = detectedIm.ThresholdBinary(new Gray(trackbar), new Gray(255)))
                {
                    imageBox1.BackgroundImage = Gray.ToBitmap();
                    detectedIm = detectedIm.ThresholdBinary(new Gray(trackbar), new Gray(255));
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
        public void StopDetector() => capture.Stop();

        public void PauseDetector() => capture.Pause();

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            capture.Retrieve(im);
            faces = classifier.DetectMultiScale(im);
            detectedIm = im.Convert<Gray, byte>();
            bm2 = im.ToBitmap();
            if (faces != null)
            {
                graphics = Graphics.FromImage(bm2);
                foreach (Rectangle face in faces)
                {
                    graphics.DrawRectangle(pen, face);
                }
                faces = null;
            }
            imageBox2.BackgroundImage = bm2;
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
                writer.WriteLine(String.Format("face{0}:{1}", Database.faceList.Count, Database.faceList[^1].PersonName));
                writer.Close();
            }

            if (!Database.AddToBasePerson(classifier)) ///Не тестирован, возможны косяки
            {
                MessageBox.Show("Something went wrong! Retry please!");
                return;
            }
            MessageBox.Show("Person added to database! Waiting \"successful message\"!(Retrain Recoginazer...)");
            RecognizeFace.TrainRecognizer(Database.faceList.Count); //Переписать на вызов в отдельный поток Task
            ///Функция тренировки
            MessageBox.Show("Successful.");
            capture.Start();
        }

        public void ValidFace()
        {
            form.textBox1.Text = "";
            if (Database.faceList.Count > 0)
            {
                //Eigen Face Algorithm
                FaceRecognizer.PredictionResult result = RecognizeFace.recognizer.Predict(detectedIm.Resize(100, 100, Inter.Cubic));
                if (result.Label == -1)
                    form.textBox1.Text = "No face in database!";
                else
                    form.textBox1.Text = Database.nameList[result.Label];
            }
        }
        #endregion
    }
}
