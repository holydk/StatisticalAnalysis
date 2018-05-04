namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public interface IVariationPair<out TVariant>
        where TVariant : class
    {
        TVariant Variant { get; }
        int Frequency { get; }
    }
}
