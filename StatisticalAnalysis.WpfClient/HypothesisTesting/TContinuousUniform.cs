using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TContinuousUniform : THypothesisOfContinuousDistribution
    {
        protected override int _r => 2;

        private double _lowerBound;
        private double _upperBound;

        public TContinuousUniform(СontinuousPair[] pairs, double significanceLevel)
            : base(pairs, significanceLevel)
        {
            var sumFrequency = pairs.Sum(dPair => dPair.Frequency);
            var mean = pairs.Sum(dPair => dPair.Variant.Middle * dPair.Frequency) / sumFrequency;
            var stdDev = Math.Sqrt(pairs.Sum(dPair => Math.Pow(dPair.Variant.Middle - mean, 2) * dPair.Frequency) / sumFrequency);
            var sqrt3 = Math.Sqrt(3);

            _lowerBound = mean - sqrt3 * stdDev;
            _upperBound = mean + sqrt3 * stdDev;

            distribution = new ContinuousUniform(_lowerBound, _upperBound);
        }

        protected override void CalculateStatistics(int sumFrequency)
        {
            var p = Probability(_pairs.First(_p => _p.Frequency >= 5 && _p.Variant.Middle > _lowerBound));
            double tFrequency = 0;
            double c1;
            double c2;

            var pairs = _pairs.ToList();

            FixVariationPairs(pairs);

            for (int i = 0; i < pairs.Count; i++)
            {
                if (i == 0)
                {
                    tFrequency = p * sumFrequency * (pairs[i].Variant.Upper - _lowerBound);
                }
                else if (i == pairs.Count - 1)
                {
                    tFrequency = p * sumFrequency * (_upperBound - pairs[i].Variant.Lower);
                }
                else
                {
                    tFrequency = p * sumFrequency; //* (pairs[i].Variant.Upper - pairs[i].Variant.Lower);
                }

                if (tFrequency < 0)
                    tFrequency = 0;

                c1 = Math.Pow(pairs[i].Frequency - tFrequency, 2);
                c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(_results.Count + 1, pairs[i].Variant.ToString(), pairs[i].Frequency, p, tFrequency, c1, c2));
            }
        }

        protected override double Probability(СontinuousPair pair)
        {
            return distribution.Density(pair.Variant.Middle);
        }
    }
}
