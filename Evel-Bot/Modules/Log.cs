using Discord;
using Evel_Bot.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evel_Bot.Modules
{
    class Log : IModule //! Log IModule
    {
        public bool IsActivated { get; set; }

        public void Activate()
        {
            Program.Client.Log += Client_Log;
        }

        public void Desactivate()
        {
            Program.Client.Log -= Client_Log;
        }

        private async Task Client_Log(LogMessage msg)
        {
            await Shell.WriteLineAsync(msg.Source + " : " + msg.Message);
        }
    }
}
