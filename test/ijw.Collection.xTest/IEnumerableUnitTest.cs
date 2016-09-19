using System;
using System.Collections.Generic;
using ijw.Collection;
using System.Linq;
using Xunit;

namespace ijw.Collection.xTest
{
    public class IEnumerableUnitTest {
        [Fact]
        public void GetSubLazyPythonStyleTest() {
            string s = "12345";

            //从头开始到结束: [1,2,3,4,5]
            var re = s.TakePythonStyle(0).ToArray();
            Assert.Equal("12345".ToCharArray(), re);

            //从第二个元素(索引为1)到结束: [2,3,4,5]
            re = s.TakePythonStyle(1).ToArray();
            Assert.Equal("2345".ToCharArray(), re);

            //从倒数第一个到结束: [5]
            re = s.TakePythonStyle(-1).ToArray();
            Assert.Equal("5".ToCharArray(), re);

            //从头到第二个元素之前: [1]
            re = s.TakePythonStyle(0, 1).ToArray();
            Assert.Equal("1".ToCharArray(), re);

            //从头到第三个元素之前: [1,2]
            re = s.TakePythonStyle(0, 2).ToArray();
            Assert.Equal("12".ToCharArray(), re);

            //从第二个元素到第二个元素之前: []
            re = s.TakePythonStyle(1, 1).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从第二个元素到第三个元素之前: [2]
            re = s.TakePythonStyle(1, 2).ToArray();
            Assert.Equal("2".ToCharArray(), re);

            //从第一个元素到第一个元素之前: []
            re = s.TakePythonStyle(0, 0).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从第一个元素到倒数第一个元素之前: [1,2,3,4]
            re = s.TakePythonStyle(0, -1).ToArray();
            Assert.Equal("1234".ToCharArray(), re);

            //从第二个元素到倒数第二个元素之前[2,3]
            re = s.TakePythonStyle(1, -2).ToArray();
            Assert.Equal("23".ToCharArray(), re);

            //从第四个元素到倒数第三个元素之前: []
            re = s.TakePythonStyle(3, -3).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从第三个元素到倒数第三个元素之前: []
            re = s.TakePythonStyle(2, -3).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从第二个元素到倒数第三个元素之前: [2]
            re = s.TakePythonStyle(1, -3).ToArray();
            Assert.Equal("2".ToCharArray(), re);

            //从第7个元素到倒数第三个元素之前: []
            re = s.TakePythonStyle(6, -2).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从倒数第一个元素到倒数第二个元素之前: []
            re = s.TakePythonStyle(-1, -2).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从倒数第二个到倒数第一个元素之前: [4]
            re = s.TakePythonStyle(-2, -1).ToArray();
            Assert.Equal("4".ToCharArray(), re);

            //从倒数第一个到倒数第一个元素之前: []
            re = s.TakePythonStyle(-1, -1).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从头到倒数第5个元素之前: []
            re = s.TakePythonStyle(0, -5).ToArray();
            Assert.Equal("".ToCharArray(), re);

            //从头到倒数第6个元素之前: []
            re = s.TakePythonStyle(0, -6).ToArray();
            Assert.Equal("".ToCharArray(), re);
        }
    }
}
