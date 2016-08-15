using System;
using System.Collections.Generic;
using System.Linq;
using ijw.Collection;
using System.Reflection;

namespace ijw.Reflection
{
    public static class TypeExt
    {
        /// <summary>
        /// 返回命名空间.类名[命名空间.泛型参数1类名,命名空间.泛型参数2类名,...,命名空间.泛型参数n类名]
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(this Type type) {
#if NETSTANDARD1_4
            string typename = type.ToString();
#else
            string typename = $"{type.Namespace}.{type.Name}";
            if (type.IsGenericType) {
                typename = typename + type.GetGenericArguments().ToAllEnumStrings(",", "[", "]", (s) => s.GetTypeName());
            }
#endif
            return typename;
        }

        /// <summary>
        /// 根据属性名获取PropertyInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">属性名，大小写敏感</param>
        /// <returns>如果没有找到， 返回null</returns>
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName) {
#if NETSTANDARD1_4
            PropertyInfo pi = type.GetTypeInfo().GetDeclaredProperty(propertyName);
#else
            PropertyInfo pi = type.GetProperty(propertyName);
#endif
            return pi;
        }

        /// <summary>
        /// 根据方法名获取MethodInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName">方法名，大小写敏感</param>
        /// <returns>没找到返回null</returns>
        public static MethodInfo GetMethodInfo(this Type type, string methodName) {
#if NETSTANDARD1_4
            MethodInfo mi = type.GetTypeInfo().GetDeclaredMethod(methodName);
#else
            MethodInfo mi = type.GetMethod(methodName);
#endif
            if (mi == null) {
                throw new ArgumentOutOfRangeException(methodName);
            }
            return mi;
        }

        public static object GetDefaultValue(this Type type) {
#if NETSTANDARD1_4
            TypeInfo t = type.GetTypeInfo();
#else
            Type t = type;
#endif
            return t.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool IsEnumType(this Type type) {
#if NETSTANDARD1_4
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }
    }
}