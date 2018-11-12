using System;

namespace BND2D
{
    internal class Function
    {
        /// <summary>
        /// Function temperature by given position
        /// </summary>
        /// <param name="Tw"></param>
        /// <param name="postision"></param>
        /// <returns></returns>
        public static double Temperature(double Tw, double postision)
        {
            double T0 = 2, // 2000,
                   M0 = 2; // 4.0;

            return Tw + (T0 - Tw) * Math.Pow(postision, M0);
        }

        /// <summary>
        /// Test function #1: z(x, y) = cos(x1 * x2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double TestFunction1(double x1, double x2) => Math.Cos(x1 * x2);

        /// <summary>
        /// Test function #2: z(x, y) = sqrt(x^2 + y^2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double TestFunction2(double x1, double x2) => Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(x2, 2));

        /// <summary>
        /// Test function #3: z(x, y) = (x^2 + y^2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double TestFunction3(double x1, double x2) => (Math.Pow(x1, 2) + Math.Pow(x2, 2));
    }
}
