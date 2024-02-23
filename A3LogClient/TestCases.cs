using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace A3LogClient
{
    internal class TestCases
    {
        private static int ratePolling = 15;
        private static string serverIp;
        private static int serverPort;
        public static void SetServerDetails(string ip, int port)
        {
            serverIp = ip;
            serverPort = port;
        }
        public static void TestManualInput()
        {
            Console.WriteLine("Enter values for JSON fields:");
            Console.Write("APP: ");
            string app = Console.ReadLine();
            Console.Write("LEVEL: ");
            string level = Console.ReadLine();
            Console.Write("LOG: ");
            string log = Console.ReadLine();

            SendJsonToServer(app, level, log);
        }

        public static void TestRateLimiting()
        {
            for (int i = 0; i < ratePolling; i++)
            {
                SendJsonToServer("test", "WARNING", "Samplle log");
            }
        }

        public static void TestEdgeCases()
        {
            Console.WriteLine("Running edge cases...");

            // Test various edge cases
            SendJsonToServer("", "", ""); // fail
            SendJsonToServer("Test_app", "", ""); // fail
            SendJsonToServer("", "info", ""); // pass
            SendJsonToServer("", "", "Sample Log"); // fail
            SendJsonToServer("Test_app", "info", "Sample Log"); // pass
            SendJsonToServer("", "", ""); // fail

            Console.WriteLine("Edge cases completed.");
        }

        public static void TestHappyPath()
        {
            Console.WriteLine("Running happy path...");
            string appValue = "Test_app";
            string levelValue = "info";
            string logValue = "Sample Log";

            SendJsonToServer(appValue, levelValue, logValue);

            Console.WriteLine("Happy path completed.");
        }

        public static void SendJsonToServer(string app, string level, string log)
        {
            JObject jsonObject = new JObject
            {
                { "APP", app },
                { "LEVEL", level },
                { "LOG", log }
            };

            string json = jsonObject.ToString();

            try
            {
                using (TcpClient client = new TcpClient(serverIp, serverPort))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent JSON to server: {json}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending JSON to server: {ex.Message}");
            }
        }
    }
}
