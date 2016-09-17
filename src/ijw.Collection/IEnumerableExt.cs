using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Contract;

namespace ijw.Collection {
    /// <summary>
    /// 提供了IEnumerable的一系列扩展方法
    /// </summary>
    public static class IEnumerableExt {
        #region Take
        public static IEnumerable<T> Take<T>(this IEnumerable<T> collection, int fromIndex, int toIndex) {
            fromIndex.ShouldNotLessThan(0);
            toIndex.ShouldNotLessThan(0);
            fromIndex.ShouldNotLargerThan(toIndex);

            return collection.TakeWhile((ele, index) => 
                index >= fromIndex && index <= toIndex
            );
        }

        /// <summary>
        /// 每次增加指定步长开始提取指定数目的元素，提取的所有元素将形成新集合, 内部使用了yield return.
        /// 步长和提取量相等, 则从起始处之后的元素全部被提取.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="step">步长，提取的索引增加量，1代表相邻的下一个</param>
        /// <param name="takeEachTime"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeEveryOther<T>(this IEnumerable<T> collection, int step, int takeEachTime) {
            step.ShouldLargerThan(0);
            takeEachTime.ShouldLargerThan(0);
            takeEachTime.ShouldNotLargerThan(step);

            return collection.TakeWhile((item, index) => 
                index % step < takeEachTime
            );
        }

        /// <summary>
        /// 类python风格的取子集. 如: 对[1,2,3,4,5], GetSub(0, -1)返回[1,2,3,4]; GetSub(-3, -1)返回[3, 4]; GetSub(1,2)返回[2]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="startAt">启始索引. 该处元素将包括在返回结果中. 0 = 第一个元素, -n = 倒数第n个元素, null = 0. 默认值是0</param>
        /// <param name="endAt">结束索引. 该处元素将不包括在返回结果中. 0 = 第一个元素, -n = 倒数第n个元素, null = 结尾. 默认值为null. </param>
        /// <returns>子集</returns>
        public static IEnumerable<T> TakePythonStyle<T>(this IEnumerable<T> collection, int? startAt = 0, int? endAt = null) {
            int startAtPython, endAtPython;
            int count = collection.Count();
            Helper.PythonStartEndCalculator(count, out startAtPython, out endAtPython, startAt, endAt);
            return collection.Take(startAtPython, endAtPython);
        }

        /// <summary>
        /// 把一个集合按指定的比率和方式分成两部分
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">源集合</param>
        /// <param name="ratioOfFirstGroup">第一部分的比例</param>
        /// <param name="ratioOfSecondGroup">第二把部分的比例</param>
        /// <param name="method">切分方式</param>
        /// <param name="firstGroup">切分后的第一部分</param>
        /// <param name="secondGroup">切分后的第二部分</param>
        public static void DivideByRatioAndMethod<T>(this IEnumerable<T> collection, int ratioOfFirstGroup, int ratioOfSecondGroup, CollectionDividingMethod method, out List<T> firstGroup, out List<T> secondGroup) {
            var source = collection;
            if(method == CollectionDividingMethod.Random) {
                IList<T> indexable = collection as IList<T>;
                source = indexable == null ? source.Random() : indexable.Random();
            }
            collection.DivideByRatio(ratioOfFirstGroup, ratioOfSecondGroup, out firstGroup, out secondGroup);
        }

        /// <summary>
        /// 按指定的比例把集合分拆成两部分
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">源集合</param>
        /// <param name="ratioOfFirstGroup">第一部分的占比</param>
        /// <param name="ratioOfSecondGroup">第二部分的占比</param>
        /// <param name="firstGroup">分拆出的第一部分</param>
        /// <param name="secondGroup">分拆出的第二部分</param>
        public static void DivideByRatio<T>(this IEnumerable<T> source, int ratioOfFirstGroup, int ratioOfSecondGroup, out List<T> firstGroup, out List<T> secondGroup) {
            var first = new List<T>();
            var second = new List<T>();
            source.ForEachWithIndex((element, index) => {
                if (index % (ratioOfFirstGroup + ratioOfSecondGroup) < ratioOfFirstGroup) {
                    first.Add(element);
                }
                else {
                    second.Add(element);
                }
            });

            firstGroup = first;
            secondGroup = second;
        }
        /// <summary>
        /// 从集合中按指定索引处, 提取相应的元素们形成新的集合. （输出按照元素在集合中的顺序，而非指定索引的顺序）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="indexes">指定的索引，一组整数</param>
        /// <returns></returns>
        public static IEnumerable<T> ElementsAt<T>(this IEnumerable<T> collection, IEnumerable<int> indexes) {
            int i = 0;
            foreach(var e in collection) {
                if(indexes.Contains(i)) {
                    yield return e;
                }
                i++;
            }

            #region 其他实现
            //另一实现
            //List<T> result = new List<T>();

            //collection.ForEachIndex((e, i) => {
            //    if (indexes.Contains(i)) {
            //        result.Add(e);
            //    }
            //});

            //return result;

            //早期实现
            //var q = from i in collection where indexes.Contains(Array.IndexOf(collection, i)) select i;
            //return q; 
            #endregion
        }

