namespace BadAppler.Render
{
    class ByteRendererOptions
    {
        public int TargetWidth { get; set; }

        public int TargetHeight { get; set; }

        public bool CropImages { get; set; } = false;

        public FrameOffset Offset { get; set; } = new FrameOffset();

        public ColorWeights Weights { get; set; } = new ColorWeights();

        public int VerticalSkipAhead { get; set; }

        public int HorizontalSkipAhead { get; set; }
    }
}
