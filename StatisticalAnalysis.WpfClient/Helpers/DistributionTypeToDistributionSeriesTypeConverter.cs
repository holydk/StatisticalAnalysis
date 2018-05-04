using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public class DistributionTypeToDistributionSeriesTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DistributionType distributionType)
            {
                return distributionType.ToDistributionSeriesType();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
