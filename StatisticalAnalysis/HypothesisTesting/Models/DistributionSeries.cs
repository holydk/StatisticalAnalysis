using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public abstract class DistributionSeries
    {
        public IUnivariateDistribution UnivariateDistribution { get; }

        public ICollection<IVariationPair<object>> Variation { get; }

        protected DistributionSeries(IUnivariateDistribution univariateDistribution, ICollection<IVariationPair<object>> variation)
        {
            UnivariateDistribution = univariateDistribution ?? throw new ArgumentNullException(nameof(univariateDistribution));
            Variation = variation ?? throw new ArgumentNullException(nameof(variation));
        }

        public abstract double CumulativeDistribution(IVariationPair<object> variationPair);
    }
}
