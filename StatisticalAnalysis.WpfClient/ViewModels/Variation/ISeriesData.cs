using StatisticalAnalysis.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Models;
using System.Collections.Generic;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public interface ISeriesData : IConvertFromFile
    {
        ICollection<IVariationPair<object>> ToVariationPairs();

        void ClearData();
    }
}
