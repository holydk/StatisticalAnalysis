using System;

namespace StatisticalAnalysis.WpfClient.Helpers
{
    public static class Statistics
    {
        /// <summary>
        /// Determination of the number of intervals by the Sturgess formula.
        /// </summary>
        /// <param name="count">The number of units in the aggregate</param>
        /// <returns></returns>
        public static int GetNumberOfIntervals(decimal count)
        {
            if (count <= decimal.Zero) return 0;

            if (decimal.ToDouble(count) > double.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            return (int)Math.Round(1 + 3.32 * Math.Log10((double)count));
        }
    }
}
