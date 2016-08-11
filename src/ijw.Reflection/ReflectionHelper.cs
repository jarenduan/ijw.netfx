using System;
using System.Linq.Expressions;
using ijw.Contract;
using System.Linq;
using ijw.Collection;

namespace ijw.Reflection {
    /// <summary>
    /// 反射功能帮助类
    /// </summary>
    public static class ReflectionHelper {
        public static T CreateNewInstance<T>(string[] propertyNames, string[] values) where T : class, new() {
            propertyNames.ShouldBeNotNullArgument();
            int fieldCount = propertyNames.Count();
            values.ShouldBeNotNullArgument();
            values.ShouldSatisfy(s => s.Count() == fieldCount);

            T obj = new T();
            CollectionHelper.ForEachPair(values, propertyNames, (s, h) => {
                obj.SetPropertyValue(h, s);
            });

            return obj;
        }

        /// <summary>
        /// 获取属性的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr">实例属性的表达式, 如foo => foo.bar </param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr) {
            var rtn = "";
            if (expr.Body is UnaryExpression) {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression) {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression) {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }
    }
}
