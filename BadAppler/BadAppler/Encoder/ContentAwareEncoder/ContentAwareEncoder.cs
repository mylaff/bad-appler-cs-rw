using BadAppler.Base;
using BadAppler.Encoder;
using BadAppler.Encoder.Base;
using System.Collections.Generic;

namespace BadAppler.Encoder.ContentAwareEncoder
{
    class ContentAwareEncoder<T> : IEncoder<T, FrameSequenceMeta, ContentAwareEncoderOptions> where T : struct
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

        public EncodedChunkSequence<T, FrameSequenceMeta> EncodeFrames(FrameSequence<T> frames, ContentAwareEncoderOptions options)
        {
            int square = frames.Meta.Width * frames.Meta.Height;
            int frameCounter = 0;

            Frame<T> keyFrame = frames[0];
            FrameDeltas<T> deltas;
            EncodedChunk<T> chunk;

            List<FrameDeltas<T>> deltaList = new List<FrameDeltas<T>>();
            List<EncodedChunk<T>> chunkList = new List<EncodedChunk<T>>();

            for (; frameCounter < frames.Count - 1; frameCounter++)
            {
                deltas = Encode(frames[frameCounter], frames[frameCounter + 1]);

                if ((double)deltas.Count / square > options.Threshold)
                {
                    chunk = new EncodedChunk<T>(keyFrame, deltaList);
                    chunkList.Add(chunk);

                    keyFrame = frames[frameCounter + 1];
                    deltaList = new List<FrameDeltas<T>>();
                }
                else
                    deltaList.Add(deltas);
            }

            chunk = new EncodedChunk<T>(keyFrame, deltaList);
            chunkList.Add(chunk);

            return new EncodedChunkSequence<T, FrameSequenceMeta>(frameCounter, chunkList, frames.Meta);
        }
    }
}
