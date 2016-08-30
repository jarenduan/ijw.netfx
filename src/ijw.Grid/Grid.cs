using System;
using ijw.Contract;

namespace ijw.Grid {
	/// <summary>
	/// 具有固定维度的二维表. 可以使用Rows和Columns属性访问其中的行和列.
	/// </summary>
	/// <typeparam name="T">单元格容纳元素的类型</typeparam>
	public class Grid<T> {
		/// <summary>
		/// 行数
		/// </summary>
		public int RowCount => this._cells.GetLength(0);

		/// <summary>
		/// 列数
		/// </summary>
		public int ColumnCount => this._cells.GetLength(1);

		/// <summary>
		/// 返回指定序号的行
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Row<T> this[int index] => this.Rows[index];

		/// <summary>
		/// 行集合
		/// </summary>
		public RowCollection<T> Rows { get; private set; }

		/// <summary>
		/// 列集合
		/// </summary>
		public ColumnCollection<T> Columns { get; private set; }

		/// <summary>
		/// 构造函数, 必须提供行数和列数, 以确定表的维度.
		/// </summary>
		/// <param name="rowCount">行数, 需为正整数</param>
		/// <param name="columnCount">列数, 需为正整数</param>
		public Grid(int rowCount, int columnCount) {
			rowCount.ShouldLargerThan(0);
			columnCount.ShouldLargerThan(0);

			this._cells = new T[rowCount, columnCount];
			this.Rows = new RowCollection<T>(this);
			this.Columns = new ColumnCollection<T>(this);
		}

		/// <summary>
		/// 构造函数. 用指定的二维数组初始化Grid.
		/// </summary>
		/// <param name="array">二维数组，grid将直接引用它作为内部存储</param>
		public Grid(T[,] array) {
			array.ShouldBeNotNullArgument();
			array.GetLength(0).ShouldLargerThan(0);
			array.GetLength(1).ShouldLargerThan(0);

			this._cells = array;
			this.Rows = new RowCollection<T>(this);
			this.Columns = new ColumnCollection<T>(this);
		}

		/// <summary>
		/// 内部使用了二维数组进行存储
		/// </summary>
		internal T[,] _cells;
	}
}
