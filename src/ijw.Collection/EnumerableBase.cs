using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ijw.Contract;

namespace ijw.Collection {
    /// <summary>
    /// 可枚举对象基类, 不可以实例化该类。可以从此类继承, 从而方便地利用定长数组实现IEnumerable!<![CDATA[<T>]]>.
    /// 通过在内部使用数组T[], 提供了一个最小的IEnumerable!<![CDATA[<T>]]>实现.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EnumerableBase<T> : IEnumerable<T>, IEnumerable {
        /// <summary>
        /// 构造一个可枚举对象。
        /// </summary>
        public EnumerableBase() { }

        /// <summary>
        /// 构造一个可枚举对象，使用指定的数组初始化。
        /// </summary>
        /// <param name="data"></param>
        public EnumerableBase(T[] data) {
            data.ShouldBeNotNullArgument();
            this._data = data;
        }

        /// <summary>
        /// 构造一个可枚举对象，使用一个IEnumerable!<![CDATA[<T>]]>初始化。
        /// </summary>
        /// <param name="data"></param>
        public EnumerableBase(IEnumerable<T> data) : this(data?.ToArray()) {
        }

        /// <summary>
        /// 内部数组
        /// </summary>
        protected T[] _data;

        /// <summary>
        /// 获取一个迭代器(由内部数组实现)
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() {
            return _data.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this._data.GetEnumerator();
        }
    }
}