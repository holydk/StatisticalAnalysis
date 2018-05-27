using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public abstract class THypothesisOfContinuousDistribution : THypothesis<СontinuousPair, IContinuousDistribution>
    {
        public THypothesisOfContinuousDistribution(СontinuousPair[] pairs, double significanceLevel)
            : base(pairs, significanceLevel)
        {

        }

        protected override void CalculateStatistics(int sumFrequency)
        {
            double p = 0;
            double tFrequency;
            double c1;
            double c2;

            var pairs = _pairs.ToList();

            FixVariationPairs(pairs);

            foreach (var pair in pairs)
            {
                p = Probability(pair);
                tFrequency = p * sumFrequency;
                c1 = Math.Pow(pair.Frequency - tFrequency, 2);
                c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(
                    _results.Count + 1, 
                    pair.Variant.ToString(), 
                    pair.Frequency, 
                    p, 
                    tFrequency, 
                    c1, 
                    c2));
            }
        }

        protected void FixVariationPairs(List<СontinuousPair> variationPairs)
        {
            if (variationPairs.Any(pair => pair.Frequency < 5))
            {
                var currentIndex = 0;
                var pairs = variationPairs.ToArray();

                for (int i = 0; i < pairs.Length; i++)
                {
                    if (pairs[i].Frequency >= 5) continue;

                    currentIndex = variationPairs.IndexOf(pairs[i]);

                    if (currentIndex == 0)
                    {
                        variationPairs[currentIndex + 1].Merge(pairs[i]);
                    }
                    else
                    {
                        variationPairs[currentIndex - 1].Merge(pairs[i]);
                    }

                    variationPairs.Remove(pairs[i]);
                }
            }
        }

        protected override double Probability(СontinuousPair pair)
        {
            return distribution.CumulativeDistribution(pair.Variant.Upper) - 
                distribution.CumulativeDistribution(pair.Variant.Lower);
        }
    }
}
