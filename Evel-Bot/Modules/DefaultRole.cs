using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Evel_Bot.Util.Extensions;
using System.Linq;

namespace Evel_Bot.Modules
{
    class DefaultRole : IModule, IJsonConfiguration<Dictionary<string, ulong>>
    {
        public Dictionary<string, ulong> DefaultConfig => new Dictionary<string, ulong>();
        public bool IsActivated { get; set; }

        private Dictionary<string, ulong> DefaultRoles { get; set; }

        public void Activate()
        {
            DefaultRoles = this.LoadConfig();

            Program.Client.UserJoined += OnJoin;
            Program.Client.MessageReceived += OnMessage;
        }

        public void Desactivate()
        {
            Program.Client.UserJoined -= OnJoin;
            Program.Client.MessageReceived -= OnMessage;
        }

        private async Task OnJoin(SocketGuildUser user)
        {
            string guildName = user.Guild.Name;

            if (DefaultRoles.ContainsKey(guildName))
            {
                IRole role = user.Guild.GetRole(DefaultRoles[guildName]);

                if (role == null)
                {
                    this.LogError($"Default role {role.Name} for {guildName} doesn't exist anymore.");
                    return;
                }

                await user.AddRoleAsync(role);
                this.Log($"Added role {role.Name} to {user.Username}");
            }
        }

        private async Task OnMessage(SocketMessage msg)
        {
            if (msg.Content.StartsWith("!DefaultRole", StringComparison.OrdinalIgnoreCase))
            {
                string role = string.Join(' ', msg.Content.Split(' ').Skip(1));
                var user = msg.Author as SocketGuildUser;

                // Clear command
                if (role.ToLower() == "clear")
                {
                    DefaultRoles.Remove(user.Guild.Name);
                    await this.SaveConfigAsync(DefaultRoles);

                    await msg.Channel.SendEmbed(EmbedTemplates.Info, $"Default role removed");
                    this.Log($"Default role removed");
                }
                // Add default role command
                else
                {
                    if (user.GuildPermissions.ManageRoles)
                    {
                        if (role == null || role.Length < 1)
                        {
                            await msg.Channel.SendEmbed(EmbedTemplates.Error, "Invalid role name, try with: \"!DefautlRole role\"");
                            return;
                        }

                        var guildRole = user.Guild.Roles.FirstOrDefault(x => x.Name == role);
                        if (guildRole == null)
                        {
                            await msg.Channel.SendEmbed(EmbedTemplates.Error, "Invalid role name, try with: \"!DefautlRole role\"");
                            return;
                        }

                        DefaultRoles[user.Guild.Name] = guildRole.Id;
                        await this.SaveConfigAsync(DefaultRoles);
                        await msg.Channel.SendEmbed(EmbedTemplates.Info, $"{role} is now the default role");
                        this.Log($"{role} is now the default role for {user.Guild.Name}");
                    }
                    else
                    {
                        await msg.Channel.SendEmbed(EmbedTemplates.Forbidden, "You don't have the permission to manage roles.");
                        return;
                    }
                }

            }
        }
    }
}
