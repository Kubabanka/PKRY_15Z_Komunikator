using Messages;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Server server = new Server.Server();
            server.Initialize();

            Console.WriteLine();

        }
    }
}
