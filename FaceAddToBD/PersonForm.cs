﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceAddToBD
{
    public partial class PersonForm : Form
    {
        private List<FaceData> faceList;
        private Image<Gray, byte> detectedIm;
        public PersonForm(ref List<FaceData> faceList, ref Image<Gray, byte> detectedIm)
        {
            
            this.faceList = faceList;
            this.detectedIm = detectedIm;
            this.DialogResult = DialogResult.Cancel;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FaceData tmpFaceData = new FaceData();
            int countId = 0;
            foreach (FaceData fd in faceList)
            {
                if (fd.PersonName == textBox1.Text)
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
            tmpFaceData.Id = countId;
            tmpFaceData.PersonName = textBox1.Text;
            tmpFaceData.CreateDate = DateTime.Now;
            tmpFaceData.FaceImage = detectedIm.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
            faceList.Add(tmpFaceData);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) => this.Close();
    }
}
