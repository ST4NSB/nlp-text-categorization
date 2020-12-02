using Preprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LearningModel;
using System.Linq;
using NLP;
using CategoryConverter;
using Evaluation;
using Newtonsoft.Json;
using LearningSection;
using LearningSection.Data;

namespace NLP.TextCategorization
{
    public class TextCategorization
    {
        private ILearningAlgorithm _learningAlg;
        private XmlDocument _xmlDoc;
        private TextModel _textModel;
        private string _path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).
                                    Parent.Parent.FullName, @"TextCategorization\");

        public TextCategorization()
        {
            _xmlDoc = new XmlDocument();
            _textModel = new TextModel();
        }

        public void Process(LearningType type, string dirpath, bool wipeCheckpoints = false)
        {
            if (wipeCheckpoints)
            {
                WipeCheckpoints();
            }

            var checkpoints = LoadCheckpointsAndModel();

            foreach (var fullText in GetAllFilesFromDir(dirpath))
            {
                var fileName = fullText.Split('\\').Last();

                if (checkpoints.Contains(fileName))
                {
                    Console.WriteLine("Already processed " + fullText);
                    continue;
                }
                _xmlDoc.Load(fullText);
                var categories = ParseCategories();
                var textLines = ParseTitleAndText();
                var processedList = Pipeline.Create(textLines, filter: new string[] { "NN" }, resultMinLength: 2);

                ProcessTextModel(processedList, categories);
                Console.WriteLine("Processed " + fullText);

                SaveCheckpointModel(fileName);
            }

            ShuffleData();
            var training = new TextModel();
            var testing = new TextModel();

            SplitData(training, testing, testingSize: 30);

            _learningAlg = LearningFactory.Create(type);
            _learningAlg.Fit(training);
            var res = _learningAlg.Evaluate(testing);

            Console.WriteLine(EvaluationPhase.GetAccuracy(res));
        }

        private void ShuffleData()
        {
            _textModel.CategoryClassifierObject = _textModel.CategoryClassifierObject.OrderBy(x => Guid.NewGuid()).ToList(); // shuffle
        }

        private void SplitData(TextModel training, TextModel testing, int testingSize)
        {
            var testingSampleSize = (int) Math.Round((_textModel.CategoryClassifierObject.Count * testingSize) / 100.0d);

            testing.GlobalWords = _textModel.GlobalWords;
            training.GlobalWords = _textModel.GlobalWords;

            testing.CategoryClassifierObject = _textModel.CategoryClassifierObject.Take(testingSampleSize).ToList();
            training.CategoryClassifierObject = _textModel.CategoryClassifierObject.Skip(testingSampleSize).ToList();
        }

        public void WipeCheckpoints()
        {
            var dir = new DirectoryInfo(_path + @"files\model");

            foreach (var fi in dir.GetFiles())
            {
                fi.Delete();
            }

            var dirProcessed = new DirectoryInfo(_path + @"files\model\processed");

            foreach (var fi in dirProcessed.GetFiles())
            {
                fi.Delete();
            }

            File.Create(_path + @"files\model\checkpoints.txt");
        }

        private string[] LoadCheckpointsAndModel()
        {
            var checkpoints = File.ReadAllLines(_path + @"files\model\checkpoints.txt");

            if (checkpoints.Any())
            {
                var global = File.ReadAllText(_path + @"files\model\global.json");
                var local = File.ReadAllText(_path + @"files\model\local.json");

                _textModel.GlobalWords = JsonConvert.DeserializeObject<List<string>>(global);
                _textModel.CategoryClassifierObject = JsonConvert.DeserializeObject<List<TextModel.CategoryClassifier>>(local);
            }

            return checkpoints;
        }

        private void SaveCheckpointModel(string fileName)
        {
            using (StreamWriter writetext = File.AppendText(_path + @"files\model\checkpoints.txt"))
            {
                writetext.WriteLine(fileName);
            }

            var globalModel = JsonConvert.SerializeObject(_textModel.GlobalWords);
            var localModel = JsonConvert.SerializeObject(_textModel.CategoryClassifierObject);
            File.WriteAllText(_path + @"files\model\global.json", globalModel);
            File.WriteAllText(_path + @"files\model\local.json", localModel);

            SaveGlobalListOfWords(fileName);
            SaveAllDocumentsCategoryAndWordFrequence(fileName);
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

        private void SaveGlobalListOfWords(string fileName)
        {
            var globalListOutput = new List<string>();
            globalListOutput.Add("There are currently " + _textModel.GlobalWords.Count + " unique words in all documents!");
            foreach(var item in _textModel.GlobalWords)
            {
                globalListOutput.Add(_textModel.GlobalWords.IndexOf(item).ToString() + " : " + item);
            }
            CreateAndWriteToTextFile("global_" + fileName, globalListOutput);
        }

        private void SaveAllDocumentsCategoryAndWordFrequence(string fileName, bool beautify = true)
        {
            var globalListOutput = new List<string>();
            globalListOutput.Add("There are currently " + _textModel.CategoryClassifierObject.Count + " unique documents!");

            if (beautify)
            {
                int index = 1;
                foreach (var item in _textModel.CategoryClassifierObject)
                {
                    var categories = "Categories: ";
                    foreach (var cat in item.Categories)
                    {
                        categories += Converter.GetCategoryFullName(cat) + " , ";
                    }
                    var wordCount = "Word Count: \r\n";
                    foreach (var keypair in item.WordCount)
                    {
                        wordCount += "\t" + _textModel.GlobalWords[keypair.Key] + " : " + keypair.Value + "\r\n";
                    }
                    globalListOutput.Add(index++ + ")\r\n" + categories + "\r\n" + wordCount);
                }
            }
            else
            {
                foreach (var item in _textModel.CategoryClassifierObject)
                {
                    var categories = "";
                    foreach (var cat in item.Categories)
                    {
                        categories += cat + " , ";
                    }
                    var wordCount = "";
                    foreach (var keypair in item.WordCount)
                    {
                        wordCount += keypair.Key + " : " + keypair.Value + " , ";
                    }
                    globalListOutput.Add("Categories: " + categories + "\r\n" + "Word Count: " + wordCount);
                }
            }
          
            CreateAndWriteToTextFile("local_" + fileName, globalListOutput);
        }

        public string GetWordForIndex(int index)
        {
            return _textModel.GlobalWords[index];
        }

        private List<string> ParseTitleAndText()
        {
            var title = _xmlDoc.SelectSingleNode("/newsitem/headline").InnerText;
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
            var fullpath = Path.Combine(_path, @"files\dataset\", dirpath);
            var fileEntries = Directory.GetFiles(fullpath);

            foreach (var fileName in fileEntries)
            {
                yield return fileName;
            }
        }

        private void CreateAndWriteToTextFile(string filename, List<string> text)
        {
            var date = DateTime.Now.ToString().Replace('/', '-')
                                              .Replace(':', '-')
                                              .Replace(' ', '_');
            var guid = _path + @"files\model\processed\[" + date + "]" + filename + ".txt";
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
