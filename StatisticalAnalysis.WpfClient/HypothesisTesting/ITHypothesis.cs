using LiveCharts;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System.Collections.ObjectModel;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public interface ITHypothesis
    {
        double SignificanceLevel { get; }

        SeriesCollection Series { get; }

        bool? IsValid { get; }

        ReadOnlyCollection<THypothesisResult> Results { get; }
    }
}
