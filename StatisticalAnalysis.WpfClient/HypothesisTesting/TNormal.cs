using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TNormal : THypothesis
    {
        protected override int _r => 2;

        public TNormal(ICollection<IVariationPair<Variant<Interval>>> intervals, double significanceLevel)
            : base(significanceLevel)
        {
            if (intervals == null) new ArgumentNullException(nameof(intervals));

            var sumFrequency = intervals.Sum(i => i.Frequency);
            var mean = intervals.Sum(i => i.Variant.Value.Middle * i.Frequency) / sumFrequency;
            var stdDev = Math.Sqrt(intervals.Sum(i => Math.Pow(i.Variant.Value.Middle - mean, 2) * i.Frequency) / sumFrequency);

            _univariateDistribution = new Normal(mean, stdDev);

            Execute(intervals, v => _univariateDistribution.CumulativeDistribution(v.Value.Upper) -
                    _univariateDistribution.CumulativeDistribution(v.Value.Lower));
        }
    }
}
