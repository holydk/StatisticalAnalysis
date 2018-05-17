using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TContinuousUniform : THypothesis<Variant<int>, ContinuousUniform>
    {
        protected override int _r => 2;

        public TContinuousUniform(IVariationPair<Variant<int>>[] discretePairs, double significanceLevel)
            : base(discretePairs, significanceLevel)
        {
            var sumFrequency = discretePairs.Sum(dPair => dPair.Frequency);
            var mean = discretePairs.Sum(dPair => dPair.Variant.Value * dPair.Frequency) / sumFrequency;
            var stdDev = Math.Sqrt(discretePairs.Sum(dPair => Math.Pow(dPair.Variant.Value - mean, 2) * dPair.Frequency) / sumFrequency);
            var sqrt3 = Math.Sqrt(3);
            var lower = (mean - sqrt3 * stdDev);
            var upper = (mean + sqrt3 * stdDev);

            distribution = new ContinuousUniform(lower, upper);
            
            Execute();
        }

        protected override double Probability(Variant<int> variant) =>
            distribution.Density(variant.Value);

        protected override void CalculateVariationData(int sumFrequency)
        {
            var p = Probability(_variationPairs[0].Variant);
            double tFrequency;
            double c1;
            double c2;

            for (int i = 0; i < _variationPairs.Length; i++)
            {             
                if (i == 0)
                {
                    tFrequency = p * sumFrequency * (_variationPairs[i].Variant.Value - distribution.LowerBound);
                }
                else if (i == _variationPairs.Length - 1)
                {
                    tFrequency = p * sumFrequency * (distribution.UpperBound - _variationPairs[i].Variant.Value);
                }
                else
                {
                    tFrequency = p * sumFrequency;
                }

                if (tFrequency < 0)
                    tFrequency = 0;

                c1 = Math.Pow(_variationPairs[i].Frequency - tFrequency, 2);
                c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(
                    _results.Count + 1, 
                    _variationPairs[i].Variant.ToString(),
                    _variationPairs[i].Frequency, 
                    p, 
                    tFrequency, 
                    c1, 
                    c2));
            }
        }
    }
}
