using System;

namespace StatisticalAnalysis.WpfClient.HypothesisTesting.Models
{
    public class THypothesisResult
    {
        public int Number { get; }
        public string Variant { get; }
        public int EmpiricalFrequency { get; }
        public double Probability { get; }
        public double TheoreticalFrequency { get; }
        public double Criterion1 { get; }
        public double Criterion2 { get; }

        public THypothesisResult(int number, string variant, int empiricalFrequency, double probability, double theoreticalFrequency, double criterion1, double criterion2)
        {
            Number = number;
            Variant = variant ?? throw new ArgumentNullException(nameof(variant));
            EmpiricalFrequency = empiricalFrequency;
            Probability = probability;
            TheoreticalFrequency = theoreticalFrequency;
            Criterion1 = criterion1;
            Criterion2 = criterion2;
        }
    }
}
