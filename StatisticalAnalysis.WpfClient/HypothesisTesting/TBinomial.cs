using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using System.Linq;
using System;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TBinomial : THypothesis
    {
        protected override int _r => 2;

        public TBinomial(ICollection<IVariationPair<Variant<int>>> discretePairs, double significanceLevel)
            : base(significanceLevel)
        {
            if (discretePairs == null) new ArgumentNullException(nameof(discretePairs));

            var sumFrequency = discretePairs.Sum(dPair => dPair.Frequency);
            var mean = (double)discretePairs.Sum(dPair => dPair.Variant.Value * dPair.Frequency) / sumFrequency;
            var p = mean / discretePairs.Count;
            var binomial = new Binomial(p, discretePairs.Count - 1);

            _univariateDistribution = binomial;

            Execute(discretePairs, v => binomial.Probability(v.Value));
        }
    }
}
