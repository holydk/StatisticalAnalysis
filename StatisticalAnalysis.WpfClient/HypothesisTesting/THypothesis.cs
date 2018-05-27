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
    public abstract class THypothesis<TPair, TDistribution> : ITHypothesis
        where TPair : IVariationPair
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

        protected TPair[] _pairs;

        protected abstract int _r { get; }

        public THypothesis(TPair[] pairs, double significanceLevel)
        {
            if (significanceLevel == 0 || significanceLevel >= 1)
                throw new ArgumentOutOfRangeException(nameof(significanceLevel));

            SignificanceLevel = significanceLevel;

            _pairs = pairs ?? throw new ArgumentNullException(nameof(pairs));
            _results = new List<THypothesisResult>();
            _series = new SeriesCollection();
        }

        protected abstract double Probability(TPair pair);

        public void Execute()
        {
            var sumFrequency = _pairs.Sum(p => p.Frequency);

            CalculateStatistics(sumFrequency);

            _empiricalValue = _results.Sum(r => 
            {
                if (double.IsInfinity(r.Criterion2))               
                    return 0;               
                else
                    return r.Criterion2;
            });

            _criticalValue = ChiSquared.InvCDF(_results.Count - _r - 1, 1 - SignificanceLevel);
            _isValid = _empiricalValue > _criticalValue ? false : true;

            BuildSeries(sumFrequency);
        }

        protected abstract void CalculateStatistics(int sumFrequency);

        protected void BuildSeries(int sumFrequency)
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
