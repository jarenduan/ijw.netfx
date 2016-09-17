using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Collection
{
    /// <summary>
    /// 提供对一维数组的扩展方法
    /// </summary>
    public static class OneRankArrayExt
    {
        public static void Initialize<T>(this T[] collection, Func<int, T> initializer) {
            for (int i = 0; i < collection.Length; i++) {
                collection[i] = initializer(i);
            }
        }
        public static IEnumerator<T> GetEnumeratorGenerics<T>(this T[] collection)
        {
            return collection.AsEnumerable().GetEnumerator();
        }
        /// <summary>
        /// 返回随机打乱顺序的序列迭代器。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<T> Random<T>(this T[] collection)
        {
            int[] order = 0.ToTotal(collection.Length).Shuffle();
            for (int i = 0; i < order.Length; i++)
            {
                yield return collection[order[i]];
            }
        }

        /// <summary>
        /// 值比较. 即依次调用Equals方法比较数组中每个元素是否相等.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool ItemEquals<T>(this T[] source, T[] comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException();
            }
            if (comparer.Length != comparer.Length)
            {
                throw new CountNotMatchException();
            }
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Equals(comparer[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 以从前向后的顺序, 移除数组中所有具有指定值的元素, 结果将保存在新的数组中返回.
        /// 数组维数不变, 其中可能出现的剩余空间将设为类型的默认值.
        /// 如: 对整形数组{3, 1, 1, 0, 4, 1, 2, 2, 0} 调用 RemoveAll(1) 将得到新数组: {3, 0, 4, 2, 2, 0, 0, 0, 0).
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source"></param>
        /// <param name="toRemove">指定的值</param>
        /// <returns></returns>
        public static T[] RemoveAll<T>(this T[] source, T toRemove)
        {
            int len = source.Length;
            T[] result = new T[len];
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (source[i].Equals(toRemove))
                    continue;
                result[j] = source[i];
                j++;
            }
            return result;
        }

        /// <summary>
        /// 以从前向后的顺序, 移除数组中所有具有指定值的元素, 结果将保存在新的数组中返回. 新数组维数会发生变化.
        /// 如: 对整形数组{3, 1, 1, 0, 4, 1, 2, 2, 0} 调用 RemoveAll(1) 将得到新数组: {3, 0, 4, 2, 2).
        /// 内部使用了Linq实现.
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source"></param>
        /// <param name="toRemove">指定的值</param>
        /// <returns></returns>
        public static T[] ShrinkByRemoving<T>(this T[] source, T toRemove)
        {
            var r = from item in source
                    where !item.Equals(toRemove)
                    select item;
            return r.ToArray();
        }

        /// <summary>
        /// 对数组中的元素进行替换. 返回新数组.
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source"></param>
        /// <param name="replace">要替换的值</param>
        /// <param name="with">替换成的值</param>
        /// <returns>新数组</returns>
        public static T[] ReplaceAll<T>(this T[] source, T replace, T with = default(T))
        {
            int len = source.Length;
            T[] result = new T[len];
            for (int i = 0; i < len; i++)
            {
                if (source[i].Equals(replace))
                    result[i] = with;
                else
                    result[i] = source[i];
            }
            return result;
        }

        #region Set Values
        /// <summary>
        /// 根据指定的一系列索引, 设置数组中的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index">指定的索引, 一组整数</param>
        /// <param name="values">指定的值</param>
        public static void SetValuesForTheIndexes<T>(this T[] collection, IEnumerable<int> index, IEnumerable<T> values)
        {
            if (index.Count() != values.Count())
                throw new CountNotMatchException();
            Dictionary<int, T> dict = new Dictionary<int, T>();
            CollectionHelper.ForEachPair(index, values, (i, v) => { dict.Add(i, v); });
            SetValuesForTheIndexes(collection, dict);
        }

        /// <summary>
        /// 为数组设置指定索引处的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="values"></param>
        public static void SetValuesForTheIndexes<T>(this T[] collection, Dictionary<int, T> values)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                T value;
                if (values.TryGetValue(i, out value))
                {
                    collection[i] = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 随机打乱数组中元素的排列顺序, 返回新数组.
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this T[] numbers)
        {
            int count = numbers.Length;
            if (count == 0)
            {
                return numbers;
            }
            Random r = new Random();
            int i = r.Next(count / 2, count);
            i.Times(() =>
            {
                int i1 = r.Next(count);
                int i2 = r.Next(count);
                var temp = numbers[i1];
                numbers[i1] = numbers[i2];
                numbers[i2] = temp;
            });
            return numbers;
        }

        public static void DivideByRatioAndMethod<T>(this T[] collection, int ratioOfFirstGroup, int ratioOfSecondGroup, CollectionDividingMethod method, out IList<T> firstGroup, out IList<T> secondGroup)
        {
            IEnumerable<T> source = collection;
            if (method == CollectionDividingMethod.Random)
            {
                IList<T> indexable = collection as IList<T>;
                if (indexable == null)
                {
                    source = source.Random();
                }
                else
                {
                    source = indexable.Random();
                }
            }
            var first = new List<T>();
            var second = new List<T>();
            source.ForEachWithIndex((element, index) =>
            {
                if (index % (ratioOfFirstGroup + ratioOfSecondGroup) < ratioOfFirstGroup)
                {
                    first.Add(element);
                }
                else
                {
                    second.Add(element);
                }
            });

            firstGroup = first;
            secondGroup = second;
        }
    }
}