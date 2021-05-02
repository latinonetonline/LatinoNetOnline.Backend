
using System.Text.RegularExpressions;

namespace LatinoNetOnline.Backend.Shared.Abstractions.Extensions
{
    public static class StringExtensions
    {
        public static bool HasFileInvalidChars(this string source)
        {
            return source.Contains('<') ||
                source.Contains('>') ||
                source.Contains(':') ||
                source.Contains('"') ||
                source.Contains('/') ||
                source.Contains('\\') ||
                source.Contains('|') ||
                source.Contains('*') ||
                source.Contains('?');
        }

        public static bool IsEmail(this string source)
        {
            string _regexPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            return !string.IsNullOrWhiteSpace(source) && Regex.IsMatch(source, _regexPattern)
;
        }

        public static bool IsPhone(this string source)
        {
            string _regexPattern = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";

            return !string.IsNullOrWhiteSpace(source) && Regex.IsMatch(source, _regexPattern)
;
        }
    }
}
