using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Collection;

namespace ijw.Grid {
    /// <summary>
    /// 行列集合的基类, 提供共有的索引器/枚举器实现, 无法实例化.
    /// </summary>
    /// <typeparam name="TElement">元素类型</typeparam>
    /// <typeparam name="TRowOrColumn">行/列类型</typeparam>
    abstract public class RowCollumnCollectionBase<TElement, TRowOrColumn> : EnumerableBase<TRowOrColumn>
    where TRowOrColumn : RowColumnBase<TElement>{
        protected Grid<TElement> _grid;
        internal RowCollumnCollectionBase(Grid<TElement> grid, int count) {
            if (grid == null) {
                throw new ArgumentNullException("grid");
            }
            if (count == 0) {
                throw new ArgumentException();
            }
            this._grid = grid;
            this._data = new TRowOrColumn[count];
            for (int i = 0; i < count; i++) {
                TRowOrColumn rowOrColl = generateEmptyRowOrColumn();
                rowOrColl._grid = grid;
                rowOrColl._rowColumnIndex = i;
                this._data[i] = rowOrColl;
            }
        }

        abstract protected TRowOrColumn generateEmptyRowOrColumn();

        public TRowOrColumn this[int index] {
            get {
                return this._data[index];
            }
            set {
                TRowOrColumn temp = this._data[index];
                this._data[index] = value;
                if (!this._data[index].BindToGrid(this._grid, index)) {
                    this._data[index] = temp;
                    this._data[index].BindToGrid(this._grid, index);
                    throw new CannotBindGridException<TRowOrColumn, TElement>(value, this._grid);
                }
            }
        }
    }
}