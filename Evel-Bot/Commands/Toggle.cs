using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Evel_Bot.Modules;
using Evel_Bot.Util;
using Evel_Bot.Util.Extensions;
using System.Linq;

namespace Evel_Bot.Commands
{
    partial class Command
    {
        public static void Toggle(string input) //? Switch a module ON/OFF
        {
            void ToggleList() //! Write Activated/Desactivated IModules
            {
                foreach (IModule mod in Module.ModulesList)
                {
                    if (mod.IsActivated)
                        Shell.WriteLine(ConsoleColor.Green, mod.GetType().Name);
                    else
                        Shell.WriteLine(ConsoleColor.Red, mod.GetType().Name);
                }
            }

            string[] split = input.Split(' ');

            if (split.Length == 1 || (split.Length > 1 && split[1] == "list"))
            {
                ToggleList();
                return;
            }

            if (split.Length < 2)
            {
                Shell.WriteLine("Invalid arguments, please try with \"toggle <module>\"");
            }

            foreach(string str in split) // Support toggling multiple modules at a time
            {
                if (str.Equals("toggle", StringComparison.OrdinalIgnoreCase))
                    continue;

                IModule m = Module.GetModule(str);

                if (m == null)
                {
                    Shell.WriteLineError("Module " + str + " don't exist.");
                    return;
                }

                if (!m.IsActivated)
                {
                    m.IsActivated = true;
                    m.Activate();
                    Shell.WriteLine(ConsoleColor.Cyan, "Module " + m.GetType().Name + " activated");
                }
                else
                {
                    m.IsActivated = false;
                    m.Desactivate();
                    Shell.WriteLine(ConsoleColor.Cyan, "Module " + m.GetType().Name + " desactivated");
                }
            }
        }

        public static void Auto(string input) //? Add/Remove modules from Autostart
        {
            ConfigurationFile module = new ConfigurationFile(Path.Combine(AppContext.BaseDirectory, "modules.config"));

            if (input.Split(' ').Length < 2 || input.Contains("list")) // List all enabled/disabled auto Module
            {
                foreach (Setting s in module.GetAll())
                    Shell.WriteLine(s.Value == "true" ? ConsoleColor.Green : ConsoleColor.Red, s.Key);
            }

            foreach (string str in input.Split(' ').SubArray(1))
            {
                string key = module.GetAll().Where(x => x.Key.Equals(str, StringComparison.OrdinalIgnoreCase)).Select(x => x.Key).FirstOrDefault();

                if (key == null)
                {
                    Shell.WriteLineError("The module " + str + " don't exist.");
                    continue;
                }

                string value = module[key].Value;

                if (value == "false")
                {
                    module.Set(key, "true");
                    module.Save();
                    Shell.WriteLine(ConsoleColor.Cyan, "Module " + key + " added to Autostart");
                }
                else if (value == "true")
                {
                    module.Set(key, "false");
                    module.Save();
                    Shell.WriteLine(ConsoleColor.Cyan, "Module " + key + " removed from Autostart");
                }
            }
        }
    }
}
