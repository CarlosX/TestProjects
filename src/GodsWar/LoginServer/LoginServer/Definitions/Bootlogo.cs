using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Definitions
{
    class Bootlogo
    {
        public static void _Load()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Title = "LoginServer " + Global.Versions.appVersion;
            Console.WriteLine("=============================LoginServer=============================");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
