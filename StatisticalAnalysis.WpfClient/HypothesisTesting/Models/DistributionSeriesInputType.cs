using System.ComponentModel;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public enum DistributionSeriesInputType
    {
        [Description("Сгруппированный")]
        Grouped,

        [Description("Несгруппированный")]
        NotGrouped
    }
}
