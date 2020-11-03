using System;
using System.Collections.Generic;
using System.IO;

namespace Preprocessing.Cleaning
{
    public static class Stopwords
    {
        public static IEnumerable<string> GetStopWordsList()
        {
            var stopWords = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).
                                       Parent.Parent.FullName, @"Preprocessing\Cleaning\stopwords.txt");

            return File.ReadAllLines(stopWords);
        }
    }
}
