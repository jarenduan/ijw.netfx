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
        internal RowCollection(Grid<T> grid): base(grid, grid.RowCount, (g, i) => {
            return new Row<T>(g, i);
        }) { }
    }
}