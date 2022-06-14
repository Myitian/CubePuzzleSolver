using System;
using System.Collections;
using System.Collections.Generic;

namespace Myitian.CubePuzzleSolver
{
    /// <summary>
    /// 排列组合枚举
    /// </summary>
    /// <typeparam name="T">要排列组合的数据类型</typeparam>
    public class Arrangement<T> : IEnumerable<T[]>
    {
        private T[] BaseArray;
        private int V_M;
        /// <summary>要排列组合的数组</summary>
        public T[] Array
        {
            get => BaseArray;
            set => BaseArray = value ?? throw new ArgumentNullException();
        }
        /// <summary>要排列组合的数量</summary>
        public int M
        {
            get => V_M;
            set => V_M = value >= 0 ? value > BaseArray.Length ? BaseArray.Length : value : 0;
        }
        /// <summary>排列组合的可能性数量</summary>
        public int Count => MathExt.ArrangementNumber(BaseArray.Length, V_M);

        /// <summary>
        /// 排列组合枚举
        /// </summary>
        /// <param name="array">要排列组合的数组</param>
        public Arrangement(T[] array)
        {
            Array = array;
            M = array.Length;
        }
        /// <summary>
        /// 排列组合枚举
        /// </summary>
        /// <param name="array">要排列组合的数组</param>
        /// <param name="m">要排列组合的数量</param>
        public Arrangement(T[] array, int m)
        {
            Array = array;
            M = m;
        }
        /// <summary>
        /// 获取指定的排列组合
        /// </summary>
        /// <param name="index">排列组合的序号</param>
        /// <returns>第<c>index</c>个排列组合</returns>
        public T[] this[int index]
        {
            get
            {
                List<int> indices = new List<int>(MathExt.Range(0, BaseArray.Length));
                T[] result = new T[V_M];
                int pos = 0;
                for (int i = 1; i <= V_M; i++)
                {
                    int a = MathExt.ArrangementNumber(BaseArray.Length - i, V_M - i);
                    int r = index / a;
                    result[pos++] = BaseArray[indices[r]];
                    indices.RemoveAt(r);
                    index %= a;
                }
                return result;
            }
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public IEnumerator<T[]> GetEnumerator() => new ArrangementEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new ArrangementEnumerator(this);

        /// <summary>排列组合枚举器</summary>
        public class ArrangementEnumerator : IEnumerator<T[]>
        {
            private Arrangement<T> _collection;
            private int currentIndex;
            private T[] currentItem;
            /// <summary>当前项</summary>
            public T[] Current => currentItem;
            /// <summary>排列组合枚举器</summary>
            public ArrangementEnumerator(Arrangement<T> collection)
            {
                _collection = collection;
                currentIndex = -1;
                currentItem = default;
            }

            object IEnumerator.Current => throw new NotImplementedException();

            void IDisposable.Dispose() { }
            /// <summary>
            /// 下一个
            /// </summary>
            /// <returns>若达到末尾，则为<c>false</c></returns>
            public bool MoveNext()
            {
                if (++currentIndex >= _collection.Count)
                {
                    return false;
                }
                else
                {
                    currentItem = _collection[currentIndex];
                }
                return true;
            }
            /// <summary>
            /// 重置
            /// </summary>
            public void Reset() => currentIndex = -1;
        }
    }
}
