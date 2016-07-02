using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.dotnet {
    /// <summary>
    /// 表示集合元素维度不匹配的异常
    /// </summary>
    public class DimensionNotMatchException : Exception {
        public DimensionNotMatchException() {
        }

        public DimensionNotMatchException(string message) : base(message) {
        }
    }
}
