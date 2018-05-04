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
    public abstract class THypothesis
    {
        public double SignificanceLevel { get; }

        protected double _empiricalValue;
        public double EmpiricalValue => _empiricalValue;

        protected double _criticalValue;
        public double CriticalValue => _criticalValue;

        protected ObservableCollection<THypothesisResult> _results;
        public ObservableCollection<THypothesisResult> Results => _results;

        protected IUnivariateDistribution _univariateDistribution;

        protected SeriesCollection _series;
        public SeriesCollection Series => _series;

        private bool? _isValid = null;
        public bool? IsValid => _isValid;

        protected abstract int _r { get; }

        public THypothesis(double significanceLevel)
        {
            if (significanceLevel == 0 || significanceLevel >= 1)
                throw new ArgumentOutOfRangeException(nameof(significanceLevel));

            SignificanceLevel = significanceLevel;
            _results = new ObservableCollection<THypothesisResult>();
            _series = new SeriesCollection();
        }

        protected void Execute<TVariant>(ICollection<IVariationPair<TVariant>> varPairs, Func<TVariant, double> execCdf)
            where TVariant : class
        {
            var sumFrequency = varPairs.Sum(p => p.Frequency);

            foreach (var pair in varPairs)
            {
                var p = execCdf(pair.Variant);
                var tFrequency = p * sumFrequency;
                var c1 = Math.Pow(pair.Frequency - tFrequency, 2);
                var c2 = c1 / tFrequency;

                _results.Add(new THypothesisResult(_results.Count + 1, pair.Variant.ToString(), pair.Frequency, p, tFrequency, c1, c2));
            }

            _empiricalValue = _results.Sum(r => r.Criterion2);
            _criticalValue = ChiSquared.InvCDF(varPairs.Count - _r, 1 - SignificanceLevel);

            _isValid = _empiricalValue > _criticalValue ? false : true;

            BuildSeries(sumFrequency);
        }

        private void BuildSeries(double sumFrequency)
        {
            var p = _results.Select(r => r.Probability);
            var empiricalF = _results.Select(r => (r.EmpiricalFrequency / (r.Probability * sumFrequency)) * r.Probability);

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
