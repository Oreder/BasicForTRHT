using System;

namespace BND1D
{
    class Filter
    {
        /// <summary>
        /// Change source data in order to except error case of spline interpolation
        /// </summary>
        /// <param name="range">The right border, very small number</param>
        /// <param name="source">input data</param>
        public static void Process(double range, ref double[] source)
        {
            long i = 0;
            while (Math.Abs(source[i + 1] - source[i]) < 1E-9)
                i++;

            if (i > 0)
            {
                for (long j = 1; j <= i; j++)
                {
                    source[j] = source[j - 1] - Generate(range);
                }
            }
        }

        /// <summary>
        /// Get random value between zero and given border
        /// </summary>
        /// <param name="range">Positive number</param>
        /// <returns></returns>
        private static double Generate(double border)
        {
            Random rd = new Random();
            return rd.NextDouble() * border;
        }
    }
}
