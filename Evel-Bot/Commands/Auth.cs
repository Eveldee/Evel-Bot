using Evel_Bot.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Evel_Bot.Commands
{
    partial class Command
    {

        public static void Auth() //? Auth Command
        {

            void OpenBrowser(string url) //? Picked from "https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/".
            {
                try
                {
                    Process.Start(url);
                }
                catch
                {
                    //! hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        url = url.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Process.Start("xdg-open", url);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Process.Start("open", url);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            if (Program.Client.TokenType != Discord.TokenType.Bot)
            {
                Shell.WriteLineError("This command can only be used with a bot account.");
                return;
            }

            string id = Program.Client.CurrentUser.Id.ToString();
            OpenBrowser("https://discordapp.com/api/oauth2/authorize?client_id=" + id + "&scope=bot&permissions=0");
        }

    }
}
