using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerminalAPI;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Terminal.WriteLine("Press a key... pls");
            ConsoleKey k = Terminal.Read(false);
            Terminal.WriteLine("You pressed " + k.ToString());
            Thread.Sleep(1000);
            Terminal.Clear();
        }
    }
}
