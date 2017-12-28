using System.Collections.Generic;

namespace ijw.Collection {
    /// <summary>
    /// 带索引的列表。内部用定长数组实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Indexable<T> : EnumerableBase<T>, IIndexable<T>{
        /// <summary>
        /// 使用一个数组进行初始化
        /// </summary>
        /// <param name="data">进行初始化的一维数组</param>
        public Indexable(T[] data)
            : base(data) {
        }

        /// <summary>
        /// 使用一个集合进行初始化
        /// </summary>
        /// <param name="data">进行初始化的集合</param>
        public Indexable(IEnumerable<T> data)
            : base(data) {
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public T this[int index] {
            get { return this._data[index]; }
            set { this._data[index] = value; }
        }


        /// <summary>
        /// 集合元素总数
        /// </summary>
        public int Count {
            get { return this._data.Length; }
        }
    }
}