using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Evel_Bot.Util;
using System.Threading.Tasks;

namespace Evel_Bot.Modules
{
    interface IModule //! IModule Interface
    {
        bool IsActivated { get; set; } //Get if the module is Activated

        void Activate(); //Turn on a module

        void Desactivate(); //Turn off a module

    }

    static class Module //! Base class to manage IModule
    {
        public static IReadOnlyList<IModule> ModulesList { get; set; } //List of all modules

        public static IModule GetModule(string name) //Get a module from his name
        {
            foreach (IModule m in ModulesList)
            {
                if (m.GetType().Name.ToLower() == name.ToLower())
                    return m;
            }
            return null;
        }

        public static string GetPath(string filename) //Get a path for a module config file.
        {
            return Misc.GetFilePath("Modules", filename);
        }

        public static void Log(this IModule module, string message, ConsoleColor color = ConsoleColor.Gray) // Extension for Modules log
        {
            Modules.Log.SendLog(new LogEventArgs(LogEventType.Info, $"[{module.GetType().Name}] {message}"));

            Shell.Write(ConsoleColor.DarkCyan, $"[{module.GetType().Name}] ");
            Shell.WriteLine(color, message);
        }

        public static async Task LogAsync(this IModule module, string message, ConsoleColor color = ConsoleColor.Gray) // Extension for Modules log
        {
            Modules.Log.SendLog(new LogEventArgs(LogEventType.Info, $"[{module.GetType().Name}] {message}"));

            await Shell.WriteAsync(ConsoleColor.DarkCyan, $"[{module.GetType().Name}] ");
            await Shell.WriteLineAsync(color, message);
        }

        public static void LogError(this IModule module, string message) // Extension for Modules log
        {
            Modules.Log.SendLog(new LogEventArgs(LogEventType.Error, $"[{module.GetType().Name}] {message}"));

            Shell.WriteError($"[{module.GetType().Name}] ", ConsoleColor.DarkRed);
            Shell.WriteLineError(message);
        }

        public static async Task LogErrorAsync(this IModule module, string message) // Extension for Modules log
        {
            Modules.Log.SendLog(new LogEventArgs(LogEventType.Error, $"[{module.GetType().Name}] {message}"));

            await Shell.WriteErrorAsync($"[{module.GetType().Name}] ", ConsoleColor.DarkRed);
            await Shell.WriteLineErrorAsync(message);
        }

    }
}
