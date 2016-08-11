using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ijw.Reflection.xTest
{
    public class UnitTestObjectExt
    {
        [Fact]
        public void UnitTestCreateNewInstance()
        {
            string[] propertyName = {
                "PropDatetime",
                "PropInt32",
                "PropInt16",
                "PropInt64",
                "PropDouble",
                "PropSingle",
                "PropDecimal",
                "PropString",
                "PropInt32Nullable",
                "PropInt16Nullable",
                "PropInt64Nullable",
                "PropDoubleNullable",
                "PropSingleNullable",
                "PropDecimalNullable"
            };
            string[] values = {
                "2016/08/08 16:44:33",
                "32", "16", "64", "128.0", "256.0", "123.45", "just a string",
                "32", "", "64", "128.0", "", "123.45"
            };
            testClass t = ReflectionHelper.CreateNewInstance<testClass>(propertyName, values);
            Assert.Equal(new DateTime(2016, 8, 8, 16, 44, 33), t.PropDatetime);
            Assert.Equal(32, t.PropInt32);
            Assert.Equal(16, t.PropInt16);
            Assert.Equal(64, t.PropInt64);
            Assert.Equal(128.0d, t.PropDouble);
            Assert.Equal(256.0f, t.PropSingle);
            Assert.Equal(123.45m, t.PropDecimal);
            Assert.Equal("just a string", t.PropString);
            Assert.Equal(32, t.PropInt32);
            Assert.Equal(null, t.PropInt16Nullable);
            Assert.Equal(64, t.PropInt64);
            Assert.Equal(128.0d, t.PropDoubleNullable);
            Assert.Equal(null, t.PropSingleNullable);
            Assert.Equal(123.45m, t.PropDecimalNullable);
        }

        private class testClass {
            public DateTime PropDatetime { get; set; }
            public int PropInt32 { get; set; }
            public Int16 PropInt16 { get; set; }
            public Int64 PropInt64 { get; set; }
            public double? PropDouble { get; set; }
            public float PropSingle { get; set; }
            public decimal PropDecimal { get; set; }
            public string PropString { get; set; }
            public int? PropInt32Nullable { get; set; }
            public Int16? PropInt16Nullable { get; set; }
            public Int64? PropInt64Nullable { get; set; }
            public double? PropDoubleNullable { get; set; }
            public float? PropSingleNullable { get; set; }
            public decimal? PropDecimalNullable { get; set; }
        }
    }
}
