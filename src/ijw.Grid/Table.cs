using System;
using System.Collections.Generic;
using System.Linq;
using ijw.Contract;

namespace ijw.Grid
{
    public class Table<TBodyCell,THeaderCell> : Grid<TBodyCell> {
        public Header<THeaderCell> ColumnHeader { get; }

        public Table(TBodyCell[,] data, THeaderCell[] columnHeader) : base(data) {
            columnHeader.ShouldBeNotNullArgument();
            columnHeader.Length.ShouldEquals(data.Length);

            this.ColumnHeader = new Header<THeaderCell>(columnHeader);
        }
    }
}
