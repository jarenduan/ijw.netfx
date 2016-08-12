using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ijw.Reflection.xTest
{
    public class UnitTestTypeExt
    {
        [Fact]
        public void TestGetTypeName() {
            List<List<DateTime>> t = new List<List<DateTime>>();
            var s = t.GetType().GetTypeName();
            Assert.Equal("System.Collections.Generic.List`1[System.Collections.Generic.List`1[System.DateTime]]", s);

            Tuple<int, List<string>> t1 = new Tuple<int, List<string>>(0, null);
            var name = t1.GetType().GetTypeName();
            Assert.Equal("System.Tuple`2[System.Int32,System.Collections.Generic.List`1[System.String]]", name);
        }

        private class test<T, V> {

        }
    }
}
