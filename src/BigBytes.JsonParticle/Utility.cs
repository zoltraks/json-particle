using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BigBytes.JsonParticle
{
    /// <summary>
    /// 
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Converts TOML section name to its C# counterpart.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string TomlSectionNameToCSharpName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            IList<string> list = new List<string>();

            var pattern = @"[\p{L}|\p{N}]+";
            var match = Regex.Match(name, pattern, RegexOptions.None);
            while (match.Success)
            {
                try
                {
                    var part = match.Value;
                    if (string.IsNullOrEmpty(part))
                    {
                        Debug.Write(null);
                    }
                    else
                    {
                        list.Add(part);
                    }
                }
                finally
                {
                    match = match.NextMatch();
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s.Length >= 2 && Char.IsUpper(s[0]) && Char.IsUpper(s[1]))
                {
                }
                else
                {
                    s = list[i].Substring(0, 1).ToUpperInvariant() + list[i].Substring(1).ToLowerInvariant();
                    if (s[0] >= '0' && s[0] <= '9')
                    {
                        s = "_" + s;
                    }
                }
                list[i] = s;
            }

            var result = string.Join("", list);
            return result;
        }

        /// <summary>
        /// Converts C# name to its JSON counterpart.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CSharpNameToJsonName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            if (name.Length >= 2 && Char.IsUpper(name[0]) && Char.IsUpper(name[1]))
            {
                return name;
            }

            var result = name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
            return result;
        }

        /// <summary>
        /// Returns current date and time string with milliseconds.
        /// </summary>
        /// <returns></returns>
        public static object Now() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
