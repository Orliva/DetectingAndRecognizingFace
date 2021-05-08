using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceAddToBD
{
    public partial class Form1 : Form
    {
        DetectorFace detectorFace;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            detectorFace = new DetectorFace(this);
        }

        private void StartBtn_Click(object sender, EventArgs e) => detectorFace.StartDetector();
        private void PauseBtn_Click(object sender, EventArgs e) => detectorFace.PauseDetector();
        private void StopBtn_Click(object sender, EventArgs e) => detectorFace.StopDetector();

        private void AddFaceBtn_Click(object sender, EventArgs e) => detectorFace.AddFace();
        private void ValidFaceBtn_Click(object sender, EventArgs e) => detectorFace.ValidFace();

        //Удалить
        private void DeleteBtn_Click(object sender, EventArgs e) => Database.DeleteAllSource();
    }
}
