using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Grid {
    /// <summary>
    /// 行集合. 无法继承.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RowCollection<T> : RowCollumnCollectionBase<T, Row<T>> {
        internal RowCollection(Grid<T> grid): base(grid, grid.RowCount) {
        }

        protected override Row<T> generateEmptyRowOrColumn() {
            return new Row<T>();
        }

        //public void SetRow(Row<T> row, int rowIndex) {
        //    this._data[rowIndex] = row;
        //    this._data[rowIndex].BindToGrid(this._grid, rowIndex);
        //}
    }
}