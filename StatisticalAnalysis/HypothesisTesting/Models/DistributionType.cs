using System.ComponentModel;

namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public enum DistributionType
    {
        [Description("Нормальное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Сontinuous)]
        Normal,

        [Description("Биноминальное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Discrete)]
        Binomial,

        [Description("Дискретное равномерное")]
        [TypeOfDistributionSeries(DistributionSeriesType.Discrete)]
        DiscreteUniform
    }
}
