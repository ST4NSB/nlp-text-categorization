using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
