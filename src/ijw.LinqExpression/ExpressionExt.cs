using System;
using System.Linq.Expressions;

namespace ijw.LinqExpression {
    public static class ExpressionExt {
        /// <summary>
        /// 把自身作为左端, 把指定表达式作为右端, 然后用逻辑与连接.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right">逻辑与表达式的右端</param>
        /// <returns>生成的逻辑与表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right) {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary>
        /// 把自身作为左端, 把指定表达式作为右端, 然后用逻辑或连接起来.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp_left"></param>
        /// <param name="exp_right">逻辑或表达式的右端</param>
        /// <returns>生成的逻辑或表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp_left, Expression<Func<T, bool>> exp_right) {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(exp_left.Body);
            var right = parameterReplacer.Replace(exp_right.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        /// <summary> 
        /// 统一ParameterExpression 
        /// </summary> 
        internal class ParameterReplacer : ExpressionVisitor {
            public ParameterReplacer(ParameterExpression paramExpr) {
                this.ParameterExpression = paramExpr;
            }

            public ParameterExpression ParameterExpression { get; private set; }

            public Expression Replace(Expression expr) {
                return this.Visit(expr);
            }

            protected override Expression VisitParameter(ParameterExpression p) {
                return this.ParameterExpression;
            }
        }
    }
}
