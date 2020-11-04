using Preprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LearningModel;
using System.Linq;
using NLP;

namespace NLP.TextCategorization
{
    public class TextCategorization
    {
        private XmlDocument _xmlDoc;
        private TextModel _textModel;
        private string _path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).
                                    Parent.Parent.FullName, @"TextCategorization\");

        //TODO: GET GET TRUE CATEGORY

        // + BEAUTIFY SETUP FOR output and json saved models (if doesnt find the json then process)












        public TextCategorization()
        {
            _xmlDoc = new XmlDocument();
            _textModel = new TextModel();
        }

        public void Process(string dirpath)
        {
            foreach (var fullText in GetAllFilesFromDir(dirpath))
            {
                _xmlDoc.Load(fullText);
                var categories = ParseCategories();
                var textLines = ParseTitleAndText();
                var processedList = Pipeline.Create(textLines, filter: new string[] { "NN" }, resultMinLength: 2);

                ProcessTextModel(processedList, categories);
                Console.WriteLine("Processed " + fullText);
            }

            // save in file
            ShowGlobalListOfWords();
            ShowAllDocumentsCategoryAndWordFrequence();
        }

        private void ProcessTextModel(IEnumerable<string> preProcessedList, string categories)
        {
            var catList = categories.Split(',')
                                    .Select(tag => tag.Trim())
                                    .Where(tag => !string.IsNullOrEmpty(tag));

            TextModel.CategoryClassifier catCl = new TextModel.CategoryClassifier();

            catCl.Categories.AddRange(catList);

            foreach(var item in preProcessedList)
            {
                if (!_textModel.GlobalWords.Contains(item))
                {
                    _textModel.GlobalWords.Add(item);
                }

                int wordIndex = _textModel.GlobalWords.IndexOf(item);

                if (catCl.WordCount.ContainsKey(wordIndex))
                {
                    catCl.WordCount[wordIndex] += 1;
                }
                else
                {
                    catCl.WordCount.Add(wordIndex, 1);
                }
            }

            _textModel.CategoryClassifierObject.Add(catCl);
        }

        public List<string> ShowGlobalListOfWords()
        {
            var globalListOutput = new List<string>();
            globalListOutput.Add("There are currently " + _textModel.GlobalWords.Count + " unique words in all documents!");
            foreach(var item in _textModel.GlobalWords)
            {
                globalListOutput.Add(_textModel.GlobalWords.IndexOf(item).ToString() + " : " + item);
            }
            CreateAndWriteToTextFile("global", globalListOutput);
            return globalListOutput;
        }

        public string GetWordForIndex(int index)
        {
            return _textModel.GlobalWords[index];
        }

        public List<string> ShowAllDocumentsCategoryAndWordFrequence()
        {
            var globalListOutput = new List<string>();
            globalListOutput.Add("There are currently " + _textModel.CategoryClassifierObject.Count + " unique documents!");
            foreach(var item in _textModel.CategoryClassifierObject)
            {
                var categories = "";
                foreach(var cat in item.Categories)
                {
                    categories += cat + " , ";
                }
                var wordCount = "";
                foreach(var keypair in item.WordCount)
                {
                    wordCount += keypair.Key + " : " + keypair.Value + " , ";
                }
                globalListOutput.Add("Categories: " + categories + "\r\n" + "Word Count: " + wordCount);
            }
            CreateAndWriteToTextFile("local", globalListOutput);
            return globalListOutput;
        }

        private List<string> ParseTitleAndText()
        {
            var title = _xmlDoc.SelectSingleNode("/newsitem/title").InnerText;
            var textXML = _xmlDoc.SelectNodes("/newsitem/text/p");

            var parsedText = new List<string>();
            if(!string.IsNullOrEmpty(title.Trim()))
            {
                parsedText.Add(title);
            }

            for(var i = 0; i < textXML.Count; ++i)
            {
                if (!string.IsNullOrEmpty(textXML[i].InnerText.Trim()))
                {
                    parsedText.Add(textXML[i].InnerText);
                }
            }

            return parsedText;
        }

        private string ParseCategories()
        {
            var codesXML = _xmlDoc.SelectSingleNode("//codes[@class='bip:topics:1.0']").ChildNodes;

            var parsedCategories = "";
            foreach(XmlNode code in codesXML)
            {
                parsedCategories += code.Attributes["code"].Value + ",";
            }
            return parsedCategories;
        }

        private IEnumerable<string> GetAllFilesFromDir(string dirpath)
        {
            var fullpath = Path.Combine(_path, @"files\", dirpath);
            var fileEntries = Directory.GetFiles(fullpath);

            foreach (var fileName in fileEntries)
            {
                yield return fileName;
            }
        }

        private void CreateAndWriteToTextFile(string filename, List<string> text)
        {
            var date = DateTime.Now.ToString();
            date = date.Replace('/', '-');
            date = date.Replace(':', '-');
            date = date.Replace(' ', '_');
            var guid = _path + @"output\[" + date + "]" + filename + ".txt";
            using (TextWriter tw = new StreamWriter(guid))
            {
                foreach (var item in text)
                {
                    tw.WriteLine(item);
                }
            }
        }
    }
}
