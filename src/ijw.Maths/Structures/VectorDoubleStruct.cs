using System;
using System.Text;

namespace ijw.Maths.Structures {
    public struct VectorDoubleStruct
    {
        public int Dimension;
        public double[] Data;

        public VectorDoubleStruct(int dimension)
        {
            Dimension = dimension;
            Data = new double[Dimension];
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder("{");
            foreach (var item in this.Data) {
                sb.Append(item.ToString());
                sb.Append(", ");
            }
            if(sb.Length >=2)   sb.Remove(sb.Length - 2, 2);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
