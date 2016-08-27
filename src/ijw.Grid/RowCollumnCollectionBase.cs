using System;
using ijw.Collection;
using ijw.Contract;

namespace ijw.Grid {
    /// <summary>
    /// 行列集合的基类, 提供共有的索引器/枚举器实现, 无法实例化.
    /// </summary>
    /// <typeparam name="TElement">元素类型</typeparam>
    /// <typeparam name="TRowOrColumn">行/列类型</typeparam>
    abstract public class RowCollumnCollectionBase<TElement, TRowOrColumn> : EnumerableBase<TRowOrColumn>
    where TRowOrColumn : RowColumnBase<TElement> {
        public TRowOrColumn this[int index] => this._data[index];

        internal RowCollumnCollectionBase(Grid<TElement> grid, int count, Func<Grid<TElement>, int, TRowOrColumn> creator) {
            grid.ShouldBeNotNullArgument();
            count.ShouldBeNotZero();
            creator.ShouldBeNotNullArgument();

            this._grid = grid;
            this._data = CollectionHelper.NewArrayWithValue(count, index => creator(this._grid, index));
        }

        protected Grid<TElement> _grid;
    }
}