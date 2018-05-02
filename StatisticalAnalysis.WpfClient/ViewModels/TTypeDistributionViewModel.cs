﻿using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.HypothesisTesting;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
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

        public IEnumerable<double> SignificanceLevels { get; }

        public double? SelectedSignificanceLevel
        {
            get => Get(() => SelectedSignificanceLevel, null);
            set => Set(() => SelectedSignificanceLevel, value);
        }

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

        public ISeriesData VariationData
        {
            get => Get(() => VariationData);
            set => Set(() => VariationData, value);
        }

        public THypothesis THypothesis
        {
            get => Get(() => THypothesis);
            set => Set(() => THypothesis, value);
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

            SignificanceLevels = new double[]
            {
                0.01, 0.025, 0.05, 0.1
            };

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

                if (VariationData == null) return;

                var varPairs = VariationData.ToVariationPairs();

                if (varPairs == null || varPairs.Any(p => p.Frequency == 0))
                {
                    MessageBox.Show("Некорректные данные.");

                    return;
                }

                SetTHypothesis(varPairs);
                
            }, () => !IsBusy && VariationData != null && SelectedDistributionType != null && SelectedDistributionSeriesInputType != null && SelectedSignificanceLevel != null));
        }

        private void SetTHypothesis(ICollection<IVariationPair<object>> varPairs)
        {
            if (!SelectedDistributionType.HasValue) return;

            switch (SelectedDistributionType)
            {
                case DistributionType.Binomial:

                    var discretePairs = (ICollection<IVariationPair<Variant<int>>>)varPairs;

                    THypothesis = new TBinomial(discretePairs, SelectedSignificanceLevel.Value);

                    break;

                case DistributionType.DiscreteUniform:

                    discretePairs = (ICollection<IVariationPair<Variant<int>>>)varPairs;

                    THypothesis = new TDiscreteUniform(discretePairs, SelectedSignificanceLevel.Value);

                    break;

                case DistributionType.Normal:

                    var intervals = (ICollection<IVariationPair<Variant<Interval>>>)varPairs;

                    THypothesis = new TNormal(intervals, SelectedSignificanceLevel.Value);

                    break;

                default:
                    break;
            }
        }

        private void SwitchVariationData()
        {
            if (!SelectedDistributionSeriesInputType.HasValue ||
                !SelectedDistributionType.HasValue)
                return;                         

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
