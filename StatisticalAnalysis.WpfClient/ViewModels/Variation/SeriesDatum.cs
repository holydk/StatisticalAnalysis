using StatisticalAnalysis.HypothesisTesting.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class SeriesDatum<T> : ValidationViewModelBase
    {
        public T Value
        {
            get => Get(() => Value);
            set => Set(() => Value, value);
        }
    }

    public class GroupSeriesDatum<TVariant> : SeriesDatum<int>
    {
        public TVariant Variant
        {
            get => Get(() => Variant);
            set => Set(() => Variant, value);
        }
    }

    public class IntervalSeriesDatum : GroupSeriesDatum<Interval>
    {
        public IntervalSeriesDatum()
        {
            AddRule(() => Variant, () => Variant != null && Variant.Upper > Variant.Lower, "Поле не должно быть пустым и верхняя граница должна быть больше нижней.");
        }
    }
}
