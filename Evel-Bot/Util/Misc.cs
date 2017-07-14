using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Evel_Bot.Util
{
    class Misc //? Some random methods.
    {
        public static string GetFilePath(params string[] path) //! Return the path of a File relative to app's directory
        {
            string final = Path.Combine(path);
            return Path.Combine(AppContext.BaseDirectory, final);
        }

    }
}
