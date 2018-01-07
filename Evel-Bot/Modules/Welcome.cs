using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;
using Evel_Bot.Util;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Evel_Bot.Modules
{
    /// <summary>
    /// A module that say a Welcome message to new Users.
    /// </summary>
    class Welcome : IModule, IJsonConfiguration<string>
    {
        public bool IsActivated { get; set; }
        public string DefaultConfig => "Welcome on %server%'s Discord, %user% !";

        private string ConfigPath { get; } = Module.GetPath("welcome.json");
        private string WelcomeMessage { get; set; }

        public void Activate()
        {
            WelcomeMessage = this.LoadConfig();

            Program.Client.UserJoined += OnJoin;
        }

        public void Desactivate()
        {
            Program.Client.UserJoined -= OnJoin;
        }

        // Say Welcome to new users
        private async Task OnJoin(SocketGuildUser user)
        {
            SocketTextChannel channel = (user.Guild.TextChannels.OrderBy(x => x.Position).FirstOrDefault(x => x.GetUser(Program.Client.CurrentUser.Id).GuildPermissions.SendMessages));
            if (channel == null || channel == default(SocketTextChannel))
                return;

            await channel.SendMessageAsync(WelcomeMessage.Replace("%user%", user.Mention).Replace("%server%", channel.Guild.Name));
        }
    }
}
