using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Contract;

namespace ijw.Grid {
    /// <summary>
    /// 提供表的行/列对象的公共数据和行为, 此类不能实例化.
    /// 行/列对象更像是表中对应行/列的视图, 所有操作将直接针对表中单元格. 其内部存储将被清空, 也不再使用.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IndexedViewBase<T> : IEnumerable<T> {
        /// <summary>
        /// 行/列的序号, 从0开始 
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// 维度. 返回表中的列数/行数.
        /// </summary>
        public abstract int Dimension { get; }

        /// <summary>
        /// 索引器. 返回(或设置)所绑定表中的对应行/列中指定索引处的单元格的值.
        /// </summary>
        /// <param name="index">指定位置</param>
        /// <returns>返回表中的对应行/列中指定索引处的单元格的值.</returns>
        public abstract T this[int index] { get; set; }

        /// <summary>
        /// 仅供内部类初始化时调用
        /// </summary>
        internal IndexedViewBase(Grid<T> grid, int index) {
            this._grid = grid;
            this.Index = index;
        }

        /// <summary>
        /// 属于那个表
        /// </summary>
        protected Grid<T> _grid;

        #region Enumerable implementation
        public IEnumerator<T> GetEnumerator() {
            return this.enumerateInGrid().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.enumerateInGrid().GetEnumerator();
        }

        /// <summary>
        /// 枚举行/列对象在所绑定表中的对应元素
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<T> enumerateInGrid() {
            for (int i = 0; i < this.Dimension; i++) {
                yield return this[i];
            }
        }
        #endregion
    }
}
