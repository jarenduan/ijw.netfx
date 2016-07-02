using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection {
    public static class IIndexableExt {
        public static IEnumerator<T> GetEnumeratorForIIndexable<T>(this IIndexable<T> indexable) {
            return new IIndexableEnumerator<T>(indexable);
        }

        public class IIndexableEnumerator<T> : IEnumerator, IEnumerator<T> {
            private int _curr;
            private IIndexable<T> indexable;

            public IIndexableEnumerator(IIndexable<T> indexable) {
                this.indexable = indexable;
            }

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

            public void Dispose() {

            }

            public bool MoveNext() {
                this._curr++;
                return (this._curr < this.indexable.Count);
            }

            public void Reset() {
                _curr = -1;
            }
        }
    }
}
