using LearningModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningBussiness
{
    public class NaiveBayes : ILearningAlgorithm
    {
        private const int alpha = 1;

        private Dictionary<string, double> _priorProbability;
        private Dictionary<string, Dictionary<int, double>> _trainedModel;

        public void Fit(TextModel learningModel)
        {
            _trainedModel = new Dictionary<string, Dictionary<int, double>>();

            ComputePriorProbability(learningModel);
            ComputePosteriorProbability(learningModel);
        }

        public IEnumerable<string> Predict(TextModel testModel)
        {
            var size = _priorProbability.Count;

            foreach (var test in testModel.CategoryClassifierObject)
            {
                Dictionary<string, double> predictedClasses = new Dictionary<string, double>();
                foreach (var tmodel in _trainedModel)
                {
                    var proc = _priorProbability[tmodel.Key];
                    foreach(var item in test.WordCount)
                    {
                        if (tmodel.Value.ContainsKey(item.Key))
                        {
                            proc = proc * tmodel.Value[item.Key];
                        }
                        else
                        {
                            var valProd = 1.0d;
                            foreach(var vals in tmodel.Value)
                            {
                                valProd *= vals.Value;
                            }
                            proc = proc * ((double) alpha / (alpha * size)) * valProd;
                        }
                    }
                    predictedClasses.Add(tmodel.Key, proc);
                }
                yield return predictedClasses.OrderByDescending(x => x.Value).First().Key;
            }
        }

        private void ComputePriorProbability(TextModel learningModel)
        {
            var priorFreq = new Dictionary<string, int>();

            foreach(var item in learningModel.CategoryClassifierObject)
            {
                var firstClass = item.Categories.FirstOrDefault();
                if (priorFreq.ContainsKey(firstClass))
                {
                    priorFreq[firstClass] += 1;
                }
                else
                {
                    priorFreq.Add(firstClass, 1);
                }
            }

            _priorProbability = new Dictionary<string, double>();

            foreach(var item in priorFreq)
            {
                _priorProbability.Add(
                    item.Key,
                    (double) item.Value / learningModel.CategoryClassifierObject.Count
                );
            }
        }

        private void ComputePosteriorProbability(TextModel learningModel)
        {
            var untrainedModel = new Dictionary<string, Dictionary<int, int>>();

            foreach (var model in learningModel.CategoryClassifierObject)
            {
                var firstCategory = model.Categories.FirstOrDefault();

                if (!untrainedModel.ContainsKey(firstCategory))
                {
                    untrainedModel.Add(firstCategory, new Dictionary<int, int>());
                }

                foreach (var item in model.WordCount)
                {
                    if (!untrainedModel[firstCategory].ContainsKey(item.Key))
                    {
                        untrainedModel[firstCategory].Add(item.Key, item.Value);
                    }
                    else
                    {
                        untrainedModel[firstCategory][item.Key] += item.Value;
                    }
                }
            }


            foreach(var model in untrainedModel)
            {
                var size = model.Value.Sum(x => x.Value);

                if (!_trainedModel.ContainsKey(model.Key))
                {
                    _trainedModel.Add(model.Key, new Dictionary<int, double>());
                }

                foreach(var item in model.Value)
                {
                    _trainedModel[model.Key].Add(item.Key, (double)item.Value / size);                    
                }
            }


        }

    }
}
