using System.Collections.Generic;
using ijw.Collection;
using ijw.Maths.Models;
using System.Linq;
using System;

namespace ijw.Maths.Functions {
    /// <summary>
    ///  目标函数包装器, 其内部包裹了一个更高维的模型, 把输入维度降低, 把输出维度降为1维
    ///  把多目标函数转换成单目标函数, 使调用者认为就是在使用一个单输出的模型. 每个包装器对应一个特定FullInput.
    /// </summary>
    public class TargetFunctionAdapter : ISimpleMathModel {
        protected IMathModel _model;
        protected IEnumerable<int> _targetParameterIndexs;
        private int _inputDimension;

        /// <summary>
        /// 输入维度. 包装后的维度.
        /// </summary>
        public int InputDimension {
            get { return this._inputDimension; }
        }

        /// <summary>
        /// 模型的输入, 缩减维度的之后的输入
        /// </summary>
        public IEnumerable<double> Input {
            get {
                if (_inputDimension == this._model.InputDimension) {
                    return this._model.Input;
                }
                else {
                    return this._model.Input.ElementsAt(this._targetParameterIndexs);
                }
            }
            set {
                if (this._inputDimension == this._model.InputDimension) {
                    this._model.Input = value;
                }
                else {
                    //获取全部维度的值
                    double[] fullinput = this.FullInput.ToArray();
                    //填充其中指定索引的值
                    fullinput.SetValuesForTheIndexes(this._targetParameterIndexs, value);
                    //进行真正的赋值
                    this.FullInput = fullinput;
                }
            }
        }

        /// <summary>
        /// 模型输入, 原维度的输入
        /// </summary>
        public IEnumerable<double> FullInput {
            get {
                return this._model.Input;
            }
            set {
                this._model.Input = value;
            }
        }

        /// <summary>
        /// 输出
        /// </summary>
        public double Output { get; protected set; }

        /// <summary>
        /// 合并多个输出为一个的计算函数委托. 默认算法是直接输出(内部函数单输出)或求和(内部函数多输出).
        /// 如果有自定义的算法或者需要添加罚函数, 请设置此委托.
        /// 输入参数是内部模型计算后的各个维度的输出.
        /// </summary>
        public Func<IEnumerable<double>, double> ToOneResult {get; set;}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actualFunction">被包装的真实函数</param>
        /// <param name="targetVarableIndexes">想最终暴露出来的输入索引,注意，不要重复</param>
        /// <param name="fullinput">完整的输入</param>
        public TargetFunctionAdapter(IMathModel actualFunction, IEnumerable<int> targetVarableIndexes, IEnumerable<double> fullinput) {
            int count = targetVarableIndexes.Count();
            if (count == 0) throw new NotNeedAdaptionException();
            if (count > actualFunction.InputDimension) throw new DimensionNotMatchException();
            this._model = actualFunction;
            this._targetParameterIndexs = targetVarableIndexes;
            this._inputDimension = count;
            this.FullInput = fullinput;
            this.ToOneResult = (outputs) => {
                if (this._model.OutputDimension == 1) {
                    return this._model.Output.First();
                }
                else {
                    return outputs.Sum(); ;
                }
            };
        }

        /// <summary>
        /// 调用内部真实函数进行计算
        /// </summary>
        public void Calculate() {
            this._model.Calculate();
            this.Output = this.ToOneResult(this._model.Output);
        }
    }
}