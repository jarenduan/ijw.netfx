using ijw.Collection;
using ijw.Contract;
using ijw.Data.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace ijw.Data.Filter {
    public static class SampleFilter {
        #region 限制波动过滤
        /// <summary>
        /// 复制样本集，并限制波动进行样本集过滤。用前一个样本+波动幅度代替。
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="diffLimitations">波动最大值绝对值的向量.</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection LimitingDiffFilter(this SampleCollection samples, IEnumerable<double> diffLimitations) {
            samples.ShouldNotBeNullOrEmpty();
            diffLimitations.Count().ShouldEquals(samples.TotalDimension);
            diffLimitations.ShouldEachSatisfy((item) => item > 0);

            SampleCollection result = samples.Clone();

            CollectionHelper.ForEachThree(
                samples.DimensionColumns,
                result.DimensionColumns,
                diffLimitations,
                (srcCol, resultCol, diff) => {
                    LimitingDiffFilter(srcCol, resultCol, diff);
                });

            return result;
        }

        /// <summary>
        /// 限制波动对集合进行过滤。用前一个样本+波动幅度代替。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="diff"></param>
        public static void LimitingDiffFilter(IIndexable<double> values, IIndexable<double> result, double diff) {
            for (int i = 1; i < values.Count; i++) {
                var curr = values[i];
                var prev = values[i - 1];
                result[i] = limitationByDiff(diff, curr, prev);
            }
        }

        private static double limitationByDiff(double diff, double curr, double prev) {
            if ((curr - prev) > diff) {
                return prev + diff;
            }
            else if ((prev - curr) > diff) {
                return prev - diff;
            }
            else {
                return curr;
            }
        }
        #endregion

        #region 限傅过滤
        /// <summary>
        /// 限幅过滤。放弃掉波动过大的样本，用前一个样本代替。
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="diffLimitations">波动最大值绝对值的向量. 都必须大于0</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection LimitingAmplifyFilter(this SampleCollection samples, IEnumerable<double> diffLimitations) {
            samples.ShouldNotBeNullOrEmpty();
            diffLimitations.Count().ShouldEquals(samples.TotalDimension);
            diffLimitations.ShouldEachSatisfy((item) => item > 0);

            SampleCollection result = samples.Clone();
            CollectionHelper.ForEachThree(
                samples.DimensionColumns,
                result.DimensionColumns,
                diffLimitations,
                (srcCol, resultCol, diff) => {
                    LimitingAmplifyFilter(srcCol, resultCol, diff);
                });

            return result;
        }

        /// <summary>
        /// 限幅过滤。放弃掉波动过大的数值，用前一个数值代替。
        /// </summary>
        /// <param name="values">待过滤的数组</param>
        /// <param name="diff">波动最大值绝对值</param>
        /// <returns></returns>
        public static void LimitingAmplifyFilter(IIndexable<double> values, IIndexable<double> result, double diff) {
            for (int i = 1; i < values.Count; i++) {
                var curr = values[i];
                var prev = values[i - 1];
                result[i] = limitingAmplification(diff, curr, prev);
            }
        } 
       

        private static double limitingAmplification(double diff, double curr, double prev) {
            if (Abs(curr - prev) > diff) {
                return prev;
            }
            else {
                return curr;
            }
        }

        #endregion

        #region 中位值过滤
        /// <summary>
        /// 中位值过滤。针对每个维度, 在窗口长度内取中位值.
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="windowLength">窗口长度</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection MedianFilter(this SampleCollection samples, int windowLength) {
            samples.ShouldNotBeNullOrEmpty();
            windowLength.ShouldLargerThan(0);
            windowLength.ShouldBeOdd();
            windowLength.ShouldNotLargerThan(samples.Count());

            int[] medians = CollectionHelper.NewArrayWithValue(samples.TotalDimension, windowLength);

            return MedianFilter(samples, medians);
        }

        /// <summary>
        /// 中位值过滤。窗口长度内取中位值
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="windowLengths">各维度的窗口长度</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection MedianFilter(this SampleCollection samples, int[] windowLengths) {
            samples.ShouldNotBeNullOrEmpty();
            windowLengths.ShouldEachSatisfy((m) => m.ShouldLargerThan(0));
            windowLengths.ShouldEachSatisfy((m) => m.ShouldBeOdd());
            windowLengths.Length.ShouldNotLargerThan(samples.Count());

            SampleCollection result = samples.Clone();
            CollectionHelper.ForEachThree(
                samples.DimensionColumns,
                result.DimensionColumns,
                windowLengths,
                (srcCol, resultCol, winlength) => {
                    LimitingAmplifyFilter(srcCol, resultCol, winlength);
                });

            return result;
        }


        public static void MedianFilter(IIndexable<double> values, IIndexable<double> result, int windowLength) {
            int half = windowLength / 2;
            for (int i = half; i < values.Count - half; i++) {
                double[] window = values.TakePythonStyle(i - half, i + half + 1).OrderBy((e) => e).ToArray();
                result[i] = window[half + 1];
            }
        }
        #endregion

        #region 算术平均值过滤
        /// <summary>
        /// 算术平均值过滤。窗口长度内取平均值
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="windowLength">窗口长度</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection MeanFilter(this SampleCollection samples, int windowLength) {
            samples.ShouldNotBeNullOrEmpty();
            windowLength.ShouldBeNotLessThanZero();
            windowLength.ShouldNotLargerThan(samples.Count());

            int[] medians = CollectionHelper.NewArrayWithValue(samples.TotalDimension, windowLength);

            return MedianFilter(samples, medians);
        }

        /// <summary>
        /// 算术平均值过滤。窗口长度内取平均值
        /// </summary>
        /// <param name="samples">待过滤的样本集</param>
        /// <param name="windowLengths">各维度的窗口长度</param>
        /// <returns>新的样本集</returns>
        public static SampleCollection MeanFilter(this SampleCollection samples, int[] windowLengths) {
            samples.ShouldNotBeNullOrEmpty();
            windowLengths.ShouldEachSatisfy((m) => m.ShouldLargerThan(0) && m.ShouldNotLargerThan(samples.Count()));
            windowLengths.Length.ShouldEquals(samples.TotalDimension);

            SampleCollection result = samples.Clone();
            CollectionHelper.ForEachThree(
               samples.DimensionColumns,
               result.DimensionColumns,
               windowLengths,
               (srcCol, resultCol, winlength) => {
                   LimitingAmplifyFilter(srcCol, resultCol, winlength);
               });
            return result;
        }

        private static void MeanFilter(IIndexable<double> values, IIndexable<double> result, int windowLength) {
            int half = windowLength / 2;
            for (int i = half; i < values.Count - half; i++) {
                result[i] = values.TakePythonStyle(i - half, i + half + 1)
                          .ToArray()
                          .Sum((e) => e) / windowLength;
            }
        }

        #endregion
    }
}
