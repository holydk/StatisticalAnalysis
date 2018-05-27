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

        protected override IVariationPair[] GetVariationPairs()
        {
            var data = new HashSet<DiscretePair>();

            foreach (var item in Data.OrderBy(d => d.Variant))
            {
                data.Add(new DiscretePair(item.Variant, item.Value));
            }

            return data.ToArray();
        }
    }
}
