using System;
using System.Collections.Generic;

namespace ijw.Collection {
    /// <summary>
    /// 提供了IList接口的一系列扩展方法
    /// </summary>
    public static class IListExt {
        public static IEnumerable<T> GetRandomSequence<T>(this IList<T> collection) {
            int[] order = 0.ToTotal(collection.Count).Shuffle();
            for (int i = 0; i < order.Length; i++) {
                yield return collection[order[i]];
            }
        }

        /// <summary>
        /// 在IList集合中查找第一个符合谓词的元素对象的索引
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="predicate">谓词, 为真则立即返回索引</param>
        /// <returns>返回第一个符合谓词的元素的索引, 如果没有符合的将会返回-1</returns>
        /// <remarks>
        /// 方法从前向后遍历列表, 因此时间复杂度是O(index), 即如果目标元素是第一个, 则只需要一次迭代.
        /// 此方法适用于预期元素处于列表中排位靠前的情况. 如果预期元素在较后的位置, 应该使用LastIndexOf&lt;T&gt;扩展方法.
        /// </remarks>
        public static int FirstIndexOf<T>(this IList<T> collection, Predicate<T> predicate) {
            for (int i = 0; i < collection.Count; i++) {
                if (predicate(collection[i])) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 在IList集合中查找最后一个符合谓词的元素对象的索引
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="predicate">谓词, 为真则立即返回索引</param>
        /// <returns>返回最后一个符合谓词的元素的索引, 如果没有符合的将会返回-1</returns>
        /// <remarks>
        /// 方法从后向前遍历列表, 因此时间复杂度是O(count-index), 即如果目标元素是最后一个, 则只需要一次迭代.
        /// 此方法适用于预期元素处于列表中排位靠后的情况. 如果预期元素在较前的位置, 应该使用FirstIndexOf&lt;T&gt;扩展方法.
        /// </remarks>
        public static int LastIndexOf<T>(this IList<T> collection, Predicate<T> predicate) {
            for (int i = collection.Count - 1; i >= 0; i--) {
                if (predicate(collection[i])) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 根据枚举策略查找第一个符合指定条件的元素的索引
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">列表</param>
        /// <param name="predicate">条件, 为真则返回</param>
        /// <param name="strategies">查找策略, 从后向前或者从前向后.</param>
        /// <returns>返回符合谓词的元素的索引, 如果没有符合的将会返回-1</returns>
        /// <remarks>
        /// 内部实际上调用了FirstIndexOf和LastIndexOf.
        /// 如果预期元素在较前的位置, 应该使用EnumeratingStrategies.Queue, 反之是EnumeratingStrategies.Stack.
        /// </remarks>
        public static int IndexOf<T>(this IList<T> collection, Predicate<T> predicate, FetchStrategies strategies) {
            int index = -1;
            switch (strategies) {
                case FetchStrategies.First:
                    index = collection.FirstIndexOf(predicate); //O(index)
                    break;
                case FetchStrategies.Last:
                    index = collection.LastIndexOf(predicate); //O(count - index)
                    break;
                default:
                    break;
            }
            return index;
        }
    }
}
