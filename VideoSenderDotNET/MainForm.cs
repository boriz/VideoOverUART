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
		private string _file = @"..\..\..\test.mp4";
		private volatile bool _exit_threads = false;
		private volatile bool _pause = false;

		// Variables to calculate statistic
		private volatile float _fps_requested = 0;
		private volatile int _fps_capture = 0;
		private volatile int _fps_send = 0;
		private volatile int _frame_missed = 0;


		public MainForm()
		{
			InitializeComponent();
		}


		private void MainForm_Load(object sender, EventArgs e)
		{
			string[] ports = SerialPort.GetPortNames();
			cmb_ComPort.Items.Clear();
			cmb_ComPort.Items.AddRange(ports);
			btn_Pause.Enabled = false;
			btn_Stop.Enabled = false;

			// Start statstic timer
			tmr_Stat.Enabled = true;
		}


		private void VideoThread()
		{
			var cap = new VideoCapture();
			if (chk_Capture.Checked)
			{
				// Open default video capture device
				cap.Open(0);
			}
			else
			{
				cap.Open(_file);
			}

			var image = new Mat();
			var image_resize = new Mat();
			var image_gray = new Mat();
			var image_mono = new Mat();
			Bitmap bmp;
			_fps_requested = (float) cap.Fps;
			float frame_time_requested = 1000/ _fps_requested;
			Stopwatch sw_frame_time = new Stopwatch();
			Stopwatch sw_delay = new Stopwatch();

			while (!_exit_threads)
			{
				// Get frame start time
				sw_frame_time.Restart();

				if (_pause)
				{
					// Playback is paused. Don't do anything
					continue;
				}

				// Read new image from capture device
				cap.Read(image);
				if (image.Empty())
				{
					continue;
				}

				// Convert image to bitmap
				bmp = BitmapConverter.ToBitmap(image);
				pb_src.Image = bmp;

				// Resize image to 128x64
				image_resize = image.Resize(new OpenCvSharp.Size(128, 64));
				bmp = BitmapConverter.ToBitmap(image_resize);
				pb_scaled.Image = bmp;

				// Turn resied image to grayscale
				image_gray = image_resize.CvtColor(ColorConversionCodes.RGB2GRAY);
				bmp = BitmapConverter.ToBitmap(image_gray);
				pb_gray.Image = bmp;

				// Apply thershold to convert grayscale to monochome 
				double th = image_gray.Mean().Val0;
				image_mono = image_gray.Threshold(th, 255, ThresholdTypes.Binary);

				// Copy and send the image to serial port thread
				Mat mat_copy = image_mono.Clone();
				_bmp_queue.Enqueue(mat_copy);	
				bmp = BitmapConverter.ToBitmap(image_mono);
				pb_mono.Image = bmp;

				// Update statistic
				_fps_capture++;

				// Calacualte how long we should pause before starting the next frame
				sw_delay.Restart();
				long elapsed_ticks = sw_frame_time.ElapsedTicks;
				float elapsed_time_ms = 1000 * elapsed_ticks / Stopwatch.Frequency;
				float remaining_time_ms = frame_time_requested - elapsed_time_ms;
				long delay_ticks = (long) ((remaining_time_ms * Stopwatch.Frequency) / 1000);
				while (sw_delay.ElapsedTicks < delay_ticks) ;
			}
		}


		private void ComThread()
		{
			string com_port = "";
			this.Invoke(new MethodInvoker(delegate () { com_port = cmb_ComPort.Text; }));

			// Open serial port
			Mat mat_image;
			SerialPort ser_port = new SerialPort(com_port, 460800, Parity.None, 8, StopBits.One);
			ser_port.Handshake = Handshake.None;
			ser_port.DtrEnable = true;
			ser_port.Open();

			// Stopwatches for statistic
			Stopwatch sw_send = new Stopwatch();
			Stopwatch sw_frame = new Stopwatch();

			while (!_exit_threads)
			{
				if (!_bmp_queue.TryDequeue(out mat_image))
				{
					// Nothing to dequeue, continue
					continue;
				}

				if (_bmp_queue.Count > 5)
				{
					// Too many items in the queue, drop this frame
					_frame_missed++;
					continue;
				}

				// Got something from the queue

				// Start frame command
				int send_array_size = 10 + ((mat_image.Height * mat_image.Width) / 8) * 2;
				byte[] send_array = new byte[send_array_size];
				int send_index = 0;

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
				ser_port.Write(send_array, 0, send_index);
				sw_send.Stop();
				sw_frame.Stop();
				// Debug.WriteLine("Frame time: {0}ms; Send time {1}ms", sw_frame.ElapsedMilliseconds, sw_send.ElapsedMilliseconds);

				// Update statistic
				_fps_send++;
			}
			// Exit thread
			ser_port.Close();
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
			// Enable play button
			btn_Pause.Enabled = false;
			btn_Stop.Enabled = false;
			btn_Play.Enabled = true;

			// Unpause 
			_pause = false;

			// Flag to exit from threads
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
			// Enable stop and pause video buttons
			btn_Pause.Enabled = true;
			btn_Stop.Enabled = true;
			btn_Play.Enabled = false;

			// Are we just paused?
			if (_pause)
			{
				_pause = false;
				return;
			}

			// Start threads
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
			// Enable play button
			btn_Play.Enabled = true;
			btn_Pause.Enabled = false;

			_pause = true;
		}


		private void tmr_Stat_Tick(object sender, EventArgs e)
		{
			// Format statistic and upade textbox
			tb_Log.Text = String.Format("File: {0}" + Environment.NewLine + Environment.NewLine +
				"Target FPS: {1:0.00}" + Environment.NewLine +
				"FPS capture: {2}" + Environment.NewLine + 
				"FPS send: {3}" + Environment.NewLine + 
				"Missed frames: {4}", _file, _fps_requested, _fps_capture, _fps_send, _frame_missed);

			// Restart fps calculation
			_fps_capture = 0;
			_fps_send = 0;
		}
	}
}
