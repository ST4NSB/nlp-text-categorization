using LearningSection;
using LearningSection.Data;
using System;

namespace LearningSection
{
    public static class LearningFactory
    {
        public static ILearningAlgorithm Create(LearningType type)
        {
            switch (type)
            {
                case LearningType.NaiveBayes:
                    return new NaiveBayes();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
