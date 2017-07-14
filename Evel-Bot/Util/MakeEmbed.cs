using Discord;
using System.Collections.Generic;

namespace Evel_Bot.Util
{
    static class MakeEmbed //? A Class to Create Embed easily
    {
        public static EmbedBuilder TestEmbed { get; } = new EmbedBuilder() //! A test Embed
        {
            Author = new EmbedAuthorBuilder() { Name = "Author-Eveldee", IconUrl = "https://image.noelshack.com/fichiers/2017/25/5/1498214428-evel-bot.png", Url = "https://www.youtube.com" },
            Color = new Color(124, 48, 0),
            Title = "The title, link with URL",
            Description = "Some description, on right is the ThumbnailUrl",
            Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder() {IsInline = true, Name = "first Field", Value = "a value"},
                new EmbedFieldBuilder() {IsInline = false, Name = "Second Field, under is the \"ImageURL\"", Value = "a value"}
            },
            Footer = new EmbedFooterBuilder() { Text = "An EmbedFooter", IconUrl = "https://image.noelshack.com/fichiers/2017/25/5/1498214428-evel-bot.png" },
            Url = "https://www.youtube.com",
            ImageUrl = "https://image.noelshack.com/fichiers/2017/25/5/1498214428-evel-bot.png",
            ThumbnailUrl = "https://image.noelshack.com/fichiers/2017/25/5/1498214428-evel-bot.png",
        };

        public static Embed SimpleEmbed(Color color ,string title, string description = null)
        {
            EmbedBuilder custom = new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
            };

            return custom.Build();
        }

        public static Embed ImageEmbed(Color color, string title, string description, string imageURl)
        {
            EmbedBuilder custom = new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
                ImageUrl = imageURl,
            };

            return custom.Build();
        }

        public static Embed ImageEmbed(Color color, string title, string description, string imageURL, string thumbnailURL)
        {
            return new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
                ImageUrl = imageURL,
                ThumbnailUrl = thumbnailURL,
            };
        }

        public static Embed AuthorEmbed(Color color, string title, string description, EmbedAuthorBuilder author)
        {
            return new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
                Author = author,
            };
        }

        public static Embed FieldsEmbed(Color color, string title, string description, List<EmbedFieldBuilder> fields)
        {
            return new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
                Fields = fields,
            };
        }

        public static Embed CustomEmbed(Color color, string title = null, string description = null, string imageURL = null, string thumbnailUrl = null, string titleURl = null, List<EmbedFieldBuilder> fields = null, EmbedAuthorBuilder author = null, EmbedFooterBuilder footer = null)
        {
            EmbedBuilder builder = new EmbedBuilder()
            {
                Color = color,
                Title = title,
                Description = description,
                ImageUrl = imageURL,
                ThumbnailUrl = thumbnailUrl,
                Url = titleURl,
                Author = author,
                Footer = footer,
            };
            if (fields != null && fields.Count > 0)
                builder.Fields = fields;

            return builder.Build();
        }

        /// <summary>
        /// Return a List of <see cref="EmbedFieldBuilder"/>
        /// </summary>
        /// <param name="field">A List of <see cref="string"/> in format "Name§Value"</param>
        /// <returns></returns>
        public static List<EmbedFieldBuilder> FieldBuilder(params string[] fields) //Create Fields in a easier way
        {
            List<EmbedFieldBuilder> list = new List<EmbedFieldBuilder>();

            foreach (string str in fields)
            {
                string[] split = str.Split('§');
                if (split.Length == 1)
                    list.Add(new EmbedFieldBuilder() { Name = split[0], Value = null });
                else
                    list.Add(new EmbedFieldBuilder() { Name = split[0], Value = split[1] });
            }
            return list;
        }
    }

    public struct EmbedTemplate //? Some default Embed to keep a certain Logic.
    {
        public static Embed Error(string message)
        {
            return MakeEmbed.AuthorEmbed(new Color(255, 0, 0), null, message, new EmbedAuthorBuilder() { Name = "Error", IconUrl = "https://image.prntscr.com/image/1tlt8aj7RY_ywP-OPPivyg.png" });
        }
        public static Embed Forbidden(string message)
        {
            return MakeEmbed.AuthorEmbed(new Color(255, 0, 0), null, message, new EmbedAuthorBuilder() { Name = "Forbidden", IconUrl = "https://image.prntscr.com/image/qruuEFfGQ7OVr5c8MP1r-w.png" });
        }
        public static Embed Info(string message)
        {
            return MakeEmbed.AuthorEmbed(new Color(52, 152, 219), null, message, new EmbedAuthorBuilder() { Name = "Info", IconUrl = "https://image.prntscr.com/image/_LqlLoxiSFWEnH8L18IJ8Q.png" });
        }
    }
}
