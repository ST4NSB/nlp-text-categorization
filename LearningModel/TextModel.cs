using System.Collections.Generic;

namespace LearningModel
{
    public class TextModel
    {
        public class CategoryClassifier
        {
            public List<string> Categories = new List<string>();
            public Dictionary<int, int> WordCount = new Dictionary<int, int>();
        }

        public List<string> GlobalWords = new List<string>();
        public List<CategoryClassifier> CategoryClassifierObject = new List<CategoryClassifier>();
    }
}
