using ijw;
using System;
using System.IO;

namespace ijw.Contract {
    /// <summary>
    /// 提供一系列Object的扩展方法
    /// </summary>
    public static class ObjectExt {
        public static bool ShouldBeNotNullArgument<T>(this T obj, string paramName = "") where T : class {
            if (obj == null) {
                if (paramName == "") {
                    throw new ArgumentNullException();
                }
                else {
                    throw new ArgumentNullException(paramName);
                }
            }
            return true;
        }

        public static bool ShouldBeNotNullReference<T>(this T obj, string message = "") where T : class {
            if (obj == null) {
                if (message == "") {
                    throw new NullReferenceException();
                }
                else {
                    throw new NullReferenceException(message);
                }
            }
            return true;
        }

        public static bool ShouldSatisfy<T>(this T obj, Predicate<T> condition) {
            if (!condition(obj)) {
                throw new NotSatisfiedConditionException<T>(obj, condition);
            }
            return true;
        }

        public static bool ShouldSatisfy<T, TException>(this T obj, Predicate<T> condition, TException excpetion) where TException : Exception {
            if (!condition(obj)) {
                throw excpetion;
            }
            return true;
        }

        public static bool ShouldEquals<T>(this T obj, T other) {
            if (!obj.Equals(other)) {
                throw new ContractBrokenException();
            }
            return true;
        }

        public static bool ShouldEquals<T, TException>(this T obj, T other, Exception excpetion) where TException : Exception {
            if (!obj.Equals(other)) {
                throw excpetion;
            }
            return true;
        }

        public static bool ShouldNotEquals<T>(this T obj, T other) {
            if (obj.Equals(other)) {
                throw new ContractBrokenException();
            }
            return true;
        }

        public static bool ShouldNotEquals<T, TException>(this T obj, T other, TException excpetion) where TException : Exception {
            if (obj.Equals(other)) {
                throw excpetion;
            }
            return true;
        }
    }
}
