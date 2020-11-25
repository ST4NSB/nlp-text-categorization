using LearningModel;
using System.Collections.Generic;

namespace LearningBussiness
{
    public interface ILearningAlgorithm
    {
        void Fit(TextModel learningModel);
        IEnumerable<string> Predict(TextModel testModel); 
    }
}