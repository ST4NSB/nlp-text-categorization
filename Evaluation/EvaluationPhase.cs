using LearningModel;
using System;
using System.Collections.Generic;

namespace Evaluation
{
    public class EvaluationPhase
    {
        public static float GetAccuracy(IEnumerable<PredictedModel> results)
        {
            var correct = 0;
            var all = 0;
            foreach(var item in results)
            {
                foreach (var pred in item.predicted)
                {
                    if (item.actual.Contains(pred))
                    {
                        correct++;
                    }
                    all++;
                }
            }

            return (float) correct / all;
        }
    }
}