        /// <summary>
        /// 查找指定元素在集合第一次出现位置的索引
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection"></param>
        /// <param name="value">指定元素</param>
        /// <returns>如果集合中不存在, 返回-1;</returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, T value) {
            var list = collection as IList<T>;
            if(list != null) {
                return list.IndexOf(value);
            }

            var ilist = collection as IList;
            if(ilist != null) {
                return ilist.IndexOf(value);
            }

            int index = -1;



            collection.ForEachWithIndexWhile((v, i) => {
                if(v.Equals(value)) {
                    index = i;
                    return false;
                }
                else
                    return true;
            });
            return index;
        }
        #endregion

        #region Random

        /// <summary>
        /// 返回随机打乱顺序的序列. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection) {
            var array = collection as T[];
            if (array != null) {
                //如果能转成T[]. 高效实现.
                foreach (var item in array.Random()) {
                    yield return item;
                }
            }
            else {
                var list = collection as IList<T>;
                if (list != null) {
                    //如果能转成Ilist. 高效实现.
                    foreach (var item in list.Random()) {
                        yield return item;
                    }
                }
                else {
                    //没办法的办法, 低效实现;
                    int[] order = 0.ToTotal(collection.Count()).Shuffle();
                    for (int i = 0; i < order.Length; i++) {
                        yield return collection.ElementAt(order[i]);
                    }
                }
            }
        }

        #endregion

        #region ToStrings
        /// <summary>
        /// 输出形如[a1, a2 ... an]的带省略号的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="maxDisplayNumber"></param>
        /// <returns></returns>
        public static string ToSimpleEnumStrings<T>(this IEnumerable<T> collection, int maxDisplayNumber = 3) {
            if(maxDisplayNumber <= 0)
                maxDisplayNumber = 3;
            int count = collection.Count();
            if(count <= maxDisplayNumber) {
                return collection.ToAllEnumStrings();
            }
            else {
                StringBuilder sb = new StringBuilder("[");
                foreach (var item in collection.Where((item,index) => index <= maxDisplayNumber - 2)){
                    appendSimpleStringIfPossible<T>(sb, item);
                }

                //collection.ForEachWithIndexAndBreak((item, index) => {
                //    if(index <= maxDisplayNumber - 2) {
                //        appendSimpleStringIfPossible<T>(sb, item);
                //        return true;
                //    }
                //    else {
                //        return false;
                //    }
                //});
                sb.Append("..., ");
                appendSimpleStringIfPossible<T>(sb, collection.Last());
                sb.Append("]");
                return sb.ToString();
            }
        }

        private static void appendSimpleStringIfPossible<T>(StringBuilder sb, T item) {
            IEnumerable<T> ienum = item as IEnumerable<T>;
            if(ienum != null) {
                sb.Append(ienum.ToSimpleEnumStrings());
            }
            else {
                sb.Append(item.ToString());
            }
            sb.Append(", ");
        }

        /// <summary>
        /// 输出包含所有元素的字符串，默认形如[a1, a2, a3, [a41, a42, a43], a5]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="separator">元素之间的分隔符，默认是", "</param>
        /// <param name="prefix">第一个元素之前的字符串，默认是"["</param>
        /// <param name="postfix">最后一个元素之后的字符串，默认是"]"</param>
        /// <returns></returns>
        public static string ToAllEnumStrings<T>(this IEnumerable<T> collection, string separator = ", ", string prefix = "[", string postfix = "]", Func<T, string> transform = null) {
            StringBuilder sb = new StringBuilder(prefix);
            foreach(var item in collection) {
                IEnumerable<T> ienum = item as IEnumerable<T>;
                if(ienum != null) {
                    sb.Append(ienum.ToAllEnumStrings(separator, prefix, postfix));
                }
                else {
                    string s = transform == null ? item.ToString(): transform(item) ;
                    sb.Append(s);
                }
                sb.Append(separator);
            }
            sb.RemoveLast(separator.Length);
            sb.Append(postfix);
            return sb.ToString();
        }

        #endregion

        #region Each pair with index
        public static IEnumerable<Tuple<T, int>> EachWithIndex<T>(this IEnumerable<T> collection) {
            int index = 0;
            foreach (var element in collection) {
                yield return Tuple.Create(element, index);
                index++;
            }
        }
        #endregion

        #region For Each
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var item in collection) {
                action(item);
            }
        }

        public static void ForEachWhile<T>(this IEnumerable<T> collection, Func<T, bool> actionWithBreak) {
            foreach (var item in collection) {
                if (!actionWithBreak(item)) {
                    break;
                }
            }
        }

        /// <summary>
        /// 在集合上遍历调用某个函数, 提供元素和索引同时作为参数, 索引从0开始. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns>集合的元素个数</returns>
        public static int ForEachWithIndex<T>(this IEnumerable<T> collection, Action<T, int> action) {
            int index = 0;
            foreach (var element in collection) {
                action(element, index);
                index++;
            }
            return index;
        }

        /// <summary>
        /// 在集合上遍历调用某个函数, 提供元素和索引同时作为参数, 索引从0开始. 函数返回值可以控制是否break循环.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="actionWithBreak">返回TRUE继续循环, 返回false则break退出</param>
        public static void ForEachWithIndexWhile<T>(this IEnumerable<T> collection, Func<T, int, bool> actionWithBreak) {
            int index = 0;
            foreach (var element in collection) {
                if (!actionWithBreak(element, index))
                    break;
                index++;
            }
        }
        #endregion

        #region Doubles' Normalizer
        /// <summary>
        /// 对浮点集合中的值逐一进行归一化
        /// </summary>
        /// <param name="collection">浮点数集合</param>
        /// <param name="maxValues">归一化上限值的集合</param>
        /// <param name="minValues">归一化下限值的集合</param>
        /// <returns>归一化后的集合</returns>
        public static List<double> Normalize(this IEnumerable<double> collection, IEnumerable<double> maxValues, IEnumerable<double> minValues) {
            return CollectionHelper.ForEachThree(collection, maxValues, minValues, (x, max, min) => x.NormalizeMaxMin(min, max));
        }

        /// <summary>
        /// 对浮点集合中的值逐一进行反归一化
        /// </summary>
        /// <param name="collection">浮点数集合</param>
        /// <param name="maxValues">归一化上限值的集合</param>
        /// <param name="minValues">归一化下限值的集合</param>
        /// <returns>反归一化后的集合</returns>
        public static List<double> Denormalize(this IEnumerable<double> collection, IEnumerable<double> maxValues, IEnumerable<double> minValues) {
            return CollectionHelper.ForEachThree(collection, maxValues, minValues, (x, max, min) => x.DenormalizeMaxMin(min, max));
        }
        #endregion

        #region First
        public static T FirstOrDefault<T>(this IEnumerable<T> collection, Func<T, int, bool> pred) {
            int index = 0;
            foreach (var item in collection) {
                if (pred(item, index)) {
                    return item;
                }
                index++;
            }
            return default(T);
        }

        public static T First<T>(this IEnumerable<T> collection, Func<T, int, bool> pred) {
            int index = 0;
            foreach (var item in collection) {
                if (pred(item, index)) {
                    return item;
                }
                index++;
            }
            throw new InvalidOperationException();
        }
        #endregion  
             
        #region IndexOf
        /// <summary>
        /// 在IEnumerable&lt;T&gt;查找第一个符合谓词的元素对象的索引
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="predicate">谓词, 为真则立即返回索引</param>
        /// <returns>返回第一个符合谓词的元素的索引, 如果没有符合的将会返回-1</returns>
        /// <remarks>
        /// 方法从后向前遍历集合, 因此时间复杂度是O(index), 即如果目标元素是第一个, 则只需要一次迭代.
        /// 此方法适用于预期元素处于列表中排位靠后的情况. 如果预期元素在较前的位置, 应该使用LastIndexOf&lt;T&gt;扩展方法.
        /// </remarks>
        public static int IndexOf<T>(this IEnumerable<T> collection, Predicate<T> predicate) {
            int index = 0;
            foreach(var item in collection) {
                if(predicate(item)) {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 在IEnumerable&lt;T&gt;查找最后一个出现的元素对象索引
        /// </summary>
        public static int LastIndexOf<T>(this IEnumerable<T> collection, T item) {
            var list = collection as List<T>;
            if (list != null) {
                return list.LastIndexOf(item);
            }
            return collection.Reverse().IndexOf(item);
        } 
        #endregion
    }
}