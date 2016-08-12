using System;
using System.Reflection;
using ijw.Collection;

namespace ijw.Reflection {
    public static class ObjectExt {
        /// <summary>
        /// 设置指定的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName">属性的名字</param>
        /// <param name="value">属性值</param>
        /// <remarks>
        /// 属性值运行时类型如果不符合，将会抛出异常
        /// </remarks>
        public static void SetPropertyValue<T>(this T obj, string propertyName, object value) {
#if NETSTANDARD1_4
            PropertyInfo p = typeof(T).GetTypeInfo().GetDeclaredProperty(propertyName);
#else
            PropertyInfo p = obj.GetType().GetProperty(propertyName);
#endif
            if (p != null) {
                p.SetValue(obj, value, null);
            }
        }

        /// <summary>
        /// 将字符串尝试转型成属性的类型（用默认的FormatProvider），并把成功转型后的值设置给指定的属性。多用于从文本文件中构建对象。
        /// 转型失败将抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName">属性的名字，必须存在</param>
        /// <param name="value">属性值</param>
        public static void SetPropertyValue<T>(this T obj, string propertyName, string value) {
#if NETSTANDARD1_4
            PropertyInfo p = typeof(T).GetTypeInfo().GetDeclaredProperty(propertyName);
#else
            PropertyInfo p = obj.GetType().GetProperty(propertyName);
#endif
            if (p != null) {
                setPropertyValue(obj, value, p);
            }
        }

        private static void setPropertyValue<T>(T obj, string value, PropertyInfo p) {
            Type propertyType = p.PropertyType;
            Type valueType = value.GetType();
            object typedValue = value;
            if (propertyType.GetTypeName() != "System.String") {
                MethodInfo mi = null;
#if NETSTANDARD1_4
                mi = typeof(StringExt).GetTypeInfo().GetDeclaredMethod("To");
#else
                mi = typeof(StringExt).GetMethod("To");
#endif
                typedValue = mi.MakeGenericMethod(propertyType).Invoke(null, new object[] { value });
            }
            p.SetValue(obj, typedValue, null);
        }
    }
}
