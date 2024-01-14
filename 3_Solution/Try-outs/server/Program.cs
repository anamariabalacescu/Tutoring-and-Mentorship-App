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

            TcpClient sendClient = new TcpClient(clientIpAddress.ToString(), 5001);

            byte[] imageData;
            using (NetworkStream receiveStream = receiveClient.GetStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await receiveStream.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            byte[] imageDataCopy;
            lock (locker)
            {
                imageDataCopy = imageData.ToArray();
            }

            await Task.Run(async () =>
            {
                foreach (var client in connectedClients)
                {
                    if (client != receiveClient)
                    {
                        try
                        {
                            using (NetworkStream sendStream = client.GetStream())
                            {
                                await sendStream.WriteAsync(imageDataCopy, 0, imageDataCopy.Length).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Eroare la trimiterea datelor catre client: {ex.Message}");
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la tratarea clientului: {ex.Message}");
        }
        finally
        {
            lock (locker)
            {
                connectedClients.Remove(receiveClient);
            }
            receiveClient.Close();
        }
    }

}
