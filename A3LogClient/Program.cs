using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace A3LogClient
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string serverIp = "10.0.0.60";
            int serverPort = 30001;
            var logObject = new
            {
                APP = "my_app",
                LEVEL = "info"
            };

            string logMessage = JsonConvert.SerializeObject(logObject);
            for (int i = 0; i < 15; i++) {
                try
                {
                    using (TcpClient client = new TcpClient(serverIp, serverPort))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(logMessage);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("Log message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            
            
            }

        }
    }
}
