using Discord;
using Discord.WebSocket;
using Evel_Bot.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evel_Bot.Modules
{
    class ClearChat : IModule //? A Module to clear the chat.
    {
        public bool IsActivated { get; set; }

        public void Activate()
        {
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        public void Desactivate()
        {
            Program.Client.MessageReceived -= Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage msg) //! Handle the Event
        {
            string message = msg.Content;
            string[] split = message.Split(' ');

            if (message.StartsWithOne(StringComparison.OrdinalIgnoreCase, "!clear", "!delete", "!remove"))
            {
                if (split.Length < 2)
                {
                    await msg.Channel.SendEmbed(EmbedTemplates.Error, $"Invalid use, try with \"{split[0]} <number>\"");
                    return;
                }

                SocketGuildUser user = msg.Author as SocketGuildUser; //Cast SocketUser to get permissions, this normally won't work.
                if (user.GuildPermissions.ManageMessages == false)
                {
                    await msg.Channel.SendEmbed(EmbedTemplates.Forbidden, "You don't have the permission remove messages.");
                    return;
                }

                int count = int.Parse(split[1]);
                await DeleteMessages(msg.Channel , count);

                await msg.Channel.SendEmbed(EmbedTemplates.Info, $"{msg.Author.Username} removed {count} messages");
            }
        }

        public static async Task DeleteMessages(ISocketMessageChannel channel, int count) //! Delete N messages in a ISocketMessageChannel
        {
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = channel.GetMessagesAsync(count + 1);
            IAsyncEnumerator<IReadOnlyCollection<IMessage>> enumerator = messages.GetEnumerator();

            while (await enumerator.MoveNext())
            {
                foreach (IMessage msg in enumerator.Current)
                {
                    if (msg.IsPinned)
                        continue;
                    await msg.DeleteAsync();
                }
            }
        }
    }
}
