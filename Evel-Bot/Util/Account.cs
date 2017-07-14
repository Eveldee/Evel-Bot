using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace Evel_Bot.Util
{
    class Account
    {

        public DiscordSocketClient Client { get; set; } = new DiscordSocketClient();
        public bool IsConnected
        {
            get
            {
                if (Client.ConnectionState == ConnectionState.Connected)
                    return true;
                else
                    return false;
            }
        }

        public List<TokenId> Accounts { get; } = new List<TokenId>();
        public bool IsEmpty { get; }

        public Account()
        {
            ConfigurationFile Accounts = new ConfigurationFile(Path.Combine(AppContext.BaseDirectory, "accounts.config"));

            if (Accounts.IsEmpty)
            {
                IsEmpty = true;
                return;
            }

            foreach (Setting s in Accounts.GetAll())
            {
                string[] split = s.Value.Split(';');
                this.Accounts.Add(new TokenId(s.Key, split[1], split[0] == "Bot" ? TokenType.Bot : TokenType.User));
            }
        }

    }

    public struct TokenId //An object representation of an account.
    {
        public string Username { get; }
        public string Token { get; }
        public TokenType Type { get; }

        public TokenId(string name, string token, TokenType type)
        {
            this.Username = name;
            this.Token = token;
            this.Type = type;
        }
    }
}
