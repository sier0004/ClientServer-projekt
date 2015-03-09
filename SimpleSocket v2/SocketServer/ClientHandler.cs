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
        //private Socket _socket;
        //List<Socket> clients;
        private TcpClient _client;
        List<TcpClient> clients;

        //private Object thisLock = new object();
        private bool _stop = false;

        //public delegate void Update(string message);
        //public Update NewInput;

        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;

        public ClientHandler(TcpClient client, List<TcpClient> clientList)
        {
            this._client = client;
            this.clients = clientList;

            netStream = _client.GetStream();
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);

            //NewInput += this.messageToClients;
        }

        public void RunServer()
        {
            GetID();
            Run();
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

        public void Run()
        {
            netStream = _client.GetStream();
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);
            writer.AutoFlush = true;

            //lock (thisLock)
            //{

            //}

            string clientInput = reader.ReadLine();

            while (!_stop)
            {
                Console.WriteLine("Received: {0}", clientInput);

                foreach (TcpClient response in clients)
                {
                    NetworkStream netStream = response.GetStream();
                    StreamWriter writer = new StreamWriter(netStream);
                    writer.AutoFlush = true;

                    string[] splitCommand = clientInput.Split(' ');
                    switch (splitCommand[1])
                    {
                        case "number":
                            writer.WriteLine("Write a number.");
                            writer.Flush();
                            string userInput = reader.ReadLine();
                            string[] splitInput = userInput.Split(' ');
                            int numberInput;
                            bool checkNumber = int.TryParse(splitInput[1], out numberInput);
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
                            string input = reader.ReadLine();
                            string[] splitUserInput = input.Split(' ');
                            char[] charInput = splitUserInput[1].ToCharArray();
                            foreach (char chars in charInput)
                            {
                                writer.WriteLine(chars);
                                writer.Flush();
                            }
                            break;
                        case "add":
                            writer.WriteLine("Input numbers.");
                            writer.Flush();
                            string readNumber = reader.ReadLine();
                            string[] splitClientInput = readNumber.Split(' ');
                            char[] splitAddInput = splitClientInput[1].ToCharArray();
                            if (splitAddInput.Length == 3)
                            {
                                int num1;
                                int num2;
                                bool num1Check = int.TryParse(splitAddInput[0].ToString(), out num1);
                                bool num2Check = int.TryParse(splitAddInput[2].ToString(), out num2);
                                if (num1Check && num2Check == true)
                                {
                                    if (splitAddInput[1].ToString() == "+")
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
                            writer.WriteLine("STOP");
                            writer.Flush();
                            CloseStreams();
                            break;
                        default:
                            writer.WriteLine(clientInput);
                            writer.Flush();
                            break;
                    }
                }
                Console.WriteLine("Sent: {0}", clientInput);

                clientInput = String.Empty;
                clientInput = reader.ReadLine();
            }
        }

        //Udvidelse til broadcast
        //private void messageToClients(string message)
        //{
        //    writer.WriteLine(message);
        //    writer.Flush();
        //}

        private void CloseStreams()
        {
            _stop = true;
            writer.Close();
            reader.Close();
            netStream.Close();
        }
    }
}
