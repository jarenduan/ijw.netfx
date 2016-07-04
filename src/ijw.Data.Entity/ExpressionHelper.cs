using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ijw.Data.Entity {
    /// <summary>
    /// Lambda表达式辅助类
    /// </summary>
    public static class ExpressionHelper {
        /// <summary>
        /// 返回总为真的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>总为假的表达式</returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 返回总为假的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>总为假的表达式</returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        
    }

}
