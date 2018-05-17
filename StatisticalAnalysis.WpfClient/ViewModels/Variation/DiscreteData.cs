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

        public override IVariationPair<object>[] ToVariationPairs()
        {
            if (Data == null || Data.Count == 0) return null;

            var distinctItems = Data.Distinct();
            var varPairs = new HashSet<IVariationPair<Variant<int>>>();

            foreach (var item in distinctItems)
            {
                var variant = new Variant<int>(item.Value);
                var frequency = Data.Count(d => d.Value == item.Value);

                varPairs.Add(new VariationPair<Variant<int>>(variant, frequency));
            }

            return varPairs.OrderBy(varPair => varPair.Variant.Value).ToArray();
        }
    }
}
