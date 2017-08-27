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
    class Hello : IModule //? A Module that say Hello and GoodBye
    {
        private List<string> Words = new List<string>();
        private string ConfigPath { get; } = Module.GetPath("hello.json");
        public bool IsActivated { get; set; }

        public void Activate()
        {
            try
            {
                if (!File.Exists(ConfigPath)) // Don't try to load config if file don't exist
                {
                    Words = new List<string>() { "Bonjour", "Hello", "Hi", "Hey", "Salut", "Wesh", "Wsh", "Yo", "Yosh", "Hola", "Yolo", "Yop", "Bjr", "Slt", "Lu", "Cya", "Bye", "Goodbye", "Aurevoir", "A plus", "Bonne soirée", "Bonne nuit", "a+", "++" };
                    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Words, Formatting.Indented));
                }
                else
                    Words = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigPath));
            }
            catch (Exception e)
            {
                Shell.WriteLineError("Can't acces to " + ConfigPath);
                Shell.WriteLineError(e.Message);
                Desactivate();
            }
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

        private async Task CheckHello(SocketMessage msg)
        {
            if (msg.Author.IsBot)
                return;

            string word = await Task.Run( () => (   from str in Words
                                                    where msg.Content.StartsWith(str, StringComparison.OrdinalIgnoreCase)
                                                    select str).FirstOrDefault());

            if (word != null)
                await msg.Channel.SendMessageAsync(word + ", " + msg.Author.Username);
        }

        /*x Unused
        class Store //! Store the 2Lists.
        {
            public List<string> HelloWords = new List<string>();
            public List<string> ByeWords = new List<string>();
        }
        */
    }
}
