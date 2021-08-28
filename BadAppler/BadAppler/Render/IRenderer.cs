using BadAppler.Base;
using System.Collections.Generic;
using System.Drawing;

namespace BadAppler.Render
{
    interface IRenderer<T> where T : struct
    {
        public Frame<T> ProcessBitmap(Bitmap bitmap);

        public List<Frame<byte>> ProcessAll(IReadOnlyList<Bitmap> bitmaps);
    }
}
