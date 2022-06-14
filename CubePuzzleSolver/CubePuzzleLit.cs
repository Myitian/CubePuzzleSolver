using System;

namespace Myitian.CubePuzzleSolver
{
    /// <summary>
    /// 点亮型机关立方解谜
    /// </summary>
    public class CubePuzzleLit
    {
        /// <summary>可交互机关立方（原始）</summary>
        public int[] InteractiveCube_Original;
        /// <summary>可交互机关立方（当前）</summary>
        public int[] InteractiveCube_Current;
        /// <summary>不可交互机关立方（原始）</summary>
        public int[] NoninteractiveCube_Original;
        /// <summary>不可交互机关立方（当前）</summary>
        public int[] NoninteractiveCube_Current;
        /// <summary>操作映射表</summary>
        public int[][] Map;

        /// <summary>
        /// 点亮型机关立方解谜
        /// </summary>
        /// <param name="interactiveCube">可交互机关立方</param>
        /// <param name="map">操作映射表</param>
        /// <param name="noninteractiveCube">不可交互机关立方</param>
        public CubePuzzleLit(int[] interactiveCube, int[][] map, int[] noninteractiveCube = null)
        {
            InteractiveCube_Original = interactiveCube;
            InteractiveCube_Current = new int[interactiveCube.Length];
            InteractiveCube_Original.CopyTo(InteractiveCube_Current, 0);
            if (noninteractiveCube != null && noninteractiveCube.Length > 0)
            {
                NoninteractiveCube_Original = noninteractiveCube;
                NoninteractiveCube_Current = new int[noninteractiveCube.Length];
                NoninteractiveCube_Original.CopyTo(NoninteractiveCube_Current, 0);
            }
            Map = map;
        }

        /// <summary>
        /// 一次交互
        /// </summary>
        /// <param name="n">要交互的机关立方</param>
        public void Interact(int n)
        {
            foreach (int i in Map[n])
            {
                PrivateInteract(i);
            }
        }

        /// <summary>
        /// 检查机关立方
        /// </summary>
        /// <returns>所有机关立方是否符合要求</returns>
        public bool Check()
        {
            int i = 0, j = 1;
            // 检查所有可交互机关立方是否相等
            while (j < InteractiveCube_Current.Length)
            {
                if (InteractiveCube_Current[i++] != InteractiveCube_Current[j++])
                {
                    return false;
                }
            }
            // 检查可交互机关立方是否为2片或3片花瓣
            if (InteractiveCube_Current[0] == 0)
            {
                return false;
            }
            if (NoninteractiveCube_Current != null && NoninteractiveCube_Current.Length > 0)
            {
                // 检查可交互机关立方是否与不可交互机关立方相等
                if (InteractiveCube_Current[0] != NoninteractiveCube_Current[0])
                {
                    return false;
                }
                // 检查所有不可交互机关立方是否相等
                i = 0;
                j = 1;
                while (j < NoninteractiveCube_Current.Length)
                {
                    if (NoninteractiveCube_Current[i++] != NoninteractiveCube_Current[j++])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 一次无影响交互
        /// </summary>
        /// <param name="n">要交互的机关立方</param>
        private void PrivateInteract(int n)
        {
            if (n >= 0) //正数和零操作可交互机关立方
            {
                InteractiveCube_Current[n]++;
                InteractiveCube_Current[n] %= 3;
            }
            else //负数操作不可交互机关立方（我也不知道有没有这种机关立方，总之先加着）
            {
                NoninteractiveCube_Current[-1 - n]++;
                NoninteractiveCube_Current[-1 - n] %= 3;
            }
        }

        /// <summary>
        /// 穷举解机关立方
        /// </summary>
        /// <returns>旋转步骤</returns>
        public int[] Crack()
        {
            //交互顺序
            int[] interactions = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            int pos;
            do
            {
                pos = 0;
                //按交互顺序进行交互
                while (pos < interactions.Length && interactions[pos] != -1)
                {
                    Interact(interactions[pos]);
                    pos++;
                }
                //检查机关立方
                if (Check())
                {
                    int i;
                    for (i = 0; i < interactions.Length; i++)
                    {
                        if (interactions[i] == -1)
                        {
                            break;
                        }
                    }
                    int[] result = new int[i];
                    Array.Copy(interactions, result, i);
                    return result;
                }
                //重置机关立方状态
                InteractiveCube_Original.CopyTo(InteractiveCube_Current, 0);
                if (NoninteractiveCube_Original != null && NoninteractiveCube_Original.Length > 0)
                {
                    NoninteractiveCube_Original.CopyTo(NoninteractiveCube_Current, 0);
                }
            }
            //修改交互顺序，并在达到交互顺序数量上限时退出循环
            while (PrivateCrackInteract(ref interactions, 0));
            return null;
        }

        /// <summary>
        /// 修改机关立方交互顺序
        /// </summary>
        /// <param name="interactions">交互顺序</param>
        /// <param name="pos">当前位置</param>
        /// <returns>是否已达到交互顺序数量上限</returns>
        private bool PrivateCrackInteract(ref int[] interactions, int pos)
        {
            if (pos >= interactions.Length)
            {
                return false;
            }
            interactions[pos]++; //当前位置交互顺序+1
            if (interactions[pos] >= InteractiveCube_Current.Length) //若超过可交互机关立方数量，则归零并操作下一位
            {
                interactions[pos] = 0;
                return PrivateCrackInteract(ref interactions, pos + 1);
            }
            return true;
        }
    }
}