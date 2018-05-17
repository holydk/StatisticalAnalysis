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

        public override IVariationPair<object>[] ToVariationPairs()
        {
            if (Data == null || Data.Count == 0) return null;

            var data = new HashSet<IVariationPair<Variant<Interval>>>();

            foreach (var item in Data.OrderBy(d => d.Variant.Middle))
            {
                data.Add(new VariationPair<Variant<Interval>>(new Variant<Interval>(item.Variant), item.Value));
            }

            return data.ToArray();
        }
    }
}
