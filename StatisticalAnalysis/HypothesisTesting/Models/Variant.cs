namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public class Variant<T>
    {
        public T Value { get; }

        public Variant(T value)
        {
            Value = value;
        }
    }
}
