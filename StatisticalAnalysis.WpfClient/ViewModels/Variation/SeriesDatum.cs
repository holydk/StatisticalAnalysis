using StatisticalAnalysis.WpfClient.HypothesisTesting.Models;

namespace StatisticalAnalysis.WpfClient.ViewModels.Variation
{
    public class SeriesDatum<T> : ValidationViewModelBase
    {
        public SeriesDatum()
        {
            Value = default(T);
        }

        public SeriesDatum(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => Get(() => Value);
            set => Set(() => Value, value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Value.Equals((obj as SeriesDatum<T>).Value);
        }

        public override int GetHashCode() => Value.GetHashCode();
    }

    public class GroupSeriesDatum<TVariant> : SeriesDatum<int>
    {
        public TVariant Variant
        {
            get => Get(() => Variant);
            set => Set(() => Variant, value);
        }

        public GroupSeriesDatum()
        {
            AddRule(() => Value, () => Value > 0, "Частота должны быть больше нуля.");
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
