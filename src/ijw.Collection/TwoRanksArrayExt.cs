using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection {
    /// <summary>
    /// 提供对二维数组的若干扩展方法
    /// </summary>
    public static class TwoRanksArrayExt {
        /// <summary>
        /// 对二维数组中每个元素进行迭代, 调用指定操作
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="array"></param>
        /// <param name="action">指定的操作</param>
        public static void ForEach<T>(this T[,] array, Action<T> action) {
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    action(array[i, j]);
                }
            }
        }
        internal static void ForEachRef<T>(this T[,] array, ActionWithRef<T> action) {
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    action(ref array[i, j]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEachWithIndex<T>(this T[,] array, Action<T, int, int> action) {
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    action(array[i, j], i, j);
                }
            }
        }
        internal static void ForEachRefWithIndex<T>(this T[,] array, ActionWithRefAndIndex<T> action) {
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    action(ref array[i, j], i, j);
                }
            }
        }

        /// <summary>
        /// 对二维数组的每一行进行迭代, 调用指定操作
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="array"></param>
        /// <param name="action">指定的操作, 接受一行作为参数</param>
        public static void ForEachRow<T>(this T[,] array, Action<IEnumerable<T>> action) {
            for (int i = 0; i < array.GetLength(0); i++) {
                action(array.GetRowAt(i));
            }
        }
        /// <summary>
        ///  对二维数组的每一列进行迭代, 调用指定操作
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="array"></param>
        /// <param name="action">指定操作</param>
        public static void ForEachColumn<T>(this T[,] array, Action<IEnumerable<T>> action) {
            for (int i = 0; i < array.GetLength(1); i++) {
                action(array.GetColumnAt(i));
            }
        }

        /// <summary>
        /// 把每个单元格设为指定值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">指定值</param>
        public static void SetEach<T>(this T[,] array, T value) {
            array.ForEachRef(delegate(ref T item) {
                item = value;
            });
        }

        /// <summary>
        /// 根据指定的方法设置每个单元格中的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="function"></param>
        public static void SetEach<T>(this T[,] array, Func<int, int, T> function = null) {
            array.ForEachRefWithIndex(delegate(ref T item, int i, int j) {
                if (function == null) {
                    item = default(T);
                }
                else {
                    item = function(i, j);
                }
            });

            //for (int i = 0; i < array.GetLength(0); i++) {
            //    for (int j = 0; j < array.GetLength(1); j++) {

            //    }
            //}
        }

        /// <summary>
        /// 对每一行使用指定的函数进行变换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="function">变换函数，接受一行，返回一组新值</param>
        public static void SetEachRow<T>(this T[,] array, Func<T[], T[]> function)
        {
           for (int i = 0; i < array.GetLength(0); i++) {
               array.SetRowAt(i, function(array.GetRowCopyOf(i)));
            }
        }

        /// <summary>
        /// 对每一列使用指定的函数进行变换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="function">变换函数，接受一列，返回一组新值</param>
        public static void SetEachColumn<T>(this T[,] array, Func<T[], T[]> function) {
            for (int i = 0; i < array.GetLength(1); i++) {
                array.SetColumnAt(i, function(array.GetColumnCopyOf(i)));
            }
        }

        /// <summary>
        /// 清空数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void Clear<T>(this T[,] array) {
            array.ForEachRef(delegate(ref T item) {
                item = default(T);
            });
        }

        internal static T[] GetRowCopyOf<T>(this T[,] array, int index) {
            return array.GetRowAt(index).ToArray();
        }

        internal static T[] GetColumnCopyOf<T>(this T[,] array, int index) {
            return array.GetColumnAt(index).ToArray();
        }
        
        /// <summary>
        /// 获取二维数组的某一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">行号</param>
        /// <returns></returns>
        public static IEnumerable<T> GetRowAt<T>(this T[,] array, int index) {
            if (index >= array.GetLength(0)) {
                throw new ArgumentOutOfRangeException();
            }
            int columnCount = array.GetLength(1);
            for (int i = 0; i < columnCount; i++) {
                yield return array[index, i];
            }
        }

        /// <summary>
        /// 获取二维数组的某一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">列号</param>
        /// <returns></returns>
        public static IEnumerable<T> GetColumnAt<T>(this T[,] array, int index) {
            if (index >= array.GetLength(1)) {
                throw new ArgumentOutOfRangeException();
            }
            int rowCount = array.GetLength(0);
            for (int i = 0; i < rowCount; i++) {
                yield return array[i, index];
            }
        }

        /// <summary>
        /// 用指定数组的值替换掉数组中某一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">行索引</param>
        /// <param name="value">给定的数组</param>
        public static void SetRowAt<T>(this T[,] array, int index, T[] value) {
            //TODO: use ijw.contract
            if (index >= array.GetLength(0)) {
                throw new ArgumentOutOfRangeException();
            }
            if (value == null) {
                throw new ArgumentNullException();
            }
            if (value.Length != array.GetLength(1)) {
                throw new ArgumentException();
            }
            int columnCount = array.GetLength(1);
            for (int i = 0; i < columnCount; i++) {
                array[index, i] = value[i];
            }
        }

        /// <summary>
        /// 用指定数组的值替换掉数组中某一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index">列索引</param>
        /// <param name="value">给定的数组</param>
        public static void SetColumnAt<T>(this T[,] array, int index, T[] value) {
            if (index >= array.GetLength(1)) {
                throw new ArgumentOutOfRangeException();
            }
            if (value == null) {
                throw new ArgumentNullException();
            }
            if (value.Length != array.GetLength(0)) {
                throw new ArgumentException();
            }
            int rowCount = array.GetLength(1);
            for (int i = 0; i < rowCount; i++) {
                array[i, index] = value[i];
            }
        }
    }
}