using System;

namespace ijw.Maths.Structures {
    public class Matrix<T>
    {
        public int CountOfX { get; private set; }
        public int CountOfY { get; private set; }


        public T[,] Data { get; set; }

        public Matrix(int countOfX, int countOfY)
        {
            this.CountOfX = countOfX;
            this.CountOfY = countOfY;
            this.Data = new T[countOfX, countOfY];
        }
    }
}
