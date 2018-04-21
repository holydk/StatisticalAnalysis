namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public interface IVariationPair<TVariant>
    {
        TVariant Variant { get; }
        int Frequency { get; }
    }
}
