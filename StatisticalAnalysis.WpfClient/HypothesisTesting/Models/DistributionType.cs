using System.ComponentModel;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public enum DistributionType
    {
        [Description("Нормальное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Сontinuous)]
        Normal,

        [Description("Биноминальное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Discrete)]
        Binomial,

        [Description("Непрерывное равномерное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Сontinuous)]
        ContinuousUniform
    }
}
