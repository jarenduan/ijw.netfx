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
        /// <summary>
        /// 无参数构造函数, 仅供Grid类初始化时内部使用
        /// </summary>
        internal Row() {
        }

        /// <summary>
        /// 构造函数. 使用指定的元素数组来初始化一个行对象.
        /// </summary>
        /// <param name="cells">一组元素</param>
        public Row(T[] cells) : base(cells) {
        }
        
        protected override int getDimensionInGrid(Grid<T> grid) {
            return grid.ColumnCount;
        }
        protected override void setCellValue(int index, T value) {
            this._grid._cells[this._rowColumnIndex][index] = value;
        }
        protected override T getCellValue(int index) {
            return this._grid._cells[_rowColumnIndex][index];
        }
    }
}