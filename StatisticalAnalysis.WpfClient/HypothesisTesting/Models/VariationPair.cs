namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public class VariationPair<TVariant> : IVariationPair<TVariant>
        where TVariant : class
    {
        protected TVariant _variant;
        public TVariant Variant => _variant;

        protected int _frequency;
        public int Frequency => _frequency;

        public VariationPair(TVariant variant, int frequency)
        {
            _variant = variant;
            _frequency = frequency;
        }
    }
}