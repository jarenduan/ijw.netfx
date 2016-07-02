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
    public class Column<T> : RowColumnBase<T>{
       /// <summary>
        /// 无参数构造函数, 仅供Grid类初始化时内部使用
        /// </summary>
        internal Column() { }

        /// <summary>
        /// 构造函数. 使用指定的元素数组来初始化一个列对象.
        /// </summary>
        /// <param name="cells">一组元素</param>
        public Column(T[] cells)
            : base(cells) {
        }

        protected override int getDimensionInGrid(Grid<T> grid) {
            return grid.ColumnCount;
        }
        protected override T getCellValue(int index) {
            return this._grid._cells[index][_rowColumnIndex];
        }

        protected override void setCellValue(int index, T value) {
             this._grid._cells[index][_rowColumnIndex] = value;
        }
    }
}