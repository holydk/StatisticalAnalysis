namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public class VariationPair<TVariant> : IVariationPair<TVariant>
    {
        public TVariant Variant { get; set; }
        public int Frequency { get; set; }
    } 
}