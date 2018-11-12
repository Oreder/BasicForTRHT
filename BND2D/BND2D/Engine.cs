using Operation;

namespace BND2D
{
    /// <summary>
    /// Operation: interpolation from F to G by given parameters.
    /// Operator: F^-1 * G
    /// </summary>
    public class RegularEngine
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private double[] srcX = null;
#pragma warning restore IDE0044 // Add readonly modifier
        private double[] srcY = null;

        /// <summary>
        /// F[i] = F(y[i], x)
        /// </summary>
        private double[][] srcF = null;

        /// <summary>
        /// G[i] = G(y[i], x)
        /// </summary>
        private double[][] srcG = null;

        private bool isValid = true;

        public RegularEngine(double[] X, double[] Y, double[][] F, double[][] G)
        {
            if (X == null || Y == null || F == null || G == null ||
                !(F.LongLength == Y.LongLength && F[0].LongLength == X.LongLength) ||
                !(G.LongLength == Y.LongLength && G[0].LongLength == X.LongLength))
                isValid = false;
            else
            {
                srcX = X;
                srcY = Y;
                srcF = F;
                srcG = G;
            }
        }

        /// <summary>
        /// Interpolation with constant axis X and given new axis Y
        /// Solution of problem:
        ///     F(x, y = y0) => G(x, y = y0) as F(x) => G(x)
        /// </summary>
        /// <param name="newY"></param>
        /// <returns></returns>
        public double[] Interpolate(double[] f)
        {
            if (!isValid)
                return null;

            // First step: find the line y = f(x)
            var exactFY = new double[sizeX];

            for (long i = 0; i < sizeX; i++)
            {
                var tmpF = GetYByX(i, srcF);      // X = x[i] = const, F(x) changes

                var spline = new Spline(srcY, tmpF);
     
                exactFY[i] = spline.BackInterpolate(f[i]);  // get the correct point (Y[i], X[i])

                spline = null;
            }

            // Second step: find the line y = g(x)
            var newG = new double[sizeX];

            for (long i = 0; i < sizeX; i++)
            {
                var tmpG = GetYByX(i, srcG);      // X = x[i] = const, G(x) changes

                var spline = new Spline(srcY, tmpG);

                newG[i] = spline.Interpolate(exactFY[i]);

                spline = null;
            }

            return newG;
        }

        public double[] getX
        {
            get
            {
                return srcX;
            }
        }

        private double[] GetYByX(long index, double[][] matrix)
        {
            if (matrix == null)
                return null;

            double[] newF = new double[matrix.LongLength];

            for (long i = 0; i < matrix.LongLength; i++)
            {
                newF[i] = matrix[i][index];
            }

            return newF;
        }

        private long sizeX => srcF[0].LongLength;
        private long sizeY => srcF.LongLength;
    }
}
