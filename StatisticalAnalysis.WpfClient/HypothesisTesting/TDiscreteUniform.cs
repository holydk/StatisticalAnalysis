using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TDiscreteUniform : THypothesis
    {
        public TDiscreteUniform(ICollection<IVariationPair<Variant<int>>> discretePairs, double significanceLevel)
            : base(significanceLevel)
        {
            if (discretePairs == null) new ArgumentNullException(nameof(discretePairs));

            _univariateDistribution = new DiscreteUniform(
                discretePairs.First().Variant.Value, 
                discretePairs.Last().Variant.Value);

            Execute(discretePairs, v => _univariateDistribution.CumulativeDistribution(v.Value));

            _criticalValue = ChiSquared.InvCDF(discretePairs.Count - 3, 1 - significanceLevel);
        }
    }
}
