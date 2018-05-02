using System;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class TypeOfDistributionSeriesAttribute : Attribute
    {
        public DistributionSeriesType DistributionSeriesType { get; }

        public TypeOfDistributionSeriesAttribute(DistributionSeriesType distributionSeriesType)
        {
            DistributionSeriesType = distributionSeriesType;
        }
    }
}
