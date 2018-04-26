using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using StatisticalAnalysis.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels.Variation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class TTypeDistributionViewModel : InformationPageViewModel
    {
        public IEnumerable<DistributionType> DistributionTypes { get; }

        public IEnumerable<DistributionSeriesInputType> DistributionSeriesInputTypes { get; }

        public IEnumerable<ICommandItem> CommandItems { get; }

        public DistributionSeriesInputType? SelectedDistributionSeriesInputType
        {
            get => Get(() => SelectedDistributionSeriesInputType, null);
            set
            {
                Set(() => SelectedDistributionSeriesInputType, value);
                SwitchVariationData();
            }
        }

        public DistributionType? SelectedDistributionType
        {
            get => Get(() => SelectedDistributionType, null);
            set
            {
                Set(() => SelectedDistributionType, value);
                SwitchVariationData();
            }
        }

        /// <summary>
        /// Discrete or Interval Variation
        /// </summary>
        public ISeriesData VariationData
        {
            get => Get(() => VariationData);
            set => Set(() => VariationData, value);
        }

        public TTypeDistributionViewModel()
            : base("О законе распределения")
        {
            var distributionType = typeof(DistributionType);
            var distributionSeriesInputType = typeof(DistributionSeriesInputType);

            DistributionTypes = Enum
                .GetValues(distributionType).OfType<DistributionType>()
                .OrderBy(d => distributionType.GetField(d.ToString()).ToDescription(), StringComparer.InvariantCultureIgnoreCase);
            DistributionSeriesInputTypes = Enum
                .GetValues(distributionSeriesInputType).OfType<DistributionSeriesInputType>()
                .OrderBy(d => distributionSeriesInputType.GetField(d.ToString()).ToDescription(), StringComparer.InvariantCultureIgnoreCase);

            CommandItems = new ICommandItem[]
            {
                new CommandItem("Загрузить", MaterialDesignThemes.Wpf.PackIconKind.Upload, ReadDataFromFileCommand),
                new CommandItem("Очистить", MaterialDesignThemes.Wpf.PackIconKind.Delete, new RelayCommand((sender) => VariationData?.ClearData()))
            };
        }

        public ICommand ReadDataFromFileCommand
        {
            get => Get(() => ReadDataFromFileCommand, new RelayCommand(async (sender) =>
            {
                if (VariationData == null) return;

                using (var openFileDialog = new OpenFileDialog()
                {
                    Filter = "Все файлы (*.*)|*.*|CSV|*.csv",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            await VariationData.FromFileAsync(openFileDialog.FileName);
                        }
                        catch (System.IO.IOException ioEx)
                        {
                            MessageBox.Show(ioEx.Message);
                        }
                        catch (NotSupportedException notSupEx)
                        {
                            // Invalid file format
                            MessageBox.Show(notSupEx.Message);
                        }
                        catch (NotImplementedException)
                        {
                            MessageBox.Show("Данная фича пока что не реализована.");
                        }
                        catch (InvalidOperationException invOpEx)
                        {
                            // Invalid Convert
                            MessageBox.Show(invOpEx.Message);
                        }
                    }
                }
            }));
        }

        public ICommand CalculateVariationDataCommand
        {
            get => Get(() => CalculateVariationDataCommand, new RelayCommand((sender) =>
            {
                //IsBusy = true;

                DistributionSeries distributionSeries = null;
                ICollection<IVariationPair<object>> varPairs = null;

                try
                {
                    varPairs = VariationData.ToVariationPairs();
                }
                catch (NotImplementedException)
                {
                    MessageBox.Show("Данная фича пока что не реализована.");
                }

                if (varPairs == null) return;

                switch (SelectedDistributionType)
                {
                    case DistributionType.Binomial:

                        

                        break;

                    case DistributionType.DiscreteUniform:

                        break;

                    case DistributionType.Normal:

                        var intervals = (ICollection<IVariationPair<Variant<Interval>>>)varPairs;
                        
                        var sumFrequency = intervals.Sum(
                            interval => interval.Frequency);

                        var mean = intervals.Sum(
                            interval => interval.Variant.Value.Middle * interval.Frequency) / sumFrequency;

                        var stddev = intervals.Sum(
                            interval => Math.Pow(interval.Variant.Value.Middle - mean, 2) * interval.Frequency) / sumFrequency;

                        distributionSeries = new СontinuousDistributionSeries(new Normal(mean, stddev), intervals);

                        break;

                    default:
                        break;
                }

                if (distributionSeries != null)
                {
                    var sum = .0;

                    foreach (var pair in varPairs)
                    {
                        sum += distributionSeries.CumulativeDistribution(pair);
                    }

                    var chi0 = ChiSquared.InvCDF(5, 8.4);
                    var chi1 = 2 * (1 - ChiSquared.InvCDF(5, sum));
                    var chi2 = ChiSquared.InvCDF(5, 0.05);
                }

            }, () => !IsBusy && VariationData != null));
        }

        private void SwitchVariationData()
        {
            if (SelectedDistributionSeriesInputType.HasValue &&
                SelectedDistributionType.HasValue)
            {
                switch (SelectedDistributionSeriesInputType)
                {
                    case DistributionSeriesInputType.Grouped:

                        SetVariationModel(SelectedDistributionType.Value.ToDistributionSeriesType().Value, true);

                        break;

                    case DistributionSeriesInputType.NotGrouped:

                        SetVariationModel(SelectedDistributionType.Value.ToDistributionSeriesType().Value, false);

                        break;

                    default:
                        break;
                }
            }
        }

        private void SetVariationModel(DistributionSeriesType seriesType, bool isGrouped)
        {
            switch (seriesType)
            {
                case DistributionSeriesType.Сontinuous:

                    if (isGrouped)
                        VariationData = new СontinuousGroupedData();
                    else
                        VariationData = new СontinuousData();

                    break;

                case DistributionSeriesType.Discrete:

                    if (isGrouped)
                        VariationData = new DiscreteGroupedData();
                    else
                        VariationData = new DiscreteData();

                    break;

                default:
                    break;
            }
        }
    }
}
