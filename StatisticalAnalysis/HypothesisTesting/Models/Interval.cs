namespace StatisticalAnalysis.HypothesisTesting.Models
{
    public class Interval
    {
        public double Lower { get; set; }
        public double Upper { get; set; }
        public double Middle => (Upper + Lower) / 2;

        public Interval()
        {

        }

        public Interval(double lower, double upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public override string ToString()
        {
            return $"{Lower} - {Upper}";
        }
    }
}
