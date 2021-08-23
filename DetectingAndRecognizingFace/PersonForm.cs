using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceAddToBD
{
    public partial class PersonForm : Form
    {
        private readonly List<FaceData> faceList;
        private readonly Image<Gray, byte> faceImage;
        private FaceData faceData;

        public PersonForm(List<FaceData> faceList, Image<Gray, byte> detectedIm)
        {
            this.faceList = faceList;
            this.faceImage = detectedIm;
            this.DialogResult = DialogResult.Cancel;
            InitializeComponent();
        }

        public DialogResult MyShowDialog(out FaceData faceDataItem)
        {
            faceDataItem = default(FaceData);
            DialogResult res = ShowDialog();
            if (res == DialogResult.OK)
                faceDataItem = faceData;
            return res;
        }

        /// <summary>
        /// Получаем FaceData, если пользователь нажал "OK"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, EventArgs e)
        {
            int countId = 1;
            foreach (FaceData fd in faceList)
            {
                if (fd.PersonName == PesonFormTextBox.Text)
                {
                    countId = fd.Id;
                    break;
                }
                else if (fd.Id >= countId)
                {
                    countId = fd.Id;
                    countId++;
                }
            }
            faceData.Id = countId;
            faceData.PersonName = PesonFormTextBox.Text;
            faceData.CreateDate = DateTime.Now;
            faceData.FaceImage = faceImage.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e) => this.Close();
    }
}
