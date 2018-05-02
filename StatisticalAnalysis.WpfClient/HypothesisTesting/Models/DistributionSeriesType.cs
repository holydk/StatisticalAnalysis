using System.ComponentModel;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public enum DistributionSeriesType
    {
        [Description("Интервальный")]
        Сontinuous,

        [Description("Дискретный")]
        Discrete
    }
}
