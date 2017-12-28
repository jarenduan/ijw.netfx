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
        /// ���һ��ö����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexable"></param>
        /// <returns></returns>
        public static IEnumerator<T> GetEnumeratorForIIndexable<T>(this IIndexable<T> indexable) {
            return new IIndexableEnumerator<T>(indexable);
        }

        /// <summary>
        /// ���������ϵ�ö����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class IIndexableEnumerator<T> : IEnumerator, IEnumerator<T> {
            private int _curr;
            private IIndexable<T> indexable;

            /// <summary>
            /// ���캯��
            /// </summary>
            /// <param name="indexable"></param>
            public IIndexableEnumerator(IIndexable<T> indexable) {
                this.indexable = indexable;
            }

            /// <summary>
            /// ��ǰԪ��
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
            /// ������Դ
            /// </summary>
            public void Dispose() {

            }

            /// <summary>
            /// ����
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() {
                this._curr++;
                return (this._curr < this.indexable.Count);
            }

            /// <summary>
            /// ��λ
            /// </summary>
            public void Reset() {
                _curr = -1;
            }
        }
    }
}
