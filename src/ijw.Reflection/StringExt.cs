using System;
using System.Collections.Generic;

namespace ijw.Reflection
{
    public static class StringExt
    {
        /// <summary>
        /// 将字符串尝试转型成指定类型（用默认的FormatProvider）
        /// 支持属性类型目前包括string、Boolean/Char/DateTime/Int16/32/64/Float/Double/Decimal及相应可空类型
        /// 转型失败将抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aString"></param>
        /// <returns>成功转型后的值</returns>
        public static object To<T>(this string value) {
            string typeName = typeof(T).GetTypeName();
            if (typeName.StartsWith("System.Nullable`1") && value.Length == 0) {
                return null;
            }
            switch (typeName) {
                case "System.DateTime":
                case "System.Nullable`1[System.DateTime]":
                    return DateTime.Parse(value);
                case "System.String":
                    return value;
                case "System.Boolean":
                case "System.Nullable`1[System.Boolean]":
                    return bool.Parse(value);
                case "System.Char":
                case "System.Nullable`1[System.Char]":
                    return char.Parse(value);
                case "System.Byte":
                case "System.Nullable`1[System.Byte]":
                    return byte.Parse(value);
                case "System.Single":
                case "System.Nullable`1[System.Single]":
                    return Single.Parse(value);
                case "System.Double":
                case "System.Nullable`1[System.Double]":
                    return Double.Parse(value);
                case "System.Int16":
                case "System.Nullable`1[System.Int16]":
                    return Int16.Parse(value);
                case "System.Int32":
                case "System.Nullable`1[System.Int32]":
                    return Int32.Parse(value);
                case "System.Int64":
                case "System.Nullable`1[System.Int64]":
                    return Int64.Parse(value);
                case "System.Decimal":
                case "System.Nullable`1[System.Decimal]":
                    return Decimal.Parse(value);
                default:
                    throw new NotSupportedException($"{typeName} is not supported currently.");
            }
        }

    }
}
