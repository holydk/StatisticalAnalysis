using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public interface ISeriesData : IConvertFromFile
    {
        IVariationPair[] ToVariationPairs();

        void ClearData();
    }
}
