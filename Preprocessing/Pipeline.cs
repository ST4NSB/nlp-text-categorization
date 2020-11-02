using System;
using Preprocessing.Normalization;

namespace Preprocessing
{
    public class Pipeline
    {
        public static void Create()
        {
            var word = "ping";
            Console.WriteLine(word.Lemmatize());
        }
    }
}
