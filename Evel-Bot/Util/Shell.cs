using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evel_Bot.Util
{
    static class Shell //? Main Shell Class, give usefull methods to manage the Console
    {
        public static ConsoleColor Color { get; set; } = ConsoleColor.Gray; //Default Shell Color
        public static string ShellMessage { get; set; } = ""; //Default message displayed on Shell.Input();

        public static string Input() //Simuate an input shell ">command"
        {
            Write(ShellMessage);
            return Console.ReadLine();
        }

        public static string Input(string message) //Read a line with a custom message
        {
            Write(message);
            return Console.ReadLine();
        }

        public static string InputLine(string message) //Read a line with a custom message
        {
            WriteLine(message);
            return Console.ReadLine();
        }

        public static string Input(ConsoleColor color) //Same as Input() with color
        {
            Write(color ,"Evel-Bot> ");
            return Console.ReadLine();
        }

        public static string Input(ConsoleColor color, string message) //Read a line with a custom message with a color
        {
            Write(color, message);
            return Console.ReadLine();
        }

        public static string InputLine(ConsoleColor color, string message) //Read a line with a custom message with a color
        {
            WriteLine(color, message);
            return Console.ReadLine();
        }

        public static void Write() //Skip a Line
        {
            Console.WriteLine();
        }

        public static void Write(object str) //Console.Write Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.Write(str);
            Console.ForegroundColor = Ccolor;
        }

        public static void Write(ConsoleColor color, object str) //Console.Write Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = Ccolor;
        }

        public static void WriteLine() //Skip a line
        {
            Console.WriteLine();
        }

        public static void WriteLine(object str) //Console.Write Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.WriteLine(str);
            Console.ForegroundColor = Ccolor;
        }

        public static void WriteLine(ConsoleColor color, object str) //Console.Write Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = Ccolor;
        }

        public static async Task WriteAsync(object str) //Console.Out.WriteAsync Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = Color;

            foreach (char chr in str.ToString())
                await Console.Out.WriteAsync(chr);

            Console.ForegroundColor = Ccolor;
        }

        public static async Task WriteAsync(ConsoleColor color, object str) //Console.Out.WriteAsync Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            foreach (char chr in str.ToString())
                await Console.Out.WriteAsync(chr);

            Console.ForegroundColor = Ccolor;
        }

        public static async Task WriteLineAsync(object str) //Console.Out.WriteLineAsync Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = Color;

            await Console.Out.WriteLineAsync(str.ToString());

            Console.ForegroundColor = Ccolor;
        }

        public static async Task WriteLineAsync(ConsoleColor color, object str) //Console.Out.WriteLineAsync Override
        {
            var Ccolor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            await Console.Out.WriteLineAsync(str.ToString());

            Console.ForegroundColor = Ccolor;
        }

    }
}
