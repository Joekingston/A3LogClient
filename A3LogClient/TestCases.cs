using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace A3LogClient
{
    //Name TestCases
    //Purpose This is a class that houses each individual test case to use for the server.
    internal class TestCases
    {
        private static int ratePolling = 15;
        private static string serverIp;
        private static int serverPort;
        // Name    : SetServerDetails
        // Purpose : Sets the IP address and port of the server.
        // Inputs  : string ip : The IP address of the server, int port : The port of the server.
        // Returns : void
        public static void SetServerDetails(string ip, int port)
        {
            serverIp = ip;
            serverPort = port;
        }
        // Name    : TestManualInput
        // Purpose : Allows manual input of values for JSON fields and sends the JSON to the server.
        // Inputs  : None
        // Returns : void
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
        // Name    : TestRateLimiting
        // Purpose : Tests rate limiting by sending a specified number of JSON messages to the server.
        // Inputs  : None
        // Returns : void
        public static void TestRateLimiting()
        {
            for (int i = 0; i < ratePolling; i++)
            {
                SendJsonToServer("test", "WARNING", "Samplle log");
            }
        }

        // Name    : TestEdgeCases
        // Purpose : Runs edge cases to test the behavior of sending JSON messages to the server.
        // Inputs  : None
        // Returns : void
        public static void TestEdgeCases()
        {
            Console.WriteLine("Running edge cases...");

            // Test various edge cases
            SendJsonToServer("", "", ""); // fail
            SendJsonToServer("Test_app", "", ""); // fail
            SendJsonToServer("", "info", ""); // pass
            SendJsonToServer("", "Bouncing", ""); // fail
            SendJsonToServer("", "", "Sample Log"); // fail
            SendJsonToServer("Test_app", "info", "Sample Log"); // pass
            SendJsonToServer("", "", ""); // fail

            Console.WriteLine("Edge cases completed.");
        }

        // Name    : TestHappyPath
        // Purpose : Tests the happy path scenario by sending a JSON message to the server.
        // Inputs  : None
        // Returns : void
        public static void TestHappyPath()
        {
            Console.WriteLine("Running happy path...");
            string appValue = "Test_app";
            string levelValue = "info";
            string logValue = "Sample Log";

            SendJsonToServer(appValue, levelValue, logValue);

            Console.WriteLine("Happy path completed.");
        }

        // Name    : SendJsonToServer
        // Purpose : Sends a JSON message to the server using the provided values for APP, LEVEL, and LOG.
        // Inputs  : string app : The value for the "APP" field, string level : The value for the "LEVEL" field, string log : The value for the "LOG" field.
        // Returns : void
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
