using ijw.Collection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.Data.Filter {
    /// <summary>
    /// 表示一个实现Max-Min变换的样本归一化器.
    /// </summary>
    public class MaxMinNormalizer {
        public IIndexable<double> Values { get; protected set; }
        /// <summary>
        /// 目标区间的上限
        /// </summary>
        public double MaxOut { get; protected set; }
        /// <summary>
        /// 目标区间的下限
        /// </summary>
        public double MinOut { get; protected set; }

        /// <summary>
        /// 源区间的上限
        /// </summary>
        public double MaxIn { get; protected set; }
        /// <summary>
        /// 源区间的下限
        /// </summary>
        public double MinIn { get; protected set; }

        /// <summary>
        /// 构造函数, 针对某数组的值情况, 初始化Max-Min归一化器
        /// </summary>
        /// <param name="values">欲归一化的数组</param>
        /// <param name="minOut">目标区间的下限, 默认取0.1</param>
        /// <param name="maxOut">目标区间的上限, 默认取0.9</param>
        public MaxMinNormalizer(IEnumerable<double> values, double minOut = 0.1, double maxOut = 0.9) {
            this.MinOut = minOut;
            this.MaxOut = maxOut;
            this.MaxIn = this.Values.Max();
            this.MinIn = this.Values.Min();
        }

        /// <summary>
        /// 进行归一化
        /// </summary>
        /// <returns>归一化后的向量</returns>
        public IIndexable<double> Normalize() {
            double[] result = new double[this.Values.Count];

            this.Values.ForEachWithIndex((v, i) => {
                result[i] = v.NormalizeMaxMin(this.MinIn, this.MaxIn, this.MinOut, this.MaxOut);
            });

            return new Indexable<double>(result);
        }

        /// <summary>
        /// 把指定值进行反归一化
        /// </summary>
        /// <param name="input">输入向量</param>
        /// <returns>反归一化后的向量</returns>
        public double[] Denormalize(double[] values) {
            double[] result = new double[values.Length];
            for (int i = 0; i < values.Length; i++) {
                result[i] = this.Denormalize(values[i]);
            }
            return result;
        }

        /// <summary>
        /// 把指定值进行反归一化
        /// </summary>
        /// <param name="input">输入向量</param>
        /// <returns>反归一化后的向量</returns>
        public double Denormalize(double value) {
            return value.DenormalizeMaxMin(this.MinIn, this.MaxIn, this.MinOut, this.MaxOut);
        }
    }
}
