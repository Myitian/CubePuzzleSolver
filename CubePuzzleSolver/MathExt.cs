namespace Myitian.CubePuzzleSolver
{
    /// <summary>
    /// 数学计算扩展
    /// </summary>
    public static class MathExt
    {
        /// <summary>排列数</summary>
        public static int ArrangementNumber(int n, int m)
        {
            int x = 1;
            for (int i = 0; i < m; i++)
            {
                x *= n - i;
            }
            return x;
        }

        /// <summary>
        /// 范围
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="end">结束</param>
        /// <param name="step">步长（仅正数）</param>
        /// <returns>整数序列</returns>
        public static int[] Range(int start, int end, int step = 1)
        {
            int len = end - start;
            if (len <= 0)
            {
                return new int[0];
            }
            int[] result = new int[len];
            int i = 0;
            while (start < end)
            {
                result[i++] = start;
                start += step;
            }
            return result;
        }
    }
}
