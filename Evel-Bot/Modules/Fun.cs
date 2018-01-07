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
    class Fun : IModule, IJsonConfiguration<Dictionary<string, string>> //? A Module to link reply to some words.
    {
        Dictionary<string, string> FunWords;
        private string ConfigPath { get; } = Module.GetPath("fun.json");

        public bool IsActivated { get; set; }
        public Dictionary<string, string> DefaultConfig => new Dictionary<string, string>() { { "trigger", "say" } };

        public void Activate()
        {
            FunWords = this.LoadConfig();
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
