using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Grid {
    /// <summary>
    /// 无法绑定Grid的异常
    /// </summary>
    /// <typeparam name="TRowOrColumn"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    public class CannotBindGridException<TRowOrColumn,TElement> : Exception {
        private object value;
        private Grid<TElement> grid;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">给定值</param>
        /// <param name="grid">指定的Grid</param>
        public CannotBindGridException(TRowOrColumn value, Grid<TElement> grid) {
            this.value = value;
            this.grid = grid;
        }
    }
}
