using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Evel_Bot.Util;

namespace Evel_Bot.Commands
{
    static partial class Command
    {

        public static void Clear() //? Clear Command
        {
            Console.Clear();
        }

        public static void Stop() //? Stop Command
        {
            //x await Disconnect();
            Shell.WriteLine(ConsoleColor.Green ,"Evel-Bot stopped...");
            Shell.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

    }
}
