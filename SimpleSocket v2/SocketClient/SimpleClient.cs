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

                server = new TcpClient(_hostName, _port);

                stream = server.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                writer.AutoFlush = true;

               

                //Thread listener = new Thread(listenToServer);
                //listener.Start();

                string serverData = reader.ReadLine();
                Console.WriteLine("Server: " + serverData);

                while (true)
                {
                    string command = Console.ReadLine();
                    Console.WriteLine(command);
                    writer.WriteLine(command);

                    string commandResponse = reader.ReadLine();
                    Console.WriteLine(commandResponse);

                    if (command == "exit")
                    {
                        Console.WriteLine("Closing connection...");
                        writer.Close();
                        reader.Close();
                        stream.Close();
                        server.Close();
                        Console.WriteLine("Connection closed.");
                    }
                }
            }

            catch (Exception)
            {
                Console.WriteLine("No connection to server.");
                Console.ReadLine();
            }

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
