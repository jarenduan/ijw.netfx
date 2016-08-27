using System.Collections.Generic;

namespace ijw.Collection {
    /// <summary>
    /// 可复写列表。内部用定长数组实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Indexable<T> : EnumerableBase<T>, IIndexable<T>{
        public Indexable(T[] data)
            : base(data) {
        }
        public Indexable(IEnumerable<T> data)
            : base(data) {
        }
        public T this[int index] {
            get { return this._data[index]; }
            set { this._data[index] = value; }
        }

        public int Count {
            get { return this._data.Length; }
        }
    }
}