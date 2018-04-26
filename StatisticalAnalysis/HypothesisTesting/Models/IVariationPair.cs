namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public interface IVariationPair<out TVariant>
        where TVariant : class
    {
        TVariant Variant { get; }
        int Frequency { get; }
    }
}
