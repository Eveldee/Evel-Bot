using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using Evel_Bot.Util;

namespace Evel_Bot.Commands
{
    static partial class Command
    {
        static async Task Connect(string input) //? Connect Command
        {
            string[] cmd = input.Split(' ');
            string token = null;
            TokenType type = TokenType.Bearer;

            if (cmd.Length < 2)
            {
                Shell.WriteLine("Invalid arguments, please try with \"connect bot/user token\"");
                return;
            }

            if (int.TryParse(cmd[1], out int i)) {
                if (i <= Program.ClientAccount.Accounts.Count)
                {
                    token = Program.ClientAccount.Accounts[i - 1].Token;
                    type = Program.ClientAccount.Accounts[i - 1].Type;
                }
            }
            else if (cmd.Length > 2)
            {
                token = cmd[2];
                if (cmd[1].Equals("bot", StringComparison.OrdinalIgnoreCase))
                    type = TokenType.Bot;
                else if (cmd[1].Equals("user", StringComparison.OrdinalIgnoreCase))
                    type = TokenType.User;
            }


            if (token == null || type == TokenType.Bearer)
            {
                Shell.WriteLineError("Invalid arguments, please try with \"connect bot/user token\"");
                return;
            }

            try
            {
                await Program.Client.StartAsync();
                await Program.Client.LoginAsync(type, token);
            }
            catch (Exception e)
            {
                Shell.WriteLineError("Error during connection...");
                Shell.WriteLineError(e.Message);
                return;
            }


            Shell.Write(ConsoleColor.Yellow, "Connecting to server...");

            int timeout = 0;

            while(!Program.ClientAccount.IsConnected)
            {
                if (timeout > 20)
                {
                    Shell.WriteLineError("Error during connection...");
                    await Disconnect();
                    return;
                }

                Shell.Write(ConsoleColor.Yellow ,".");
                System.Threading.Thread.Sleep(500);
                timeout++;
            }
            Shell.Write();
            Shell.WriteLine(ConsoleColor.Green, "Connected to Discord server !");

            ConfigurationFile accounts = new ConfigurationFile(Path.Combine(AppContext.BaseDirectory, "accounts.config"));
            accounts.Add(Program.Client.CurrentUser.Username, type + ";" + token);
            accounts.Save();
        }

        static async Task Disconnect() //? Disconnect command
        {
            Shell.Write(ConsoleColor.Yellow, "Disconnecting from Discord Server...");
            await Program.Client.StopAsync();

            while (Program.ClientAccount.IsConnected)
            {
                Shell.Write(ConsoleColor.Yellow, ".");
                System.Threading.Thread.Sleep(500);
            }
            Shell.Write();
            Shell.WriteLine(ConsoleColor.Green, "Disconnected from Discord server !");

            Program.Client.Dispose();
        }
    }
}
