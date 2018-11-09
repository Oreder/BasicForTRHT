using System;

namespace CubicSplineView
{
    class Spline
    {
        private double[] sourceX = null;
        private double[] sourceY = null;
        private bool flag = true;

        /// <summary>
        /// Given by source data
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public Spline(double[] X, double[] Y)
        {
            if (X != null && Y != null)
            {
                sourceX = X;
                sourceY = Y;

                if (Process() == -1)
                    flag = false;
            }
            else
                flag = false;
        }

        /// <summary>
        /// It's Interpolation with cubic splines.
        /// </summary>
        /// <returns>The New data OY (new ordinate)</returns>
        /// sourceX is only data where sourceX[i] greater than sourceX[i - 1].
        /// sourceX.Length must be more than 3.
        /// You can see the full algotithm at "de Boor, C., A Practical Guide to Splines, Springer-Verlag, 1978".
        public long Process()
        {
            long N = sourceX.LongLength;
            if (sourceX.LongLength != sourceY.LongLength)
                return -1;

            if (sourceX.LongLength <= 3)
                return -1;

            /*
             * Spline[i] = f[i] + b[i]*(x - x[i]) + c[i]*(x - x[i])^2 + d[i]*(x - x[i])^3
             * First: We prepare data for algorithm by calculate dx[i]. If dx[i] equal to zero then function return null.
             * Second: We need calculate coefficients b[i]. 
             * b[i] = 3 * ( (f[i] - f[i - 1])*dx[i]/dx[i - 1] + (f[i + 1] - f[i])*dx[i - 1]/dx[i] ),  i = 1, ... , N - 2
             * How calculate b[0] and b[N - 1] you can see below. And b can be find by means of tridiagonal matrix A[N, N].
             * 
             * A[N, N] - Tridiagonal Matrix:
             *      beta(0)     gama(0)     0            0           0   ...
             *      alfa(1)     beta(1)     gama(1)      0           0   ...
             *      0           alfa(2)     beta(2)     gama(2)      0
             *      ...
             * A*x=b
             * We calculate inverse of tridiagonal matrix by Gauss method and transforming equation A*x=b to the form I*x=b, where I - Identity matrix.
             * Fird: Now we can found coefficients c[i], d[i] where i = 0, ... , N - 2
             */

            long Nx = N - 1;
            double[] dx = new double[Nx];

            double[] b = new double[N];
            double[] alfa = new double[N];
            double[] beta = new double[N];
            double[] gama = new double[N];

            coefs = new double[4][];
            for (long i = 0; i < 4; i++)
                coefs[i] = new double[Nx];

            for (long i = 0; i + 1 <= Nx; i++)
            {
                dx[i] = sourceX[i + 1] - sourceX[i];
                if (dx[i] == 0.0)   // non-continuous function
                    return -1;
            }

            for (long i = 1; i + 1 <= Nx; i++)
            {
                b[i] = 3.0 * (dx[i] * ((sourceY[i] - sourceY[i - 1]) / dx[i - 1]) + dx[i - 1] * ((sourceY[i + 1] - sourceY[i]) / dx[i]));
            }

            b[0] = ((dx[0] + 2.0 * (sourceX[2] - sourceX[0])) * dx[1] * ((sourceY[1] - sourceY[0]) / dx[0]) +
                        Math.Pow(dx[0], 2.0) * ((sourceY[2] - sourceY[1]) / dx[1])) / (sourceX[2] - sourceX[0]);

            b[N - 1] = (Math.Pow(dx[Nx - 1], 2.0) * ((sourceY[N - 2] - sourceY[N - 3]) / dx[Nx - 2]) + (2.0 * (sourceX[N - 1] - sourceX[N - 3])
                + dx[Nx - 1]) * dx[Nx - 2] * ((sourceY[N - 1] - sourceY[N - 2]) / dx[Nx - 1])) / (sourceX[N - 1] - sourceX[N - 3]);

            beta[0] = dx[1];
            gama[0] = sourceX[2] - sourceX[0];
            beta[N - 1] = dx[Nx - 1];
            alfa[N - 1] = (sourceX[N - 1] - sourceX[N - 3]);
            for (long i = 1; i < N - 1; i++)
            {
                beta[i] = 2.0 * (dx[i] + dx[i - 1]);
                gama[i] = dx[i];
                alfa[i] = dx[i - 1];
            }
            double c = 0.0;
            for (long i = 0; i < N - 1; i++)
            {
                c = beta[i];
                b[i] /= c;
                beta[i] /= c;
                gama[i] /= c;

                c = alfa[i + 1];
                b[i + 1] -= c * b[i];
                alfa[i + 1] -= c * beta[i];
                beta[i + 1] -= c * gama[i];
            }

            b[N - 1] /= beta[N - 1];
            beta[N - 1] = 1.0;
            for (long i = N - 2; i >= 0; i--)
            {
                c = gama[i];
                b[i] -= c * b[i + 1];
                gama[i] -= c * beta[i];
            }

            for (long i = 0; i < Nx; i++)
            {
                double dzzdx = (sourceY[i + 1] - sourceY[i]) / Math.Pow(dx[i], 2.0) - b[i] / dx[i];
                double dzdxdx = b[i + 1] / dx[i] - (sourceY[i + 1] - sourceY[i]) / Math.Pow(dx[i], 2.0);
                coefs[0][i] = (dzdxdx - dzzdx) / dx[i];
                coefs[1][i] = (2.0 * dzzdx - dzdxdx);
                coefs[2][i] = b[i];
                coefs[3][i] = sourceY[i];
            }

            return 0;
        }

