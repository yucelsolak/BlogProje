using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Core.Helpers
{
    public static class StringHelper
    {
        public static string StripHtmlTagsAndDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 1. HTML taglerini temizle
            string noHtml = Regex.Replace(input, "<.*?>", string.Empty);

            // 2. HTML entity'lerini decode et (örn: &uuml; => ü)
            string decoded = HttpUtility.HtmlDecode(noHtml);

            return decoded;
        }
    }
}
