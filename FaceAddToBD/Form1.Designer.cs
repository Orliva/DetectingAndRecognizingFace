namespace FaceAddToBD
{
    partial class Form1
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
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(2, 464);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(262, 40);
            this.StartBtn.TabIndex = 3;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(270, 464);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(291, 40);
            this.PauseBtn.TabIndex = 4;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(567, 464);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(262, 40);
            this.StopBtn.TabIndex = 5;
            this.StopBtn.Text = "Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // AddFaceBtn
            // 
            this.AddFaceBtn.Location = new System.Drawing.Point(846, 450);
            this.AddFaceBtn.Name = "AddFaceBtn";
            this.AddFaceBtn.Size = new System.Drawing.Size(204, 53);
            this.AddFaceBtn.TabIndex = 6;
            this.AddFaceBtn.Text = "Добавить лицо";
            this.AddFaceBtn.UseVisualStyleBackColor = true;
            this.AddFaceBtn.Click += new System.EventHandler(this.AddFaceBtn_Click);
            // 
            // ValidFaceBtn
            // 
            this.ValidFaceBtn.Location = new System.Drawing.Point(789, 360);
            this.ValidFaceBtn.Name = "ValidFaceBtn";
            this.ValidFaceBtn.Size = new System.Drawing.Size(261, 71);
            this.ValidFaceBtn.TabIndex = 7;
            this.ValidFaceBtn.Text = "Проверка лица";
            this.ValidFaceBtn.UseVisualStyleBackColor = true;
            this.ValidFaceBtn.Click += new System.EventHandler(this.ValidFaceBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(391, 431);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(424, 27);
            this.textBox1.TabIndex = 8;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(792, 12);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(258, 45);
            this.DeleteBtn.TabIndex = 9;
            this.DeleteBtn.Text = "Удалить базу лиц";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(425, 369);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(358, 56);
            this.trackBar1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 516);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ValidFaceBtn);
            this.Controls.Add(this.AddFaceBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.StartBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button AddFaceBtn;
        private System.Windows.Forms.Button ValidFaceBtn;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button DeleteBtn;
        public System.Windows.Forms.TrackBar trackBar1;
    }
}

