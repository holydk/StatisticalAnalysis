using System.ComponentModel;

namespace StatisticalAnalysis.WpfClient.Models
{
    public enum DistributionSeriesInputType
    {
        [Description("Сгруппированный")]
        Grouped,

        [Description("Несгруппированный")]
        NotGrouped
    }
}
