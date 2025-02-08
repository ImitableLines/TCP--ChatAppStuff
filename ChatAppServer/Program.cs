using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TCPServer
{
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Server started, waiting for connections...");

        while (true) 
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    
    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] data = new byte[256];
        int bytesRead;

        while (true)
        {
            bytesRead = stream.Read(data, 0, data.Length);
            if (bytesRead == 0) break; 

            string clientMessage = Encoding.UTF8.GetString(data, 0, bytesRead);
            Console.WriteLine("Received from client: " + clientMessage);

            string responseMessage = "Message received successfully.";
            byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Sent to client: " + responseMessage);

            if (clientMessage.ToLower() == "close" || clientMessage.ToLower() == "exit")
            {
                Console.WriteLine("Closing connection with client...");
                break;
            }
        }

        client.Close();
    }
}
