using LiveCharts;
using LiveCharts.Wpf;
using MathNet.Numerics.Distributions;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting
{
    public abstract class THypothesis<TVariant, TDistribution> : ITHypothesis
        where TVariant : class 
        where TDistribution : class, IUnivariateDistribution
    {
        public double SignificanceLevel { get; }

        protected double _empiricalValue;
        public double EmpiricalValue => _empiricalValue;

        protected double _criticalValue;
        public double CriticalValue => _criticalValue;

        protected List<THypothesisResult> _results;
        public ReadOnlyCollection<THypothesisResult> Results => _results?.AsReadOnly();

        protected TDistribution distribution;

        protected SeriesCollection _series;
        public SeriesCollection Series => _series;

        private bool? _isValid = null;
        public bool? IsValid => _isValid;

        protected IVariationPair<TVariant>[] _variationPairs;

        protected abstract int _r { get; }

        public THypothesis(IVariationPair<TVariant>[] variationPairs, double significanceLevel)
        {
            if (significanceLevel == 0 || significanceLevel >= 1)
                throw new ArgumentOutOfRangeException(nameof(significanceLevel));

            SignificanceLevel = significanceLevel;

            _variationPairs = variationPairs ?? throw new ArgumentNullException(nameof(variationPairs));
            _results = new List<THypothesisResult>();
            _series = new SeriesCollection();
        }

        protected abstract double Probability(TVariant variant);

        protected virtual void Execute()
        {
            var sumFrequency = _variationPairs.Sum(p => p.Frequency);

            CalculateVariationData(sumFrequency);

            _empiricalValue = _results.Sum(r => 
            {
                if (double.IsInfinity(r.Criterion2))               
                    return 0;               
                else
                    return r.Criterion2;
            });
            _criticalValue = ChiSquared.InvCDF(_variationPairs.Length - _r - 1, 1 - SignificanceLevel);
            _isValid = _empiricalValue > _criticalValue ? false : true;

            BuildSeries(sumFrequency);
        }

        protected virtual void CalculateVariationData(int sumFrequency)
        {
            double p;
            double tFrequency;
            double c1;
            double c2;

            foreach (var pair in _variationPairs)
            {
                p = Probability(pair.Variant);
                tFrequency = p * sumFrequency;
                c1 = Math.Pow(pair.Frequency - tFrequency, 2);
                c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(_results.Count + 1, pair.Variant.ToString(), pair.Frequency, p, tFrequency, c1, c2));
            }
        }

        protected virtual void BuildSeries(int sumFrequency)
        {
            var p = _results.Select(r => r.Probability);
            var empiricalF = _results.Select(
                r => (r.EmpiricalFrequency / (r.Probability * sumFrequency)) * r.Probability);

            _series.AddRange(new object[]
            {
                new ColumnSeries()
                {
                    Title = "Эмп. частоты",
                    Values = new ChartValues<double>(empiricalF)                    
                },
                new LineSeries()
                {
                    Title = "Вер. распред.",
                    Values = new ChartValues<double>(p)
                }
            });
        }
    }
}
