using Evel_Bot.Util;
using Evel_Bot.Util.Extensions;
using System;
using System.Threading.Tasks;


namespace Evel_Bot.Commands
{
    static partial class Command
    {

        public static async Task ExeCommand(string input) //? Basic Method to execute a command
        {
            switch(input)
            {
                case string str when str.Length == 0:
                    break;
                case string str when str.StartsWith("connect", StringComparison.OrdinalIgnoreCase):
                    await Connect(input);
                    break;
                case string str when str.StartsWithOne(StringComparison.OrdinalIgnoreCase, "cls", "clear"):
                    Clear();
                    break;
                case string str when str.StartsWith("disconnect", StringComparison.OrdinalIgnoreCase):
                    await Disconnect();
                    break;
                case string str when str.StartsWithOne(StringComparison.OrdinalIgnoreCase, "stop", "exit"):
                    Stop();
                    break;
                case string str when str.StartsWith("toggle", StringComparison.OrdinalIgnoreCase):
                    Toggle(input);
                    break;
                case string str when str.StartsWithOne(StringComparison.OrdinalIgnoreCase, "auth", "authorize"):
                    Auth();
                    break;
                case string str when str.StartsWithOne(StringComparison.OrdinalIgnoreCase, "auto", "autoadd", "autostart"):
                    Auto(input);
                    break;
                case string str when str.StartsWithOne(StringComparison.OrdinalIgnoreCase, "play", "game"):
                    await Play(input);
                    break;
                case string str when str.StartsWith("chat", StringComparison.OrdinalIgnoreCase):
                    Chat.Connect(input.Substring(5));
                    break;
                case string str when str.StartsWith("reload", StringComparison.OrdinalIgnoreCase):
                    Reload(input);
                    break;
                //case string str when str.StartsWith("connnect", StringComparison.OrdinalIgnoreCase):
                //    break;
                //case string str when str.StartsWith("connnect", StringComparison.OrdinalIgnoreCase):
                //    break;
                default:
                    Shell.WriteLine("Invalid command.");
                    break;
            }
        }

    }
}
