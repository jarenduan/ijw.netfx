using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Grid {
    /// <summary>
    /// 表示表中的一行
    /// </summary>
    /// <typeparam name="T">行中每个单元格内容纳的元素类型</typeparam>
    public class Row<T>: RowColumnBase<T> {
        public override int Dimension => this._grid.ColumnCount;

        public override T this[int index] {
            get { 
                return this._grid._cells[this.Index][index];
            }
            set {
                this._grid._cells[this.Index][index] = value;
            }
        }

        /// <summary>
        /// 构造函数, 仅供Grid类初始化时内部使用
        /// </summary>
        internal Row(Grid<T> grid, int index) : base(grid, index) {
        }
    }
}