using LearningModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LearningSection
{
    public class KNearestNeighbour : ILearningAlgorithm
    {
        private int ConstK;
        private int attrLength;

        private List<List<float>> TrainNormalizedVector;
        private List<List<float>> TestNormalizedVector;

        private List<List<string>> TrainClasses;
        private List<List<string>> TestClasses;

        private List<List<float>> KnnMatrix;
        public List<string> knnTestTagsClasses;

        public void Fit(TextModel learningModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PredictedModel> Evaluate(TextModel testModel)
        {
            throw new NotImplementedException();
        }

        public void SetK(int ConstK)
        {
            this.ConstK = ConstK;
        }

        public string GetKnnInformationsAsString()
        {
            string output = "";
            /*
            output += "Normalized Test Vector # Classes: \n";
            for (int i = 0; i < TestNormalizedVector.Count; i++)
            {
                foreach (var item in TestNormalizedVector[i])
                    output += item + "|";
                output += " # ";
                foreach (var item in TestClasses[i])
                    output += item + " ";
                output += "\n";
            }
            output += "\nNormalized Trained Vector # Classes: \n";
            for (int i = 0; i < TrainNormalizedVector.Count; i++) 
            {
                foreach (var item in TrainNormalizedVector[i])
                    output += item + "|";
                output += "# ";
                foreach (var item in TrainClasses[i])
                    output += item + " ";
                output += "\n";
            }
            
            output += "\nKNN Test(files) Matrix: \n";
            foreach (var line in KnnMatrix)
            {
                foreach (var item in line)
                    output += item + "|";
                output += "\n";
            }
            */
            output += "\nKNN Classes (for each test file vector): \n";
            int m = 1;
            foreach (var item in knnTestTagsClasses)
            {
                output += "[" + m + "]: " + item + "\n";
                m++;
            }  
            return output;
        }


        public List<List<string>> ReadFromFolder(string Folder, string FileExtension)
        {
            List<List<string>> knnMatrix = new List<List<string>>();
            foreach (string file in Directory.EnumerateFiles(Folder, FileExtension))
            {
                string line;
                StreamReader sR = new StreamReader(file);
                bool foundDataLine = false;
                bool foundAttributeSize = false;
                while ((line = sR.ReadLine()) != null)
                {
                    if (!foundAttributeSize)
                    {
                        string[] str = line.Split(' ');
                        if (str[0].Equals("#Attributes"))
                        {
                            this.attrLength = Convert.ToInt32(str[1]);
                            foundAttributeSize = true;
                        }
                    }
                    if(foundDataLine && !String.IsNullOrWhiteSpace(line))
                    {
                        List<string> splitLine = line.Split(' ').ToList();
                        knnMatrix.Add(splitLine);
                    }
                    if (line.Equals("@data"))
                        foundDataLine = true;
                }
                sR.Dispose();
            }
            return knnMatrix;
        }

        public void NominalNormalization(string type, List<List<string>> input)
        {
            if (type.Equals("train"))
            {
                TrainNormalizedVector = new List<List<float>>();
                TrainClasses = new List<List<string>>();
            }
            else if (type.Equals("test"))
            {
                TestNormalizedVector = new List<List<float>>();
                TestClasses = new List<List<string>>();
            }

            foreach (var list in input) 
            {
                int max = -1;
                List<float> normalizeList = new List<float>();
                List<int> SvmValues = new List<int>();
                int nr = 0;

                List<string> classesList = new List<string>();
                bool canGetClasses = false;
                foreach (var item in list)
                {
                    if(canGetClasses)
                    {
                        if(!string.IsNullOrWhiteSpace(item))
                            classesList.Add(item);
                        continue;
                    }

                    if (item.Equals("#"))
                    {
                        canGetClasses = true;
                        continue;
                    }

                    string[] splittedValue = item.Split(':');
                    int key = Convert.ToInt32(splittedValue[0]);
                    if (nr == key)
                    {
                        int value = Convert.ToInt32(splittedValue[1]);
                        SvmValues.Add(value);
                        if (value > max)
                            max = value;
                    }
                    else if (nr < key)
                    {
                        for (nr = nr; nr < key; nr++)
                            SvmValues.Add(0);
                        int value = Convert.ToInt32(splittedValue[1]);
                        SvmValues.Add(value);
                        if (value > max)
                            max = value;
                    }
                    else Console.WriteLine("You shouldn't be here");
                    nr++;
                }

                for (int i = nr; i < this.attrLength; i++)
                    SvmValues.Add(0);

               // Console.WriteLine("max: " + max);

                foreach(var item in SvmValues)
                {
                    float val = (float)item / max;
                    normalizeList.Add(val);
                }

                if (type.Equals("train"))
                {
                    TrainNormalizedVector.Add(normalizeList);
                    TrainClasses.Add(classesList);
                }
                else if (type.Equals("test"))
                {
                    TestNormalizedVector.Add(normalizeList);
                    TestClasses.Add(classesList);
                }
            }
        }

        public void SumNormalization(string type, List<List<string>> input)
        {
            if (type.Equals("train"))
            {
                TrainNormalizedVector = new List<List<float>>();
                TrainClasses = new List<List<string>>();
            }
            else if (type.Equals("test"))
            {
                TestNormalizedVector = new List<List<float>>();
                TestClasses = new List<List<string>>();
            }

            foreach (var list in input)
            {
                int sum = 0;
                List<float> normalizeList = new List<float>();
                List<int> SvmValues = new List<int>();
                int nr = 0;

                List<string> classesList = new List<string>();
                bool canGetClasses = false;
                foreach (var item in list)
                {
                    if (canGetClasses)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                            classesList.Add(item);
                        continue;
                    }

                    if (item.Equals("#"))
                    {
                        canGetClasses = true;
                        continue;
                    }

                    string[] splittedValue = item.Split(':');
                    int key = Convert.ToInt32(splittedValue[0]);
                    if (nr == key)
                    {
                        int value = Convert.ToInt32(splittedValue[1]);
                        SvmValues.Add(value);
                        sum += value;
                    }
                    else if (nr < key)
                    {
                        for (nr = nr; nr < key; nr++)
                            SvmValues.Add(0);
                        int value = Convert.ToInt32(splittedValue[1]);
                        SvmValues.Add(value);
                        sum += value;
                    }
                    else Console.WriteLine("You shouldn't be here");
                    nr++;
                }

                for (int i = nr; i < this.attrLength; i++)
                    SvmValues.Add(0);

                // Console.WriteLine("sum: " + sum);

                foreach (var item in SvmValues)
                {
                    float val = (float)item / sum;
                    normalizeList.Add(val);
                }

                if (type.Equals("train"))
                {
                    TrainNormalizedVector.Add(normalizeList);
                    TrainClasses.Add(classesList);
                }
                else if (type.Equals("test"))
                {
                    TestNormalizedVector.Add(normalizeList);
                    TestClasses.Add(classesList);
                }
            }
        }

        public void ManhattanDistance(bool canSkipValues = false, float skipValueThreshold = 1.0f)
        {
            KnnMatrix = new List<List<float>>();
            for (int i = 0; i < TestNormalizedVector.Count; i++)
            {
                int valuesPassingThresholdCount = 0;
                List<float> knnLine = new List<float>();
                for (int j = 0; j < TrainNormalizedVector.Count; j++)
                {
                    float sum = TestNormalizedVector[i].Zip(TrainNormalizedVector[j], (x, y) => Math.Abs(x - y)).Sum();
                    knnLine.Add(sum);
                    if (canSkipValues)
                    {
                        if (sum < skipValueThreshold)
                            valuesPassingThresholdCount++;
                        if (valuesPassingThresholdCount >= this.ConstK)
                            break;
                    }
                }
                KnnMatrix.Add(knnLine);
            }
                
            /*
            foreach(var x in TestNormalizedVector)
            {
                List<float> knnLine = new List<float>();
                foreach(var y in TrainNormalizedVector)
                {
                    //float sum = 0.0f;
                    //for (int i = 0; i < this.attrLength; i++) 
                    //    sum += Math.Abs(x[i] - y[i]);
                    float sum = x.Zip(y, (i, j) => Math.Abs(i - j)).Sum();
                    knnLine.Add(sum);
                }
                KnnMatrix.Add(knnLine);
            }
            */
        }

        public void EuclideanDistance(bool canSkipValues = false, float skipValueThreshold = 1.0f)
        {
            KnnMatrix = new List<List<float>>();
            foreach (var x in TestNormalizedVector)
            {
                int valuesPassingThresholdCount = 0;
                List<float> knnLines = new List<float>();
                foreach (var y in TrainNormalizedVector)
                {
                    float sum = x.Zip(y, (i, j) => (float)Math.Pow((i - j), 2.0f)).Sum();
                    sum = (float)Math.Sqrt(sum);
                    knnLines.Add(sum);
                    if (canSkipValues)
                    {
                        if (sum < skipValueThreshold)
                            valuesPassingThresholdCount++;
                        if (valuesPassingThresholdCount >= this.ConstK)
                            break;
                    }
                }
                KnnMatrix.Add(knnLines);
            }
        }

        public void GetKnnTestClasses()
        {
            knnTestTagsClasses = new List<string>();
            List<List<float>> knnMatrixCopy = new List<List<float>>();
            foreach(var line in KnnMatrix)
            {
                List<float> floatList = new List<float>(line);
                knnMatrixCopy.Add(floatList);
            }
            foreach (var line in knnMatrixCopy)
            {
                List<string> tags = new List<string>();
                // repeta k-ori
                for (int i = 0; i < this.ConstK; i++)
                {
                    int index = line.IndexOf(line.Min());
                    tags.AddRange(TrainClasses[index]);
                    line[index] = float.MaxValue;
                }
                string tagClass = tags.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                knnTestTagsClasses.Add(tagClass);
            }
        }

    }
}