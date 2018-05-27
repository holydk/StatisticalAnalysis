using MathNet.Numerics.Distributions;
using System.Linq;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public class TBinomial : THypothesisOfDiscreteDistribution
    {
        protected override int _r => 2;

        public TBinomial(DiscretePair[] pairs, double significanceLevel)
            : base(pairs, significanceLevel)
        {
            var sumFrequency = pairs.Sum(dPair => dPair.Frequency);
            var mean = (double)pairs.Sum(dPair => dPair.Variant * dPair.Frequency) / sumFrequency;
            var p = mean / pairs.Length;

            distribution = new Binomial(p, pairs.Length);
        }
    }
}
