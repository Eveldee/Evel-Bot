using Discord;
using Evel_Bot.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Evel_Bot.Modules
{
    class Log : IModule, IJsonConfiguration<bool> //! Log IModule
    {
        public static event LogEventArgs.LogEventHandler LogEvent;

        public bool IsActivated { get; set; }

        private static string DirPath { get; } = Module.GetPath("log");
        private static string LogPath { get; } = Path.Combine(DirPath, "log.txt");
        private static string ErrorPath { get; } = Path.Combine(DirPath, "error.txt");
        private static bool IsLoggingDiscord { get; set; }

        public bool DefaultConfig => true;

        public void Activate()
        {
            try // Check files
            {
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);

                if (!File.Exists(LogPath))
                    File.Create(LogPath).Close();

                if (!File.Exists(ErrorPath))
                    File.Create(ErrorPath).Close();

                IsLoggingDiscord = this.LoadConfig();
            }
            catch (IOException e)
            {
                Desactivate();
                this.LogError("A file can't be loaded.");
                this.LogError(e.Message);
            }

            LogEvent += Log_LogEvent;
            Program.Client.Log += Client_Log;
            Program.ShellEvent += Program_ShellEvent;
        }

        public void Desactivate()
        {
            LogEvent -= Log_LogEvent;
            Program.Client.Log -= Client_Log;
        }

        public static void SendLog(LogEventArgs e) // Static Method to call LogEvent
        {
            LogEvent?.Invoke(e);
        }
        public static void SendLog(string message, bool error = false) // Overload
        {
            LogEvent?.Invoke(new LogEventArgs(error == false ? LogEventType.Info : LogEventType.Error, message));
        }

        private void Log_LogEvent(LogEventArgs e) // LogEvent Handler
        {
            if (e.Type == LogEventType.Info)
                File.AppendAllText(LogPath, $"[{DateTime.Now.ToShortDateString()}]({DateTime.Now.ToLongTimeString()})|{e.Message}{Environment.NewLine}");
            else
                File.AppendAllText(ErrorPath, $"[{DateTime.Now.ToShortDateString()}]({DateTime.Now.ToLongTimeString()})|{e.Message}{Environment.NewLine}");
        }

        private Task Client_Log(LogMessage msg) // Redirect Discord logs to LogEvent
        {
            if ((int)msg.Severity > 2)
                SendLog(new LogEventArgs(LogEventType.Info, "[Discord] " + msg.Source + " : " + msg.Message));
            else
                SendLog(new LogEventArgs(LogEventType.Info, "[Discord] " + msg.Source + " : " + msg.Message));

            return Task.CompletedTask;
        }

        // Handle commands
        private async Task Program_ShellEvent(ShellEventArgs e)
        {
            string[] input = e.Input.Split(' ');

            // Check input
            if (input[0].ToLower() == "log")
            {
                e.Handled = true;

                switch(input[1].ToLower())
                {
                    case "clear":
                        await Clear();
                        break;
                    case "discord":
                        await DiscordLog();
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        private async Task Clear()
        {
            await File.WriteAllTextAsync(LogPath, "");
            await File.WriteAllTextAsync(ErrorPath, "");

            await this.LogAsync("Log cleared");
        }

        private async Task DiscordLog()
        {
            if (IsLoggingDiscord)
            {
                IsLoggingDiscord = false;
                await this.SaveConfigAsync(IsLoggingDiscord);

                Program.Client.Log -= Client_Log;
                await this.LogAsync("Discord log disabled");
            }
            else
            {
                IsLoggingDiscord = true;
                await this.SaveConfigAsync(IsLoggingDiscord);

                Program.Client.Log += Client_Log;
                await this.LogAsync("Discord log enabled");
            }
        }
    }

    public enum LogEventType { Info, Error } // Type for LogEvent

    class LogEventArgs //! Args for all Log events
    {
        public delegate void LogEventHandler(LogEventArgs e);

        public LogEventType Type { get; }
        public string Message { get; }

        public LogEventArgs(LogEventType type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
