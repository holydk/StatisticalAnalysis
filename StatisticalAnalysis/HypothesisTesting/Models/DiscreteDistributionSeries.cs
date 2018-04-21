using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public class DiscreteDistributionSeries : DistributionSeries
    {
        public DiscreteDistributionSeries(IUnivariateDistribution univariateDistribution, ICollection<IVariationPair<int>> discretePairs)
            : base(univariateDistribution, (ICollection<IVariationPair<object>>)discretePairs)
        { }

        public override double CumulativeDistribution(IVariationPair<object> variationPair)
        {
            if (variationPair is IVariationPair<double> pair)
            {
                return UnivariateDistribution.CumulativeDistribution(pair.Variant);
            }
            else
            {
                throw new ArgumentException(nameof(variationPair));
            }
        }
    }
}
