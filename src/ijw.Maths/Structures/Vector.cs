using System;
using System.Collections.Generic;
using System.Text;
using ijw.Collection;

namespace ijw.Maths.Structures {
    public class Vector<T> : IEnumerable<T> {
        public int Dimension { get; private set; }

        protected T[] Data { get; set; }

        public T this[int index] {
            get { return this.Data[index]; }
            set { this.Data[index] = value; }
        }

        public Vector(int dimension) {
            this.Dimension = dimension;
            this.Data = new T[this.Dimension];
        }

        public void Reset() {
            this.Data = new T[this.Dimension];
        }

        public IEnumerator<T> GetEnumerator() {
            return this.Data.GetEnumeratorGenerics();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.Data.GetEnumerator();
        }
    }
}
