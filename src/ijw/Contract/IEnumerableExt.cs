using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Contract {
    public static class IEnumerableExt {
        public static bool ShouldNotBeNullOrEmpty<T>(this IEnumerable<T> collection) {
            return collection.Count().ShouldBeNotZero();
        }


        public static bool ShouldEachSatisfy<T>(this IEnumerable<T> collection, Predicate<T> condition) {
            foreach (var item in collection) {
                item.ShouldSatisfy(condition);
            }
            return true;
        }
    }
}
