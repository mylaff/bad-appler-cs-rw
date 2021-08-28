using System;

namespace BadAppler.Base
{
    [Serializable]
    class Frame<T> where T : struct
    {
        private T[,] _frame;

        public int Width { get; }

        public int Height { get; }

        public void SetPixel(int x, int y, T value) => _frame[y, x] = value;

        public T GetPixel(int x, int y) => _frame[y, x];

        public Frame(int width, int height) 
        {
            Width = width;
            Height = height;
            _frame = new T[Height, Width];
        }
    }
}
