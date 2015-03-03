using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class ClientHandler
    {
        private Socket _socket;
        private bool _stop = false;

        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;

        public ClientHandler(Socket socket)
        {
            this._socket = socket;

            netStream = new NetworkStream(this._socket);
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);
        }

        public void RunClient()
        {
            GetID();
            StartupLogic();
        }

        private void GetID()
        {
            string sentID = CreateClientID();
            writer.WriteLine(sentID);
            writer.Flush();
            //Console.WriteLine(sentID);
            //StartupLogic();
        }

        public string CreateClientID()
        {
            string ID = String.Format("{0:d9}", DateTime.Now.Ticks);
            string clientID = ID.Substring(ID.Length - 6, 6);
            try
            {
                Console.WriteLine("Client with ID " + clientID + " is connected.");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not obtain ID.");
            }

            return clientID;
        }

        public void StartupLogic()
        {
            while (!_stop)
            {
                string clientCommand = reader.ReadLine();
                Console.WriteLine(clientCommand);
                switch (clientCommand)
                {
                    case "number":
                        writer.WriteLine("Write a number.");
                        writer.Flush();
                        string userInput = reader.ReadLine();
                        int numberInput;
                        bool checkNumber = int.TryParse(userInput, out numberInput);
                        if (checkNumber == true)
                        {
                            writer.WriteLine("You wrote " + numberInput + ".");
                            writer.Flush();
                        }
                        else
                        {
                            writer.WriteLine("That is not a number.");
                            writer.Flush();
                        }
                        break;
                    case "split":
                        writer.WriteLine("Write a string.");
                        writer.Flush();
                        string splitUserInput = reader.ReadLine();
                        char[] charInput = splitUserInput.ToCharArray();
                        foreach (char chars in charInput)
                        {
                            writer.WriteLine(chars);
                            writer.Flush();
                        }
                        break;
                    case "add":
                        writer.WriteLine("Input numbers.");
                        writer.Flush();
                        string clientInput = reader.ReadLine();
                        char[] splitInput = clientInput.ToCharArray();
                        if (splitInput.Length == 3)
                        {
                            int num1;
                            int num2;
                            bool num1Check = int.TryParse(splitInput[0].ToString(), out num1);
                            bool num2Check = int.TryParse(splitInput[2].ToString(), out num2);
                            if (num1Check && num2Check == true)
                            {
                                if (splitInput[1].ToString() == "+")
                                {
                                    int sumResult = num1 + num2;
                                    writer.WriteLine(sumResult.ToString());
                                    writer.Flush();
                                }
                                else
                                {
                                    writer.WriteLine("Invalid operator");
                                    writer.Flush();
                                }
                            }
                            else
                            {
                                writer.WriteLine("Invalid input");
                                writer.Flush();
                            }
                        }
                        else
                        {
                            writer.WriteLine("Ivalid text length");
                            writer.Flush();
                        }

                        break;
                    case "time":
                        //DateTime time = DateTime.Now;
                        //int hour = time.Hour;
                        //writer.WriteLine(hour);
                        writer.WriteLine(DateTime.Now.ToShortTimeString());
                        writer.Flush();
                        break;
                    case "date":
                        writer.WriteLine(DateTime.Now.ToShortDateString());
                        writer.Flush();
                        break;
                    case "":
                        writer.WriteLine("");
                        writer.Flush();
                        break;
                    case "exit":
                        Console.WriteLine("Closing connection...");
                        CloseStreams();
                        break;
                    default:
                        writer.WriteLine("Unknown command");
                        writer.Flush();
                        break;
                }

                //try
                //{
                //    writer.Flush();
                //}
                //catch (Exception)
                //{
                //    Console.WriteLine("Connection closed.");
                //}
            }

            //string theReader = reader.ReadLine();
            //string line = "" + theReader;
            //string returned = "test";
            //writer.WriteLine(returned);
            //writer.Flush();
            //Console.WriteLine(line);
        }

        private void messageToClients(string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }

        private void CloseStreams()
        {
            _stop = true;
            writer.Close();
            reader.Close();
            netStream.Close();
        }
    }
}