        /// <summary>
        /// Get interpolated value
        /// </summary>
        /// <param name="newX">The new data OX</param>
        /// <returns>The New data OY (new ordinate)</returns>
        public double Interpolate(double newX)
        {
            if (!flag)
                return double.NaN;

            long index = Search.BinarySearching(sourceX, newX);
            double h = newX - sourceX[index];
            return coefs[3][index] + h * (coefs[2][index] + h * (coefs[1][index] + h * coefs[0][index] / 3.0) / 2.0);
        }

        /// <summary>
        /// Get interpolated array
        /// </summary>
        /// <param name="newX">The new data OX</param>
        /// <returns>The New data OY (new ordinate)</returns>
        /// sourceX[0] must be equal newX[0] and sourceX[end] must be equal to newX[end].
        /// You can see the full algotithm at "de Boor, C., A Practical Guide to Splines, Springer-Verlag, 1978".
        public double[] Interpolate(double[] newX)
        {
            if (!flag)
                return null;

            if (sourceX[0] != newX[0])
                return null;

            if (sourceX[sourceX.LongLength - 1] != newX[newX.LongLength - 1])
                return null;

            long N = sourceX.LongLength;
            double[] newY = new double[newX.LongLength];
            long j = 0;
            for (long i = 0; i < N - 1; i++)
            {
                double h = 0.0;
                if (j >= newX.LongLength)
                    break;
                while (newX[j] < sourceX[i + 1])
                {
                    h = newX[j] - sourceX[i];
                    newY[j] = coefs[3][i] + h * (coefs[2][i] + h * (coefs[1][i] + h * coefs[0][i] / 3.0) / 2.0);
                    j++;
                    if (j >= newX.LongLength)
                        break;
                }
                if (j >= newX.LongLength)
                    break;
            }

            newY[newY.LongLength - 1] = sourceY[N - 1];
            return newY;
        }

        /// <summary>
        /// Back interpolation process
        /// </summary>
        /// <param name="newY">The new data OY</param>
        /// <returns>The New data OX (new ordinate)</returns>
        public double BackInterpolate(double newY)
        {
            if (!flag)
                return double.NaN;

            long index = Search.BinarySearching(sourceY, newY);
            double[] Coefs = GetCoefs(index);
            Coefs[3] -= newY;

            return sourceX[index] + Root.Solve(Coefs, sourceX[index + 1] - sourceX[index]);
        }

        /// <summary>
        /// Coeffients of cubic interpolation
        /// </summary>
        private static double[][] coefs = null;

        public static double[] GetCoefs(long index)
        {
            if (coefs == null)
                return null;

            return new double[] { coefs[0][index], coefs[1][index], coefs[2][index], coefs[3][index] };
        }


    }
}
