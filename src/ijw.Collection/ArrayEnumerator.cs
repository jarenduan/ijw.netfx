using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection {
   

    public class ArrayEnumerator<T> : IEnumerator<T> {
        protected T[] _data;

        public ArrayEnumerator(T[] Data) {
            this._data = Data;
        }

        private int _curr = -1;
        public T Current {
            get { return this._data[_curr]; }
        }
        public void Dispose() {
        }

        object IEnumerator.Current {
            get { return this.Current; }
        }
        public bool MoveNext() {
            this._curr++;
            return (this._curr < this._data.Length);
        }
        public void Reset() {
            this._curr = -1;
        }
    }
}
