using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLP;

namespace Preprocessing.FeatureSelection
{
    public class PartOfSpeech
    {
        private static PartOfSpeechModel ReadModelFiles()
        {
            var dataFilepath = @"packages\PartofSpeech\data\";
            var modelsPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, dataFilepath);

            var unigram = File.ReadAllText(modelsPath + "unigram.json");
            var bigram = File.ReadAllText(modelsPath + "bigram.json");
            var trigram = File.ReadAllText(modelsPath + "trigram.json");
            var capitalizedPrefix = File.ReadAllText(modelsPath + "capitalizedPrefix.json");
            var nonCapitalizedPrefix = File.ReadAllText(modelsPath + "nonCapitalizedPrefix.json");
            var capitalizedSuffix = File.ReadAllText(modelsPath + "capitalizedSuffix.json");
            var nonCapitalizedSuffix = File.ReadAllText(modelsPath + "nonCapitalizedSuffix.json");
            var emission = File.ReadAllText(modelsPath + "emission.json");
            var emissionWithCapital = File.ReadAllText(modelsPath + "emissionWithCapital.json");

            var unigramFreq = JsonConvert.DeserializeObject<Dictionary<string, int>>(unigram);
            var bigramNonConverted = JsonConvert.DeserializeObject<Dictionary<string, int>>(bigram);
            var trigramNonConverted = JsonConvert.DeserializeObject<Dictionary<string, int>>(trigram);
            var capitalizedPrefixProb = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionProbabilisticModel>>(capitalizedPrefix);
            var nonCapitalizedPrefixProb = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionProbabilisticModel>>(nonCapitalizedPrefix);
            var capitalizedSuffixProb = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionProbabilisticModel>>(capitalizedSuffix);
            var nonCapitalizedSuffixProb = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionProbabilisticModel>>(nonCapitalizedSuffix);
            var emissionFreq = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionModel>>(emission);
            var emissionWithCapitalFreq = JsonConvert.DeserializeObject<List<PartOfSpeechModel.EmissionModel>>(emissionWithCapital);

            Dictionary<Tuple<string, string>, int> bigramFreq = new Dictionary<Tuple<string, string>, int>();
            Dictionary<Tuple<string, string, string>, int> trigramFreq = new Dictionary<Tuple<string, string, string>, int>();

            foreach (var item in bigramNonConverted)
            {
                string[] split = item.Key.Split(',');
                var charsToRemove = new string[] { "(", ")", " " };
                foreach (var c in charsToRemove)
                {
                    split[0] = split[0].Replace(c, string.Empty);
                    split[1] = split[1].Replace(c, string.Empty);
                }
                bigramFreq.Add(new Tuple<string, string>(split[0], split[1]), item.Value);
            }

            foreach (var item in trigramNonConverted)
            {
                string[] split = item.Key.Split(',');
                var charsToRemove = new string[] { "(", ")", " " };
                foreach (var c in charsToRemove)
                {
                    split[0] = split[0].Replace(c, string.Empty);
                    split[1] = split[1].Replace(c, string.Empty);
                    split[2] = split[2].Replace(c, string.Empty);
                }
                trigramFreq.Add(new Tuple<string, string, string>(split[0], split[1], split[2]), item.Value);
            }

            PartOfSpeechModel model = new PartOfSpeechModel(emissionFreq, emissionWithCapitalFreq, unigramFreq, bigramFreq, trigramFreq,
                nonCapitalizedSuffixProb, nonCapitalizedPrefixProb, capitalizedSuffixProb, capitalizedPrefixProb);
            return model;
        }

        public static IEnumerable<Tokenizer.WordTag> GetPartsOfSpeechOfSentence(string sentence)
        {
            var model = ReadModelFiles();

            var decoder = new NLP.Decoder();
            var preprocessedInput = Tokenizer.TokenizeSentenceWords(sentence);

            preprocessedInput = TextPreprocessing.PreProcessingPipeline(preprocessedInput);
            model.CalculateHiddenMarkovModelProbabilitiesForTestCorpus(preprocessedInput, model: "trigram");

            List<Tokenizer.WordTag> inputTest = new List<Tokenizer.WordTag>();
            foreach (var item in preprocessedInput)
            {
                if (item == "." || item == "!" || item == "?")
                    inputTest.Add(new Tokenizer.WordTag(item, "."));
                else inputTest.Add(new Tokenizer.WordTag(item, ""));
            }
            if(inputTest.Count >= 1)
            {
                if (inputTest[inputTest.Count - 1].tag != ".") // safe case check
                    inputTest.Add(new Tokenizer.WordTag(".", "."));
            }

            var sentences = new List<List<Tokenizer.WordTag>>();
            var sentc = new List<Tokenizer.WordTag>();
            foreach(var item in inputTest)
            {
                sentc.Add(item);

                if (string.Equals(item.tag, "."))
                {
                    if (sentc.Count > 1)
                    {
                        sentences.Add(sentc);
                    }
                    sentc = new List<Tokenizer.WordTag>();
                    continue;
                }
            }

            foreach(var input in sentences)
            {
                decoder.ViterbiDecoding(model, input, modelForward: "trigram", modelBackward: "trigram", mode: "f+b");

                for (int i = 0; i < decoder.PredictedTags.Count; i++)
                {
                    if (string.Equals(input[i].tag, "."))
                        continue;
                    input[i] = new Tokenizer.WordTag(input[i].word, decoder.PredictedTags[i]);

                    yield return input[i];
                }
            }
        }
    }
}
