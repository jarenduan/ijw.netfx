using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Collection;
using ijw.Contract;

namespace ijw.Data.Samples {
    public class Sample : IEnumerable<double> {
        #region Constructors
        public Sample(double[] data, int outputDimension = 0, string[] fieldNames = null) {
            data.ShouldBeNotNullArgument();
            outputDimension.ShouldNotLargerThan(data.Length);
            this._data = data;
            this.InputDimension = data.Length - outputDimension;
            this.OutputDimension = outputDimension;
            this.Fields = fieldNames;
        }

        public Sample(IEnumerable<double> inputData, IEnumerable<double> outputData = null, IEnumerable<string> fieldNames = null) {
            inputData.ShouldBeNotNullArgument();
            inputData.ShouldNotBeNullOrEmpty();

            this.InputDimension = inputData.Count();
            this.OutputDimension = 0;

            double[] input = inputData.ToArray();
            double[] output = null;
            if (outputData != null) {
                output = outputData.ToArray();
                this.OutputDimension = outputData.Count();
            }
            this._data = new double[this.Dimension];
            for (int i = 0; i < _data.Length; i++) {
                if (i < this.InputDimension) {
                    this._data[i] = input[i];
                }
                else {
                    this._data[i] = output[i - InputDimension];
                }
            }
            this.Fields = fieldNames.ToArray();
        }
        #endregion

        #region Members
        private double[] _data; 
        #endregion

        #region Properties
        public IEnumerable<double> Input {
            get {
                for (int i = 0; i < this.InputDimension; i++) {
                    yield return _data[i];
                }
            }
        }
        public IEnumerable<double> Output {
            get {
                for (int i = this.InputDimension; i < this._data.Length; i++) {
                    yield return _data[i];
                }
            }
        }
        public string[] Fields { get; protected set; }
        public double this[string fieldName] {
            get {
                int i = Fields.IndexOf(fieldName);
                if (i < 0) throw new IndexOutOfRangeException();
                return _data[i];
            }
        }
        public int Dimension => this.InputDimension + this.OutputDimension;
        public int InputDimension { get; protected set; }
        public int OutputDimension { get; protected set; }
        #endregion

        #region Methods
        // public static Sample GenerateEmptySample(int inputDimension, int outputDimension = 0, double defaultValue = 0.0, IEnumerable<string> fieldNames = null) {
        //    var data = CollectionHelper.NewArrayWithValue(inputDimension + outputDimension, defaultValue);
        //    return new Sample(data, inputDimension, fieldNames);
        //}

