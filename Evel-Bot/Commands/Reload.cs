using Evel_Bot.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using Evel_Bot.Util.Extensions;
using Evel_Bot.Util;

namespace Evel_Bot.Commands
{
    partial class Command
    {
        public static void Reload(string input) //? Reload Modules
        {
            string[] args = input.Split(' ');

            if (args.Length < 2) //! Reload all modules
            {
                foreach (IModule module in Module.ModulesList)
                {
                    if (module.IsActivated)
                    {
                        module.Desactivate();
                        module.Activate();
                    }
                }

                Shell.WriteLine("[Reload] All modules have been reloaded");
            }
            else //! Reload specified modules
            {
                foreach (string str in args.SubArray(1))
                {
                    IModule module = Module.GetModule(str);

                    if (module != null && module.IsActivated)
                    {
                        module.Desactivate();
                        module.Activate();
                        Shell.WriteLine($"[Reload] Module {module.GetType().Name} reloaded");
                    }
                }
            }
        }
    }
}
