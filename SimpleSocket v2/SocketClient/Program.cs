using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleClient client = new SimpleClient("127.0.0.1", 11000);
            client.Run();
        }
    }
}
