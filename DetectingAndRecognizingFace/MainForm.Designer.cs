namespace FaceAddToBD
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            detectorFace.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartBtn = new System.Windows.Forms.Button();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.AddFaceBtn = new System.Windows.Forms.Button();
            this.ValidFaceBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.TestBtn = new System.Windows.Forms.Button();
            this.AddFaceInPhotoBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(12, 433);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(165, 40);
            this.StartBtn.TabIndex = 3;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(183, 433);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(165, 40);
            this.PauseBtn.TabIndex = 4;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(354, 433);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(165, 40);
            this.StopBtn.TabIndex = 5;
            this.StopBtn.Text = "Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // AddFaceBtn
            // 
            this.AddFaceBtn.Location = new System.Drawing.Point(12, 536);
            this.AddFaceBtn.Name = "AddFaceBtn";
            this.AddFaceBtn.Size = new System.Drawing.Size(507, 51);
            this.AddFaceBtn.TabIndex = 6;
            this.AddFaceBtn.Text = "Добавить лицо";
            this.AddFaceBtn.UseVisualStyleBackColor = true;
            this.AddFaceBtn.Click += new System.EventHandler(this.AddFaceBtn_Click);
            // 
            // ValidFaceBtn
            // 
            this.ValidFaceBtn.Location = new System.Drawing.Point(543, 536);
            this.ValidFaceBtn.Name = "ValidFaceBtn";
            this.ValidFaceBtn.Size = new System.Drawing.Size(507, 51);
            this.ValidFaceBtn.TabIndex = 7;
            this.ValidFaceBtn.Text = "Проверка лица";
            this.ValidFaceBtn.UseVisualStyleBackColor = true;
            this.ValidFaceBtn.Click += new System.EventHandler(this.ValidFaceBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(543, 503);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(507, 27);
            this.textBox1.TabIndex = 8;
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(12, 479);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(507, 51);
            this.TestBtn.TabIndex = 10;
            this.TestBtn.Text = "Тест на качество распознавания";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // AddFaceInPhotoBtn
            // 
            this.AddFaceInPhotoBtn.Location = new System.Drawing.Point(11, 12);
            this.AddFaceInPhotoBtn.Name = "AddFaceInPhotoBtn";
            this.AddFaceInPhotoBtn.Size = new System.Drawing.Size(166, 73);
            this.AddFaceInPhotoBtn.TabIndex = 11;
            this.AddFaceInPhotoBtn.Text = "Добавить лица из фотосета";
            this.AddFaceInPhotoBtn.UseVisualStyleBackColor = true;
            this.AddFaceInPhotoBtn.Click += new System.EventHandler(this.AddFaceInPhotoBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 599);
            this.Controls.Add(this.AddFaceInPhotoBtn);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ValidFaceBtn);
            this.Controls.Add(this.AddFaceBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.StartBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button TestBtn;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button AddFaceBtn;
        private System.Windows.Forms.Button ValidFaceBtn;
        private System.Windows.Forms.Button AddFaceInPhotoBtn;
    }
}

