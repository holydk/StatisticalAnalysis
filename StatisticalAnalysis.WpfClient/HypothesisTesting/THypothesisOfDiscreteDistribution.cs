using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public abstract class THypothesisOfDiscreteDistribution : THypothesis<DiscretePair, IDiscreteDistribution>
    {
        public THypothesisOfDiscreteDistribution(DiscretePair[] pairs, double significanceLevel)
            : base(pairs, significanceLevel)
        {

        }

        protected override void CalculateStatistics(int sumFrequency)
        {
            double tFrequency;
            double c1;
            double c2;

            double p = 0;
            int frequency = 0;

            for (int i = 0; i < _pairs.Length; i++)
            {
                frequency += _pairs[i].Frequency;
                p += Probability(_pairs[i]);

                if (frequency < 5)
                {
                    // Если есть результаты, добавляем к последнему
                    if (_results.Any())
                    {
                        var lastResult = _results.Last();

                        p += lastResult.Probability;
                        frequency += lastResult.EmpiricalFrequency;
                        tFrequency = p * sumFrequency;
                        c1 = Math.Pow(frequency - tFrequency, 2);
                        c2 = c1 / tFrequency;

                        var newResult = new THypothesisResult(
                            lastResult.Number,
                            lastResult.Variant,
                            frequency,
                            p,
                            tFrequency,
                            c1,
                            c2);

                        _results[_results.Count - 1] = newResult;

                        p = 0;
                        frequency = 0;
                    }

                    continue;
                }

                tFrequency = p * sumFrequency;
                c1 = Math.Pow(frequency - tFrequency, 2);
                c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(_results.Count + 1, _pairs[i].Variant.ToString(), frequency, p, tFrequency, c1, c2));

                p = 0;
                frequency = 0;
            }
        }

        protected override double Probability(DiscretePair pair)
        {
            return distribution.Probability(pair.Variant);
        }
    }
}
