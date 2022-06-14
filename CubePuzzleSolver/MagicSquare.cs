using System;
using System.Collections.Generic;

namespace Myitian.CubePuzzleSolver
{
    /// <summary>机关立方幻方</summary>
    public class MagicSquare
    {
        /// <summary>朝向字符串数组</summary>
        public static string[] FacingStrings => new string[] { "u", "n", "e", "s", "w", "n+", "e+", "s+", "w+", "x" };
        /// <summary>幻方数字</summary>
        public int[,] MagicSquare_Number;
        /// <summary>幻方字符串</summary>
        public string[,] MagicSquare_String => MagicSquareConvert(MagicSquare_Number);

        /// <summary>
        /// 机关立方幻方
        /// </summary>
        /// <param name="facings">朝向</param>
        public MagicSquare(int[,] facings)
        {
            MagicSquare_Number = facings;
        }
        /// <summary>
        /// 机关立方幻方
        /// </summary>
        /// <param name="facings">朝向</param>
        public MagicSquare(string[,] facings)
        {
            MagicSquare_Number = MagicSquareConvert(facings);
        }
        /// <summary>
        /// 朝向转换
        /// </summary>
        /// <param name="facing">朝向字符串</param>
        /// <returns>朝向数字</returns>
        public static int MagicSquareFacingConvert(string facing)
        {
            switch (facing.ToLower())
            {
                case "0":
                case "u":
                case "unknown":
                    return 0;
                case "1":
                case "n":
                case "notrh":
                    return 1;
                case "2":
                case "e":
                case "east":
                    return 2;
                case "3":
                case "s":
                case "south":
                    return 3;
                case "4":
                case "w":
                case "west":
                    return 4;
                case "5":
                case "n+":
                case "notrh+":
                    return 5;
                case "6":
                case "e+":
                case "east+":
                    return 6;
                case "7":
                case "s+":
                case "south+":
                    return 7;
                case "8":
                case "w+":
                case "west+":
                    return 8;
                case "9":
                case "x":
                case "empty":
                    return 9;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 朝向转换
        /// </summary>
        /// <param name="facing">朝向数字</param>
        /// <param name="facingStrings">朝向字符串数组</param>
        /// <returns>朝向字符串</returns>
        public static string MagicSquareFacingConvert(int facing, string[] facingStrings = null)
        {
            if (facingStrings == null || facingStrings.Length < 9)
            {
                facingStrings = FacingStrings;
            }
            if (facing >= 0 && facing <= 9)
            {
                return facingStrings[facing];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 幻方表示方法转换
        /// </summary>
        /// <param name="facings">幻方（字符串）</param>
        /// <returns>幻方（数字）</returns>
        public static int[,] MagicSquareConvert(string[,] facings)
        {
            int w = facings.GetLength(0), h = facings.GetLength(1);
            int[,] result = new int[w, h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = MagicSquareFacingConvert(facings[i, j]);
                }
            }
            return result;
        }
        /// <summary>
        /// 幻方表示方法转换
        /// </summary>
        /// <param name="facings">幻方（数字）</param>
        /// <param name="facingStrings">朝向字符串数组</param>
        /// <returns>幻方（字符串）</returns>
        public static string[,] MagicSquareConvert(int[,] facings, string[] facingStrings = null)
        {
            int w = facings.GetLength(0), h = facings.GetLength(1);
            string[,] result = new string[w, h];
            if (facingStrings == null || facingStrings.Length <= 9)
            {
                facingStrings = FacingStrings;
            }
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[i, j] = MagicSquareFacingConvert(facings[i, j], facingStrings);
                }
            }
            return result;
        }
        /// <summary>
        /// 检查幻方
        /// </summary>
        /// <returns>是否符合幻方要求</returns>
        public bool Check() => Check(MagicSquare_Number);
        /// <summary>
        /// 检查幻方
        /// </summary>
        /// <param name="currentFacings">当前朝向</param>
        /// <returns>是否符合幻方要求</returns>
        public static bool Check(int[,] currentFacings)
        {
            int w = currentFacings.GetLength(0), h = currentFacings.GetLength(1);
            int[] rowSums = new int[h];
            int[] columnSums = new int[w];
            int diagonalSum0 = 0;
            int diagonalSum1 = 0;
            for (int c = 0; c < w; c++)
            {
                for (int r = 0; r < h; r++)
                {
                    int current = currentFacings[r, c];
                    rowSums[r] += current;
                    columnSums[c] += current;
                    if (r == c)
                    {
                        diagonalSum0 += current;
                    }
                    if (r == w - 1 - c)
                    {
                        diagonalSum1 += current;
                    }
                }
            }
            if (diagonalSum0 != diagonalSum1)
            {
                return false;
            }
            foreach (int rs in rowSums)
            {
                if (rs != diagonalSum0)
                {
                    return false;
                }
            }
            foreach (int cs in columnSums)
            {
                if (cs != diagonalSum0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 解幻方
        /// </summary>
        /// <returns>幻方的解</returns>
        public int[,] Crack()
        {
            int w = MagicSquare_Number.GetLength(0), h = MagicSquare_Number.GetLength(1);
            int[,] result = new int[w, h];
            Array.Copy(MagicSquare_Number, result, result.Length);
            List<long> unknownSquares = new List<long>();
            List<int> unusedNumbers = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    unusedNumbers.Remove(result[i, j]);
                    if (result[i, j] == 0)
                    {
                        unknownSquares.Add(((long)i << 32) | (long)j);
                    }
                }
            }
            Arrangement<int> arrangement = new Arrangement<int>(unusedNumbers.ToArray());
            foreach (int[] seq in arrangement)
            {
                for (int i = 0; i < seq.Length; i++)
                {
                    int x = (int)(unknownSquares[i] >> 32);
                    int y = (int)(unknownSquares[i] & 0xFFFFFFFF);
                    result[x, y] = seq[i];
                }
                if (Check(result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}
