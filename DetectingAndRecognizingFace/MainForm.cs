using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceAddToBD
{
    partial class MainForm : Form
    {
        private DetectorFace detectorFace;

        public MainForm()
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
            TestBtn.Enabled = val;
            StartBtn.Enabled = val;
            PauseBtn.Enabled = val;
            StopBtn.Enabled = val;
            AddFaceBtn.Enabled = val;
            ValidFaceBtn.Enabled = val;
        }
    }
}
