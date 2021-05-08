namespace Config
{
    public enum HaarCascade
    {
        Haarcascade_eye,
        Haarcascade_eye_tree_eyeglasses,
        Haarcascade_frontalcatface,
        Haarcascade_frontalcatface_extended,
        Haarcascade_frontalface_alt,
        Haarcascade_frontalface_alt_tree,
        Haarcascade_frontalface_alt2,
        Haarcascade_frontalface_default,
        Haarcascade_lefteye_2splits,
        Haarcascade_profileface,
        Haarcascade_righteye_2splits
        //,Haarcascade_smile
    }
    public static class Config
    {
        public static string FacePhotosPath = @"Source\FacesPhoto\";
        public static string EigenFaceRecognizerPath = @"Source\EigenFaceRecognizer.txt";
        public static string FaceListTextFile = @"Source\FaceList.txt";///Заменить на полноценную бд и прикрутить форму для заполнения полей БД
        public static string ImageFileExtension = ".bmp";
        public static int DefaultCameraIndex = 0;//0: Default active camera device
        public static double ScaleFactorCascade = 1.1;
        public static double MinNeighborsCascade = 3;
        public static System.Drawing.Size MinSizeCascade = default;
        public static System.Drawing.Size MaxSizeCascade = default;
    }

    public static class HaarCascadePath
    {
        private readonly static string rootFolder = @"CascadeHaarSrc\";
        private readonly static string Haarcascade_eye = "haarcascade_eye.xml";
        private readonly static string Haarcascade_eye_tree_eyeglasses = "haarcascade_eye_tree_eyeglasses.xml";
        private readonly static string Haarcascade_frontalcatface = "haarcascade_frontalcatface.xml";
        private readonly static string Haarcascade_frontalcatface_extended = "haarcascade_frontalcatface_extended.xml";
        private readonly static string Haarcascade_frontalface_alt = "haarcascade_frontalface_alt.xml";
        private readonly static string Haarcascade_frontalface_alt_tree = "haarcascade_frontalface_alt_tree.xml";
        private readonly static string Haarcascade_frontalface_alt2 = "haarcascade_frontalface_alt2.xml";
        private readonly static string Haarcascade_frontalface_default = "haarcascade_frontalface_default.xml";
        private readonly static string Haarcascade_lefteye_2splits = "haarcascade_lefteye_2splits.xml";
        private readonly static string Haarcascade_profileface = "haarcascade_profileface.xml";
        private readonly static string Haarcascade_righteye_2splits = "haarcascade_righteye_2splits.xml";
      //private readonly string haarcascade_smile = "haarcascade_smile.xml";

        public static string GetHaarCascadePath(HaarCascade haarCascade)
        {
            return haarCascade switch
            {
                HaarCascade.Haarcascade_eye => rootFolder + Haarcascade_eye,
                HaarCascade.Haarcascade_eye_tree_eyeglasses => rootFolder + Haarcascade_eye_tree_eyeglasses,
                HaarCascade.Haarcascade_frontalcatface => rootFolder + Haarcascade_frontalcatface,
                HaarCascade.Haarcascade_frontalcatface_extended => rootFolder + Haarcascade_frontalcatface_extended,
                HaarCascade.Haarcascade_frontalface_alt => rootFolder + Haarcascade_frontalface_alt,
                HaarCascade.Haarcascade_frontalface_alt2 => rootFolder + Haarcascade_frontalface_alt2,
                HaarCascade.Haarcascade_frontalface_alt_tree => rootFolder + Haarcascade_frontalface_alt_tree,
                HaarCascade.Haarcascade_frontalface_default => rootFolder + Haarcascade_frontalface_default,
                HaarCascade.Haarcascade_lefteye_2splits => rootFolder + Haarcascade_lefteye_2splits,
                HaarCascade.Haarcascade_profileface => rootFolder + Haarcascade_profileface,
                HaarCascade.Haarcascade_righteye_2splits => rootFolder + Haarcascade_righteye_2splits,
              //HaarCascade.Haarcascade_smile => rootFolder + Haarcascade_smile,
                _ => throw new System.Exception("Enum value exception!"),
            };
        }
    }
}
