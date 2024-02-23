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
            Console.Write("Enter the server IP: ");
            string serverIp = Console.ReadLine();

            Console.Write("Enter the server port: ");
            int serverPort;
            while (!int.TryParse(Console.ReadLine(), out serverPort))
            {
                Console.WriteLine("Invalid port. Please enter a valid integer value.");
                Console.Write("Enter the server port: ");
            }

            TestCases.SetServerDetails(serverIp, serverPort);

            Console.WriteLine("Choose a test case to run:");
            Console.WriteLine("1. Manual Input");
            Console.WriteLine("2. Test Edge Cases");
            Console.WriteLine("3. Happy Path");
            Console.WriteLine("4. Rate Limit");

            int choice;
            while(true)
            {
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            TestCases.TestManualInput();
                            break;
                        case 2:
                            TestCases.TestEdgeCases();
                            break;
                        case 3:
                            TestCases.TestHappyPath();
                            break;
                        case 4:
                            TestCases.TestRateLimiting();
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please choose a valid test case.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }

        }
    }
}
