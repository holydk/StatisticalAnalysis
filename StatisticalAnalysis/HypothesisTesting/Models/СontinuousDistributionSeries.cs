﻿using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public class СontinuousDistributionSeries : DistributionSeries
    {
        public СontinuousDistributionSeries(IUnivariateDistribution univariateDistribution, ICollection<IVariationPair<Interval>> intervals)
            : base(univariateDistribution, (ICollection<IVariationPair<object>>)intervals)
        { }

        public static СontinuousDistributionSeries FromCsv(string fileName)
        {
            throw new NotImplementedException();
        }

        public double CumulativeDistribution(Interval interval) =>
            UnivariateDistribution.CumulativeDistribution(interval.Upper) - 
            UnivariateDistribution.CumulativeDistribution(interval.Lower);

        public override double CumulativeDistribution(IVariationPair<object> variationPair)
        {
            if (variationPair is IVariationPair<Interval> pair)
            {
                return CumulativeDistribution(pair.Variant);
            }
            else
            {
                throw new ArgumentException(nameof(variationPair));
            }
        }
    }
}
