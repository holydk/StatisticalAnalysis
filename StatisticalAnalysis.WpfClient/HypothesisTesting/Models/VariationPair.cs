namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public abstract class VariationPair<TVariant> : IVariationPair
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

        public void Merge(IVariationPair variationPair)
        {
            if (variationPair is VariationPair<TVariant> pair && pair != null)
            {
                Merge(pair);
            }
        }

        protected abstract void Merge(VariationPair<TVariant> withPair);
    }
}