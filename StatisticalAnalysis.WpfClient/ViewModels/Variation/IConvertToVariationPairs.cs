using StatisticalAnalysis.HypothesisTesting.Models;
using System.Collections.Generic;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public interface IConvertToVariationPairs
    {
        IEnumerable<IVariationPair<object>> ToVariationPairs();
    }
}
