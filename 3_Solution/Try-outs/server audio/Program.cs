using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class ProgramAudio
{
    static async Task Main()
    {
        int audioReceivePort = 8000;

        TcpListener audioReceiveServer = new TcpListener(IPAddress.Any, audioReceivePort);

        audioReceiveServer.Start();

        Console.WriteLine($"Serverul audio a pornit pe portul {audioReceivePort}");

        while (true)
        {
            TcpClient audioReceiveClient = await audioReceiveServer.AcceptTcpClientAsync();

            Console.WriteLine("Un client s-a conectat pentru audio");

            _ = HandleAudioClientAsync(audioReceiveClient);
        }
    }

    static async Task HandleAudioClientAsync(TcpClient audioReceiveClient)
    {
        try
        {
            // Obține adresa IP a clientului pentru audio
            IPAddress clientIpAddress = ((IPEndPoint)audioReceiveClient.Client.RemoteEndPoint).Address;

            // Creează un TcpClient pentru trimiterea datelor audio la același client
            TcpClient audioSendClient = new TcpClient(clientIpAddress.ToString(), 8001);

            using (NetworkStream audioReceiveStream = audioReceiveClient.GetStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Primește datele audio de la clientul pentru audio
                await audioReceiveStream.CopyToAsync(memoryStream);
                Console.WriteLine("Date audio primite");

                byte[] audioData = memoryStream.ToArray();

                // Trimite datele audio către clientul pentru trimitere audio
                using (NetworkStream audioSendStream = audioSendClient.GetStream())
                {
                    await audioSendStream.WriteAsync(audioData, 0, audioData.Length);
                    Console.WriteLine("Date audio trimise");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la tratarea clientului audio: {ex.Message}");
        }
    }
}
