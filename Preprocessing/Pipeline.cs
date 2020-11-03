using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Preprocessing.Normalization;

namespace Preprocessing
{
    public class Pipeline
    {
        public static IEnumerable<string> Create(string text)
        {
            // TODO
            // part of speech + tolower + lemma then return
            var nextText = text.ToLower();
            var splitted = nextText.Split(' ', '.');
            foreach(var item in splitted)
            {
                if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                {
                    yield return item;
                }
            }
        }
    }
}
