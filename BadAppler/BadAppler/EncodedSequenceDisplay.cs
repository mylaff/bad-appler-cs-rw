using System;
using System.Collections.Generic;
using System.Timers;
using BadAppler.Base;
using BadAppler.Encoder.Base;

namespace BadAppler
{
    class EncodedSequenceDisplay<T> where T : struct
    {
        public EncodedChunkSequence<T, FrameSequenceMeta> Sequence { get; private set; }

        private Timer timer;

        private int chunkNumber;

        private int chunkInnerPos;

        private int frameOrder;

        private bool isDrawing;

        private bool clearOnKeyFrame;

        private bool ignoreOverdraw;

        public EncodedSequenceDisplay(EncodedChunkSequence<T, FrameSequenceMeta> sequence, bool clearOnKeyFrame, bool ignoreOverdraw)
        {
            Sequence = sequence;
            this.clearOnKeyFrame = clearOnKeyFrame;
            this.ignoreOverdraw = ignoreOverdraw;

            timer = new Timer(1000 / sequence.Meta.FrameRate);
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            isDrawing = false;
        }

        public void Start()
        {
            Reset();
            timer.Enabled = true;
        }

        public void Stop() => timer.Enabled = false;

        private void DisplayKeyFrame(Frame<T> keyFrame)
        {
            string line = "";
            List<string> lines = new List<string>();

            for (int y = 0; y < keyFrame.Height; y++)
            {
                for (int x = 0; x < keyFrame.Width; x++)
                    line += keyFrame.GetPixel(x, y);

                lines.Add(line);
                line = "";
            }

            foreach (var _line in lines)
                Console.WriteLine(_line);
        }

        private void DisplayIntermediateFrame(FrameDeltas<T> deltas)
        {
            foreach (PixelDelta<T> delta in deltas.Deltas)
            {
                Console.SetCursorPosition(delta.X, delta.Y);
                Console.Write(delta.Value);
            }
        }

        private void DisplayChunk()
        {
            var chunk = Sequence.Content[chunkNumber];
            if (chunkInnerPos == 0)
            {
                if (clearOnKeyFrame)
                    Console.Clear();
                else
                    Console.SetCursorPosition(0, 0);
                DisplayKeyFrame(chunk.KeyFrame);
                chunkInnerPos++;
            }
            else if (chunkInnerPos - 1 < chunk.Deltas.Count)
            {
                DisplayIntermediateFrame(chunk.Deltas[chunkInnerPos - 1]);
                chunkInnerPos++;
            }
            else
            {
                chunkNumber++;
                chunkInnerPos = 0;
                DisplayChunk();
            }
        }

        private void Tick(Object source, ElapsedEventArgs e)
        {
            if (frameOrder == Sequence.Count)
                Stop();

            if (isDrawing && !ignoreOverdraw)
                return;

            Console.Title = $"[Displaying] {frameOrder}/{Sequence.Count}";

            isDrawing = true;

            DisplayChunk();
            frameOrder++;
            isDrawing = false;
        }

        public void Dispose() => timer.Dispose();

        public void Reset()
        {
            isDrawing = false;
            chunkInnerPos = 0;
            chunkNumber = 0;
            frameOrder = 0;
        }
    }
}
