using System;

namespace ijw.Grid {
	/// <summary>
	/// 具有固定维度的二维表. 可以使用Rows和Columns属性访问其中的行和列.
	/// </summary>
	/// <typeparam name="T">单元格容纳元素的类型</typeparam>
	public class Grid<T> {
		/// <summary>
		/// 行数
		/// </summary>
		public int RowCount {
			get {
				return this._cells.Length;
			}
		}

		/// <summary>
		/// 列数
		/// </summary>
		public int ColumnCount {
			get { return this._cells[0].Length; }
		}

		/// <summary>
		/// 返回指定序号的行
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Row<T> this[int index] { get { return this.Rows[index]; } }

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
			if (rowCount <= 0 || columnCount <= 0) {
				throw new ArgumentException("rowCount and columnCount should be both larger than zero.");
			}
			this._cells  = new T[rowCount][];
			for (int i = 0; i < rowCount; i++)
			{
				this._cells[i] = new T[columnCount];
			}
			this.Rows = new RowCollection<T>(this);
			this.Columns = new ColumnCollection<T>(this);
		}

		/// <summary>
		/// 构造函数. 用指定的二维数组初始化Grid.
		/// </summary>
		/// <param name="array"></param>
		public Grid(T[,] array) {
			if (array == null) {
				throw new ArgumentNullException();
			}
			if (array.GetLength(0) == 0 || array.GetLength(1) == 0) {
				throw new ArgumentException("rowCount and columnCount should be both larger than zero.");
			}
			int rowCount = array.GetLength(0);
			int columnCount = array.GetLength(1);
			this._cells = new T[rowCount][];
			for (int i = 0; i < rowCount; i++) {
				this._cells[i] = new T[columnCount];
				for (int j = 0; j < columnCount; j++) {
					this._cells[i][j] = array[i, j];
				}
			}
			this.Rows = new RowCollection<T>(this);
			this.Columns = new ColumnCollection<T>(this);
		}
		
		/// <summary>
		/// 内部使用了二维数组进行存储
		/// </summary>
		internal T[][] _cells;
	}
}
