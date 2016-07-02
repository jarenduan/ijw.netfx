using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Grid {
    /// <summary>
    /// 提供表的行/列对象的公共数据和行为, 此类不能实例化.
    /// 为行/列对象在是否绑定表的情况下提供了两类行为: 1)当未绑定时, 所有操作将针对内部存储. 
    /// 2)一旦绑定到某个表, 行/列对象更像是表中对应行/列的视图, 所有操作将直接针对表中单元格. 其内部存储将被清空, 也不再使用.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RowColumnBase<T> : IEnumerable<T> {
        /// <summary>
        /// 仅供内部类初始化时调用
        /// </summary>
        internal RowColumnBase() { 
        }
        
        /// <summary>
        /// 使用一组元素进行初始化
        /// </summary>
        /// <param name="cells"></param>
        public RowColumnBase(T[] cells) {
            this._data = cells;
        }

        /// <summary>
        /// 行/列的序号
        /// </summary>
        internal int _rowColumnIndex;

        /// <summary>
        /// 属于那个表
        /// </summary>
        internal Grid<T> _grid;

        /// <summary>
        /// 内部的数据存储
        /// </summary>
        protected T[] _data;

        /// <summary>
        /// 维度. 对如果没有绑定到表的行/列对象, 返回内部数组的长度.
        /// 对于已绑定到表的行/列对象, 返回表中的列数/行数.
        /// </summary>
        public int Dimension {
            get {
                if (this._grid != null) {
                    return getDimensionInGrid(this._grid);
                }
                else {
                    return this._data.Length;
                }
            }
        }

        /// <summary>
        /// 索引器. 对如果没有绑定到表的行/列对象, 返回(或设置)指定索引处的内部单元格的值.
        /// 对于已绑定到表的行/列对象, 返回(或设置)所绑定表中的对应行/列中指定索引处的单元格的值.
        /// </summary>
        /// <param name="index">指定位置</param>
        /// <returns> 对如果没有绑定到表的行/列对象, 返回指定索引处的内部单元格的值.
        /// 对于已绑定到表的行/列对象, 返回表中的对应行/列中指定索引处的单元格的值.</returns>
        public T this[int index] {
            get {
                if (this._grid != null) {
                    return getCellValue(index);
                }
                else {
                    return this._data[index];
                }
            }
            set {
                if (this._grid != null) {
                    setCellValue(index, value);
                }
                else {
                    this._data[index] = value;
                }
            }
        }

        /// <summary>
        /// 获取指定索引处对应的Grid中的单元格的值. 必须实现.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        abstract protected T getCellValue(int index);

        /// <summary>
        /// 设置指定索引处对应的Grid中的单元格的值. 必须实现.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        abstract protected void setCellValue(int index, T value);

        /// <summary>
        /// 将当前行/列绑定到指定表的指定序号上, 将会覆盖表中相应位置的已有数据
        /// </summary>
        /// <param name="grid">欲绑定的表</param>
        /// <param name="index">序号</param>
        internal bool BindToGrid(Grid<T> grid, int index) {
            if (grid == this._grid) return false;
            if (grid == null || this._data == null || this._data.Length == 0) return false;
            if (!checkDimension(grid)) return false;

            this._grid = grid;
            this._rowColumnIndex = index;
            for (int i = 0; i < getDimensionInGrid(this._grid); i++) {
                // 把内部的元素复制到绑定的表中, 覆盖对应值.
                setCellValue(i, this._data[i]);
            }
            this._data = null;
            return true;
        }

        /// <summary>
        /// 检查当前维度是否与指定grid的对应维度符合
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected bool checkDimension(Grid<T> grid) {
            return this._data.Length == getDimensionInGrid(grid);
        }

        /// <summary>
        /// 获取指定表中的目标维度. 行对象的目标为度是列数, 列对象的目标维度是行数.
        /// </summary>
        /// <returns></returns>
        abstract protected int getDimensionInGrid(Grid<T> grid);

        /// <summary>
        /// 枚举行/列对象在所绑定表中的对应元素
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<T> enumerateInGrid() {
            for (int i = 0; i < this.getDimensionInGrid(this._grid); i++) {
                yield return getCellValue(i);
            }
        }

        #region Enumerable implementation
        public IEnumerator<T> GetEnumerator() {
            if (this._grid != null) {
                return this.enumerateInGrid().GetEnumerator();
            }
            else {
                return this._data.AsEnumerable().GetEnumerator();
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            if (this._grid != null) {
                return this.enumerateInGrid().GetEnumerator();
            }
            else {
                return this._data.GetEnumerator();
            }
        } 
        #endregion
      }
}
