using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ijw.Data.Entity {
    /// <summary>
    /// 实体框架帮助类
    /// </summary>
    public static class IQueryableExt {
        /// <summary>
        /// 查询指定分页的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">查询的页码索引</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns>返回包含指定分页的查询</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageIndex, int pageSize) {
            if (pageIndex <= 0 || pageSize <= 0) throw new ArgumentOutOfRangeException();

            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 使用条件查询数据，同时获得符合条件的记录数量。注：此方法会导致实际发生一次记录数量统计的查询。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="where">查询条件</param>
        /// <param name="totalRecord">符合条件的记录总数</param>
        /// <returns>条件过滤后的查询</returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, Expression<Func<T, bool>> where, out int totalRecord) {
            var tempResult = query.Where(where);
            totalRecord = tempResult.Count();
            return tempResult;
        }

        /// <summary>
        /// 排序，可选使用正序或者逆序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TOrderBy"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderby">排序字段的表达式</param>
        /// <param name="ascending">是否正序排列</param>
        /// <returns></returns>
        public static IQueryable<TEntity> OrderBy<TEntity, TOrderBy>(this IQueryable<TEntity> query, Expression<Func<TEntity, TOrderBy>> orderby, bool ascending) {
            if (ascending) {
                return query.OrderBy(orderby);
            }
            else {
                return query.OrderByDescending(orderby);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">查询</param>
        /// <param name="propertyName">排序根据的属性名</param>
        /// <param name="ascending">是否正向排序</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending = true) {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "Not Exist");

            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
