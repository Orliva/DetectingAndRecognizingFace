using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using FaceAddToBD.Configuration;
using FaceAddToBD.Configuration.Enum;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace FaceAddToBD
{
    class DetectorFace : IDisposable
    {
        #region Field
        private readonly Emgu.CV.UI.ImageBox imageBoxColorCNN;
        private readonly Emgu.CV.UI.ImageBox imageBoxColorHaar;
        private readonly Emgu.CV.UI.ImageBox imageBoxGrayHaar;
        private readonly Emgu.CV.UI.ImageBox imageBoxGrayCnn;
        private readonly MainForm form;
        private readonly VideoCapture capture;
        private readonly Pen pen;
        private readonly CascadeClassifier classifier = new CascadeClassifier(
            HaarCascadePath.GetHaarCascadePath(HaarCascade.Haarcascade_frontalface_alt_tree) //решающие деревья (набор вейетов) - Haarcascade_frontalface_alt_tree
            );
        private readonly Image<Bgr, byte> sourceIm;
        private Image<Bgr, byte> CNNImage = null;
        private readonly MySimpleDatabase database;
        private readonly RecognizeFace recognizer;
        private Image<Gray, byte> detectedImCNN;
        private Image<Gray, byte> detectedImHaar;
        private Image<Gray, byte> tmpGrayImHaar;
        private Bitmap bm;
        private Graphics graphics;
        private Rectangle[] faces;

        UltraFaceDotNet.UltraFaceParameter param = new UltraFaceDotNet.UltraFaceParameter()
        {
            BinFilePath = "RFB-320.bin",
            ParamFilePath = "RFB-320.param",
            IouThreshold = 0.3f,
            InputLength = 100,
            InputWidth = 100,
            NumThread = 1,
            ScoreThreshold = 0.7f
        };
    #endregion

    #region Constructor
    public DetectorFace(MainForm form)
        {
            database = new MySimpleDatabase();
            recognizer = new RecognizeFace(database);
            database.Recognizer = recognizer;

            this.form = form;
            imageBoxColorHaar = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(form.Width / 2 + 12, 12),
                Size = new Size(form.Width / 2 - 36, form.Height - 180),
                Name = "imageBox2",
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = BorderStyle.None,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            imageBoxGrayHaar = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(form.Width / 2 + 12 + imageBoxColorHaar.Width / 2 + 100,
                    12),
                Size = new Size(imageBoxColorHaar.Width / 2 - 100, imageBoxColorHaar.Height / 2 - 100),
                Name = "imageBox3",
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = BorderStyle.None,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            imageBoxColorCNN = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(12, 12),
                Size = new Size(form.Width / 2 - 12, form.Height - 180), //просчитать размеры для оптимального детектирования картинки в зависимости от скалирования каскада
                Name = "imageBox1",
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = BorderStyle.None,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            imageBoxGrayCnn = new Emgu.CV.UI.ImageBox
            {
                Location = new Point(12 + imageBoxColorCNN.Width / 2 + 100,
                    12),
                Size = new Size(imageBoxColorCNN.Width / 2 - 100, imageBoxColorCNN.Height / 2 - 100), //просчитать размеры для оптимального детектирования картинки в зависимости от скалирования каскада
                Name = "imageBox4",
                TabStop = false,
                FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum,
                BorderStyle = BorderStyle.None,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            this.form.Controls.Add(imageBoxColorCNN);
            this.form.Controls.Add(imageBoxColorHaar);
            this.form.Controls.Add(imageBoxGrayHaar);
            this.form.Controls.Add(imageBoxGrayCnn);

            imageBoxGrayCnn.BringToFront();
            imageBoxGrayHaar.BringToFront();

            capture = new VideoCapture(Configuration.Config.DefaultCameraIndex, VideoCapture.API.Any);
            //Смотрим возможности VideoCapture
            #region VideoCapturePossibility
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
            #endregion
            //capture.SetCaptureProperty(CapProp.FrameWidth, imageBox1.Width);
            //capture.SetCaptureProperty(CapProp.FrameHeight, imageBox1.Height);
            //capture.SetCaptureProperty(CapProp.Fps, 30.0);

            pen = new Pen(Color.Red, 3);

            database.TryRestoreDB();

            sourceIm = capture.QueryFrame().ToImage<Bgr, byte>();
            detectedImCNN = null;
            detectedImHaar = null;

            tmpGrayImHaar = null;
            capture.ImageGrabbed += Capture_ImageGrabbed;
        }
        #endregion

        #region Class Interface
        public void StartDetector()
        {
            capture.Grab();
            capture.Start();
        }
        public void StopDetector() => capture.Stop();
        public void PauseDetector() => capture.Pause();

        /// <summary>
        /// Распознать юзера из имеющейся базы
        /// </summary>
        public void ValidFace()
        {
            form.textBox1.Text = "";
            if (detectedImCNN != null)
                form.textBox1.Text = ValidFace(detectedImCNN);
            else
                MessageBox.Show("Image for detection not found", "Error", MessageBoxButtons.OK);
        }

        /// <summary>
        /// Добавить юзера
        /// </summary>
        public void AddFace()
        {
            capture.Pause();
            if (detectedImCNN == null)
            {
                MessageBox.Show("No face detected!", "No face detected error", MessageBoxButtons.OK);
                capture.Start();
                return;
            }
            ImpAddFace(detectedImCNN);
        }
        #endregion

        #region Implementation

        /// <summary>
        /// Пробуем распознать юзера.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private string ValidFace(Image<Gray, byte> image)
        {
            if (database.NameList.Count > 0)
            {
                FaceRecognizer.PredictionResult result = recognizer.recognizer.Predict(image.Resize(100, 100, Inter.Cubic));
                if (result.Label == -1)
                    return Configuration.Config.NoFaceInDB;
                else
                    return database.NameList[result.Label - 1];
            }
            return "";
        }

        /// <summary>
        /// Обрабатываем видеопоток
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            //Работает не в основном потоке, try для корректного завершения программы
            try
            {
                //Получаем кадр из видеопотока
                if (capture.Retrieve(sourceIm))
                {
                    ////Определяем лицо методом хаара
                    DetectHaar(sourceIm);

                    ////Определяем лицо нейронкой
                    DetectCNN(sourceIm);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void DetectHaar(Image<Bgr, byte> imageSource, bool isSmallesFind = true)
        {
            //Определяем лицо методом хаара
            tmpGrayImHaar = imageSource.Convert<Gray, byte>();
            faces = classifier.DetectMultiScale(image: tmpGrayImHaar, scaleFactor: 1.1, minNeighbors: 0);
            bm = imageSource.ToBitmap();
            if (faces.Length != 0)
            {
                if (isSmallesFind)
                    GetSmallesBoxes();
                else
                    GetAllBoxes();
                imageBoxGrayHaar.BackgroundImage = detectedImHaar.ToBitmap();
                //if (testFlag)
                //  ImpTest(detectedIm, "Ivan", 300);
            }
            imageBoxColorHaar.BackgroundImage = bm;
        }

        private void GetSmallesBoxes()
        {
            graphics = Graphics.FromImage(bm);
            detectedImHaar = tmpGrayImHaar.GetSubRect(faces[0]).Resize(100, 100, Inter.Cubic);///Самый маленький квадрат
            graphics.DrawRectangle(pen, faces[0]);
        }

        private void GetAllBoxes()
        {
            graphics = Graphics.FromImage(bm);
            foreach (var i in faces)
            {
                detectedImHaar = tmpGrayImHaar.GetSubRect(i).Resize(100, 100, Inter.Cubic);///Самый маленький квадрат
                graphics.DrawRectangle(pen, i);
            }
        }

        private void DetectCNN(Image<Bgr, byte> imageSource)
        {
            //Определяем лицо нейронкой
            using (var ultraFace = UltraFaceDotNet.UltraFace.Create(param))
            {
                CNNImage = imageSource.Resize(1000, 1000, Inter.Cubic);
                using var inMat = NcnnDotNet.Mat.FromPixels(CNNImage.Mat.DataPointer, NcnnDotNet.PixelType.Bgr2Rgb, 1000, 1000);

                var faceInfos = ultraFace.Detect(inMat).ToArray();

                for (var j = 0; j < faceInfos.Length; j++)
                {
                    var face = faceInfos[j];
                    var pt1 = new NcnnDotNet.OpenCV.Point<float>(face.X1, face.Y1);
                    var pt2 = new NcnnDotNet.OpenCV.Point<float>(face.X2, face.Y2);
                    var rect = new System.Drawing.Rectangle((int)face.X1, (int)face.Y1, (int)(face.X2 - face.X1), (int)(face.Y2 - face.Y1));

                    detectedImCNN = CNNImage.Convert<Gray, byte>().GetSubRect(rect).Resize(100, 100, Inter.Cubic);
                    CNNImage.Draw(rect, new Bgr(181, 68, 157), 2);
                }
                imageBoxColorCNN.BackgroundImage = CNNImage.ToBitmap();
                imageBoxGrayCnn.BackgroundImage = detectedImCNN?.ToBitmap();
            }
        }

        public void DetectHaar(string pathToPhoto, bool isSmallestFind)
        {
            DetectHaar(new Image<Bgr, byte>(pathToPhoto), isSmallestFind);
        }

        public void DetectCNN(string pathToPhoto)
        {
            DetectCNN(new Image<Bgr, byte>(pathToPhoto));
        }

        private bool IsMaybePersonInclude(Image<Gray, byte> image, out string name)
        {
            name = ValidFace(image);

            if (name != "" && name != Config.NoFaceInDB)
            {
                DialogResult res = MessageBox.Show($"Возможно, на фото: {name}.\n" +
                    $"Добавить фотографии к пользователю \"{name}\"? (Если нажать \"нет\", создаться новый пользователь!)",
                    caption: "Добавить к существующему пользователю?", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                    return true;
            }
            return false;
        }

        private bool IsFaceDataInclude(in FaceData faceData)
        {
            foreach (FaceData fd in database.FaceList)
            {
                if (fd == faceData)
                    return true;
            }
            return false;
        }

        private void ImpAddFace(Image<Gray, byte> image)
        {
            FaceData tmpFaceData;

            ///IsMaybePersonInclude пробуем определить лицо и сравнить с алгоритмом распознавания
            ///если нашли выдать юзеру сообщение, что такой уже есть и спросить добавить ли нового юзера или добавить фото к найденному
            if (IsMaybePersonInclude(image, out string name))
            {
                tmpFaceData = database.GetUser(name);
                tmpFaceData.FaceImage = image;
            }
            else
            {
                //С помощью формы получаем экземпляр FaceData и проверяем, есть ли уже юзер с таким id
                //Если нет, то добавляем нового юзера, иначе работаем с уже существующим (меняем только текущую фотографию в FaceData)
                if (new PersonForm(database.FaceList, detectedImCNN).MyShowDialog(out FaceData faceData) != DialogResult.Cancel)
                {
                    if (!IsFaceDataInclude(in faceData))
                    {
                        database.FaceList.Add(faceData);
                        database.NameList.Add(faceData.PersonName);
                    }
                    tmpFaceData = faceData;
                }
                else
                    return;
            }
            if (!database.AddToBasePerson(in tmpFaceData))
            {
                MessageBox.Show("Something went wrong! Retry please!");
                return;
            }
            Task.Run((Action)(() => recognizer.TrainRecognizer())).Wait();
            MessageBox.Show("Successful.");
            detectedImCNN = null;
            capture.Start();
        }
        #endregion

        #region Recognize George Bush Test

        /// <summary>
        /// Взял датасет с фотографиями Джоржа Буша.
        /// </summary>
        public void AddFaceINFOTO()
        {
            StopDetector();
            FaceData faceData = new FaceData
            {
                PersonName = "George Bush",
                CreateDate = DateTime.Now,
            };
            database.FaceList.Add(faceData);
            database.NameList.Add(database.FaceList[^1].PersonName);
            Image<Gray, byte> tmpIm;
            Bitmap tmpBitmap;
            for (int i = 1; i < 10; i++)
            {
                tmpBitmap = Bitmap.FromFile(@"lfw-bush\lfw\George_W_Bush\George_W_Bush_" + HelpGetPhotoName(i) + ".bmp") as Bitmap;
                tmpIm = tmpBitmap.ToImage<Gray, byte>();
                GetFacePhoto(tmpIm, tmpBitmap, faceData);
                Thread.Sleep(50);
            }
            Task.Run((Action)(() => recognizer.TrainRecognizer())).Wait();
            MessageBox.Show("Successful.");
        }

        /// <summary>
        /// Получаем лицо с фотографии
        /// </summary>
        /// <param name="im"></param>
        /// <param name="bm"></param>
        /// <param name="faceData"></param>
        private void GetFacePhoto(Image<Gray, byte> im, Bitmap bm, in FaceData faceData)
        {
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
                if (!database.AddToBasePerson(in faceData))
                    return;
                tmpIm = null;
            }
            imageBoxColorCNN.Invoke((Action)(() => imageBoxColorCNN.BackgroundImage = bm));
        }

        /// <summary>
        /// Помощь в получении корректного имени файла фотографии
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string HelpGetPhotoName(int i)
        {
            if (i < 10)
                return "000" + i;
            else if (i < 100) return "00" + i;
            else if (i < 1000) return "0" + i;
            return "";
        }
        #endregion

        #region Recognition percentage test

        int countTest;
        int countSuccessTest;
        bool testFlag = false;
        DialogResult resultTest;

        /// <summary>
        /// Инициализируем счетчики. В бесконечном цикле ждем N итераций обнаружения лица на вебкамере.
        /// В другом потоке происходит обработка видеопотока и увеличение счетчиков.
        /// </summary>
        /// <returns></returns>
        public Task Test()
        {
            resultTest = DialogResult.Cancel;
            countTest = 0;
            countSuccessTest = 0;
            testFlag = true;
            while (resultTest != DialogResult.OK) { Thread.Sleep(100); }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Происходит увеличение счетчиков, после сравнения с алгоритмом распознавания.
        /// Выполняется в одном потоке с обработкой видеопотока.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="nameDest"></param>
        /// <param name="countTestIteration"></param>
        private void ImpTest(Image<Gray, byte> image, string nameDest, int countTestIteration)
        {
            countTest++;
            if (ValidFace(image) == nameDest)
                countSuccessTest++;
            if (countTest == countTestIteration)
            {
                this.StopDetector();
                testFlag = false;
                MessageBox.Show(text: $"Test completed! \"{nameDest}\": {countSuccessTest / countTest * 100} % success recognized.",
                                caption: "Test completed!",
                                buttons: MessageBoxButtons.OK);
                resultTest = DialogResult.OK;
                PrintResultInFile(nameDest, countSuccessTest, countTest);
            }
        }

        /// <summary>
        /// Записываем результат в файл.
        /// </summary>
        /// <param name="nameDest"></param>
        /// <param name="countSuccessTest"></param>
        /// <param name="countTest"></param>
        /// <returns></returns>
        private bool PrintResultInFile(string nameDest, int countSuccessTest, int countTest)
        {
            using (StreamWriter sw = new StreamWriter(Config.TESTPATH, true))
            {
                sw.WriteLine($"\"{nameDest}\": {countSuccessTest / countTest * 100} %");
            }
            return true;
        }
        #endregion

        /// Освобождаем память
        #region IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    faces = null;
                }
                imageBoxColorCNN?.Dispose();
                imageBoxColorHaar?.Dispose();
                capture?.Dispose();
                pen?.Dispose();
                classifier?.Dispose();
                sourceIm?.Dispose();
                database?.Dispose();
                recognizer?.Dispose();
                detectedImCNN?.Dispose();
                tmpGrayImHaar?.Dispose();
                bm?.Dispose();
                graphics?.Dispose();
                disposedValue = true;
            }
        }

         ~DetectorFace()
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
