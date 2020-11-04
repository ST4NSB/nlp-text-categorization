using System;
using System.Collections.Generic;
using Preprocessing.Normalization;
using Preprocessing.Cleaning;
using System.Linq;
using Preprocessing.FeatureSelection;

namespace Preprocessing
{
    public static class Pipeline
    {
        private static List<string> Tokenize(string input)
        {
            List<string> wordsOutput = new List<string>();
            string word = "";
            foreach (char c in input)
            {
                if (Char.IsLetter(c)) // || Char.IsDigit -> ?
                    word += c;
                else
                {
                    if (word != "\r\n" && word != "\t" &&
                        word != "" && word != " " && word != null)
                        wordsOutput.Add(word);
                    word = "";
                }
            }
            if (word.Length > 0) wordsOutput.Add(word); // daca a mai ramas ceva (nu s-a pus punct la sfarsit)
            return wordsOutput;
        }

        public static IEnumerable<string> Create(List<string> nodes, string[] filter, int resultMinLength)
        {
            // old code
            //foreach (var node in nodes)
            //{
            //    foreach (var token in Tokenize(node))
            //    {
            //        string lowerToken = token.ToLower(); // Can = can
            //        if (!Stopwords.GetStopWordsList().Contains(lowerToken))
            //        {
            //            string stemWord = new PorterStemmer().StemWord(lowerToken);
            //            yield return stemWord;
            //        }
            //    }
            //}

            foreach (var text in nodes)
            {
                var posSentence = PartOfSpeech.GetPartsOfSpeechOfSentence(text);
                foreach (var posWord in posSentence)
                {
                    Console.WriteLine("!!!!!                         " + posWord.word + " -> " + posWord.tag);
                    if (filter.Contains(posWord.tag)
                        && posWord.word.Length > resultMinLength)   // FILTER = TAG
                    {
                        yield return posWord.word.ToLower()
                                                 .Lemmatize()
                                                 .RemoveApostrof();
                    }
                }
            }
        }
    }
}
