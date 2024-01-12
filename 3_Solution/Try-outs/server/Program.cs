using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Server
{
    static List<TcpClient> connectedClients = new List<TcpClient>();
    static object locker = new object();

    static async Task Main()
    {
        int receivePort = 5000;

        TcpListener receiveServer = new TcpListener(IPAddress.Any, receivePort);

        receiveServer.Start();

        Console.WriteLine($"Serverul a pornit pe portul {receivePort} (primire)");

        while (true)
        {
            TcpClient receiveClient = await receiveServer.AcceptTcpClientAsync();

            lock (locker)
            {
                Console.WriteLine("ClientConectat.");
                connectedClients.Add(receiveClient);
            }

            _ = HandleClientAsync(receiveClient);
        }
    }

    static async Task HandleClientAsync(TcpClient receiveClient)
    {
        try
        {
            IPAddress clientIpAddress = ((IPEndPoint)receiveClient.Client.RemoteEndPoint).Address;

            // Crează o nouă conexiune către același client pe portul 5001
            TcpClient sendClient = new TcpClient(clientIpAddress.ToString(), 5001);

            byte[] imageData;
            using (NetworkStream receiveStream = receiveClient.GetStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await receiveStream.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            // Trimite datele înapoi către același client pe portul 5001
            using (NetworkStream sendStream = sendClient.GetStream())
            {
                await sendStream.WriteAsync(imageData, 0, imageData.Length).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la tratarea clientului: {ex.Message}");
        }
    }

}
