using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class СontinuousData : SeriesData<SeriesDatum<double>>
    {
        protected override SeriesDatum<double> Parse(string item)
        {
            if (double.TryParse(item, out double value))
                return new SeriesDatum<double>(value);
            else
            {
                throw new InvalidOperationException(
                    $"Обнаружена непредвиденная последовательность символов: '{item}'." +
                    $"\nНевозможно преобразовать в число с плавающей запятой.");
            }
        }

        protected override void FixParsedData(ICollection<SeriesDatum<double>> data)
        {
            var distinctItems = data.Distinct();
            var avg = distinctItems.Average(d => d.Value);
            var expectedErrors = data
                .Where(d => d.Value > avg * 5)
                .ToArray();

            foreach (var item in expectedErrors)
            {
                data.Remove(item);
            }
        }

        protected override IVariationPair[] GetVariationPairs()
        {
            var varPairs = new HashSet<СontinuousPair>();
            var numberOfIntervals = Statistics.GetNumberOfIntervals(Data.Count);
            var min = Data.Min(datum => datum.Value);
            var max = Data.Max(datum => datum.Value);
            var step = (max - min) / numberOfIntervals;

            double lower;
            double upper;
            int frequency;

            for (int i = 0; i < numberOfIntervals; i++)
            {
                lower = min + (i * step);
                upper = lower + step;
                frequency = Data.Count(varPair => varPair.Value >= lower && varPair.Value <= upper);

                varPairs.Add(new СontinuousPair(new Interval(lower, upper), frequency));
            }

            return varPairs
                .OrderBy(pair => pair.Variant.Middle)
                .ToArray();
        }
    }
}
