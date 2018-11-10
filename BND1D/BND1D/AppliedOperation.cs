using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operation;

namespace BND1D
{
    class AppliedOperation
    {
        private double[] sourceX = null;
        private double[] sourceF = null;
        private double[] sourceG = null;
        private bool isValid = true;

        public AppliedOperation(double[] srcX, double[] srcF, double[] srcG)
        {
            if (srcX.LongLength == srcF.LongLength && srcX.LongLength == srcG.LongLength && srcX.LongLength >= 3)
            {
                sourceX = srcX;
                sourceF = srcF;
                sourceG = srcG;
            }
            else
                isValid = false;
        }

        public AppliedOperation(List<double> srcX, List<double> srcF, List<double> srcG)
        {
            if (srcX.Count == srcF.Count && srcX.Count == srcG.Count && srcX.Count >= 3)
            {
                sourceX = srcX.ToArray();
                sourceF = srcF.ToArray();
                sourceG = srcG.ToArray();
            }
            else
                isValid = false;
        }

        /// <summary>
        /// Main interpolation process (first way)
        /// Applying both direct and backtrack cubic interpolation
        /// </summary>
        /// <param name="f0">The new F-value</param>
        /// <returns>The new G-value</returns>
        public double Interpolate(double f0)
        {
            if (!isValid)
                return double.NaN;

            // filtering data in order to apply back interpolation
            // remember that the second array (sourceF) has ordering type.
            Filter.Process(1E-5, ref sourceF);

            // Step 1. Operator F^-1 (backtrack process)
            var engine = new Spline(sourceX, sourceF);
            double value = engine.BackInterpolate(f0);
            engine = null;

            // Step 2. Operator G (direct process)
            engine = new Spline(sourceX, sourceG);
            double g0 = engine.Interpolate(value);
            engine = null;

            return g0;
        }

        /// <summary>
        /// Main interpolation process (second way)
        /// Applying only direct interpolation
        /// </summary>
        /// <param name="f0">The new F-value</param>
        /// <returns>The new G-value</returns>
        public double DirectInterpolate(double f0)
        {
            if (!isValid)
                return double.NaN;

            // filtering data in order to apply back interpolation
            // remember that the second array (sourceF) has ordering type.
            Filter.Process(1E-5, ref sourceF);

            // Main operator
            var engine = new Spline(sourceF, sourceG);
            double g0 = engine.Interpolate(f0);
            engine = null;

            return g0;
        }

        public void ShowCoefs(long index)
        {
            if (!isValid)
                return;

            // filtering data in order to apply back interpolation
            // remember that the second array (sourceF) has ordering type.
            Filter.Process(1E-5, ref sourceF);

            var e1 = new Spline(sourceX, sourceF);
            var e2 = new Spline(sourceX, sourceG);
            var e3 = new Spline(sourceF, sourceG);

            long N = e1.SizeOfCoefs;
            double[][] C1 = e1.GetCoefs();
            double[][] C2 = e2.GetCoefs();
            double[][] C3 = e3.GetCoefs();

            for (long i = 0; i < N; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}", C1[index][i], C2[index][i], C3[index][i]);
            }

            e1 = null;
            e2 = null;
            e3 = null;
        }
    }
}
