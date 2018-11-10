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
        /// Main interpolation process
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
    }
}
