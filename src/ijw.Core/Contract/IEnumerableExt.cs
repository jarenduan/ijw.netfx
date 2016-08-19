using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Contract {
    public static class IEnumerableExt {
        public static bool ShouldNotBeEmpty<T>(this IEnumerable<T> collection) {
            collection.ShouldBeNotNullReference();
            return collection.Count().ShouldBeNotZero();
        }
        public static bool ShouldNotBeNullOrEmpty<T>(this IEnumerable<T> collection) {
            collection.ShouldBeNotNullReference();
            return collection.ShouldNotBeEmpty();
        }
        public static bool ShouldEachSatisfy<T>(this IEnumerable<T> collection, Predicate<T> condition) {
            foreach (var item in collection) {
                item.ShouldSatisfy(condition);
            }
            return true;
        }
    }
}
