using System;
using Xunit;
using ijw.Diagnostic;

namespace ijw.Core.xTest {
    public class DebugHelperTest {
#if NET452
        [Fact]
        public void TestGetCallerName() {
            var n = test();
            Assert.Equal("DebugHelperTest.TestGetCallerName", n);
        }

        private static string test() {
            return DebugHelper.GetCallerName();
        }
#endif
    }
}