using Xunit;
using ijw.Data.Samples;
using ijw.Data.Filter;
using System;

namespace ijw.Data.xTest {
    public class UnitTestSampleFilter {
        [Fact]
        public void TestLimitingDiffFilter() {
            double[] sampleData1 = new double[4] { 1.0, 2.0, 3.0, 1.5 };
            double[] sampleData2 = new double[4] { 1.2, 2.2, 3.2, 1.6 };
            double[] sampleData3 = new double[4] { 1.4, 2.4, 3.4, 1.7 };
            double[] sampleData4 = new double[4] { 1.7, 2.7, 3.6, 1.9 };
            string[] fieldName = new string[4] { "input1", "input2", "output1", "output2" };
            Sample[] samples = new Sample[4]{
                new Sample(sampleData1, 2, fieldName),
                new Sample(sampleData2, 2, fieldName),
                new Sample(sampleData3, 2, fieldName),
                new Sample(sampleData4, 2, fieldName)
            };
            SampleCollection sc = new SampleCollection(samples);

            double[] diff = { 0.2, double.MaxValue, 0.1, 0.3 };

            var filterdSamples = sc.LimitingDiffFilter(diff);

            Assert.Equal(1.0, filterdSamples[0]["input1"]);
            Assert.Equal(2.0, filterdSamples[0]["input2"]);
            Assert.Equal(3.0, filterdSamples[0]["output1"]);
            Assert.Equal(1.5, filterdSamples[0]["output2"]);

            Assert.Equal(1.2, filterdSamples[1]["input1"]);
            Assert.Equal(2.2, filterdSamples[1]["input2"]);
            Assert.Equal(3.1, filterdSamples[1]["output1"]);
            Assert.Equal(1.6, filterdSamples[1]["output2"]);

            Assert.Equal(1.4, filterdSamples[2]["input1"]);
            Assert.Equal(2.4, filterdSamples[2]["input2"]);
            Assert.Equal(3.3, Math.Round(filterdSamples[2]["output1"],2));
            Assert.Equal(1.7, filterdSamples[2]["output2"]);

            Assert.Equal(1.6, Math.Round(filterdSamples[3]["input1"],2));
            Assert.Equal(2.7, filterdSamples[3]["input2"]);
            Assert.Equal(3.5, filterdSamples[3]["output1"]);
            Assert.Equal(1.9, filterdSamples[3]["output2"]);
        }
    }
}
