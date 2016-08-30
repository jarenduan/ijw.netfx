using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Grid {
    /// <summary>
    /// 表中的一列
    /// </summary>
    /// <typeparam name="T">列中每个单元格容纳的元素的类型</typeparam>
    public class Column<T> : IndexedViewBase<T>{
        public override int Dimension => this._grid.ColumnCount;

        public override T this[int index] {
            get {
                return this._grid._cells[index, this.Index];
            }
            set {
                this._grid._cells[index, this.Index] = value;
            }
        }

        /// <summary>
        /// 无参数构造函数, 仅供Grid类初始化时内部使用
        /// </summary>
        internal Column(Grid<T> grid, int index) : base(grid, index) {
        }
    }
}