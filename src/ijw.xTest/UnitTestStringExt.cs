using System.Linq;
using Xunit;

namespace ijw.xTest {
    public class UnitTestStringExt {

        [Fact]
        public void TestFormatJson() {

            string jstr = @"{""age"":23,""aihao"":[""pashan"",""movies""],""name"":{""firstName"":""zhang"",""lastName"":""san""}}";
            string formatted = jstr.FormatJson();
            string shouldBe =
@"{
   ""age"": 23,
   ""aihao"": [
      ""pashan"",
      ""movies""
   ],
   ""name"": {
      ""firstName"": ""zhang"",
      ""lastName"": ""san""
   }
}";
            //shouldBe = shouldBe.Replace("\r", "");
            Assert.Equal(shouldBe, formatted);
        }

        [Fact]
        public void TestGetSubStringPythonStyle() {
            string s = "12345";

            //从头开始到结束: [1,2,3,4,5]
            var re = s.GetSubStringPythonStyle(0);
            Assert.Equal(5, re.Length);

            //从第二个元素(索引为1)到结束: [2,3,4,5]
            re = s.GetSubStringPythonStyle(1);
            Assert.Equal(4, re.Length);

            //从倒数第一个到结束: [5]
            re = s.GetSubStringPythonStyle(-1);
            Assert.Equal(1, re.Length);

            //从头到第二个元素之前: [1]
            re = s.GetSubStringPythonStyle(0, 1);
            Assert.Equal(1, re.Length);

            //从头到第三个元素之前: [1,2]
            re = s.GetSubStringPythonStyle(0, 2);
            Assert.Equal(2, re.Length);

            //从第二个元素到第二个元素之前: []
            re = s.GetSubStringPythonStyle(1, 1);
            Assert.Equal(0, re.Length);

            //从第二个元素到第三个元素之前: [2]
            re = s.GetSubStringPythonStyle(1, 2);
            Assert.Equal(1, re.Length);

            //从第一个元素到第一个元素之前: []
            re = s.GetSubStringPythonStyle(0, 0);
            Assert.Equal(0, re.Length);

            //从第一个元素到倒数第一个元素之前: [1,2,3,4]
            re = s.GetSubStringPythonStyle(0, -1);
            Assert.Equal(4, re.Length);

            //从第二个元素到倒数第二个元素之前[2,3]
            re = s.GetSubStringPythonStyle(1, -2);
            Assert.Equal(2, re.Length);

            //从第四个元素到倒数第三个元素之前: []
            re = s.GetSubStringPythonStyle(3, -3);
            Assert.Equal(0, re.Length);

            //从第三个元素到倒数第三个元素之前: []
            re = s.GetSubStringPythonStyle(2, -3);
            Assert.Equal(0, re.Length);

            //从第二个元素到倒数第三个元素之前: [2]
            re = s.GetSubStringPythonStyle(1, -3);
            Assert.Equal(1, re.Length);

            //从第7个元素到倒数第三个元素之前: []
            re = s.GetSubStringPythonStyle(6, -2);
            Assert.Equal(0, re.Length);

            //从倒数第一个元素到倒数第二个元素之前: []
            re = s.GetSubStringPythonStyle(-1, -2);
            Assert.Equal(0, re.Length);

            //从倒数第二个到倒数第一个元素之前: [4]
            re = s.GetSubStringPythonStyle(-2, -1);
            Assert.Equal(1, re.Length);

            //从倒数第一个到倒数第一个元素之前: []
            re = s.GetSubStringPythonStyle(-1, -1);
            Assert.Equal(0, re.Length);

            //从头到倒数第5个元素之前: []
            re = s.GetSubStringPythonStyle(0, -5);
            Assert.Equal(0, re.Length);

            //从头到倒数第6个元素之前: []
            re = s.GetSubStringPythonStyle(0, -6);
            Assert.Equal(0, re.Length);
        }
    }
}
