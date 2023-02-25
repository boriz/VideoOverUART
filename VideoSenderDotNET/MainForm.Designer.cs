namespace VideoTest
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pb_mono = new System.Windows.Forms.PictureBox();
			this.fd_OpenFile = new System.Windows.Forms.OpenFileDialog();
			this.pb_scaled = new System.Windows.Forms.PictureBox();
			this.pb_src = new System.Windows.Forms.PictureBox();
			this.pb_gray = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btn_OpenFile = new System.Windows.Forms.Button();
			this.btn_Play = new System.Windows.Forms.Button();
			this.btn_Stop = new System.Windows.Forms.Button();
			this.cmb_ComPort = new System.Windows.Forms.ComboBox();
			this.btn_Exit = new System.Windows.Forms.Button();
			this.btn_Pause = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.tb_Log = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pb_mono)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_scaled)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_src)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_gray)).BeginInit();
			this.SuspendLayout();
			// 
			// pb_mono
			// 
			this.pb_mono.Location = new System.Drawing.Point(518, 318);
			this.pb_mono.Name = "pb_mono";
			this.pb_mono.Size = new System.Drawing.Size(128, 64);
			this.pb_mono.TabIndex = 0;
			this.pb_mono.TabStop = false;
			// 
			// fd_OpenFile
			// 
			this.fd_OpenFile.FileName = "openFileDialog1";
			this.fd_OpenFile.Filter = "MP4 Files(*.mp4)|*.mp4|AVI Files (*.avi)|*.avi";
			this.fd_OpenFile.Title = "Open Video";
			// 
			// pb_scaled
			// 
			this.pb_scaled.Location = new System.Drawing.Point(518, 22);
			this.pb_scaled.Name = "pb_scaled";
			this.pb_scaled.Size = new System.Drawing.Size(128, 64);
			this.pb_scaled.TabIndex = 1;
			this.pb_scaled.TabStop = false;
			// 
			// pb_src
			// 
			this.pb_src.Location = new System.Drawing.Point(13, 22);
			this.pb_src.Name = "pb_src";
			this.pb_src.Size = new System.Drawing.Size(480, 360);
			this.pb_src.TabIndex = 2;
			this.pb_src.TabStop = false;
			// 
			// pb_gray
			// 
			this.pb_gray.Location = new System.Drawing.Point(518, 165);
			this.pb_gray.Name = "pb_gray";
			this.pb_gray.Size = new System.Drawing.Size(128, 64);
			this.pb_gray.TabIndex = 3;
			this.pb_gray.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Source";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(515, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Resized";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(515, 149);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Grayscale";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(515, 302);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Monochrome";
			// 
			// btn_OpenFile
			// 
			this.btn_OpenFile.Location = new System.Drawing.Point(682, 22);
			this.btn_OpenFile.Name = "btn_OpenFile";
			this.btn_OpenFile.Size = new System.Drawing.Size(75, 23);
			this.btn_OpenFile.TabIndex = 8;
			this.btn_OpenFile.Text = "Open File";
			this.btn_OpenFile.UseVisualStyleBackColor = true;
			this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
			// 
			// btn_Play
			// 
			this.btn_Play.Location = new System.Drawing.Point(682, 139);
			this.btn_Play.Name = "btn_Play";
			this.btn_Play.Size = new System.Drawing.Size(75, 23);
			this.btn_Play.TabIndex = 9;
			this.btn_Play.Text = "Play";
			this.btn_Play.UseVisualStyleBackColor = true;
			this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
			// 
			// btn_Stop
			// 
			this.btn_Stop.Location = new System.Drawing.Point(682, 179);
			this.btn_Stop.Name = "btn_Stop";
			this.btn_Stop.Size = new System.Drawing.Size(75, 23);
			this.btn_Stop.TabIndex = 10;
			this.btn_Stop.Text = "Stop";
			this.btn_Stop.UseVisualStyleBackColor = true;
			this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
			// 
			// cmb_ComPort
			// 
			this.cmb_ComPort.FormattingEnabled = true;
			this.cmb_ComPort.Location = new System.Drawing.Point(682, 83);
			this.cmb_ComPort.Name = "cmb_ComPort";
			this.cmb_ComPort.Size = new System.Drawing.Size(158, 21);
			this.cmb_ComPort.TabIndex = 11;
			// 
			// btn_Exit
			// 
			this.btn_Exit.Location = new System.Drawing.Point(765, 365);
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.Size = new System.Drawing.Size(75, 23);
			this.btn_Exit.TabIndex = 13;
			this.btn_Exit.Text = "Exit";
			this.btn_Exit.UseVisualStyleBackColor = true;
			this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
			// 
			// btn_Pause
			// 
			this.btn_Pause.Location = new System.Drawing.Point(765, 139);
			this.btn_Pause.Name = "btn_Pause";
			this.btn_Pause.Size = new System.Drawing.Size(75, 23);
			this.btn_Pause.TabIndex = 14;
			this.btn_Pause.Text = "Pause";
			this.btn_Pause.UseVisualStyleBackColor = true;
			this.btn_Pause.Click += new System.EventHandler(this.btn_Pause_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(682, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 13);
			this.label5.TabIndex = 15;
			this.label5.Text = "COM Port";
			// 
			// tb_Log
			// 
			this.tb_Log.Location = new System.Drawing.Point(685, 221);
			this.tb_Log.Multiline = true;
			this.tb_Log.Name = "tb_Log";
			this.tb_Log.ReadOnly = true;
			this.tb_Log.Size = new System.Drawing.Size(155, 113);
			this.tb_Log.TabIndex = 16;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(883, 400);
			this.ControlBox = false;
			this.Controls.Add(this.tb_Log);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btn_Pause);
			this.Controls.Add(this.btn_Exit);
			this.Controls.Add(this.cmb_ComPort);
			this.Controls.Add(this.btn_Stop);
			this.Controls.Add(this.btn_Play);
			this.Controls.Add(this.btn_OpenFile);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pb_gray);
			this.Controls.Add(this.pb_src);
			this.Controls.Add(this.pb_scaled);
			this.Controls.Add(this.pb_mono);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Video Test";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pb_mono)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_scaled)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_src)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pb_gray)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pb_mono;
		private System.Windows.Forms.OpenFileDialog fd_OpenFile;
		private System.Windows.Forms.PictureBox pb_scaled;
		private System.Windows.Forms.PictureBox pb_src;
		private System.Windows.Forms.PictureBox pb_gray;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btn_OpenFile;
		private System.Windows.Forms.Button btn_Play;
		private System.Windows.Forms.Button btn_Stop;
		private System.Windows.Forms.ComboBox cmb_ComPort;
		private System.Windows.Forms.Button btn_Exit;
		private System.Windows.Forms.Button btn_Pause;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tb_Log;
	}
}

