using System;
using System.Collections.Generic;
using System.Linq;
using ijw.Contract;

namespace ijw.Collection {
	/// <summary>
	/// 集合操作的帮助类
	/// </summary>
	public static class CollectionHelper {
		#region NewArray with values
		/// <summary>
		/// 创建数组，并使用指定的值填充数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dimension">数组维度</param>
		/// <param name="value">指定的值</param>
		/// <returns></returns>
		public static T[] NewArrayWithValue<T>(int dimension, T value) {
			T[] result = new T[dimension];
			for (int i = 0; i < result.Length; i++) {
				result[i] = value;
			}
			return result;
		}

		/// <summary>
		/// 创建数组，使用指定的函数填充数组。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dimension">数组维度</param>
		/// <param name="computer">值计算函数，数组索引作为传入参数</param>
		/// <returns></returns>
		public static T[] NewArrayWithValue<T>(int dimension, Func<int, T> computer) {
			T[] result = new T[dimension];
			for (int i = 0; i < result.Length; i++) {
				result[i] = computer(i);
			}
			return result;
		}
		#endregion

		#region For Each Two
		/// <summary>
		/// 对两个集合进行同步迭代，对每一对元素进行操作。
		/// </summary>
		/// <typeparam name="T1">第一个集合里面元素的类型</typeparam>
		/// <typeparam name="T2">另一个集合里面元素的类型</typeparam>
		/// <param name="collection1">第一个集合</param>
		/// <param name="collection2">另一个集合</param>
		/// <param name="doWork">需要对每对元素执行的操作</param>
		/// <exception cref="CountNotMatchException">
		/// 集合1元素数需要小于集合2元素数, 否则会抛出 CountNotMatchException 异常。
		/// </exception>
		public static void ForEachPair<T1, T2>(IEnumerable<T1> collection1, IEnumerable<T2> collection2, Action<T1, T2> doWork) {
			IEnumerator<T2> iter = collection2.GetEnumerator();
			foreach (var e1 in collection1) {
				if (!iter.MoveNext()) throw new CountNotMatchException();
				doWork(e1, iter.Current);
			}
		}

		/// <summary>
		/// 对两个集合进行同步迭代，对每一对元素进行指定函数计算（延迟），返回迭代器。
		/// </summary>
		/// <typeparam name="T1">第一个集合里面元素的类型</typeparam>
		/// <typeparam name="T2">另一个集合里面元素的类型</typeparam>
		/// <typeparam name="TResult">函数计算返回值得类型</typeparam>
		/// <param name="collection1">第一个集合</param>
		/// <param name="collection2">另一个集合</param>
		/// <param name="theFunction">执行计算的函数</param>
		/// <returns>返回的结果迭代器</returns>
		/// <exception cref="CountNotMatchException">
		/// 集合1元素数需要小于集合2元素数, 否则会抛出 CountNotMatchException 异常。
		/// </exception>
		/// <remarks>
		/// 本函数返回时指定函数计算并没有进行，本函数只返回一个迭代器。计算将延迟在对结果的迭代访问时进行。
		/// </remarks>
		public static IEnumerable<TResult> ForEachPair<T1, T2, TResult>(IEnumerable<T1> collection1, IEnumerable<T2> collection2, Func<T1, T2, TResult> theFunction) {
			//don't try to test the count(), cos' it might cause iterations.
			//if (collection1.Count() != collection2.Count()) {
			//	throw new CountNotMatchException();
			//}

			IEnumerator<T2> iter = collection2.GetEnumerator();
			foreach (var e1 in collection1) {
				if (!iter.MoveNext()) throw new CountNotMatchException(); //test during iterations.
				yield return theFunction(e1, iter.Current);
			}
		}

		#endregion

		#region For Each Three

		/// <summary>
		/// 对三个集合进行同步迭代，对每一组（三个）元素进行指定操作。
		/// </summary>
		/// <typeparam name="T1">第一个集合里面元素的类型</typeparam>
		/// <typeparam name="T2">第二个个集合里面元素的类型</typeparam>
		/// <typeparam name="T3">第三个个集合里面元素的类型</typeparam>
		/// <param name="collection1">第一个集合</param>
		/// <param name="collection2">另二个集合</param>
		/// <param name="collection3">第三个集合</param>
		/// <param name="doWork">执行的操作</param>
		/// <exception cref="CountNotMatchException">
		/// 集合1元素数需要小于集合2元素数, 否则会抛出 CountNotMatchException 异常。
		/// </exception>
		public static void ForEachThree<T1, T2, T3>(IEnumerable<T1> collection1, IEnumerable<T2> collection2, IEnumerable<T3> collection3, Action<T1, T2, T3> doWork) {
			IEnumerator<T2> iter2 = collection2.GetEnumerator();
			IEnumerator<T3> iter3 = collection3.GetEnumerator();

			foreach (var e1 in collection1) {
				if (!iter2.MoveNext()) throw new CountNotMatchException();
				if (!iter3.MoveNext()) throw new CountNotMatchException();
				doWork(e1, iter2.Current, iter3.Current);
			}
		}

		/// <summary>
		/// 对三个集合进行同步迭代，对每一组（三个）元素进行指定函数计算
		/// </summary>
		/// <typeparam name="T1">第一个集合里面元素的类型</typeparam>
		/// <typeparam name="T2">第二个个集合里面元素的类型</typeparam>
		/// <typeparam name="T3">第三个个集合里面元素的类型</typeparam>
		/// <typeparam name="TResult">函数计算返回值得类型</typeparam>
		/// <param name="collection1">第一个集合</param>
		/// <param name="collection2">另二个集合</param>
		/// <param name="collection3">第三个集合</param>
		/// <param name="theFunction">需要计算的函数</param>
		/// <returns>返回的结果迭代器</returns>
		/// <exception cref="CountNotMatchException">
		/// 集合1元素数同时需要小于集合2、3的元素数, 否则会抛出 CountNotMatchException 异常。
		/// </exception>
		/// <remarks>
		/// 本函数只返回一个迭代器。计算将延迟在对结果的迭代访问时进行。
		/// </remarks>
		public static IEnumerable<TResult> ForEachThree<T1, T2, T3, TResult>(IEnumerable<T1> collection1, IEnumerable<T2> collection2, IEnumerable<T3> collection3, Func<T1, T2, T3, TResult> theFunction) {
			IEnumerator<T2> iter2 = collection2.GetEnumerator();
			IEnumerator<T3> iter3 = collection3.GetEnumerator();

			foreach (var e1 in collection1) {
				if (!iter2.MoveNext()) throw new CountNotMatchException();
				if (!iter3.MoveNext()) throw new CountNotMatchException();
				yield return theFunction(e1, iter2.Current, iter3.Current);
			}
		}
		#endregion
	}
}