using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Text.Json.Serialization;

namespace FaceAddToBD
{
    public struct FaceData
    {
        public string PersonName { get; set; }
        [JsonIgnore]
        public Image<Gray, byte> FaceImage { get; set; }
        public DateTime CreateDate { get; set; }
        public int Id { get; set; }

        public static bool operator == (FaceData fd1, FaceData fd2) => fd1.Id == fd2.Id;
        public static bool operator !=(FaceData fd1, FaceData fd2) => fd1.Id != fd2.Id;
    }
}
