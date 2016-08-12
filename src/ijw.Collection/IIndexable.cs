using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Collection {
    /// <summary>
    /// 带有索引器的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIndexable<T>: IEnumerable<T>, IEnumerable{
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] {
            get;
            set;
        }

        int Count { get; }
    }
}
