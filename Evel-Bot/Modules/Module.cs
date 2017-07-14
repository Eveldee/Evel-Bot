using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Evel_Bot.Util;

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

    }
}
