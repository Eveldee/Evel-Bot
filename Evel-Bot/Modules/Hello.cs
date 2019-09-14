using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Evel_Bot.Util;

namespace Evel_Bot.Modules
{
    class Hello : IModule, IJsonConfiguration<List<string>> //? A Module that say Hello and GoodBye
    {
        public bool IsActivated { get; set; }
        public List<string> DefaultConfig => new List<string>() { "Bonjour", "Hello", "Hi", "Hey", "Salut", "Wesh", "Wsh", "Yo", "Yosh", "Hola", "Yolo", "Yop", "Bjr", "Slt", "Lu", "Cya", "Bye", "Goodbye", "Aurevoir", "A plus", "Bonne soirée", "Bonne nuit", "a+", "++" };
        public List<string> Words;

        private string ConfigPath { get; } = Module.GetPath("hello.json");
        private string LastWord { get; set; }


        public async void Activate()
        {
            Words = await this.LoadConfigAsync();
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        public void Desactivate()
        {
            Program.Client.MessageReceived -= Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            await CheckHello(arg);
        }

        private async Task CheckHello(SocketMessage msg) // Check if message is in HelloWords List
        {
            if (msg.Author.IsBot)
                return;

            string word = await Task.Run(() => (from str in Words
                                                where msg.Content.Equals(str, StringComparison.OrdinalIgnoreCase)
                                                select str).FirstOrDefault());

            if (word != null && word != LastWord)
            {
                await msg.Channel.SendMessageAsync(word + ", " + msg.Author.Username);
                LastWord = word;
            }
        }
    }
}
