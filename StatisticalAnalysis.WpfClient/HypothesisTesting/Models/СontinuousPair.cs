namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public class СontinuousPair : VariationPair<Interval>
    {
        public СontinuousPair(Interval interval, int frequency)
            : base(interval, frequency)
        {

        }

        protected override void Merge(VariationPair<Interval> withPair)
        {
            if (withPair.Variant == null) return;

            if (_variant.Lower > withPair.Variant.Lower)
                _variant.Lower = withPair.Variant.Lower;

            if (_variant.Upper < withPair.Variant.Upper)
                _variant.Upper = withPair.Variant.Upper;

            _frequency += withPair.Frequency;
        }
    }
}
