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

        while (true) // Continuous loop to accept multiple client connections
        {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Create a new thread to handle this client
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    // Method to handle communication with each client
    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] data = new byte[256];
        int bytesRead;

        while (true)
        {
            bytesRead = stream.Read(data, 0, data.Length);
            if (bytesRead == 0) break; // Client disconnected

            string clientMessage = Encoding.UTF8.GetString(data, 0, bytesRead);
            Console.WriteLine("Received from client: " + clientMessage);

            // Send a response to the client
            string responseMessage = "Message received successfully.";
            byte[] responseData = Encoding.UTF8.GetBytes(responseMessage);
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Sent to client: " + responseMessage);

            // Check for close or exit command
            if (clientMessage.ToLower() == "close" || clientMessage.ToLower() == "exit")
            {
                Console.WriteLine("Closing connection with client...");
                break;
            }
        }

        // Close the connection when done
        client.Close();
    }
}
