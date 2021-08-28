using System;
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

        public IReadOnlyList<Bitmap> Bitmaps { get; }

        public ByteRenderer(ByteRendererOptions options)
        {
            Options = options;
        }

        public ByteRenderer(IReadOnlyList<Bitmap> bitmaps, ByteRendererOptions options) : this(options)
        {
            Bitmaps = bitmaps;
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
            return ProcessAll(Bitmaps);
        }

        public List<Frame<byte>> ProcessAll(IReadOnlyList<Bitmap> bitmaps)
        {
            if (bitmaps.Count == 0)
                throw new Exception("Empty Bitmap collection was given to the renderer.");

            calculateRatios(bitmaps[0]);

            List<Frame<byte>> processed = new List<Frame<byte>>();

            foreach (var bitmap in bitmaps)
            {
                using (bitmap)
                {
                    if (Options.CropImages)
                        processed.Add(ProcessBitmap(CropBitmap(bitmap)));
                    else
                        processed.Add(ProcessBitmap(bitmap));
                }
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
