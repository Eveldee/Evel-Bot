using Evel_Bot.Modules;
using Evel_Bot.Util;
using Evel_Bot.Util.Extensions;
using System;
using System.Threading.Tasks;

namespace Evel_Bot.Commands
{
    partial class Command
    {
        public static async Task Play(string input)
        {
            if (!Program.ClientAccount.IsConnected)
            {
                await Shell.WriteErrorAsync("[Play] ", ConsoleColor.DarkRed);
                await Shell.WriteLineErrorAsync("You must be connected before setting the playing game");
                Log.SendLog("[Play] You must be connected before setting the playing game", true);
                return;
            }

            string str = input.Split(' ').SubArray(1).Concat(" ");
            await Program.Client.SetGameAsync(str);
            Shell.Write(ConsoleColor.DarkCyan, "[Play] ");
            Shell.WriteLine($"Current game set to: {str}");
            Log.SendLog($"[Play] Current game set to: {str}");
        }
    }
}
