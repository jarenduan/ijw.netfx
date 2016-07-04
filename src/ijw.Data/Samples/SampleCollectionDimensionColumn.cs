using System.Collections;
using System.Collections.Generic;
using ijw.Contract;
using ijw.Collection;

namespace ijw.Data.Samples {
    public class SampleCollectionDimensionColumn : IIndexable<double> {
        internal SampleCollectionDimensionColumn(double[][] data, int columnIndex) {
            columnIndex.ShouldLessThan(data[0].Length);
            this._data = data;
            this.ColumnIndex = columnIndex;
        }

        /// <summary>
        /// 内部引用的所有数据，不要进行任何更改。直接引用样本集合的数据数组。
        /// </summary>
        private double[][] _data;

        /// <summary>
        /// 维度列的索引号，代表在样本中是第几列
        /// </summary>
        public int ColumnIndex { get; protected set; }

        /// <summary>
        /// 列总数
        /// </summary>
        public int Count => _data.Length;

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <returns></returns>
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
