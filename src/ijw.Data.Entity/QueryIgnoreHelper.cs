using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Data.Entity {
    /// <summary>
    /// 查询时忽略值的帮助类.
    /// </summary>
    public static class QueryIgnoreHelper {
        /// <summary>
        /// 字符串型字段设为此值代表查询时忽略该字段
        /// </summary>
        public const string QueryIgnoreValue = "~查~询~忽~略~值~";

        /// <summary>
        /// 查询字符串是否是忽略值
        /// </summary>
        /// <param name="queryParameter"></param>
        /// <returns></returns>
        public static bool IfShouldIgnore(string queryParameter) {
            return queryParameter == QueryIgnoreValue;
        }
    }
}
