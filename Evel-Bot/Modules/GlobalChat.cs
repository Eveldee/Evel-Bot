using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Evel_Bot.Util;
using Discord.WebSocket;

namespace Evel_Bot.Modules
{
    class GlobalChat : IModule //! GlobalChat IModule
    {
        public bool IsActivated { get; set; } = false;

        public void Activate()
        {
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        public void Desactivate()
        {
            Program.Client.MessageReceived -= Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage msg)
        {
            await this.LogAsync(msg.Channel.Name + "/" + msg.Author.Username + ": " + msg.Content);
        }
    }
}
