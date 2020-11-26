﻿using LearningModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LearningBussiness
{
    public class NaiveBayes : ILearningAlgorithm
    {
        private const int _alpha = 1;
        private int _alphaSize;

        private Dictionary<string, double> _priorProbability;
        private Dictionary<string, Dictionary<int, double>> _trainedModel;

        public void Fit(TextModel learningModel)
        {
            _trainedModel = new Dictionary<string, Dictionary<int, double>>();

            ComputePriorProbability(learningModel);
            ComputePosteriorProbability(learningModel);
        }

        public IEnumerable<PredictedModel> Evaluate(TextModel testModel)
        {
            foreach (var test in testModel.CategoryClassifierObject)
            {
                Dictionary<string, double> predictedClasses = new Dictionary<string, double>();
                foreach (var tmodel in _trainedModel)
                {
                    var proc = Math.Log(_priorProbability[tmodel.Key]);
                    foreach(var item in test.WordCount)
                    {
                        if (tmodel.Value.ContainsKey(item.Key))
                        {
                            proc = proc * Math.Log(tmodel.Value[item.Key]);
                        }
                        else
                        {
                            proc = proc * Math.Log(((double)_alpha / (_alpha * Math.Pow(_alphaSize, 5))));
                        }
                    }
                    predictedClasses.Add(tmodel.Key, proc);
                }

                var ordered = predictedClasses.OrderByDescending(x => x.Value);
                var acPred = new List<string> { ordered.ElementAt(0).Key };

                yield return new PredictedModel
                {
                    actual = test.Categories,
                    predicted = acPred
                };
            }
        }

        private void ComputePriorProbability(TextModel learningModel)
        {
            var priorFreq = new Dictionary<string, int>();

            foreach(var item in learningModel.CategoryClassifierObject)
            {
                foreach(var cat in item.Categories)
                {
                    if (priorFreq.ContainsKey(cat))
                    {
                        priorFreq[cat] += 1;
                    }
                    else
                    {
                        priorFreq.Add(cat, 1);
                    }
                }
            }

            _alphaSize = priorFreq.Sum(x => x.Value);
            _priorProbability = new Dictionary<string, double>();

            foreach(var item in priorFreq)
            {
                _priorProbability.Add(
                    item.Key,
                    (double) item.Value / _alphaSize
                );
            }
        }

        private void ComputePosteriorProbability(TextModel learningModel)
        {
            var untrainedModel = new Dictionary<string, Dictionary<int, int>>();

            foreach (var model in learningModel.CategoryClassifierObject)
            {
                foreach(var cat in model.Categories)
                {
                    if (!untrainedModel.ContainsKey(cat))
                    {
                        untrainedModel.Add(cat, new Dictionary<int, int>());
                    }

                    foreach (var item in model.WordCount)
                    {
                        if (!untrainedModel[cat].ContainsKey(item.Key))
                        {
                            untrainedModel[cat].Add(item.Key, item.Value);
                        }
                        else
                        {
                            untrainedModel[cat][item.Key] += item.Value;
                        }
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
