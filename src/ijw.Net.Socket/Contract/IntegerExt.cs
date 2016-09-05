using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ijw.Contract;

namespace ijw.Net.Socket {
    public static class IntegerExt {
        public static bool ShouldBeValidPortNumber(this int num) {
            return num.ShouldNotLargerThan(65535) && num.ShouldBeNotLessThanZero();
        }
    }
}
