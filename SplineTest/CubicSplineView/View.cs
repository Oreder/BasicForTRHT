using System;
using System.Windows.Forms;

namespace CubicSplineView
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();

            // Clear chart
            foreach (var s in chart.Series)
            {
                s.Points.Clear();
            }

            try
            {
                int N = 40;
                double T0 = 10000,
                       Tw = 2000,
                       M0 = 4;

                double[] x = new double[N];
                double[] y = new double[N];

                for (long i = 0; i < N; i++)
                {
                    x[i] = (double)i / N;
                    y[i] = Temperature(x[i], Tw, T0, M0);
                }

                var machine = new Spline(x, y);

                double tmpX = 0.51;
                double exactX = Temperature(tmpX, Tw, T0, M0);
                double interpolatedX = machine.Interpolate(tmpX);

                double backInterpolatedX = machine.BackInterpolate(interpolatedX);

                double[] x1 = new double[2 * N];
                for (long i = 0; i < x1.Length; i++)
                    x1[i] = Math.Pow((double)i / (x1.Length - 1), 4.0);

                // Validate value
                x1[0] = x[0];
                x1[x1.Length - 1] = x[x.Length - 1];

                double[] y1 = machine.Interpolate(x1);

                Draw(0, x, y);
                Draw(1, x1, y1);

                MessageBox.Show(string.Format("{0} : {1}\n{2} : {3}", exactX, interpolatedX, tmpX, backInterpolatedX));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        /// <summary>
        /// Draw chart from given 2 lists of points
        /// </summary>
        /// <param name="iGraph"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void Draw(int iGraph, double[] X, double[] Y)
        {
            try
            {
                chart.Series[iGraph].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart.Series[iGraph].BorderWidth = 2;

                for (int i = 0; i < X.Length; i++)
                {
                    chart.Series[iGraph].Points.AddXY(X[i], Y[i]);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());    
            }
        }

        private double Temperature(double position, double Tw, double T0, double M0)
        {
            return T0 + (Tw - T0) * Math.Pow(position, M0);
        }
    }
}
