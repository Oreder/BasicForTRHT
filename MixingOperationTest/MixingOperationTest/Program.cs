using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operation;

namespace MixingOperationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Generate source
            long NF = 10, NG = 17;

            // Generate pairs ( x, f(x) )
            double[] orgX = new double[NF];
            double[] orgF = new double[NF];

            orgX[0] = 0;
            orgF[0] = f(orgX[0]);

            for (long i = 1; i < orgX.LongLength; i++)
            {
                orgX[i] = orgX[i - 1] + Generate(0.1, 0.3);
                orgF[i] = f(orgX[i]);
            }

            // Generate pairs ( x, g(x) )
            double[] newX = new double[NG];
            double[] newG = new double[NG];

            double node = (orgX[orgX.LongLength - 1] - orgX[0]) / (newX.LongLength);
            for (long i = 0; i < newX.LongLength; i++)
            {
                newX[i] = i * node + Generate(node) * Generate(0.5, 1);
                newG[i] = g(newX[i]);
            }

            // Show source data
            Console.WriteLine("\torgX\t\torgF\n     ---------------------------");
            for (long i = 0; i < orgX.LongLength; i++)
            {
                Console.WriteLine("\t{0:F4}\t{1}", orgX[i], orgF[i]);
            }
            Console.WriteLine("\n\tnewX\t\tnewG\n     ---------------------------");
            for (long i = 0; i < newX.LongLength; i++)
            {
                Console.WriteLine("\t{0:F4}\t{1}", newX[i], newG[i]);
            }
            #endregion

            #region OperationTest
            
            // Custom test
            double testX = 0.572326;
            double testF = f(testX);
            double testG = g(testX);

            // Mixing method
            var orgFX = new Spline(orgX, orgF);
            var newGX = new Spline(newX, newG);

            double backtrackX = orgFX.BackInterpolate(testF);
            double resultG = newGX.Interpolate(backtrackX);

            double error = Math.Abs((testG - resultG) / testG);

            Console.WriteLine("\n- Custom test #1: F(x) = {0}", testF);
            Console.WriteLine("\n- Original X: {0}\n- Interpolated X: {1}", testX, backtrackX);
            Console.WriteLine("\n- Original G(x): {0}\n- Interpolated G(x): {1}\n- Absolute error: {2}",
                testG, resultG, error);


            testF = (orgF[0] + orgF[orgF.LongLength - 1]) / 3 * 2;
            resultG = newGX.Interpolate(orgFX.BackInterpolate(testF));
            Console.WriteLine("\n\n- Custom test #2: F(x) = {0} -> G(x) = {1}", testF, resultG);

            #endregion

            Console.ReadKey();
        }

        static double f(double x)
        {
            return Math.Pow(x, 8.0);
        }

        static double g(double x)
        {
            return Math.Cos(x);
        }

        static double Generate(double border)
        {
            var r = new Random();
            return r.NextDouble() * border;
        }

        static double Generate(double left, double right)
        {
            var r = new Random();
            return left + r.NextDouble() * (right - left);
        }
    }
}
