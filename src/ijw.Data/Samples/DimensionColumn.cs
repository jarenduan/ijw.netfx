using System.Collections;
using System.Collections.Generic;
using ijw.Contract;
using ijw.Collection;

namespace ijw.Data.Samples {
    public class DimensionColumn : IIndexable<double> {
        internal DimensionColumn(double[][] data, int columnIndex) {
            columnIndex.ShouldLessThan(data[0].Length);
            this._data = data;
            this.ColumnIndex = columnIndex;
        }

        private double[][] _data;

        public int ColumnIndex { get; protected set; }

        public int Count => _data.Length;

        public double this[int rowIndex]
        {
            get { return this._data[rowIndex][ColumnIndex]; }
            set { this._data[rowIndex][ColumnIndex] = value; }
        }

        public IEnumerator<double> GetEnumerator() {
            return this.GetEnumeratorForIIndexable();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumeratorForIIndexable();
        }
    }

   
}
