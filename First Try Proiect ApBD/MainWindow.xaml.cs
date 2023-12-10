using System;
using System.Windows;
using System.Windows.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Net;
using NAudio.Wave;

namespace First_Try_Proiect_ApBD
{
    public partial class MainWindow : Window
    {
        private TcpListener tcpListener;
        private TcpClient peerClient;
        private NetworkStream stream;
        private bool isSending = false;
        private bool isReceiving = false;
        FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice videoCaptureDevice;
        private Task receivingTask;
        private Task sendingTask;
        private TcpClient otherPeerClient;
        private bool isMicrophoneMuted = true;
        private WaveInEvent waveIn;
        private TcpClient otherPeerAudioClient;

        private string otherPeerIpAddress = "10.10.23.240"; // Adresa IP a celuilalt peer
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Load;
            Closed += MainWindow_Closed;
            _ = ListenForConnectionsAsync();
        }

        private BufferedWaveProvider waveProvider;
        private WaveOut waveOut;
        
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                // Trimiteți datele audio către celălalt client pe fluxul de rețea
                if (otherPeerClient != null && otherPeerClient.Connected)
                {
                    otherPeerClient.GetStream().Write(e.Buffer, 0, e.BytesRecorded);
                }

                // Adăugați datele audio la BufferedWaveProvider pentru redare locală
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

                // Închideți conexiunea și eliberați resursele pentru datele audio
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


