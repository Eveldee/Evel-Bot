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
                await Shell.WriteLineErrorAsync("You must be connected before setting the playing game");
                return;
            }

            string str = input.Split(' ').SubArray(1).Concat(" ");
            await Program.Client.SetGameAsync(str);
            Shell.WriteLine($"Current game setted to: {str}");
        }
    }
}
