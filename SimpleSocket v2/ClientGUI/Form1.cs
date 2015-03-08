using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ClientGUI
{
    public partial class Form1 : Form
    {
        private TcpClient server;
        NetworkStream netStream;
        StreamReader reader;
        StreamWriter writer;

        private string _clientID;
        
        public delegate void WriteThread(string txt);

        String message;

        public Form1()
        {
            InitializeComponent();
        }

        //public void skriv(object s, EventArgs e)
        public void Write(string s)
        {
            this.textBox1.Text += s + "\r\n";
        }

        public void ClientThread()
        {
            WriteThread textBoxHandler = Write; 
            message = reader.ReadLine();
            while (message != "STOP")
            {
                this.Invoke(textBoxHandler, message);
                message = reader.ReadLine();
            }
        }

        public void GetID()
        {
            string receivedID = reader.ReadLine();
            _clientID = receivedID;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            server = new TcpClient("localhost", 11000);
            netStream = server.GetStream();
            reader = new StreamReader(netStream);
            writer = new StreamWriter(netStream);
            writer.AutoFlush = true;
            Thread t = new Thread(this.ClientThread);
            t.Start();
            GetID();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            writer.WriteLine(_clientID + ": " + textBox2.Text);
        }
    }
}
