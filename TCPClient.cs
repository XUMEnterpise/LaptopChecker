using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    internal class TCPClient
    {
        // Define a delegate for the event

        public void SendPacket(Laptop laptop)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient("127.0.0.1", port);
                string json=JsonConvert.SerializeObject(laptop);
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.ASCII.GetBytes(json);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                MessageBox.Show($"SocketException: {e}");
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
