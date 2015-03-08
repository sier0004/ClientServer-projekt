using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class TestClass
    {
        public delegate void Update(string message);
        public Update NewInput;
        public Update ClientMessage;
        string clientID;

        public string CheckInput(string lines)
        {
            int line = lines.IndexOf('|');
            clientID = lines.Substring(0, line);
            double number = double.Parse(lines.Substring(line + 1));

            string message = "TEST";
            NewInput(message);
            return message;
        }
    }
}
