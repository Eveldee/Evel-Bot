using Discord.WebSocket;
using Evel_Bot.Util;
using Evel_Bot.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Evel_Bot.Modules
{
    class RemoteControl : IModule //? A Module to receive remote Commands
    {
        List<string> RemoteUsers = new List<string>();
        public string ConfigPath { get; } = Module.GetPath("remote.json");
        public bool IsActivated { get; set; }


        public void Activate()
        {
            try
            {
                if (!File.Exists(ConfigPath)) //Don't read the file if it doesn't exist.
                {
                    File.WriteAllText(ConfigPath, "[]"); //! Avoid a glitch that make the file corrupted.
                    goto Skip;
                }
                RemoteUsers = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigPath));
            }
            catch (Exception e)
            {
                this.LogError("Can't read " + ConfigPath);
                this.LogError(e.Message);
            }
            Skip:
            Program.ShellEvent += OnInput;
            Program.Client.MessageReceived += OnMessage;
        }

        public void Desactivate()
        {
            Program.ShellEvent -= OnInput;
            Program.Client.MessageReceived -= OnMessage;
        }

        private async Task OnMessage(SocketMessage msg) //Execute a remote command.
        {
            if (msg.Content[0] == '$' && IsRemoteUser(msg.Author.Username))
            {
                if (msg.Content.EqualsOne(StringComparison.OrdinalIgnoreCase, "$disconnect", "$stop", "$exit"))
                {
                    await msg.Channel.SendEmbed(EmbedTemplates.Error, "Disconnect command can't be remote executed.");
                    return;
                }

                await msg.DeleteAsync();
                string commands = "";

                foreach (string str in msg.Content.Split("&&")) // Execute command
                {
                    string cmd = str.Trim().TrimStart('$');

                    commands += cmd + "\n";
                    await Program.SendCommand(cmd);
                }

                await msg.Channel.SendEmbed(EmbedTemplates.Info, $"{msg.Author.Username} used command:\n{commands}");
                await this.LogAsync($"{msg.Author.Username} executed \"{msg.Content}\"");
            }
            else if (msg.Content[0] == '$')
                await msg.Channel.SendEmbed(EmbedTemplates.Forbidden, "You don't have the permission to send remote commands.");
        }

        private async Task OnInput(ShellEventArgs e) //Handle command in the shell
        {
            if (!e.Input.StartsWith("remote", StringComparison.OrdinalIgnoreCase))
                return;
            e.Handled = true;

            string[] args = e.Input.Split(' ').SubArray(1);

            if (args[0].Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                this.Log(RemoteUsers.Concat(", "));
                return;
            }
            if (args.Length < 2)
            {
                this.Log("Invalid use, try with \"remote <add/remove> <username>\"");
                return;
            }

            await Task.Run(() => ManageUsers(args));
        }

        private bool IsRemoteUser(string username) //Chekc if a User is in the list.
        {
            foreach (string str in RemoteUsers)
            {
                if (username == str)
                    return true;
            }
            return false;
        }

        private void ManageUsers(string[] args) //Manage RemoteUsers list
        {
            if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                foreach (string str in args.SubArray(1))
                {
                    RemoteUsers.Add(str);
                    this.Log("Added " + str + " to RemoteUsers", ConsoleColor.Cyan);
                }
            }
            else if (args[0].Equals("remove", StringComparison.OrdinalIgnoreCase))
            {
                foreach (string str in args.SubArray(1))
                {
                    RemoteUsers.Remove(str);
                    this.Log("Removed " + str + " from RemoteUsers", ConsoleColor.Cyan);
                }
            }
            Save();
        }

        private void Save() //Serialize the List to ConfigPath
        {
            try
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(RemoteUsers, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            catch (Exception e)
            {
                this.Log("Can't save config to " + ConfigPath);
                this.Log(e.Message);
            }
        }
    }
}
