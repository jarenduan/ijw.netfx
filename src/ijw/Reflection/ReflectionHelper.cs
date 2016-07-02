using System;
using System.Linq.Expressions;

namespace ijw.Reflection {
    /// <summary>
    /// 反射功能帮助类
    /// </summary>
    public static class ReflectionHelper {
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
