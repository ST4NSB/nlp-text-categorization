using LearningModel;
using System.Collections.Generic;

namespace LearningSection
{
    public interface ILearningAlgorithm
    {
        void Fit(TextModel learningModel);
        IEnumerable<PredictedModel> Evaluate(TextModel testModel); 
    }
}