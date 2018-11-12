using System;

namespace BND2D
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<double, double, double> TestFunction = Function.TestFunction3;

            #region Source
            long N = 8, M = 5;
            double[] X = new double[N + 1];
            for (long j = 0; j <= N; j++)
            {
                X[j] = (double)j / N;
            }

            double[] T = new double[M];
            for (long i = 0; i < M; i++)
            {
                T[i] = 10.0 + i;
            }

            var surf = new SurfaceGenerate(X, T);
            double[][] F = surf.Generate(Function.Temperature);
            double[][] G = surf.Generate(TestFunction);
            surf = null;

            // Show source
            Show(X, T, F);
            Show(X, T, G);
            #endregion

            #region Operation Test 2D-surface: F -> G

            double testT = 13.7172;

            var engine = new RegularEngine(X, T, F, G);
            
            // Test data
            double[] newF = new double[X.LongLength];
            for (long i = 0; i < newF.LongLength; i++)
            {
                newF[i] = Function.Temperature(testT, X[i]);
            }

            // Mixing method
            double[] newG = engine.Interpolate(newF);

            // Real data
            double[] realG = new double[X.LongLength];
            double[] err = new double[X.LongLength];

            for (long i = 0; i < X.LongLength; i++)
            {
                realG[i] = TestFunction(testT, X[i]);
                err[i] = Math.Abs((realG[i] - newG[i]) / realG[i]) * 100.0;
            }

            #endregion

            #region Output (except the last value)
            // First line: X
            Console.Write("\n------------------------------------------");
            Console.WriteLine("--------------------------------------\n");
            for (long i = 0; i < X.LongLength - 1; i++)
            {
                Console.Write("\t{0}", X[i]);
            }

            // Second line: F(X, T=const) - correct values
            Console.Write("\nF0:");
            for (long i = 0; i < X.LongLength - 1; i++)
            {
                Console.Write("\t{0:F2}", newF[i]);
            }

            // Third line: G(X, T=const) - interpolated values
            Console.Write("\nG1:");
            for (long i = 0; i < X.LongLength - 1; i++)
            {
                Console.Write("\t{0:F2}", newG[i]);
            }

            // Forth line: G(X) - correct values
            Console.Write("\nG0:");
            for (long i = 0; i < X.LongLength - 1; i++)
            {
                Console.Write("\t{0:F2}", realG[i]);
            }

            // Fifth line: (absolute) Error
            Console.Write("\nErr(%):");
            for (long i = 0; i < X.LongLength - 1; i++)
            {
                Console.Write("\t{0:F4}", err[i]);
            }
            Console.WriteLine();

            #endregion

            Console.ReadLine();
        }

        static void Show(double[] x, double[] y, double[][] z)
        {
            if (x != null && y != null && z != null &&
                y.LongLength == z.LongLength && x.LongLength == z[0].LongLength)
            {
                // First line X[]
                for (long i = 0; i < x.LongLength; i++)
                    Console.Write("\t{0}", x[i]);
                Console.WriteLine();

                // Second line Y[k] Z[]
                for (long i = 0; i < y.LongLength; i++)
                {
                    Console.Write("{0}", y[i]);
                    for (long j = 0; j < z[i].LongLength; j++)
                    {
                        Console.Write("\t{0:F2}", z[i][j]);
                    }
                    Console.WriteLine();
                }

                // Finally
                Console.WriteLine();
            }
        }
    }
}
