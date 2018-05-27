namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public class DiscretePair : VariationPair<int>
    {
        public DiscretePair(int value, int frequency)
            : base(value, frequency)
        {

        }

        protected override void Merge(VariationPair<int> withPair)
        {
            if (_variant < withPair.Variant)
                _variant = withPair.Variant;

            _frequency += withPair.Frequency;
        }
    }
}
