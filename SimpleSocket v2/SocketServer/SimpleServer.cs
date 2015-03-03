using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class SimpleServer
    {
        private IPAddress _ip = IPAddress.Parse("127.0.0.1");
        private int _port;
        private bool _stop = false;

        public SimpleServer(int port)
        {
            this._port = port;
        }

        public void Run()
        {
            Console.WriteLine("Server started. IP: " + _ip + " on port " + _port + ".");

            TcpListener listener = new TcpListener(this._ip, this. _port);
            try
            {
                listener.Start();

                while (!_stop)
                {
                    Console.WriteLine("Waiting...");
                    Socket clientSocket = listener.AcceptSocket();
                    Console.WriteLine("Found " + _ip + ".");

                    ClientHandler clientHandler = new ClientHandler(clientSocket);
                    Thread clientThread = new Thread(clientHandler.RunClient);
                    //clientThread.IsBackground = true;
                    clientThread.Start();
                    

                    //    NetworkStream netStream = new NetworkStream(clientSocket);
                    //    StreamReader reader = new StreamReader(netStream);
                    //    StreamWriter writer = new StreamWriter(netStream);

                    //    string clientText = reader.ReadLine();
                    //    Console.WriteLine("Client says: " + clientText);

                    //    writer.WriteLine("Hello client");
                    //    writer.Flush();

                    //    while (!_stop)
                    //    {
                    //        string clientCommand = reader.ReadLine();
                    //        switch (clientCommand)
                    //        {
                    //            case "number":
                    //                writer.WriteLine("Write a number.");
                    //                writer.Flush();
                    //                string userInput = reader.ReadLine();
                    //                int numberInput;
                    //                bool checkNumber = int.TryParse(userInput, out numberInput);
                    //                if (checkNumber == true)
                    //                {
                    //                    writer.WriteLine("You wrote " + numberInput + ".");
                    //                }
                    //                else
                    //                {
                    //                    writer.WriteLine("That is not a number.");
                    //                }
                    //                break;
                    //            case "split":
                    //                writer.WriteLine("Write a string.");
                    //                writer.Flush();
                    //                string splitUserInput = reader.ReadLine();
                    //                char[] charInput = splitUserInput.ToCharArray();
                    //                foreach (char chars in charInput)
                    //                {
                    //                    writer.WriteLine(chars);;
                    //                }
                    //                break;
                    //            case "add":
                    //                writer.WriteLine("Input numbers.");
                    //                writer.Flush();
                    //                string clientInput = reader.ReadLine();
                    //                char[] splitInput = clientInput.ToCharArray();
                    //                if (splitInput.Length == 3)
                    //                {
                    //                    int num1;
                    //                    int num2;
                    //                    bool num1Check = int.TryParse(splitInput[0].ToString(), out num1);
                    //                    bool num2Check = int.TryParse(splitInput[2].ToString(), out num2);
                    //                    if (num1Check &&num2Check == true)
                    //                    {
                    //                        if (splitInput[1].ToString() == "+")
                    //                        {
                    //                            int sumResult = num1 + num2;
                    //                            writer.WriteLine(sumResult.ToString());
                    //                        }
                    //                        else
                    //                        {
                    //                            writer.WriteLine("Invalid operator");
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        writer.WriteLine("Invalid input");
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    writer.WriteLine("Ivalid text length");
                    //                }

                    //                break;
                    //            case "time":
                    //                //DateTime time = DateTime.Now;
                    //                //int hour = time.Hour;
                    //                //writer.WriteLine(hour);
                    //                writer.WriteLine(DateTime.Now.ToShortTimeString());
                    //                break;
                    //            case "date":
                    //                writer.WriteLine(DateTime.Now.ToShortDateString());
                    //                break;
                    //            case "":
                    //                writer.WriteLine("");
                    //                break;
                    //            case "exit":
                    //                Console.WriteLine("Closing connection...");
                    //                writer.Close();
                    //                reader.Close();
                    //                netStream.Close();
                    //                clientSocket.Close();
                    //                Console.WriteLine("Closed.");
                    //                Console.ReadLine();
                    //                break;
                    //            default:
                    //                writer.WriteLine("Unknown command");
                    //                break;
                    //        }

                    //        writer.Flush();

                    //    }
                }

            }

            catch (Exception)
            {
                Console.WriteLine("Socket error: Lost connection with client(s).");
                Console.ReadLine();
            }

        }
    }
}
