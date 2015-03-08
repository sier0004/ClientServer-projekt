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

            List<TcpClient> clients = new List<TcpClient>();

            try
            {
                listener.Start();

                while (!_stop)
                {
                    Console.WriteLine("Waiting...");
                    //Socket clientSocket = listener.AcceptSocket();
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client found on " + _ip + ".");
                    clients.Add(client);

                    ClientHandler clientHandler = new ClientHandler(client, clients);
                    Thread clientThread = new Thread(clientHandler.RunServer);
                    //clientThread.IsBackground = true;
                    clientThread.Start();
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
