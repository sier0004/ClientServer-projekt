using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class SimpleClient
    {
        private string _clientID;

        private string _hostName;
        private int _port;

        TcpClient server;

        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;

        public SimpleClient(string hostName, int port)
        {
            this._hostName = hostName;
            this._port = port;
        }

        public void Run()
        {
            Console.WriteLine("Client started on: " + _hostName + " Port: " + _port);

            try
            {
                StartConnection();

                //Thread listener = new Thread(listenToServer);
                //listener.Start();

                GetID();

                //string serverData = reader.ReadLine();
                //Console.WriteLine("Server: " + serverData);
                 
                while (true)
                {
                    string input = Console.ReadLine();
                    writer.WriteLine(_clientID + ": " + input);

                    string commandResponse = reader.ReadLine();
                    Console.WriteLine(commandResponse);

                    if (input == "exit")
                    {
                        CloseConnection();
                    }
                }
            }

            catch (Exception)
            {
                Console.WriteLine("No connection to server.");
                Console.ReadLine();
            }
        }

        public void CloseConnection()
        {
            Console.WriteLine("Closing connection...");
            writer.Close();
            reader.Close();
            stream.Close();
            server.Close();
            Console.WriteLine("Connection closed.");
            return;
        }

        public void GetID()
        {
            string receivedID = reader.ReadLine();
            _clientID = receivedID;
            Console.WriteLine("Client ID is: " + _clientID);
        }

        public void StartConnection()
        {
            server = new TcpClient(_hostName, _port);

            stream = server.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
        }

        //private void listenToServer()
        //{
        //    while (executeCommand())
        //    {
        //        //reader.ReadLine();
        //    }
        //}

        //private bool executeCommand()
        //{
        //    string input = reader.ReadLine();

        //    if (input == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}
