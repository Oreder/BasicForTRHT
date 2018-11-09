namespace CubicSplineView
{
    class Search
    {
        /// <summary>
        /// Method Binary searching data
        /// Applying only for the ordered-array
        /// </summary>
        /// <param name="data">An ordered array</param>
        /// <param name="value">The searching-data</param>
        /// <returns></returns>
        public static long BinarySearching(double[] data, double value)
        {
            long left = 0,
                 right = data.LongLength - 1;
            
            while(right - left != 1)
            {
                long middle = (left + right) / 2;
                if ((data[middle] - value) * (data[left] - value) < 0)
                    right = middle;
                else
                    left = middle;
            }

            return left;
        }
    }
}
