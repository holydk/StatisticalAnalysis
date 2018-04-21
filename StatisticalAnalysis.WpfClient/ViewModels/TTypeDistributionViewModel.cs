using StatisticalAnalysis.HypothesisTesting.Models;
using StatisticalAnalysis.WpfClient.Helpers;
using StatisticalAnalysis.WpfClient.Models;
using StatisticalAnalysis.WpfClient.ViewModels.Variation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatisticalAnalysis.WpfClient.ViewModels
{
    public class TTypeDistributionViewModel : InformationPageViewModel
    {
        public IEnumerable<DistributionType> DistributionTypes { get; }

        public IEnumerable<DistributionSeriesInputType> DistributionSeriesInputTypes { get; }

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
        public IConvertToVariationPairs VariationData
        {
            get => Get(() => VariationData);
            set => Set(() => VariationData, value);
        }

        public TTypeDistributionViewModel()
            : base("О законе распределения")
        {
            SelectedDistributionType = null;
            SelectedDistributionSeriesInputType = null;

            var distributionType = typeof(DistributionType);
            var distributionSeriesInputType = typeof(DistributionSeriesInputType);

            DistributionTypes = Enum
                .GetValues(distributionType).OfType<DistributionType>()
                .OrderBy(d => distributionType.GetField(d.ToString()).ToDescription(), StringComparer.InvariantCultureIgnoreCase);
            DistributionSeriesInputTypes = Enum
                .GetValues(distributionSeriesInputType).OfType<DistributionSeriesInputType>()
                .OrderBy(d => distributionSeriesInputType.GetField(d.ToString()).ToDescription(), StringComparer.InvariantCultureIgnoreCase);
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
