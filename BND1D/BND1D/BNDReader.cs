using System.Collections.Generic;
using System.IO;

namespace BND1D
{
    /// <summary>
    /// Reading specific file *.csv / *.txt of TRHT project
    /// </summary>
    class BNDReader
    {
        public int ReLength = 0;
        public List<double> RePosition = null;
        public List<double> ReTemperature = null;
        public List<double> ReDivergence = null;

        public BNDReader(string fileName, FileType fileType)
        {
            using (var reader = new StreamReader(fileName))
            {
                RePosition = new List<double>();
                ReTemperature = new List<double>();
                ReDivergence = new List<double>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    char splitBy = (fileType == FileType.CSV) ? ',' : ' ';
                    var values = line.Split(splitBy);

                    RePosition.Add(double.Parse(values[0]));
                    ReTemperature.Add(double.Parse(values[1]));
                    ReDivergence.Add(double.Parse(values[2]));
                }

                ReLength = RePosition.Count;
            }
        }
    }

    enum FileType
    {
        CSV,
        TXT
    }
}
