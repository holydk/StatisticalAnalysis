using System;
using System.Collections.Generic;

namespace StatisticalAnalysis.Helpers
{
    public static class StatisticalAnalysisProvider
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

        /// <summary>
        /// Returns the interval step
        /// </summary>
        /// <param name="max">Max interval value</param>
        /// <param name="min">Min interval value</param>
        /// <param name="count">The number of units in the aggregate</param>
        /// <returns></returns>
        public static double GetIntervalStep(double min, double max, decimal count)
        {
            if (count <= decimal.Zero ||
                min >= max ||
                min < 0 ||
                max <= 0)
                return 0;

            return (max - min) / GetNumberOfIntervals(count);
        }
    }
}
