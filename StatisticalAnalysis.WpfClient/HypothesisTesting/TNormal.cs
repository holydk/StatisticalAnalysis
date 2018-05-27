using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TNormal : THypothesisOfContinuousDistribution
    {
        protected override int _r => 2;

        public TNormal(СontinuousPair[] intervals, double significanceLevel)
            : base(intervals, significanceLevel)
        {
            var sumFrequency = intervals.Sum(i => i.Frequency);
            var mean = intervals.Sum(i => i.Variant.Middle * i.Frequency) / sumFrequency;
            var stdDev = Math.Sqrt(intervals.Sum(i => Math.Pow(i.Variant.Middle - mean, 2) * i.Frequency) / sumFrequency);

            distribution = new Normal(mean, stdDev);
        }
    }
}
