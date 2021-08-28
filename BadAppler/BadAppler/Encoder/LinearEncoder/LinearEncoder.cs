using BadAppler.Base;
using BadAppler.Encoder.Base;
using System.Collections.Generic;

namespace BadAppler.Encoder.LinearEncoder
{
    class LinearEncoder<T> : IEncoder<T, FrameSequenceMeta, LinearEncoderOptions> where T : struct
    {
        private FrameDeltas<T> Encode(Frame<T> baseFrame, Frame<T> diffFrame)
        {
            FrameDeltas<T> deltas = new FrameDeltas<T>(new List<PixelDelta<T>>());

            for (int y = 0; y < baseFrame.Height; y++)
                for (int x = 0; x < baseFrame.Width; x++)
                    if (!baseFrame.GetPixel(x, y).Equals(diffFrame.GetPixel(x, y)))
                        deltas.AddDelta(new PixelDelta<T>(x, y, diffFrame.GetPixel(x, y)));

            return deltas;
        }

        public EncodedChunkSequence<T, FrameSequenceMeta> EncodeFrames(FrameSequence<T> frames, LinearEncoderOptions options)
        {
            List<FrameDeltas<T>> deltaList = new List<FrameDeltas<T>>();
            List<EncodedChunk<T>> chunkList = new List<EncodedChunk<T>>();

            for (int i = 0; i < frames.Count - 1; i++)
            {
                deltaList.Add(Encode(frames[i], frames[i + 1]));

                if ((i + 1) % options.KeyFrameInterval == 0)
                {
                    chunkList.Add(new EncodedChunk<T>(frames[i + 1 - options.KeyFrameInterval], deltaList)); // Pack chunk as it reached KeyFrameInterval
                    deltaList = new List<FrameDeltas<T>>();
                }
            }

            if (deltaList.Count > 0)
                chunkList.Add(new EncodedChunk<T>(frames[frames.Count - frames.Count % options.KeyFrameInterval], deltaList)); // Packing the rest

            return new EncodedChunkSequence<T, FrameSequenceMeta>(frames.Count, chunkList, frames.Meta);
        }
    }
}
