using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceAddToBD
{
    public struct FaceData
    {
        public string PersonName { get; set; }
        public Image<Gray, byte> FaceImage { get; set; }
        public DateTime CreateDate { get; set; }

        public int Id { get; set; }

    }
}
