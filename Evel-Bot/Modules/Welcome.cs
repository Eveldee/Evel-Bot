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
    class Welcome : IModule //? A module that say Welcome to new Users.
    {
        private List<string> KnowUsers = new List<string>(); //A string List in this format : Guild§Username
        private string ConfigPath { get; } = Module.GetPath("welcome.json");

        public bool IsActivated { get; set; }

        public void Activate() //! Get KnowUsers List
        {
            try
            {
                if (!File.Exists(ConfigPath)) // Don't try to load saved List if the file don't exist.
                    File.WriteAllText(ConfigPath, "[]");
                else
                {
                    using (TextReader stream = File.OpenText(ConfigPath))
                    {
                        JsonReader reader = new JsonTextReader(stream);
                        JsonSerializer serializer = new JsonSerializer();
                        KnowUsers = serializer.Deserialize<List<string>>(reader);
                    }
                }
            }
            catch (Exception e)
            {
                this.LogError("Can't open config file, please retry.");
                this.LogError(e.Message);
                Desactivate();
            }

            Program.Client.UserBanned += Client_UserBanned;
            Program.Client.UserLeft += Client_UserLeft;
            Program.Client.UserJoined += OnJoin;
        }

        public void Desactivate()
        {
            Program.Client.UserJoined -= OnJoin;
        }

        /*x private async Task Ini()
        {
            if (Program.ClientAccount.IsConnected)
                await CheckAll();

            else
                Program.Client.Ready += OnConnect;
        }

        private async Task OnConnect() //Wait that the client connect to Discord.
        {
            await CheckAll();
            Program.Client.Connected -= OnConnect;
        }*/

        /*x private async Task CheckAll() //! Get all connecteds Users and add them to KnowUsers List
        //{
        //    await Task.Run(() =>
        //    {
        //        foreach (SocketGuild guild in Program.Client.Guilds)
        //        {
        //            foreach (SocketGuildUser user in guild.Users)
        //            {
        //                if (user.Status == Discord.UserStatus.Offline)
        //                    continue;
        //                if (!IsKnow(guild.Name ,user.Username))
        //                    KnowUsers.Add((guild.Name, user.Username));
        //            }
        //        }
              });
        //    await Save();
        //}*/

        private Task Client_UserBanned(SocketUser user, SocketGuild guild) // Don't say welcome to banned users
        {
            KnowUsers.Add(guild.Name + "§" + user.Username);
            return Task.CompletedTask;
        }

        private Task Client_UserLeft(SocketGuildUser e) // Don't say welcome to users 2times
        {
            KnowUsers.Add(e.Guild.Name + "§" + e.Username);
            return Task.CompletedTask;
        }

        private async Task OnJoin(SocketGuildUser user) // Say Welcome to new users (Use some LINQ)
        {
            if (IsKnow(user.Guild.Name ,user.Username))
                return;

            SocketTextChannel channel = (user.Guild.TextChannels.OrderBy(x => x.Position).Where(x => x.GetUser(Program.Client.CurrentUser.Id).GuildPermissions.SendMessages)).FirstOrDefault();
            if (channel == null || channel == default(SocketTextChannel))
                return;

            await channel.SendMessageAsync($"Welcome on {channel.Guild.Name}'s Discord, {user.Mention}");
            KnowUsers.Add((user.Guild.Name + "§" +  user.Username));

            await Save();
        }

        private bool IsKnow(string guild, string username)
        {
            foreach (string str in KnowUsers)
            {
                string[] split = str.Split('§');
                string server = split[0]; string name = split[1];

                if (guild == server && username == name)
                    return true;
            }
            return false;
        }

        private async Task Save() //! Serialize to File
        {
            await Task.Run(() =>
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(ConfigPath))
                    {
                        JsonSerializer serializer = new JsonSerializer()
                        {
                            Formatting = Formatting.Indented
                        };
                        serializer.Serialize(writer, KnowUsers);
                    }
                }
                catch (Exception e)
                {
                    this.LogError("Can't open config file, please retry.");
                    this.LogError(e.Message);
                    Desactivate();
                }
            });
        }

        /*x Unused.
        //struct User //! User class to serialize.
        //{
        //    public string Name { get; set; }

        //    public User(string name)
        //    {
        //        Name = name;
        //    }
        }*/
    }
}
