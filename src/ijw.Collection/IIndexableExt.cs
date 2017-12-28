using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection {
    /// <summary>
    /// 
    /// </summary>
    public static class IIndexableExt {
        /// <summary>
        /// 获得一个枚举器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexable"></param>
        /// <returns></returns>
        public static IEnumerator<T> GetEnumeratorForIIndexable<T>(this IIndexable<T> indexable) {
            return new IIndexableEnumerator<T>(indexable);
        }

        /// <summary>
        /// 可索引集合的枚举器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class IIndexableEnumerator<T> : IEnumerator, IEnumerator<T> {
            private int _curr;
            private IIndexable<T> indexable;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="indexable"></param>
            public IIndexableEnumerator(IIndexable<T> indexable) {
                this.indexable = indexable;
            }

            /// <summary>
            /// 当前元素
            /// </summary>
            public T Current {
                get {
                    return this.indexable[_curr];
                }
            }

            object IEnumerator.Current {
                get {
                    return this.Current;
                }
            }

            /// <summary>
            /// 清理资源
            /// </summary>
            public void Dispose() {

            }

            /// <summary>
            /// 迭代
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() {
                this._curr++;
                return (this._curr < this.indexable.Count);
            }

            /// <summary>
            /// 复位
            /// </summary>
            public void Reset() {
                _curr = -1;
            }
        }
    }
}
