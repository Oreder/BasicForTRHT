using System;

namespace BND2D
{
    class SurfaceGenerate
    {
        private double[] srcX = null;
        private double[] srcY = null;
        private bool isValid = true;

        public SurfaceGenerate(double[] X, double[] Y)
        {
            if (X != null && Y != null)
            {
                srcX = X;
                srcY = Y;
            }
            else
                isValid = false;
        }

        /// <summary>
        /// Generate 2D surface-data
        /// </summary>
        /// <param name="func">The generating function</param>
        /// <returns>The 2D-surface</returns>
        public double[][] Generate(Func<double, double, double> func)
        {
            if (!isValid)
                return null;

            var surf = new double[srcY.LongLength][];
            for (long i = 0; i < srcY.LongLength; i++)
            {
                surf[i] = new double[srcX.LongLength];
            }

            for (long i = 0; i < srcY.LongLength; i++)
            {
                for (long j = 0; j < srcX.LongLength; j++)
                {
                    surf[i][j] = func(srcY[i], srcX[j]);
                }
            }

            return surf;
        }
    }
}
