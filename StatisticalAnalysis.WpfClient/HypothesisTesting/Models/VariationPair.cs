namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public class VariationPair<TVariant> : IVariationPair<TVariant>
        where TVariant : class
    {
        public TVariant Variant { get; }
        public int Frequency { get; }

        public VariationPair(TVariant variant, int frequency)
        {
            Variant = variant;
            Frequency = frequency;
        }
    } 
}