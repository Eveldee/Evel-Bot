using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using Evel_Bot.Util;
using Evel_Bot.Modules;
using System.Threading.Tasks;

namespace Evel_Bot.Commands
{
    static class Chat //? Provide message channel to cmd.
    {
        public static ISocketMessageChannel CurrentChannel { get; private set; }

        public static void Connect(ulong id) //! Connect to a channel by ID
        {
            ISocketMessageChannel channel = GetChannel(id);

            if (channel == null)
                return;

            CurrentChannel = channel;

            Loop();
        }

        public static void Connect(string name) //! Get and connect to a channel by Name
        {
            if (ulong.TryParse(name, out ulong id)) //Check if input is an ID.
            {
                Connect(id);
                return;
            }

            ISocketMessageChannel channel = GetChannel(name);

            if (channel == null)
                return;

            CurrentChannel = channel;

            Loop();
        }

        private static void Loop() //! Write in a channel
        {
            Shell.WriteLine(ConsoleColor.Cyan ,$"Connected to {CurrentChannel.Name}.");

            bool IsGcOn = Module.GetModule("GlobalChat").IsActivated;
            if (IsGcOn)
                Module.GetModule("GlobalChat").Desactivate();

            Program.Client.MessageReceived += Client_MessageReceived;

            while(true)
            {
                string input = Shell.Input();

                if (input == "!exit")
                    break;
                else if (input[0] == '!')
                    Command.ExeCommand(input.Substring(1)).GetAwaiter().GetResult();
                else
                    CurrentChannel.SendMessageAsync(input);
            }

            if (IsGcOn)
                Module.GetModule("GlobalChat").Activate();

            Program.Client.MessageReceived -= Client_MessageReceived;

            Shell.WriteLine(ConsoleColor.Cyan ,"Disconnected from chanel.");
        }

        public static ISocketMessageChannel GetChannel(string name) //! Get a channel by Name
        {
            SocketGuildChannel[] channels = (from guild in Program.Client.Guilds
                                             from channel in guild.Channels
                                             where channel is ISocketMessageChannel
                                             where channel.Name == name
                                             select channel).ToArray();

            if (channels == null || channels.Length == 0)
            {
                Shell.WriteLineError("Channel with the name " + name + " don't exist.");
                return null;
            }
            if (channels.Length == 1)
                return channels[0] as ISocketMessageChannel;
            else
            {
                Shell.WriteLine($"Found {channels.Length} channels, select which one you want to connect");

                int i = 0;
                foreach (SocketGuildChannel ch in channels)
                {
                    Shell.WriteLine($"{i}: {ch.Guild}/{ch.Name}");
                    i++;
                }
                string input = Shell.Input();

                if (!int.TryParse(input, out int index) || index > channels.Length)
                {
                    Shell.WriteLine("Invalid input.");
                    return null;
                }
                return channels[index] as ISocketMessageChannel;
            }
        }

        public static ISocketMessageChannel GetChannel(ulong id) //! Get a channel by ID
        {
            var channel = Program.Client.GetChannel(id);

            if (channel == null)
            {
                Shell.WriteLineError("Invalid ID, can't connect to channel");
                return null;
            }

            if (!(channel is ISocketMessageChannel))
            {
                Shell.WriteLineError("Error, can only connect to a TextChannel.");
                return null;
            }

            return channel as ISocketMessageChannel;
        }

        private static async Task Client_MessageReceived(SocketMessage msg)
        {
            if (msg.Channel.Id == CurrentChannel.Id && msg.Author.Id != Program.Client.CurrentUser.Id)
                await Shell.WriteLineAsync($"{msg.Author.Username}: {msg.Content}");
        }
    }
}
