using System.Collections.Generic;

namespace LearningModel
{
    public class TrainedModel
    {
        public class ProcessedClassifier
        {
            public string Category;
            public Dictionary<int, double> WordCount = new Dictionary<int, double>();
        }

        public List<string> GlobalWords = new List<string>();
        public List<ProcessedClassifier> ProcessedClassifierObject = new List<ProcessedClassifier>();
    }
}
