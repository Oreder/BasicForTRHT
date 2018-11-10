using System;
using Operation;

namespace OperationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            long N = 37;
            double[] srcX = new double[N],
                     srcY = new double[N];

            for (long i = 0; i < N; i++)
            {
                srcX[i] = Math.Pow((double)i / (N - 1), 2.0);
                srcY[i] = F(srcX[i]);
            }

            double testX = 0.54812;

            var machine = new Spline(srcX, srcY);
            double interpolatedY = machine.Interpolate(testX),
                   realY = F(testX);

            double backInpterpolatedY = machine.BackInterpolate(interpolatedY);

            double absErrorX = 100.0 * Math.Abs((testX - backInpterpolatedY) / testX),
                   absErrorY = 100.0 * Math.Abs((realY - interpolatedY) / realY);

            Console.WriteLine("Source X: {0}\nInterpolated X: {1}\nAbsolute error X: {2} percents.\n",
                testX, backInpterpolatedY, absErrorX);
            Console.WriteLine("Source Y: {0}\nInterpolated Y: {1}\nAbsolute error Y: {2} percents.",
                realY, interpolatedY, absErrorY);
            Console.ReadLine();
        }

        static double F(double x)
        {
            return 1 / 12 * Math.Pow(x, 9) - Math.Pow(x, 2) + 2 * x;
            //return Math.Exp(x * Math.PI);
        }
    }
}
