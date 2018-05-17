using MathNet.Numerics.Distributions;
using System.Linq;
using System;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TBinomial : THypothesis<Variant<int>, Binomial>
    {
        protected override int _r => 2;

        public TBinomial(IVariationPair<Variant<int>>[] discretePairs, double significanceLevel)
            : base(discretePairs, significanceLevel)
        {
            var sumFrequency = discretePairs.Sum(dPair => dPair.Frequency);
            var mean = (double)discretePairs.Sum(dPair => dPair.Variant.Value * dPair.Frequency) / sumFrequency;
            var p = mean / discretePairs.Length;

            distribution = new Binomial(p, discretePairs.Length - 1);

            Execute();
        }

        protected override double Probability(Variant<int> variant) =>
            distribution.Probability(variant.Value);
    }
}
