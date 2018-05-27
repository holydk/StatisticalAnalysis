using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class СontinuousGroupedData : SeriesData<IntervalSeriesDatum>
    {
        protected override IntervalSeriesDatum Parse(string item)
        {
            throw new NotImplementedException();
        }

        protected override IVariationPair[] GetVariationPairs()
        {
            var data = new HashSet<СontinuousPair>();

            foreach (var item in Data.OrderBy(d => d.Variant.Middle))
            {
                data.Add(new СontinuousPair(item.Variant, item.Value));
            }

            return data.ToArray();
        }
    }
}
