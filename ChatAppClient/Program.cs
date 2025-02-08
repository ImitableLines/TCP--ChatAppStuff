using System.Net.Sockets;
using System.Text;

namespace ChatAppClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();

            while (true) {
                Console.Write("Write Message: ");
                string messageToSend = Console.ReadLine();
                byte[] messageData = Encoding.UTF8.GetBytes(messageToSend);

                stream.Write(messageData, 0, messageData.Length);

                byte[] responseData = new byte[256];
                int bytesRead = stream.Read(responseData, 0, responseData.Length);
                string serverResponse = Encoding.UTF8.GetString(responseData, 0, bytesRead);
                Console.WriteLine("Received");
            }

            client.Close();
        }
    }
}
