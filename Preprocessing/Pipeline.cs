using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Preprocessing.Normalization;
using Preprocessing.Cleaning;
using System.Linq;

namespace Preprocessing
{
    public class Pipeline
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


        public static IEnumerable<string> Create(string text)
        {
            // TODO
            // part of speech + tolower + lemma then return
            foreach (var token in Tokenize(text))
            {
                string lowerToken = token.ToLower(); // Can = can
                if (!Stopwords.GetStopWordsList().Contains(lowerToken))
                {
                    string stemWord = new PorterStemmer().StemWord(lowerToken);
                    yield return stemWord;
                }
            }
        }
    }
}
