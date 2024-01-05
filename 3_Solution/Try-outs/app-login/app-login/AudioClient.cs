using NAudio.Wave;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace app_login
{
    internal class AudioClient
    {
        private TcpClient tcpClient;
        private WaveInEvent waveIn;
        private WaveOut waveOut;
        private BufferedWaveProvider waveProvider;

        public async Task StartClientAsync(string serverIpAddress, int serverPort)
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(serverIpAddress, serverPort);

            // Inițializare pentru trimitere
            waveIn = new WaveInEvent();
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.WaveFormat = new WaveFormat(44100, 1); // Mono, 44.1 kHz
            waveIn.StartRecording();

            // Inițializare pentru recepție
            waveOut = new WaveOut();
            waveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1)); // Mono, 44.1 kHz
            waveOut.Init(waveProvider);
            waveOut.Play();

            // Pornirea unei bucle de ascultare pentru recepție
            _ = ListenForAudioAsync();

            Console.WriteLine("Audio client started. Press Enter to exit.");
            Console.ReadLine();

            // Oprirea înregistrării și a recepției la ieșirea din aplicație
            waveIn.StopRecording();
            waveOut.Stop();
            tcpClient.Close();
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.Write(e.Buffer, 0, e.BytesRecorded);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending audio data: {ex.Message}");
            }
        }

        private async Task ListenForAudioAsync()
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        break; // Serverul s-a deconectat
                    }

                    waveProvider.AddSamples(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listening for audio: {ex.Message}");
            }
        }
    }
}

