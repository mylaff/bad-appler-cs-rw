namespace BadAppler.Encoder.Base
{
    struct PixelDelta<T>
    {
        public int X { get; init; }

        public int Y { get; init; }

        public T Value { get; init; }

        public PixelDelta(int x, int y, T value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}