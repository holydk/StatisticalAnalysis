using StatisticalAnalysis.HypothesisTesting.Models;
using System.ComponentModel;
using System.Reflection;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public static class DescriptionProvider
    {
        public static string ToDescription(this FieldInfo fieldInfo) =>
            fieldInfo.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? string.Empty;

        public static DistributionSeriesType? ToDistributionSeriesType(this FieldInfo fieldInfo) =>
            fieldInfo.GetCustomAttribute<TypeOfDistributionSeriesAttribute>(false)?.DistributionSeriesType;

        public static DistributionSeriesType? ToDistributionSeriesType(this DistributionType distributionType) =>
            distributionType.GetType().GetField(distributionType.ToString()).ToDistributionSeriesType();
        
    }
}
