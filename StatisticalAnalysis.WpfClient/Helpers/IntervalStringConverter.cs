using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public class IntervalStringConverter : IValueConverter
    {
        private static Regex _regexBack = new Regex(@"(?:(?<Lower>[\d]+) *-? *(?:(?<Upper>[\d]+)))");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string intervalString)
            {
                if (_regexBack.IsMatch(intervalString))
                {
                    var match = _regexBack.Match(intervalString);
                    
                    if (match.Success &&
                        double.TryParse(match.Groups["Lower"].Value, out double lower) &&
                        double.TryParse(match.Groups["Upper"].Value, out double upper))
                    {
                        return new Interval(lower, upper);
                    }
                }
            }

            return null;
        }
    }
}
