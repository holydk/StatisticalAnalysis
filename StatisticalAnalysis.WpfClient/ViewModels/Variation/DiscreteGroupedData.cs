using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class DiscreteGroupedData : SeriesData<GroupSeriesDatum<int>>
    {
        protected override GroupSeriesDatum<int> Parse(string item)
        {
            throw new NotImplementedException();
        }

        public override ICollection<IVariationPair<object>> ToVariationPairs()
        {
            if (Data == null || Data.Count == 0) return null;

            var data = new HashSet<IVariationPair<Variant<int>>>();

            foreach (var item in Data.OrderBy(d => d.Variant))
            {
                data.Add(new VariationPair<Variant<int>>(new Variant<int>(item.Variant), item.Value));
            }

            return data.ToArray();
        }
    }
}
