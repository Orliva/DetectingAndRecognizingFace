using System;
using System.Threading.Tasks;
using Emgu.CV.Face;

namespace FaceAddToBD
{
    class RecognizeFace : IDisposable
    {
        public EigenFaceRecognizer recognizer;
     //   public LBPHFaceRecognizer rec2;
     //   public FisherFaceRecognizer rec3;
        private readonly MySimpleDatabase database;

        public RecognizeFace(MySimpleDatabase db)
        {
            recognizer = new EigenFaceRecognizer(2, 1.8 * Math.Pow(10.0, 10.0));
            database = db;
        }

        public Task TrainRecognizer()
        {
            recognizer.Train(database.ImageList, database.LabelList);
            recognizer.Write(Configuration.Config.EigenFaceRecognizerPath);
            return Task.CompletedTask;
        }

        //Освобождам память
        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
           //     rec2.Dispose();
           //     rec3.Dispose();
                recognizer?.Dispose();
                disposedValue = true;
            }
        }

         ~RecognizeFace()
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
