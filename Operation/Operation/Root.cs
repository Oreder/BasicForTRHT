using System;

namespace Operation
{
    public class Root
    {
        /// <summary>
        /// Get the only root of equation: 
        ///     coefs[0] / 6 * x^3 + coefs[1] / 2 * x^2 + coefs[2] * x + coefs[3] = 0
        /// by Bisector method
        /// </summary>
        /// <param name="coefs">Coefficients</param>
        /// <param name="H">Right border of X-value</param>
        /// <returns>Root in given range</returns>
        public static double Solve(double[] coefs, double H)
        {
            double left = 0.0,
                   right = H,
                   middle = (left + right) / 2.0;

            double y = F(coefs, middle);
            double leftY = F(coefs, left);

            while (Math.Abs(y) > Eps || Math.Abs(left - right) > Eps)
            {
                if (y * leftY <= 0)
                    right = middle;
                else
                    left = middle;

                middle = (left + right) / 2.0;
                y = F(coefs, middle);
                leftY = F(coefs, left);
            }

            return middle;
        }

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="coefs"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double F(double[] coefs, double x)
        {
            return coefs[3] + x * (coefs[2] + x * (coefs[1] + x * coefs[0] / 3.0) / 2.0);
        }

#pragma warning disable IDE0044 // Add readonly modifier
        /// <summary>
        /// Epsilon value
        /// </summary>
        private static double Eps = 1E-9;
#pragma warning restore IDE0044 // Add readonly modifier
    }
}
