using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using NAudio.Wave;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Linq;

namespace app_login
{
    public partial class VideoLesson : Window
    {
        private int id_user { get; set; }

        public void setId(int id) { this.id_user = id; }

        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoSource;

        private VideoCaptureDevice videoSourceReceiver;
        
        private bool isSending = false;

        private bool isImage1 = true;

        string destinatarIpAddress = "192.168.0.192";
        int destinatarPort = 8080;

        private WaveInEvent waveIn;
        private BufferedWaveProvider waveProvider;
        private TcpClient otherPeerAudioClient;
        private WaveOut waveOut;

        public VideoLesson()
        {
            InitializeComponent();
            Loaded += MainWindow_Load;
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (otherPeerAudioClient != null && otherPeerAudioClient.Connected)
                {
                    otherPeerAudioClient.GetStream().Write(e.Buffer, 0, e.BytesRecorded);
                }

                waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending audio data: " + ex.Message);
            }
        }

        private void StartMicrophone()
        {
            try
            {
                waveIn = new WaveInEvent();
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1 kHz, mono

                waveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
                waveProvider.BufferDuration = TimeSpan.FromSeconds(10);
                waveProvider.DiscardOnBufferOverflow = true;

                waveOut = new WaveOut();
                waveOut.Init(waveProvider);
                waveOut.Play();

                waveIn.StartRecording();
                Console.WriteLine("Microphone started...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting microphone: " + ex.Message);
            }
        }

        private void StopMicrophone()
        {
            try
            {
                waveIn?.StopRecording();
                waveIn?.Dispose();
                waveIn = null;

                waveOut?.Stop();
                waveOut?.Dispose();
                waveOut = null;

                waveProvider = null;

                if (otherPeerAudioClient != null)
                {
                    otherPeerAudioClient.Close();
                }

                Console.WriteLine("Microphone stopped...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping microphone: " + ex.Message);
            }
        }

        private async Task SendImageToOtherPeerAsync(byte[] imageData)
        {

            try
            {
                using (TcpClient client = new TcpClient(destinatarIpAddress, destinatarPort))
                using (NetworkStream stream = client.GetStream())
                {
                    await stream.WriteAsync(imageData, 0, imageData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la trimiterea datelor: {ex.Message}");
            }
        }

        private async Task<byte[]> ReceiveImageFromOtherPeerAsync()
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            try
            {
                listener.Start();

                TcpClient client = await listener.AcceptTcpClientAsync();
                NetworkStream stream = client.GetStream();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la primirea datelor: {ex.Message}");
                return new byte[0];
            }
            finally
            {
                listener.Stop();
            }
        }

        private async void MainWindow_Load(object sender, RoutedEventArgs e)
        {
            await LoadCameraInfoAsync();
        }

        private async Task LoadCameraInfoAsync()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cboCamera1.Items.Add(filterInfo.Name);
            }
            if (cboCamera1.Items.Count > 0)
            {
                cboCamera1.SelectedIndex = 0;
            }
        }



        private async Task ConnectToOtherPeerAsync(string ipAddress, int port)
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(ipAddress, port);
                var audioPort = 5001; // Alegeți un alt port
                otherPeerAudioClient = new TcpClient();
                await otherPeerAudioClient.ConnectAsync(ipAddress, audioPort);

                Console.WriteLine($"Conectat la {ipAddress}:{port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la conectare: {ex.Message}");
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (videoSource != null && cboCamera1.SelectedIndex >= 0)
            {
                videoSource = new VideoCaptureDevice(filterInfoCollection[cboCamera1.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoCaptureDevice_NewFrame;
                videoSource.Start();

                await ConnectToOtherPeerAsync(destinatarIpAddress, destinatarPort);

                StartSending();
            }
        }


        private async void StartSending()
        {
            while (isSending && videoSource.IsRunning)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pic1.Source));
                    encoder.Save(stream);

                    byte[] imageData = stream.ToArray();
                    await SendImageToOtherPeerAsync(imageData);
                }

                await Task.Delay(50);
            }
        }


        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {
            Dispatcher.Invoke(() => pic1.Source = ToBitmapSource(e.Frame));
        }

        private BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitmapSource;
        }

        private void StopSending()
        {
            isSending = false;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            StopSending();
            Stop();
        }

        private void Mic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (isImage1)
                {
                    mic.Source = new BitmapImage(new Uri("C:\\Users\\Ali Kiwe\\Documents\\GitHub\\Tutoring-and-Mentorship-App\\3_Solution\\Try-outs\\app-login\\app-login\\mic_muted.png", UriKind.Absolute));
                    StopMicrophone();
                }
                else
                {
                    mic.Source = new BitmapImage(new Uri("C:\\Users\\Ali Kiwe\\Documents\\GitHub\\Tutoring-and-Mentorship-App\\3_Solution\\Try-outs\\app-login\\app-login\\mic_unmuted.png", UriKind.Absolute));
                    StartMicrophone();
                }

                // Inverseaza starea
                isImage1 = !isImage1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la gestionarea evenimentului de clic: " + ex.Message);
            }
        }

        private void CloseApp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StopSending();
            Stop();
            StopMicrophone();


            var feedback = new Feedback();
            feedback.setId(this.id_user);
            feedback.Show();
            Close();
        }
    }
}

