﻿using StatisticalAnalysis.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class DiscreteData : ObservableCollection<SeriesDatum<int>>, IConvertToVariationPairs
    {
        public IEnumerable<IVariationPair<object>> ToVariationPairs()
        {
            throw new NotImplementedException();
        }
    }
}
