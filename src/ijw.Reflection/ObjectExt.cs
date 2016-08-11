using System;
using System.Reflection;
using ijw.Collection;

namespace ijw.Reflection {
    public static class ObjectExt {
#if NETSTANDARD1_4
        public static void SetPropertyValue<T>(this T obj, string propertyName, object value) {
            PropertyInfo p = typeof(T).GetTypeInfo().GetDeclaredProperty(propertyName);
            if (p != null) {
                setPropertyValue(obj, value, p);
            }
        }
#else
        public static void SetPropertyValue<T>(this T obj, string propertyName, object value) {
            PropertyInfo p = obj.GetType().GetProperty(propertyName);
            if (p != null) {
                setPropertyValue(obj, value, p);
            }
        }
#endif
        private static void setPropertyValue<T>(T obj, object value, PropertyInfo p) {
            Type propertyType = p.PropertyType;
            Type valueType = value.GetType();
            if (p.Name != valueType.Name) {
                value = convert(value, propertyType);
            }
            p.SetValue(obj, value, null);
        }
        private static object convert(object value, Type propertyType) {

#if NETSTANDARD1_4
            switch (propertyType.ToString()) {
#else
            string typeName = $"{propertyType.Namespace}.{propertyType.Name}";
            if (propertyType.IsGenericType) {
                typeName = typeName + propertyType.GetGenericArguments().ToAllEnumStrings(",");
            }
            switch (typeName) {
#endif
                case "System.DateTime":
                    return value is DateTime ? (DateTime)value : DateTime.Parse(value.ToString());
                case "System.String":
                    return value.ToString();
                case "System.Single":
                    return value is Single ? (Single)value : Single.Parse(value.ToString());
                case "System.Double":
                    return value is Double ? (Double)value : Double.Parse(value.ToString());
                case "System.Int16":
                    return value is Int16 ? (Int16)value : Int16.Parse(value.ToString());
                case "System.Int32":
                    return value is Int32 ? (Int32)value : Int32.Parse(value.ToString());
                case "System.Int64":
                    return value is Int64 ? (Int64)value : Int64.Parse(value.ToString());
                case "System.Decimal":
                    return value is Decimal ? (Decimal)value : Decimal.Parse(value.ToString());
                case "System.Nullable`1[System.Double]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is double ? (double)value : double.Parse(value.ToString());
                    }
                case "System.Nullable`1[System.Single]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is Single ? (Single)value : Single.Parse(value.ToString());
                    }
                case "System.Nullable`1[System.Int16]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is Int16 ? (Int16)value : Int16.Parse(value.ToString());
                    }
                case "System.Nullable`1[System.Int32]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is Int32 ? (Int32)value : Int32.Parse(value.ToString());
                    }
                case "System.Nullable`1[System.Int64]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is Int64 ? (Int64)value : Int64.Parse(value.ToString());
                    }
                case "System.Nullable`1[System.Decimal]":
                    if (value.ToString().Length == 0) {
                        return null;
                    }
                    else {
                        return value is Decimal ? (Decimal)value : Decimal.Parse(value.ToString());
                    }
                default:
                    throw new NotSupportedException($"{propertyType.Name} is not supported currently.");
            }
        }
    }
}
