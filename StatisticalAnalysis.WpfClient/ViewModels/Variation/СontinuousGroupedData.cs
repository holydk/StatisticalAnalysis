using StatisticalAnalysis.HypothesisTesting.Models;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class СontinuousGroupedData : SeriesData<IntervalSeriesDatum>
    {
        protected override IntervalSeriesDatum Parse(string item)
        {
            throw new NotImplementedException();
        }

        public override ICollection<IVariationPair<object>> ToVariationPairs()
        {
            throw new NotImplementedException();
        }
    }
}