        private async Task ListenForConnectionsAsync()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 5000);
                tcpListener.Start();

                peerClient = await tcpListener.AcceptTcpClientAsync();
                stream = peerClient.GetStream();

                StartReceiving(); // Începe să primești date de la peer
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la ascultarea conexiunilor: " + e.Message);
            }
        }

        private async Task ReceiveImageFromPeerAsync()
        {
            try
            {
                while (isReceiving)
                {
                    byte[] imageLengthBytes = new byte[sizeof(int)];
                    int bytesRead = await stream.ReadAsync(imageLengthBytes);

                    if (bytesRead == 0)
                    {
                        // No data received; the peer is no longer sending
                        continue;
                    }

                    int imageLength = BitConverter.ToInt32(imageLengthBytes, 0);
                    byte[] imageData = new byte[imageLength];
                    bytesRead = await stream.ReadAsync(imageData.AsMemory(0, imageLength));

                    if (bytesRead > 0)
                    {
                        // Use Dispatcher to update UI on the main thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            using MemoryStream memoryStream = new MemoryStream(imageData);
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();

                            // Update pic2 with the received image
                            pic2.Source = bitmapImage;
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }


        private void StartReceiving()
        {
            if (!isReceiving)
            {
                isReceiving = true;
                Task.Run(ReceiveImageFromPeerAsync);
            }
        }
        private void StopReceiving()
        {
            isReceiving = false;

            // Închide conexiunea și eliberează resursele pentru datele video
            if (peerClient != null)
            {
                stream?.Close();
                peerClient.Close();
            }

            // Închide conexiunea și eliberează resursele pentru datele audio
            if (otherPeerAudioClient != null)
            {
                otherPeerAudioClient.Close();
            }
        }

        private async Task SendImageToOtherPeerAsync(BitmapSource bitmapSource)
        {
            try
            {
                JpegBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using MemoryStream memoryStream = new();
                encoder.Save(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                byte[] imageLengthBytes = BitConverter.GetBytes(imageData.Length);
                await otherPeerClient.GetStream().WriteAsync(imageLengthBytes);

                await otherPeerClient.GetStream().WriteAsync(imageData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la trimiterea imaginii către celălalt peer: " + e.Message);
            }
        }


        private async Task ReceiveImageFromOtherPeerAsync()
        {
            try
            {
                while (isReceiving)
                {
                    byte[] imageLengthBytes = new byte[sizeof(int)];
                    await otherPeerClient.GetStream().ReadAsync(imageLengthBytes);

                    int imageLength = BitConverter.ToInt32(imageLengthBytes, 0);
                    byte[] imageData = new byte[imageLength];
                    await otherPeerClient.GetStream().ReadAsync(imageData.AsMemory(0, imageLength));

                    using MemoryStream memoryStream = new MemoryStream(imageData);
                    BitmapImage bitmapImage = new();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();

                    // Actualizează pic2 cu imaginea primită de la celălalt peer
                    pic2.Source = bitmapImage;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la primirea imaginii de la celălalt peer: " + e.Message);
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
                otherPeerClient = new TcpClient();
                await otherPeerClient.ConnectAsync(ipAddress, port);

                // Deschideți un al doilea socket pentru datele audio
                var audioPort = 5001; // Alegeți un alt port
                otherPeerAudioClient = new TcpClient();
                await otherPeerAudioClient.ConnectAsync(ipAddress, audioPort);

                isSending = true;
                isReceiving = true;
                _ = ReceiveImageFromOtherPeerAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la conectarea la celălalt peer: " + e.Message);
            }
        }

        private async Task ReceiveLoop()
        {
            try
            {
                while (isReceiving)
                {
                    await ReceiveImageFromPeerAsync();
                    await Task.Delay(10); // O mică întârziere pentru a nu supraîncărca procesorul
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la citire: " + e.Message);
            }
        }

        private async Task SendLoop()
        {
            try
            {
                while (isSending)
                {
                    if (pic1.Source != null)
                    {
                        await SendImageToOtherPeerAsync(pic1.Source as BitmapSource);
                    }

                    await Task.Delay(10);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la scriere: " + e.Message);
            }
        }

        private void StartConnectionToOtherPeer()
        {
            int otherPeerPort = 5000; // Portul la care ascultă celălalt peer

            _ = ConnectToOtherPeerAsync(otherPeerIpAddress, otherPeerPort);

            receivingTask = Task.Run(ReceiveLoop);
            sendingTask = Task.Run(SendLoop);
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cboCamera1.SelectedIndex >= 0)
            {
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera1.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();
                StartConnectionToOtherPeer();
                StartSending();
            }
        }
        private void StartSending()
        {

            if (pic1.Source != null)
            {
                isSending = true;
                Task.Run(SendImageToPeerAsync);
            }
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                pic1.Source = Helper.BitmapToImageSource((System.Drawing.Bitmap)e.Frame.Clone());
                if (isSending)
                {
                    SendImageToPeerAsync();
                }
            });
        }
        private async void SendImageToPeerAsync()
        {
            if (otherPeerClient != null && otherPeerClient.Connected)
            {
                await SendImageToOtherPeerAsync(pic1.Source as BitmapSource);
            }
        }


        private void StopSending()
        {
            isSending = false;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            StopReceiving();
            StopSending();
            if (Application.Current.Windows.OfType<Feedback>().Any() == false)
            {
                Feedback feedbackWindow = new();
                feedbackWindow.Show();
            }
        }
        

        private void Stop(object sender, RoutedEventArgs e)
        {
            videoCaptureDevice.SignalToStop();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            videoCaptureDevice.SignalToStop();
            videoCaptureDevice.WaitForStop();
            videoCaptureDevice.Stop();
            StopSending();
            pic1.Source = null;
            //pic2.Source = null;
        }


        private bool isImage1 = true;

        private void Mic_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (isImage1)
                {
                    mic.Source = new BitmapImage(new Uri("F:\\C Copy\\source\\repos\\ApBD\\First Try Proiect ApBD\\mic_muted.png", UriKind.Absolute));
                    StopMicrophone();
                }
                else
                {
                    mic.Source = new BitmapImage(new Uri("F:\\C Copy\\source\\repos\\ApBD\\First Try Proiect ApBD\\mic_unmuted.png", UriKind.Absolute));
                    StartMicrophone();
                }

                // Inversează starea
                isImage1 = !isImage1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la gestionarea evenimentului de clic: " + ex.Message);
            }
        }

    }
}