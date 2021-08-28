using System.Drawing;
using System.Collections.Generic;
using BadAppler.Base;


namespace BadAppler.Render
{
    class ByteRenderer : IRenderer<byte>
    {
        private int widthRatio;

        private int heightRatio;

        private void calculateRatios(Bitmap bitmap) 
        {
            widthRatio = (bitmap.Width - Options.Offset.Left - Options.Offset.Right) / Options.TargetWidth;
            heightRatio = (bitmap.Height - Options.Offset.Top - Options.Offset.Bottom) / Options.TargetHeight;
        }

        public ByteRendererOptions Options { get; init; }

        public Queue<Bitmap> Bitmaps { get; }

        public ByteRenderer(ByteRendererOptions options)
        {
            Options = options;
        }

        public ByteRenderer(Queue<Bitmap> bitmaps, ByteRendererOptions options) : this(options)
        {
            Bitmaps = bitmaps;
            calculateRatios(bitmaps.Peek());
        }

        private byte GetPixelIntensity(int x, int y, Bitmap bitmap)
        {
            Color pixel = bitmap.GetPixel(x, y);

            return (byte)((pixel.R * Options.Weights.R) + (pixel.G * Options.Weights.G) + (pixel.B * Options.Weights.B));
        }

        private byte GetBlockIntensity(int bx, int by, Bitmap bitmap)
        {
            uint intensity = 0, count = 0;

            for (int y = by * heightRatio; y < (by + 1) * heightRatio; y += Options.VerticalSkipAhead)
                for (int x = bx * widthRatio; x < (bx + 1) * widthRatio; x += Options.HorizontalSkipAhead)
                {
                    intensity += GetPixelIntensity(x, y, bitmap);
                    count += 1;
                }

            return (byte)(intensity / count);
        }

        public Frame<byte> ProcessBitmap(Bitmap bitmap)
        {
            Frame<byte> grayScale = new Frame<byte>(Options.TargetWidth, Options.TargetHeight);

            for (int y = 0; y < Options.TargetHeight - 1; y++)
                for (int x = 0; x < Options.TargetWidth - 1; x++)
                    grayScale.SetPixel(x, y, GetBlockIntensity(x, y, bitmap));

            return grayScale;
        }

        public List<Frame<byte>> ProcessAll()
        {
            calculateRatios(Bitmaps.Peek());

            return ProcessAll(Bitmaps);
        }

        public List<Frame<byte>> ProcessAll(Queue<Bitmap> bitmaps)
        {
            List<Frame<byte>> processed = new List<Frame<byte>>();

            while (bitmaps.Count > 0)
            {
                if (Options.CropImages)
                    processed.Add(ProcessBitmap(CropBitmap(bitmaps.Dequeue())));
                else
                    processed.Add(ProcessBitmap(bitmaps.Dequeue()));
            }

            return processed;
        }

        public Bitmap CropBitmap(Bitmap bitmap)
        {
            return bitmap.Clone(new Rectangle(
                Options.Offset.Left,
                Options.Offset.Top, 
                bitmap.Width - Options.Offset.Left - Options.Offset.Right,
                bitmap.Height - Options.Offset.Top - Options.Offset.Bottom
            ), bitmap.PixelFormat);
        }
    }
}
