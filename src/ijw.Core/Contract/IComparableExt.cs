using System;

namespace ijw.Contract
{
    public static class IComparableExt
    {
        public static bool ShouldLargerThan<T>(this T obj, T other) where T : IComparable<T> {
            if (obj.CompareTo(other) <= 0) {
                throw new ContractBrokenException();
            }

            return true;
        }

        public static bool ShouldLessThan<T>(this T obj, T other) where T : IComparable<T> {
            if (obj.CompareTo(other) >= 0) {
                throw new ContractBrokenException();
            }
            return true;
        }

        public static bool ShouldNotLargerThan<T>(this T obj, T other) where T : IComparable<T> {
            if (obj.CompareTo(other) > 0) {
                throw new ContractBrokenException();
            }
            return true;
        }

        public static bool ShouldNotLessThan<T>(this T obj, T other) where T : IComparable<T> {
            if (obj.CompareTo(other) < 0) {
                throw new ContractBrokenException();
            }
            return true;
        }
    }
}
