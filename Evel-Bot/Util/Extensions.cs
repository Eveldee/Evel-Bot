using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evel_Bot.Util.Extensions //todo Namespace for Extensions
{
    public enum EmbedTemplates { Error, Forbidden, Info }

    public static class ArrayExtensions //? Extensions for Array type.
    {
        public static T[] SubArray<T>(this T[] arr, int index) //! Return a subpart of an Array
        {
            int lenght = arr.Length - index;
            T[] subarr = new T[lenght];
            Array.Copy(arr, index, subarr, 0, lenght);
            return subarr;
        }
        public static T[] SubArray<T>(this T[] arr, int index, int lenght) //! Return a subpart of an Array
        {
            T[] subarr = new T[lenght];
            Array.Copy(arr, index, subarr, 0, lenght);
            return subarr;
        }

        public static string Concat<T>(this IEnumerable<T> arr, string delim = "") //! Return all elementens of an Enumerable<> in one line.
        {
            string final = "";

            foreach (T str in arr)
                final += str.ToString() + delim;

            final = final.Remove(final.Length - delim.Length);
            return final;
        }
    }

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) //! Check if a string is Null or Empty (" "/"")
        {
            if (string.IsNullOrEmpty(str))
                return true;
            if (string.IsNullOrWhiteSpace(str))
                return true;
            return false;
        }
        public static bool EqualsOne(this string str, params string[] values) //! Return true if the string equal another one string in the Array
        {
            foreach (string s in values)
            {
                if (str == s)
                    return true;
            }
            return false;
        }
        public static bool EqualsOne(this string str, StringComparison comparisonType, params string[] values) //! Return true if the string equal another one string in the Array, Add ComparisonType
        {
            foreach (string s in values)
            {
                if (str.Equals(s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        public static bool StartsWithOne(this string str, params string[] values) //! Return true if the string Start with a a string in the Array
        {
            foreach (string s in values)
            {
                if (str.StartsWith(s))
                    return true;
            }
            return false;
        }
        public static bool StartsWithOne(this string str, StringComparison comparisonType, params string[] values) //! Return true if the string Start with a a string in the Array, Add Comparison Type
        {
            foreach (string s in values)
            {
                if (str.StartsWith(s, comparisonType))
                    return true;
            }
            return false;
        }
    }

    public static class ISocketMessageChannelExtensions
    {
        public static async Task SendEmbed(this ISocketMessageChannel channel, Embed embed)
        {
            await channel.SendMessageAsync("", false, embed);
        }
        public static async Task SendEmbed(this ISocketMessageChannel channel, EmbedTemplates template, string message)
        {
            switch (template)
            {
                case EmbedTemplates.Error:
                    await channel.SendMessageAsync("", false, EmbedTemplate.Error(message));
                    break;
                case EmbedTemplates.Forbidden:
                    await channel.SendMessageAsync("", false, EmbedTemplate.Forbidden(message));
                    break;
                case EmbedTemplates.Info:
                    await channel.SendMessageAsync("", false, EmbedTemplate.Info(message));
                    break;
            }
        }
    }
}
