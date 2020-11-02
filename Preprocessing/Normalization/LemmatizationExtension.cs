using System.IO;
using LemmaSharp.Classes;

namespace Preprocessing.Normalization
{
    static class LemmatizationExtension
    {
        public static string Lemmatize(this string word)
        {
            var dataFilepath = @"packages\LemmaGenerator.1.1.0\data\full7z-mlteast-en-modified.lem";
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, dataFilepath);
            var stream = File.OpenRead(path);
            var lemmatizer = new Lemmatizer(stream);
            var lemmaWord = lemmatizer.Lemmatize(word);
            return lemmaWord;
        }
    }
}
