using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis
{
    public class HypothesisTesting
    {
        private IEnumerable<HypothesisModelData> _values;

        public HypothesisTesting(IEnumerable<HypothesisModelData> values)
        {
            _values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public void Test()
        {
            
        }
    }
}
