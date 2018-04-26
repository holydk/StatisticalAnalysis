using StatisticalAnalysis.HypothesisTesting.Models;
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

        public override ICollection<IVariationPair<object>> ToVariationPairs()
        {
            if (Data == null || Data.Count == 0) return null;

            var distinctItems = Data.Distinct();
            var varPairs = new HashSet<IVariationPair<Variant<int>>>();

            foreach (var item in distinctItems)
            {
                var variant = new Variant<int>(item.Value);
                var frequency = Data.Where((datum) => datum.Value == item.Value).Count();

                varPairs.Add(new VariationPair<Variant<int>>(variant, frequency));
            }

            return (ICollection<IVariationPair<object>>)varPairs.OrderBy(varPair => varPair.Variant.Value);
        }
    }
}
