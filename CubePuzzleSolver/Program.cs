#if !Class
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Myitian.CubePuzzleSolver
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("机关立方解谜求解器 by Myitian");
            Console.WriteLine();
            bool bcontinue = true;
            while (bcontinue)
            {
                Console.WriteLine("输入要解的机关立方解谜类型（0：旋转型机关立方，1：点亮型机关立方，2：海祇岛机关立方幻方），或按Esc退出程序");
                Console.Write("：");
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D0:
                    case 0 when key.KeyChar == '0':
                        Console.WriteLine('0');
                        Solve0();
                        break;
                    case ConsoleKey.D1:
                    case 0 when key.KeyChar == '1':
                        Console.WriteLine('1');
                        Solve1();
                        break;
                    case ConsoleKey.D2:
                    case 0 when key.KeyChar == '2':
                        Console.WriteLine('2');
                        Solve2();
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("Esc");
                        bcontinue = false;
                        break;
                    default:
                        Console.WriteLine(key.KeyChar);
                        break;
                }
            }
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        static readonly decimal freq = Stopwatch.Frequency;
        static readonly char[] sp = { ' ' };
        static readonly char[] index = { '一', '二', '三' };
        static readonly string[] facingstr = { "未知", "北", "东", "南", "西", "北+", "东+", "南+", "西+", "空" };

        static void Solve0()
        {
            Console.WriteLine("旋转型机关立方");
            string line;
            string[] nums;
            List<int> cubesList;
            Console.WriteLine(@"          后面：2");
            Console.WriteLine(@"         +--------+");
            Console.WriteLine(@"         |        |");
            Console.WriteLine(@"  左面：1|        |右面：3");
            Console.WriteLine(@"         |        |");
            Console.WriteLine(@"         +--------+");
            Console.WriteLine(@"          前面：0");
            Console.WriteLine();
            Console.WriteLine(@"             ↑");
            Console.WriteLine(@"          观察方向");
            //
            Console.WriteLine("请输入当前可交互机关立方旋转状态（用空格分隔）");
            while (true)
            {
                Console.Write("：");
                line = (Console.ReadLine() ?? "").Trim();
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                cubesList = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= 3)
                        {
                            cubesList.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-3）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                if (cubesList.Count != 0)
                {
                    break;
                }
                Console.WriteLine("该项不能为空！");
            }
            int[] cubes = cubesList.ToArray();
            //
            Console.WriteLine("请输入当前不可交互机关立方旋转状态（用空格分隔，可留空）");
            Console.Write("：");
            line = (Console.ReadLine() ?? "").Trim();
            int[] nicubes = null;
            if (!string.IsNullOrEmpty(line))
            {
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                cubesList = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= 3)
                        {
                            cubesList.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-3）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                nicubes = cubesList.ToArray();
            }
            //
            int[][] map = new int[cubes.Length][];
            Console.WriteLine("请输入映射关系（用空格分隔）");
            int max = cubes.Length - 1;
            for (int i = 0; i < cubes.Length; i++)
            {
                Console.Write($"{i}：");
                line = (Console.ReadLine() ?? "").Trim();
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                List<int> mapitem = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= max)
                        {
                            mapitem.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-{max}）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                map[i] = mapitem.ToArray();
            }
            //
            Console.WriteLine();
            Console.WriteLine("当前可交互机关立方状态：");
            Console.WriteLine(Join(",", cubes));
            Console.WriteLine("当前不可交互机关立方状态：");
            Console.WriteLine(nicubes == null ? "" : Join(",", nicubes));
            Console.WriteLine("当前映射关系状态：");
            for (int i = 0; i < map.Length; i++)
            {
                Console.WriteLine($"{i}：{Join(",", map[i])}");
            }
            //
            bool bcontinue = true;
            while (bcontinue)
            {
                Console.WriteLine("请确认（按Enter键确认，按Esc键重新选择解谜类型）");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        bcontinue = false;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
            Console.WriteLine();
            //
            CubePuzzleRotate cp = new CubePuzzleRotate(cubes, map, nicubes);
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] steps = cp.Crack();
            stopwatch.Stop();
            if (steps == null)
            {
                Console.WriteLine("未在10步内找到解！");
            }
            else
            {
                Console.WriteLine("步骤：");
                Console.WriteLine(Join(",", steps));
            }
            Console.WriteLine($"用时 {1000 * stopwatch.Elapsed.Ticks / freq} 毫秒");
            Console.WriteLine();
        }
        static void Solve1()
        {
            Console.WriteLine("点亮型机关立方");
            //
            Console.WriteLine("请输入当前可交互机关立方亮起状态（用空格分隔）");
            Console.WriteLine("一个花瓣亮起：0");
            Console.WriteLine("两个花瓣亮起：1");
            Console.WriteLine("三个花瓣亮起：2");
            string line;
            string[] nums;
            List<int> cubesList;
            while (true)
            {
                Console.Write("：");
                line = (Console.ReadLine() ?? "").Trim();
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                cubesList = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= 2)
                        {
                            cubesList.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-2）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                if (cubesList.Count != 0)
                {
                    break;
                }
                Console.WriteLine("该项不能为空！");
            }
            int[] cubes = cubesList.ToArray();
            //
            Console.WriteLine("请输入当前不可交互机关立方亮起状态（用空格分隔，可留空）");
            Console.Write("：");
            line = (Console.ReadLine() ?? "").Trim();
            int[] nicubes = null;
            if (!string.IsNullOrEmpty(line))
            {
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                cubesList = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= 2)
                        {
                            cubesList.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-2）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                nicubes = cubesList.ToArray();
            }
            //
            int[][] map = new int[cubes.Length][];
            Console.WriteLine("请输入映射关系（用空格分隔）");
            int max = cubes.Length - 1;
            for (int i = 0; i < cubes.Length; i++)
            {
                Console.Write($"{i}：");
                line = (Console.ReadLine() ?? "").Trim();
                nums = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                List<int> mapitem = new List<int>();
                foreach (string num in nums)
                {
                    if (int.TryParse(num, out int value))
                    {
                        if (value >= 0 && value <= max)
                        {
                            mapitem.Add(value);
                        }
                        else
                        {
                            Console.WriteLine($"“{num}”不在范围内（0-{max}）！");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"“{num}”不是数字！");
                    }
                }
                map[i] = mapitem.ToArray();
            }
            //
            Console.WriteLine();
            Console.WriteLine("当前可交互机关立方状态：");
            Console.WriteLine(Join(",", cubes));
            Console.WriteLine("当前不可交互机关立方状态：");
            Console.WriteLine(nicubes == null ? "" : Join(",", nicubes));
            Console.WriteLine("当前映射关系状态：");
            for (int i = 0; i < map.Length; i++)
            {
                Console.WriteLine($"{i}：{Join(",", map[i])}");
            }
            //
            bool bcontinue = true;
            while (bcontinue)
            {
                Console.WriteLine("请确认（按Enter键确认，按Esc键重新选择解谜类型）");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        bcontinue = false;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
            Console.WriteLine();
            //
            CubePuzzleLit cp = new CubePuzzleLit(cubes, map, nicubes);
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] steps = cp.Crack();
            stopwatch.Stop();
            if (steps == null)
            {
                Console.WriteLine("未在10步内找到解！");
            }
            else
            {
                Console.WriteLine("步骤：");
                Console.WriteLine(Join(",", steps));
            }
            Console.WriteLine($"用时 {1000 * stopwatch.Elapsed.Ticks / freq} 毫秒");
            Console.WriteLine();
        }

        static void Solve2()
        {
            Console.WriteLine("海祇岛机关立方幻方");
            string line;
            string[] facinginput;
            List<int> cubesList;
            bool[] facingExists = new bool[10];
            int[,] cubes = new int[3, 3];
            string[,] strresult;
            Console.WriteLine("输入帮助：");
            Console.WriteLine("可旋转机关立方用“u”代替；");
            Console.WriteLine("朝北，不带顶，不可旋转机关立方用“n”代替；");
            Console.WriteLine("朝东，不带顶，不可旋转机关立方用“e”代替；");
            Console.WriteLine("朝南，不带顶，不可旋转机关立方用“s”代替；");
            Console.WriteLine("朝西，不带顶，不可旋转机关立方用“w”代替；");
            Console.WriteLine("朝北，带顶，不可旋转机关立方用“n+”代替；");
            Console.WriteLine("朝东，带顶，不可旋转机关立方用“e+”代替；");
            Console.WriteLine("朝南，带顶，不可旋转机关立方用“s+”代替；");
            Console.WriteLine("朝西，带顶，不可旋转机关立方用“w+”代替；");
            Console.WriteLine("空位用“x”代替；");
            //
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"请输入当前第{index[i]}行机关立方旋转状态（用空格分隔）");
                while (true)
                {
                    Console.Write("：");
                    line = (Console.ReadLine() ?? "").Trim().ToLower();
                    facinginput = line.Split(sp, StringSplitOptions.RemoveEmptyEntries);
                    cubesList = new List<int>(3);
                    bool[] tempFacingExists = new bool[10];
                    facingExists.CopyTo(tempFacingExists, 0);
                    foreach (string facing in facinginput)
                    {
                        int ifacing = MagicSquare.MagicSquareFacingConvert(facing);
                        switch (ifacing)
                        {
                            case 0:
                                cubesList.Add(ifacing);
                                break;
                            case -1:
                                Console.WriteLine($"{facing}不是有效的值！");
                                break;
                            default:
                                if (tempFacingExists[ifacing])
                                {
                                    Console.WriteLine($"{facing}已存在！");
                                    continue;
                                }
                                else
                                {
                                    tempFacingExists[ifacing] = true;
                                    goto case 0;
                                }
                        }
                    }
                    if (cubesList.Count == 3)
                    {
                        cubes[i, 0] = cubesList[0];
                        cubes[i, 1] = cubesList[1];
                        cubes[i, 2] = cubesList[2];
                        facingExists = tempFacingExists;
                        break;
                    }
                    Console.WriteLine("该项必须填入3个内容！");
                }
            }
            //
            Console.WriteLine();
            Console.WriteLine("当前机关立方状态：");
            strresult = MagicSquare.MagicSquareConvert(cubes, facingstr);
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(Join(",", strresult[i, 0], strresult[i, 1], strresult[i, 2]));
            }
            //
            bool bcontinue = true;
            while (bcontinue)
            {
                Console.WriteLine("请确认（按Enter键确认，按Esc键重新选择解谜类型）");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        bcontinue = false;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
            Console.WriteLine();
            //
            MagicSquare ms = new MagicSquare(cubes);
            Stopwatch stopwatch = Stopwatch.StartNew();
            int[,] result = ms.Crack();
            stopwatch.Stop();
            if (result == null)
            {
                Console.WriteLine("未找到解！");
            }
            else
            {
                strresult = MagicSquare.MagicSquareConvert(result, facingstr);
                Console.WriteLine("结果：");
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(Join(",", strresult[i, 0], strresult[i, 1], strresult[i, 2]));
                }
            }
            Console.WriteLine($"用时 {1000 * stopwatch.Elapsed.Ticks / freq} 毫秒");
            Console.WriteLine();
        }

        public static string Join<T>(string separator, IEnumerable<T> value)
        {
            if (value == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (T t in value)
            {
                sb.Append(t + separator);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        public static string Join<T>(string separator, params T[] value)
        {
            if (value == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (T t in value)
            {
                sb.Append(t + separator);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}
#endif