using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection {
    /// <summary>
    /// 基于一维数组的数组迭代器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayEnumerator<T> : IEnumerator<T> {
        /// <summary>
        /// 内部使用的一维数组
        /// </summary>
        protected T[] _data;
        private int _curr = -1;

        /// <summary>
        /// 构造函数，使用一个一维数组进行初始化
        /// </summary>
        /// <param name="Data">一维数组</param>
        public ArrayEnumerator(T[] Data) {
            this._data = Data;
        }

        /// <summary>
        /// 当前元素
        /// </summary>
        public T Current {
            get { return this._data[_curr]; }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose() {
        }

        object IEnumerator.Current {
            get { return this.Current; }
        }

        /// <summary>
        /// 向后迭代
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() {
            this._curr++;
            return (this._curr < this._data.Length);
        }

        /// <summary>
        /// 复位
        /// </summary>
        public void Reset() {
            this._curr = -1;
        }
    }
}