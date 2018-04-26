using StatisticalAnalysis.HypothesisTesting.Models;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}
