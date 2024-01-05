using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    private readonly List<TcpClient> clients = new List<TcpClient>();
    private TcpListener tcpListener;

    static async Task Main(string[] args)
    {
        Program server = new Program();
        await server.StartServerAsync(5000); 
    }

    public async Task StartServerAsync(int port)
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();

        Console.WriteLine($"Server started on port {port}");

        while (true)
        {
            TcpClient client = await tcpListener.AcceptTcpClientAsync();
            clients.Add(client);
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    break;
                }

                BroadcastAudioData(buffer, bytesRead, client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            clients.Remove(client);
            client.Close();
        }
    }

    private void BroadcastAudioData(byte[] audioData, int length, TcpClient senderClient)
    {
        foreach (TcpClient client in clients)
        {
            if (client != senderClient)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(audioData, 0, length);
            }
        }
    }
}
