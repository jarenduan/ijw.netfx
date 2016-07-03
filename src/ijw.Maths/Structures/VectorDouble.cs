using System;

namespace ijw.Maths.Structures {
    public class VectorDouble : Vector<double> {
        public VectorDouble(int dimension) : base(dimension) { }

        public static VectorDouble RandomNew(int dimension) {
            Random r = new Random();
            var result = new VectorDouble(dimension);
            for (int i = 0; i < dimension; i++) {
                result.Data[i] = r.NextDouble();
            }
            return result;
        }

        /// <summary>
        /// 向量加法. 此方法不返回新的向量, 将会改变自身的值
        /// </summary>
        /// <param name="addend"></param>
        public void Plus(VectorDouble addend) {
            if (addend.Dimension != this.Dimension) {
                throw new DimensionNotMatchException();
            }
            for (int i = 0; i < this.Data.Length; i++) {
                this.Data[i] += addend.Data[i];
            }
        }

        /// <summary>
        ///  + 操作符重载, 执行向量加法
        ///  注意, 该方法行为像值类型一样, 每次都返回新的浮点向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorDouble operator +(VectorDouble a, VectorDouble b) {
            if (a.Dimension != b.Dimension) {
                throw new DimensionNotMatchException();
            }
            VectorDouble result = new VectorDouble(a.Dimension);
            for (int i = 0; i < result.Data.Length; i++) {
                result.Data[i] = a.Data[i] + b.Data[i];
            }
            return result;
        }

        /// <summary>
        ///  - 操作符重载, 执行向量减法
        ///  注意, 该方法行为像值类型一样, 每次都返回新的浮点向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorDouble operator -(VectorDouble a, VectorDouble b) {
            if (a.Dimension != b.Dimension) {
                throw new DimensionNotMatchException();
            }
            VectorDouble result = new VectorDouble(a.Dimension);
            for (int i = 0; i < result.Data.Length; i++) {
                result.Data[i] = a.Data[i] - b.Data[i];
            }
            return result;
        }

        /// <summary>
        /// 操作符 * 重载, 实现向量和浮点数的乘法
        ///  注意, 该方法行为像值类型一样, 每次都返回新的浮点向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorDouble operator *(VectorDouble a, double b) {
            VectorDouble result = new VectorDouble(a.Dimension);
            for (int i = 0; i < result.Data.Length; i++) {
                result.Data[i] = a.Data[i] * b;
            }
            return result;
        }

        /// <summary>
        /// 操作符 * 重载, 实现向量和浮点数的乘法.
        ///  注意, 该方法行为像值类型一样, 每次都返回新的浮点向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorDouble operator *(double b, VectorDouble a ) {
            return a * b;
        }

        /// <summary>
        /// 操作符 / 重载, 实现向量除以浮点数的除法
        ///  注意, 该方法行为像值类型一样, 每次都返回新的浮点向量
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static VectorDouble operator /(VectorDouble a, double b) {
            VectorDouble result = new VectorDouble(a.Dimension);
            for (int i = 0; i < result.Data.Length; i++) {
                result.Data[i] = a.Data[i] / b;
            }
            return result;
        }
    }
}