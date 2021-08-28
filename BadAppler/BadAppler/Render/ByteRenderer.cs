using System;
using System.Collections.Generic;
using System.Drawing; 


namespace BadAppler.Render
{
    class ByteRendererOptions
    { 
        public class ColorWeights
        {
            public float R { get; set; } = 0.3F;

            public float G { get; set; } = 0.59F;

            public float B { get; set; } = 0.11F;
        }

        public struct FrameOffset
        {
            public int Top { get; set; }

            public int Right { get; set; }

            public int Bottom { get; set; }

            public int Left { get; set; }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool CropImages { get; set; }

        public FrameOffset Offset { get; set; }

        public ColorWeights Weights { get; set; }

        public int WidthRatio { get; set; }

        public int HeightRatio { get; set; }

        public int VerticalSkipAhead { get; set; }

        public int HorizontalSkipAhead { get; set; }
    }

    class ByteRenderer
    {
        public ByteRendererOptions Options { get; init; }

        private Queue<Bitmap> Bitmaps { get; }

        public ByteRenderer(Queue<Bitmap> bitmaps, ByteRendererOptions options)
        {
            Options = options;
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

            for (int y = by * Options.HeightRatio; y < (by + 1) * Options.HeightRatio; y += Options.VerticalSkipAhead)
                for (int x = bx * Options.WidthRatio; x < (bx + 1) * Options.WidthRatio; x += Options.HorizontalSkipAhead)
                {
                    intensity += GetPixelIntensity(x, y, bitmap);
                    count += 1;
                }

            return (byte)(intensity / count);
        }

        public Frame<byte> ProcessBitmap(Bitmap bitmap)
        {
            Frame<byte> grayScale = new Frame<byte>(Options.Width, Options.Height);

            for (int y = 0; y < Options.Height - 1; y++)
                for (int x = 0; x < Options.Width - 1; x++)
                    grayScale.SetPixel(x, y, GetBlockIntensity(x, y, bitmap));

            return grayScale;
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
