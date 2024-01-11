using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using NAudio.Wave;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Windows.Media;
using System.Text;

namespace app_login
{
    public partial class VideoLesson : Window
    {
        private int id_user { get; set; }

        public void setId(int id) { this.id_user = id; }

        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoSource;
        
        private bool isSending = false;
        private bool isMuted = false;
        private bool isImage1 = true;

        string destinatarIpAddress = "192.168.79.42";
        private int destinatarPortSend = 5000;
        private int destinatarPortReceive = 5001;

        private int destinatarAudioPortSend = 8000;
        private int destinatarAudioPortReceive = 8001;

        private WaveInEvent waveIn;
        private BufferedWaveProvider waveProvider;
        private TcpClient otherPeerAudioClient;
        private WaveOut waveOut;

        public VideoLesson()
        {
            InitializeComponent();
            Loaded += MainWindow_Load;
            Closed += MainWindow_Closed;
            StartCommunication();
        }

        private async Task SendImageToServerAsync(byte[] imageData, int serverPort)
        {
            await Task.Run(async () =>
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        await client.ConnectAsync(destinatarIpAddress, serverPort);

                        using (NetworkStream stream = client.GetStream())
                        {
                            await stream.WriteAsync(imageData, 0, imageData.Length);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare la trimiterea datelor imaginii la server: {ex.Message}");
                }
            });
        }

        private async Task<byte[]> ReceiveImageFromServerAsync(int serverPort)
        {
            return await Task.Run(async () =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, serverPort);

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
                    Console.WriteLine($"Eroare la primirea datelor imaginii de la server: {ex.Message}");
                    return new byte[0];
                }
                finally
                {
                    listener.Stop();
                }
            });
        }

        private async void StartCommunication()
        {
            await Task.Delay(5000);
            while (true)
            {
                if (pic1.Source != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pic1.Source));
                        encoder.Save(stream);

                        byte[] imageData = stream.ToArray();

                        await SendImageToServerAsync(imageData, destinatarPortSend);
                    }
                }

                byte[] receivedImageData = await ReceiveImageFromServerAsync(destinatarPortReceive);
                if (receivedImageData.Length > 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        BitmapImage bitmapImage = ConvertToBitmapImage(receivedImageData);
                        pic2.Source = bitmapImage;
                    });
                }
            }
        }

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
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

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cboCamera1.SelectedIndex >= 0)
            {
                videoSource = new VideoCaptureDevice(filterInfoCollection[cboCamera1.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoCaptureDevice_NewFrame;
                videoSource.Start();
            }
        }

        public static ImageSource BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            var memory = new System.IO.MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                pic1.Source = BitmapToImageSource((System.Drawing.Bitmap)e.Frame.Clone());

            }); 
        }

        private void VideoCaptureDeviceReceiver_NewFrame(object sender, NewFrameEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                pic2.Source = BitmapToImageSource((System.Drawing.Bitmap)e.Frame.Clone());
            });
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
            pic1.Source = null;
        }

        private void Mic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (isImage1)
                {
                    isMuted = false;
                    mic.Source = new BitmapImage(new Uri("C:\\Users\\Ali Kiwe\\Documents\\GitHub\\Tutoring-and-Mentorship-App\\3_Solution\\Try-outs\\app-login\\app-login\\mic_muted.png", UriKind.Absolute));
                    StopMicrophone();
                }
                else
                {
                    isMuted = true;
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
    }
}

