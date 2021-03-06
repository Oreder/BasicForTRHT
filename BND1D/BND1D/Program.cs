﻿using System;

namespace BND1D
{
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new BNDReader("Report.csv", FileType.CSV);

            for (int i = 0; i < machine.ReLength; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}", machine.RePosition[i], machine.ReTemperature[i], machine.ReDivergence[i]);
            }

            var engine = new AppliedOperation(machine.RePosition, machine.ReTemperature, machine.ReDivergence);
            double tmp = 15000.0;

            Console.WriteLine("\nDivergence #1: {0}", engine.Interpolate(tmp));
            Console.WriteLine("Divergence #2: {0}\n", engine.DirectInterpolate(tmp));

            engine.ShowCoefs(0);

            engine = null;
            machine = null;
            Console.ReadLine();
        }
    }
}
