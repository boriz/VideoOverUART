using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace VideoTest
{
	public partial class MainForm : Form
	{

		private Thread _video_thread;
		private Thread _com_thread;
		private ConcurrentQueue<Mat> _bmp_queue = new ConcurrentQueue<Mat>();
		private string _file = @"C:\Boris\Projects\VideoTest\test.mp4";
		private volatile bool _exit_threads = false;


		public MainForm()
		{
			InitializeComponent();
		}


		private void MainForm_Load(object sender, EventArgs e)
		{
			string[] ports = SerialPort.GetPortNames();
			cmb_ComPort.Items.Clear();
			cmb_ComPort.Items.AddRange(ports);
		}


		private void VideoThread()
		{
			var cap = new VideoCapture();
			cap.Open(_file);
			//cap.Open(0);

			var image = new Mat();
			var image_resize = new Mat();
			var image_gray = new Mat();
			var image_mono = new Mat();
			Bitmap bmp;
			int del_ms = (int) (1000/cap.Fps);
			DateTime frame_start_time;

			while (!_exit_threads)
			{
				frame_start_time = DateTime.Now;

				cap.Read(image);
				if (image.Empty())
				{
					continue;
				}

				bmp = BitmapConverter.ToBitmap(image);
				pb_src.Image = bmp;

				image_resize = image.Resize(new OpenCvSharp.Size(128, 64));
				bmp = BitmapConverter.ToBitmap(image_resize);
				pb_scaled.Image = bmp;

				image_gray = image_resize.CvtColor(ColorConversionCodes.RGB2GRAY);
				bmp = BitmapConverter.ToBitmap(image_gray);
				pb_gray.Image = bmp;

				double th = image_gray.Mean().Val0;
				image_mono = image_gray.Threshold(th, 255, ThresholdTypes.Binary);
				// Copy and send the image to serial port thread
				Mat mat_copy = image_mono.Clone();
				_bmp_queue.Enqueue(mat_copy);	
				bmp = BitmapConverter.ToBitmap(image_mono);
				pb_mono.Image = bmp;

				var elapsed_time = DateTime.Now - frame_start_time;
				int remaining_time = del_ms - (int)elapsed_time.TotalMilliseconds;
				if (remaining_time > 0)
				{
					Thread.Sleep(remaining_time);
				}
			}
		}


		[DllImport("FastSerial.dll")]
		static extern int OpenPort(int port_number);

		[DllImport("FastSerial.dll")]
		static extern int SendArray(byte[] byte_array, int size_bytes);

		[DllImport("FastSerial.dll")]
		static extern int ClosePort();


		private void ComThread()
		{
			string com_port = "";
			this.Invoke(new MethodInvoker(delegate () { com_port = cmb_ComPort.Text; }));

			Mat mat_image;
			//OpenPort(10);
			SerialPort ser_port = new SerialPort(com_port, 921600, Parity.None, 8, StopBits.One);
			ser_port.Handshake = Handshake.None;
			ser_port.DtrEnable = true;
			ser_port.Open();

			while (!_exit_threads)
			{
				if (!_bmp_queue.TryDequeue(out mat_image))
				{
					//Thread.Sleep(1);
					continue;
				}

				if (_bmp_queue.Count > 5)
				{
					continue;
				}

				// Got something from the queue

				// Start frame command
				int send_array_size = 10 + ((mat_image.Height * mat_image.Width) / 8) * 2;
				byte[] send_array = new byte[send_array_size];
				int send_index = 0;
				Stopwatch sw_send = new Stopwatch();
				Stopwatch sw_frame = new Stopwatch();
				send_array[send_index++] = 0x69;
				send_array[send_index++] = 0x01;

				// Parse bmp
				sw_frame.Restart();
				for (int y_byte_offset = 0; y_byte_offset < mat_image.Height; y_byte_offset += 8)
				{
					for (int x = 0; x < mat_image.Width; x++)
					{
						byte message_byte = 0;
						for (int bit_index = 0; bit_index < 8; bit_index++)
						{
							int y = y_byte_offset + bit_index;
							if (mat_image.At<bool>(y, x))
							{
								// White pixel - set bit
								message_byte |= (byte)(0x01 << bit_index);
							}
						}
						// Send byte
						// Check if it is a special case
						if (message_byte == 0x69)
						{
							// Send 0x69 0x69 if the data byte suposed to be 69
							send_array[send_index++] = 0x69;
						}
						send_array[send_index++] = message_byte;
					}
				}
				sw_send.Restart();
				//SendArray(send_array, send_index);
				ser_port.Write(send_array, 0, send_index);
				sw_send.Stop();
				sw_frame.Stop();
				Debug.WriteLine("Frame time: {0}ms; Send time {1}ms", sw_frame.ElapsedMilliseconds, sw_send.ElapsedMilliseconds);
				//Thread.Sleep(1);
			}
			// Exit thread
			ser_port.Close();
			//ClosePort();
		}


		private void btn_OpenFile_Click(object sender, EventArgs e)
		{
			var res = fd_OpenFile.ShowDialog();
			if (res == DialogResult.OK)
			{
				_file = fd_OpenFile.FileName;
			}			
		}


		private void btn_Stop_Click(object sender, EventArgs e)
		{
			_exit_threads = true;

			if (_video_thread != null)
			{
				_video_thread.Join();
			}
			_video_thread = null;

			if (_com_thread != null)
			{
				_com_thread.Join();
			}
			_com_thread = null;
		}


		private void btn_Play_Click(object sender, EventArgs e)
		{
			_exit_threads = false;

			if (_video_thread == null)
			{
				_video_thread = new Thread(new ThreadStart(VideoThread));
				_video_thread.Start();
			}

			if (_com_thread == null)
			{
				_com_thread = new Thread(new ThreadStart(ComThread));
				_com_thread.Start();
			}
		}


		private void btn_Exit_Click(object sender, EventArgs e)
		{
			_exit_threads = true;

			if (_video_thread != null)
			{
				_video_thread.Join();
			}

			if (_com_thread != null)
			{
				_com_thread.Join();
			}

			this.Close();
		}

		private void btn_Pause_Click(object sender, EventArgs e)
		{

		}
	}
}
