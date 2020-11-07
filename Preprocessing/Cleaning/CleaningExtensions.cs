using System.Text.RegularExpressions;

namespace Preprocessing.Cleaning
{
    public static class CleaningExtensions
    {
        public static string RemoveApostrof(this string text)
        {
            return text.Replace("\'", string.Empty);
        }

        public static string RemoveLine(this string text)
        {
            return text.Replace("/", string.Empty);
        }

        public static string RemoveDuplicateWordConnectionLine(this string text)
        {
            return Regex.Replace(text, @"\-\-+", string.Empty);
        }
    }
}
