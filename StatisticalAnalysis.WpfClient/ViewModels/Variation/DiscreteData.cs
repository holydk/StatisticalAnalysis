using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class DiscreteData : SeriesData<SeriesDatum<int>>
    {
        protected override SeriesDatum<int> Parse(string item)
        {
            if (int.TryParse(item, out int value))
                return new SeriesDatum<int>(value);
            else
            {
                throw new InvalidOperationException(
                    $"Обнаружена непредвиденная последовательность символов: '{item}'." +
                    $"\nНевозможно преобразовать в целочисленный тип.");
            }
        }

        protected override void FixParsedData(ICollection<SeriesDatum<int>> data)
        {
            var distinctItems = data.Distinct();
            var avg = distinctItems.Average(d => d.Value);
            var expectedErrors = data
                .Where(d => d.Value > avg * 2)
                .ToArray();

            foreach (var item in expectedErrors)
            {
                data.Remove(item);
            }
        }

        protected override IVariationPair[] GetVariationPairs()
        {
            var distinctItems = Data.Distinct();
            var varPairs = new HashSet<DiscretePair>();

            int frequency;

            foreach (var item in distinctItems)
            {
                frequency = Data.Count(d => d.Value == item.Value);

                varPairs.Add(new DiscretePair(item.Value, frequency));
            }

            return varPairs
                .OrderBy(pair => pair.Variant)
                .ToArray();
        }
    }
}
