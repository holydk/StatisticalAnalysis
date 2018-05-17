using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TNormal : THypothesis<Variant<Interval>, Normal>
    {
        protected override int _r => 2;

        public TNormal(IVariationPair<Variant<Interval>>[] intervals, double significanceLevel)
            : base(intervals, significanceLevel)
        {
            var sumFrequency = intervals.Sum(i => i.Frequency);
            var mean = intervals.Sum(i => i.Variant.Value.Middle * i.Frequency) / sumFrequency;
            var stdDev = Math.Sqrt(intervals.Sum(i => Math.Pow(i.Variant.Value.Middle - mean, 2) * i.Frequency) / sumFrequency);

            distribution = new Normal(mean, stdDev);

            Execute();
        }

        protected override double Probability(Variant<Interval> variant) =>
            distribution.CumulativeDistribution(variant.Value.Upper) - 
            distribution.CumulativeDistribution(variant.Value.Lower);
    }
}
