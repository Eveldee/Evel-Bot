using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Evel_Bot.Util;
using System.IO;
using Newtonsoft.Json;

namespace Evel_Bot.Modules
{
    class Fun : IModule //? A Module to link reply to some words.
    {
        Dictionary<string, string> FunWords = new Dictionary<string, string>();
        private string ConfigPath { get; } = Module.GetPath("fun.json");

        public bool IsActivated { get; set; }

        public void Activate()
        {
            try
            {
                if (!File.Exists(ConfigPath)) // Don't try to load config if file don't exist
                {
                    FunWords.Add("trigger", "say");
                    File.WriteAllText(ConfigPath ,JsonConvert.SerializeObject(FunWords, Formatting.Indented));
                }
                else
                    FunWords = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigPath));
            }
            catch (Exception e)
            {
                Shell.WriteLine(ConsoleColor.Red, "Can't acces to " + ConfigPath);
                Shell.WriteLine(e.Message);
                Desactivate();
            }
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        public void Desactivate()
        {
            Program.Client.MessageReceived -= Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return;

            ISocketMessageChannel channel = msg.Channel;

            foreach (var key in FunWords)
            {
                if (msg.Content.ToLower().Trim(' ') == key.Key)
                    await channel.SendMessageAsync(key.Value);
            }
        }

    }
}
