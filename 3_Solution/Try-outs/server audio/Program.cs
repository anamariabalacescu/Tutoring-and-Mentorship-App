using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class ProgramAudio
{
    private readonly List<TcpClient> clients = new List<TcpClient>();
    private TcpListener tcpListener;

    private const int audioPort = 5001;

    static async Task Main(string[] args)
    {
        ProgramAudio server = new ProgramAudio();
        await server.StartServerAsync(audioPort);
    }

    public async Task StartServerAsync(int audioPort)
    {
        tcpListener = new TcpListener(IPAddress.Any, audioPort);

        tcpListener.Start();

        Console.WriteLine($"Server started on audio port {audioPort}");

        while (true)
        {
            TcpClient audioClient = await tcpListener.AcceptTcpClientAsync();

            clients.Add(audioClient);

            Console.WriteLine("Un client s-a conectat");
            _ = HandleClientAsync(audioClient);

        }
    }

    private async Task HandleClientAsync(TcpClient audioClient)
    {
        try
        {
            NetworkStream audioStream = audioClient.GetStream();

            byte[] audioBuffer = new byte[1024];

            while (true)
            {
                int audioBytesRead = await audioStream.ReadAsync(audioBuffer, 0, audioBuffer.Length);

                if (audioBytesRead == 0)
                {
                    break;
                }

                BroadcastAudioData(audioBuffer, audioBytesRead, audioClient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            clients.Remove(audioClient);
            audioClient.Close();
        }
    }

    private void BroadcastAudioData(byte[] audioData, int length, TcpClient senderClient)
    {
        foreach (TcpClient client in clients)
        {
            //if (client != senderClient)
            //{
            NetworkStream stream = client.GetStream();
            stream.Write(audioData, 0, length);
            //}
        }
    }
}
