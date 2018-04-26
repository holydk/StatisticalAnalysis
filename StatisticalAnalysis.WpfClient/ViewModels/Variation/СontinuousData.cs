using StatisticalAnalysis.Helpers;
using StatisticalAnalysis.HypothesisTesting.Models;
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

        public override ICollection<IVariationPair<object>> ToVariationPairs()
        {
            if (Data == null || Data.Count == 0) return null;

            var varPairs = new HashSet<IVariationPair<Variant<Interval>>>();
            var numberOfIntervals = StatisticalAnalysisProvider.GetNumberOfIntervals(Data.Count);
            var min = Data.Min(datum => datum.Value);
            var max = Data.Max(datum => datum.Value);
            var step = (max - min) / numberOfIntervals;

            for (int i = 0; i < numberOfIntervals; i++)
            {
                var lower = min + (i * step);
                var upper = lower + step;

                var variant = new Variant<Interval>(new Interval(lower, upper));
                var frequency = Data.Where(varPair => varPair.Value >= lower && varPair.Value <= upper).Count();

                varPairs.Add(new VariationPair<Variant<Interval>>(variant, frequency));
            }

            return varPairs.ToArray();
        }
    }
}
