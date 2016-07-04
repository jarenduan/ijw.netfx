using ijw.Collection;
using ijw.Contract;
using ijw.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ijw.Data.Samples {
    /// <summary>
    /// 表示样本的集合, 非线程安全. 此集合始终非空，至少包含一条样本数据。
    /// </summary>
    public class SampleCollection : IEnumerable<Sample> {
        #region Constructors
        protected SampleCollection() { }

        /// <summary>
        /// 从double数组的集合创建一个样本集，样本集将直接引用这些数组，这意味着对样本集的一切编辑将导致这些数组的直接变更。
        /// </summary>
        /// <param name="samples">原始数据数组</param>
        /// <param name="outputDimension">输出维度，可选，默认是0，无输出</param>
        /// <param name="fieldNames">列名，可选</param>
        public SampleCollection(IEnumerable<double[]> samples, int outputDimension = 0, IEnumerable<string> fieldNames = null) {
            samples.ShouldBeNotNullArgument(nameof(samples));
            samples.ShouldNotBeNullOrEmpty();
            int totalDimension = samples.ElementAt(0).Length;
            outputDimension.ShouldNotLargerThan(totalDimension);
            fieldNames.Count().ShouldEquals(totalDimension);

            this.FieldNames = fieldNames.ToArray();
            this.InputDimension = totalDimension - outputDimension;
            this.OutputDimension = outputDimension;

            this._data = new double[samples.Count()][];
            samples.ForEachWithIndex((row, index) => {
                row.Length.ShouldEquals(totalDimension);
                _data[index] = row;
                this._samples.Add(new Sample(row, outputDimension, this.FieldNames));
            });
            initializeDimensionColumns();
        }

        /// <summary>
        /// 从Sample集合samples创建一个样本集，会导致值的复制。创建的样本集不引用samples，二者可以相互更改。
        /// </summary>
        /// <param name="samples">样本的集合</param>
        public SampleCollection(IEnumerable<Sample> samples) {
            samples.ShouldBeNotNullArgument(nameof(samples));
            samples.ShouldNotBeNullOrEmpty();

            var firstSample = samples.ElementAt(0);
            int totalDimension = firstSample.Dimension;
            this.OutputDimension = firstSample.OutputDimension;
            this.InputDimension = firstSample.InputDimension;
            this.FieldNames = firstSample.Fields;

            this._data = new double[samples.Count()][];
            samples.ForEachWithIndex((s, index) => {
                s.Dimension.ShouldEquals(totalDimension);
                s.InputDimension.ShouldEquals(this.InputDimension);
                s.Fields.Count().ShouldEquals(totalDimension);

                //新建一行样本数据
                var line = new double[totalDimension];
                s.ForEachWithIndex((v, jndex) => {
                    line[jndex] = v;
                });
                this._data[index] = line;

                //新建一个样本元素对象，目的是不影响传入的Sample对象
                this._samples.Add(new Sample(line, OutputDimension, this.FieldNames));
            });
            initializeDimensionColumns();
        }

        private void initializeDimensionColumns() {
            for (int i = 0; i < this.TotalDimension; i++) {
                this._dimensionColumns.Add(new SampleCollectionDimensionColumn(this._data, i));
            }
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// 样本输入维度
        /// </summary>
        public int InputDimension { get; protected set; }

        /// <summary>
        /// 样本输出维度
        /// </summary>
        public int OutputDimension { get; protected set; }

        /// <summary>
        /// 样本数据总维度
        /// </summary>
        public int TotalDimension => InputDimension + OutputDimension;

        /// <summary>
        /// 样本总数
        /// </summary>
        public int Count { get { return this._data.Length; } }

        /// <summary>
        /// 样本集合
        /// </summary>
        public IEnumerable<Sample> Samples => this._samples;

        /// <summary>
        /// 列集合
        /// </summary>
        public IEnumerable<SampleCollectionDimensionColumn> DimensionColumns => this._dimensionColumns;

        /// <summary>
        /// 按索引访问样本
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Sample this[int index] => _samples[index];

        /// <summary>
        /// 按列名访问样本列
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public SampleCollectionDimensionColumn this[string fieldname] => _dimensionColumns[getDimensionIndex(fieldname)];
       
        /// <summary>
        /// 字段名称
        /// </summary>
        public string[] FieldNames { get; protected set; }
        #endregion

        #region Members

        /// <summary>
        /// 行视图集合内部存储
        /// </summary>
        protected List<Sample> _samples = new List<Sample>();

        /// <summary>
        /// 列视图集合内部存储
        /// </summary>
        private List<SampleCollectionDimensionColumn> _dimensionColumns = new List<SampleCollectionDimensionColumn>();

        /// <summary>
        /// 内部数据实际存储
        /// </summary>
        protected double[][] _data;
        
        #endregion

        #region Collection Element Maintanence

        /// <summary>
        /// 向集合添加一个样本. 
        /// 添加时会保证样本与集合内样本的输入输出维度一致.
        /// </summary>
        /// <param name="sample">待添加的样本</param>
        protected void Add(Sample sample) {
            if (this._samples.Count != 0 && (sample.InputDimension != this.InputDimension || sample.OutputDimension != this.OutputDimension)) {
                throw new DimensionNotMatchException();
            }
            this._samples.Add(sample);
        }

        /// <summary>
        /// 将样本集划分为两个子集
        /// </summary>
        /// <param name="ratioOfFirstGroup">第一个子集的占比</param>
        /// <param name="ratioOfSecondGroup">第二个子集的占比</param>
        /// <param name="method">切分方法</param>
        /// <param name="firstGroup">第一个子集</param>
        /// <param name="secondGroup">第二个子集</param>
        public void DivideIntoTwo(int ratioOfFirstGroup, int ratioOfSecondGroup, CollectionDividingMethod method, out SampleCollection firstGroup, out SampleCollection secondGroup) {
            List<Sample> samples1, samples2;
            this.DivideByRatioAndMethod(ratioOfFirstGroup, ratioOfSecondGroup, method, out samples1, out samples2);

            firstGroup = new SampleCollection(samples1);
            secondGroup = new SampleCollection(samples2);
        }
       
        #endregion

        #region Methods

        #region IEnumerable Implementation
        public IEnumerator<Sample> GetEnumerator() {
            return this._samples.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this._samples.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// 检查字符串是否是样本集中的字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>是字段名返回真，反之假</returns>
        public bool IsInputField(string fieldName) {
            var index = getDimensionIndex(fieldName);
            if (index < 0) return false;
            return index < InputDimension;
        }

        /// <summary>
        /// 检查字符串是否是样本集中的字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>是字段名返回真，反之假</returns>
        public bool IsOutputField(string fieldName) {
            var index = getDimensionIndex(fieldName);
            if (index < 0) return false;
            return index >= InputDimension;
        }

        /// <summary>
        /// 获取指定索引处的维度列
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>维度列</returns>
        public SampleCollectionDimensionColumn GetDimensionColumnByIndex(int index) {
            return this._dimensionColumns[index];
        }

        public double[][] ToArray() {
            double[][] array = new double[this._data.Length][];
            for (int i = 0; i < array.Length; i++) {
                array[i] = new double[this.TotalDimension];
                for (int j = 0; j < this.TotalDimension; j++) {
                    array[i][j] = this._data[i][j];
                }
            }
            return array;
        }

        public SampleCollection Clone() {
            double[][] cloneData = ToArray();
            return new SampleCollection(cloneData, this.OutputDimension, this.FieldNames);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// 检查字符串是否是样本集中的字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>是字段名返回真，反之假</returns>
        public static SampleCollection LoadFromCSVFile(StreamReader reader, int outputDimension = 0, bool ignoreInvalidLine = false, bool firstLineFieldName = true) {
            SampleCollection sc = new SampleCollection();
            int lineNum = 0;
            string[] fieldNames = null;
            bool validLine;
            int totalDimension = -1;
            foreach (var line in reader.ReadLines()) {
                lineNum++;
                validLine = true;
                var strValues = line.Replace(" ", "").Split(',');
                if (totalDimension == -1) {
                    totalDimension = strValues.Length;
                }
                else {
                    if (strValues.Length != totalDimension) {
                        if (ignoreInvalidLine) {
                            validLine = false;
                            continue;
                        }
                        else {
                            throw new DimensionNotMatchException("Dimension not match in line:" + lineNum.ToString());
                        }
                    }
                }
                if (lineNum == 1 && firstLineFieldName) {
                    fieldNames = strValues;
                    continue;
                }

                var values = new double[totalDimension];
                for (int i = 0; i < strValues.Length; i++) {
                    double value;
                    if (!double.TryParse(strValues[i], out value)) {
                        if (ignoreInvalidLine) {
                            validLine = false;
                            break;
                        }
                        else {
                            throw new Exception("Has non-double value in line:" + lineNum.ToString());
                        }
                    }
                    values[i] = value;
                }
                if (validLine) {
                    sc.Add(new Sample(values, outputDimension, fieldNames));
                }
            }

            return sc;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 检查字符串是否是样本集中的字段名
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>是字段名返回真，反之假</returns>
        private int getDimensionIndex(string fieldname) => this.FieldNames?.IndexOf(fieldname) ?? -1; 
        #endregion

        //#region Normalization

        ///// <summary>
        ///// 是否归一化
        ///// </summary>
        //public bool IsNormalized { get { return Normalizer != null; } }
        ///// <summary>
        ///// 样本归一化器
        ///// </summary>
        //public SampleMaxMinNormalizer Normalizer { get; protected set; }
        ///// <summary>
        ///// 把当前样本集归一化
        ///// </summary>
        ///// <returns>归一化后的新样本集</returns>
        //public SampleCollection Normalize() {
        //    if (this.IsNormalized) 
        //        throw new AlreadyNormalizedExcpetion();
        //    else
        //        this.Normalizer = new SampleMaxMinNormalizer(this);
        //    var n = new SampleCollection(this._sampleData.Select(s => this.Normalizer.Normalize(s)));
        //    n.Normalizer = this.Normalizer;
        //    return n;
        //}

        ///// <summary>
        ///// 把当前样本集反归一化
        ///// </summary>
        ///// <returns>反归一化后的新样本集</returns>
        //public SampleCollection Denormalize(SampleCollection samples) {
        //    if (!samples.IsNormalized ) throw new NonNormalizedException();
        //    var n = new SampleCollection(this._sampleData.Select(s => this.Normalizer.DeNormalize(s)));
        //    n.Normalizer = this.Normalizer;
        //    return n;
        //}

        ///// <summary>
        ///// 针对输入向量中的某一维度的值进行反归一化
        ///// </summary>
        ///// <param name="value">某一维度的输入值</param>
        ///// <param name="index">在输入向量中处于第几维</param>
        ///// <returns></returns>
        //public double DenormalizeInput(double value, int index) {
        //    return value.DenormalizeMaxMin(this.Normalizer.MinInput.ElementAt(index), this.Normalizer.MaxInput.ElementAt(index));
        //}

        ///// <summary>
        ///// 针对输出向量中的某一维度的值进行反归一化
        ///// </summary>
        ///// <param name="value">某一维度的输入值</param>
        ///// <param name="index">在输出向量中处于第几维</param>
        ///// <returns></returns>
        //public double DenormalizeOutput(double value, int index = 0) {
        //    return value.DenormalizeMaxMin(this.Normalizer.MinInput.ElementAt(index), this.Normalizer.MaxOutput.ElementAt(index));
        //}
        //#endregion
    }
}