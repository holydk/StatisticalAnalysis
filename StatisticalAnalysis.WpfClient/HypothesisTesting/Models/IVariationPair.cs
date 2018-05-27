namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public interface IVariationPair
    {
        int Frequency { get; }

        void Merge(IVariationPair variationPair);
    }
}
