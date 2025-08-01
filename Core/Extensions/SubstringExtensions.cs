using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class SubstringExtensions
    {
        public static string ToShort(this string text, int length)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Length <= length ? text : text.Substring(0, length) + "...";
        }

        public static string ToWordShort(this string text, int wordCount)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var words = text.Split(' ');
            if (words.Length <= wordCount)
                return text;

            return string.Join(" ", words.Take(wordCount)) + "...";
        }

    }
}
