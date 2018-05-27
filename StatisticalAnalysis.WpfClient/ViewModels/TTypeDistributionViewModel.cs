using StatisticalAnalysis.WpfClient.Commands;
using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.HypothesisTesting;
using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels.Variation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class TTypeDistributionViewModel : InformationPageViewModel
    {
        public IEnumerable<DistributionType> DistributionTypes { get; }

        public IEnumerable<DistributionSeriesInputType> DistributionSeriesInputTypes { get; }

        public IEnumerable<ICommandItem> CommandItems { get; }

        public ICollection<ICommandItem> ResultCommandItems { get; }

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

        public ITHypothesis THypothesis
        {
            get => Get(() => THypothesis);
            set => Set(() => THypothesis, value);
        }

        public bool IsResult
        {
            get => Get(() => IsResult);
            set => Set(() => IsResult, value);
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

            ResultCommandItems = new List<ICommandItem>();

            _informationItems = new IInformationItem[]
            {
                new InformationItem("Как ввести данные", "Вы можете ввести данные двумя способами: самостоятельно или загрузить из файла. Поддерживаемые форматы: *.csv. При загрузке данных учитывается тип вариационного ряда."),
                new InformationItem("Хотите сохранить результат?", "Сохранить вычисленные данные, включая график, вы можете в форматы: *.docx, *.xlsx, *.pdf.")
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
                        catch
                        {
                            MessageBox.Show("Произошла непредвиденная ошибка.");
                        }
                    }
                }
            }));
        }

        public ICommand CalculateVariationDataCommand
        {
            get => Get(() => CalculateVariationDataCommand, new RelayCommand(async (sender) =>
            {                             
                if (VariationData == null) return;

                var varPairs = VariationData.ToVariationPairs();

                if (varPairs == null)
                {
                    //MessageBox.Show("Некорректные данные.");

                    return;
                }

                IsBusy = true;

                await Task.Delay(3000);

                SetTHypothesis(varPairs);

                if (THypothesis != null)
                {
                    try
                    {
                        THypothesis.Execute();

                        IsResult = true;
                    }
                    catch
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка.");
                    }
                }

                IsBusy = false;

            }, () => !IsBusy && VariationData != null && SelectedDistributionType != null && SelectedDistributionSeriesInputType != null && SelectedSignificanceLevel != null));
        }

        public ICommand BackToInputDataCommand
        {
            get => Get(() => BackToInputDataCommand, new RelayCommand(async (sender) =>
            {
                IsBusy = true;

                await Task.Delay(250);

                IsBusy = false;
                IsResult = false;
            }));
        }

        private void SetTHypothesis(IVariationPair[] varPairs)
        {
            if (!SelectedDistributionType.HasValue) return;

            switch (SelectedDistributionType)
            {
                case DistributionType.Binomial:

                    var discretePairs = (DiscretePair[])varPairs;
                    
                    THypothesis = new TBinomial(discretePairs, SelectedSignificanceLevel.Value);

                    break;

                case DistributionType.ContinuousUniform:

                    var intervals = (СontinuousPair[])varPairs;

                    THypothesis = new TContinuousUniform(intervals, SelectedSignificanceLevel.Value);

                    break;

                case DistributionType.Normal:

                    intervals = (СontinuousPair[])varPairs;

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