        public bool IsInputField(string fieldName) {
            int i = Fields.IndexOf(fieldName);
            if (i < 0) return false;
            return i < InputDimension;
        }
        public bool IsOutputField(string fieldName) {
            int i = Fields.IndexOf(fieldName);
            return i >= InputDimension;
        }
        public override string ToString() {
            StringBuilder sb = new StringBuilder("Sample: {");
            var inputFieldNames = this.Fields == null ? null : this.Fields.Take(this.InputDimension);
            fieldsToStringHelper(sb, this.Input, inputFieldNames);
            sb.Append("}, {");
            var outputFieldNames = this.Fields == null ? null : this.Fields.Skip(this.InputDimension);
            fieldsToStringHelper(sb, this.Output, outputFieldNames);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region Operations

        #region Binary Operation Between Samples
        public static Sample BinaryOperationBetweenPairs(Sample s1, Sample s2, Func<double, double, double> binOp) {
            s1.Dimension.ShouldEquals(s2.Dimension);
            s1.InputDimension.ShouldEquals(s2.InputDimension);
            s1.OutputDimension.ShouldEquals(s2.OutputDimension);

            var data = CollectionHelper.ForEachPair(s1, s2, binOp).ToArray();

            return new Sample(data, s1.InputDimension, s1.Fields);
        }

        public static Sample Add(Sample left, Sample right) {
            return BinaryOperationBetweenPairs(left, right, (s1, s2) => s1 + s2);
        }

        public static Sample Minus(Sample left, Sample right) {
            return BinaryOperationBetweenPairs(left, right, (s1, s2) => s1 - s2);
        }

        public static Sample Multiply(Sample left, Sample right) {
            return BinaryOperationBetweenPairs(left, right, (s1, s2) => s1 * s2);
        }

        public static Sample Divide(Sample left, Sample right) {
            return BinaryOperationBetweenPairs(left, right, (s1, s2) => s1 / s2);
        }

        public static Sample operator +(Sample left, Sample right) {
            return Add(left, right);
        }

        public static Sample operator -(Sample left, Sample right) {
            return Minus(left, right);
        }

        public static Sample operator *(Sample left, Sample right) {
            return Multiply(left, right);
        }

        public static Sample operator /(Sample left, Sample right) {
            return Divide(left, right);
        }
        #endregion

        #region Binary Operations With Number

        public static Sample BinaryOperationWithNumber(Sample s, double number, Func<double, double, double> op) {
            var input = (from i in s select op(i, number)).ToArray();
            var output = (from i in s select op(i, number)).ToArray();
            return new Sample(input, output, s.Fields);
        }

        public static Sample Add(Sample left, double right) {
            return BinaryOperationWithNumber(left, right, (s, num) => s + num);
        }
        public static Sample Add(double left, Sample right) {
            return BinaryOperationWithNumber(right, left, (s, num) => s + num);
        }

        public static Sample Minus(Sample left, double right) {
            return BinaryOperationWithNumber(left, right, (s, num) => s - num);
        }
        public static Sample Minus(double left, Sample right) {
            return BinaryOperationWithNumber(right, left, (s, num) => num - s);
        }

        public static Sample Multiply(Sample left, double right) {
            return BinaryOperationWithNumber(left, right, (s, num) => s * num);
        }
        public static Sample Multiply(double left, Sample right) {
            return BinaryOperationWithNumber(right, left, (s, num) => s * num);
        }

        public static Sample Divide(Sample left, double right) {
            return BinaryOperationWithNumber(left, right, (s, num) => s / num);
        }
        public static Sample Divide(double left, Sample right) {
            return BinaryOperationWithNumber(right, left, (s, num) => num / s);
        }

        public static Sample operator +(Sample left, double right) {
            return Add(left, right);
        }
        public static Sample operator +(double left, Sample right) {
            return Add(left, right);
        }

        public static Sample operator -(Sample left, double right) {
            return Minus(left, right);
        }
        public static Sample operator -(double left, Sample right) {
            return Minus(left, right);
        }

        public static Sample operator *(Sample left, double right) {
            return Multiply(left, right);
        }
        public static Sample operator *(double left, Sample right) {
            return Multiply(left, right);
        }

        public static Sample operator /(Sample left, double right) {
            return Divide(left, right);
        }
        public static Sample operator /(double left, Sample right) {
            return Divide(left, right);
        }
        #endregion 

        #endregion

        #region IEnumerable
        public IEnumerator<double> GetEnumerator() {
            foreach (var i in this.Input) {
                yield return i;
            }
            foreach (var o in this.Output) {
                yield return o;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        #endregion

        #region Private Methods

        private void fieldsToStringHelper(StringBuilder sb, IEnumerable<double> fieldValues, IEnumerable<string> fieldNames) {
            if (fieldValues == null)
                sb.Append("No values,");
            else if (fieldNames == null) {
                foreach (var i in fieldValues)
                    sb.Append(i.ToString("F3")).Append(",");
            }
            else {
                CollectionHelper.ForEachPair(fieldValues, fieldNames, (i, j) => {
                    sb.Append("[").Append(j == null ? "no name" : j).Append(": ").Append(i.ToString("F3")).Append("],");
                });
            }
            sb.RemoveLast();
        } 
        #endregion
    }
}