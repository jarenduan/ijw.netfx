using System;
using System.Collections.Generic;
using System.Linq;
using ijw.Collection;

namespace ijw.Reflection
{
    public static class TypeExt
    {
        /// <summary>
        /// 返回命名空间.类名[泛型参数类型类名]
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
    }
}