using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.ToLowerInvariant();
            value = ConvertTurkishCharacters(value); // Türkçe karakter dönüşümü

            value = Regex.Replace(value, @"[^a-z0-9\s-]", ""); // Sadece harf, rakam, boşluk, tire
            value = Regex.Replace(value, @"\s+", " ").Trim();
            value = value.Replace(" ", "-");

            return value;
        }

        private static string ConvertTurkishCharacters(string text)
        {
            var replacements = new Dictionary<char, char>
            {
                { 'ç', 'c' }, { 'ğ', 'g' }, { 'ı', 'i' }, { 'ö', 'o' },
                { 'ş', 's' }, { 'ü', 'u' }
            };

            var sb = new StringBuilder(text.Length);

            foreach (var c in text)
            {
                sb.Append(replacements.ContainsKey(c) ? replacements[c] : c);
            }

            return sb.ToString();
        }
    }
}
