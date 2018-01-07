using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Evel_Bot.Util;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Evel_Bot.Modules
{
    public interface IModule //! IModule Interface
    {
        bool IsActivated { get; set; } //Get if the module is Activated

        void Activate(); //Turn on a module

        void Desactivate(); //Turn off a module
    }

    public interface IJsonConfiguration<T> //! A Json configuration file
    {
        T DefaultConfig { get; } //? Default Module config
    }

    public static class Module //! Base class to manage IModule
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
    }

    public static class ModuleExtensions
    {
        public static T LoadConfig<T>(this IJsonConfiguration<T> module) // Load config file
        {
            string path = Module.GetPath(module.GetType().Name + ".json");

            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, JsonConvert.SerializeObject(module.DefaultConfig, typeof(T), Formatting.Indented, null));
                    return module.DefaultConfig;
                }

                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                if (module is IModule log)
                {
                    log.LogError("Error while loading " + path + ".");
                    log.LogError(e.Message);
                }
                Modules.Log.SendLog(e.StackTrace, true);


                return module.DefaultConfig;
            }
        }

        public static bool SaveConfig<T>(this IJsonConfiguration<T> module, T value) // Save config file
        {
            string path = Module.GetPath(module.GetType().Name + ".json");

            try
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(value, typeof(T), Formatting.Indented, null));

                return true;
            }
            catch (Exception e)
            {
                if (module is IModule log)
                {
                    log.LogError("Error while loading " + path + ".");
                    log.LogError(e.Message);
                }
                Modules.Log.SendLog(e.StackTrace, true);

                return false;
            }
        }

        public static async Task<T> LoadConfigAsync<T>(this IJsonConfiguration<T> module) // Load config file async
        {
            string path = Module.GetPath(module.GetType().Name + ".json");

            try
            {
                if (!File.Exists(path))
                {
                    await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(module.DefaultConfig, typeof(T), Formatting.Indented, null));
                    return module.DefaultConfig;
                }

                return JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));
            }
            catch (Exception e)
            {
                if (module is IModule log)
                {
                    await log.LogErrorAsync("Error while loading " + path + ".");
                    await log.LogErrorAsync(e.Message);
                }
                Modules.Log.SendLog(e.StackTrace, true);

                return module.DefaultConfig;
            }
        }

        public static async Task<bool> SaveConfigAsync<T>(this IJsonConfiguration<T> module, T value) // Save config file async
        {
            string path = Module.GetPath(module.GetType().Name + ".json");

            try
            {
                await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(value, typeof(T), Formatting.Indented, null));

                return true;
            }
            catch (Exception e)
            {
                if (module is IModule log)
                {
                    await log.LogErrorAsync("Error while loading " + path + ".");
                    await log.LogErrorAsync(e.Message);
                }
                Modules.Log.SendLog(e.StackTrace, true);

                return false;
            }
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
