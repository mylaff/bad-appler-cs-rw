using System;
using BadAppler.Utils;
using BadAppler.Render;
using BadAppler.Providers;
using BadAppler.Translator;
using BadAppler.Base;
using BadAppler.Encoder.ContentAwareEncoder;

namespace BadAppler
{
    class Program
    {
        static void TestSplit()
        {
            IVideoHandler videoHandler = new FFMpegVideoHandler("./bin/ffmpeg.exe");
            videoHandler.ToFrames("source.mp4", "./processed/", new Base.FrameSequenceMeta { FrameRate = 30 });
        }

        static void Main(string[] args)
        {
            ByteRendererOptions byteRendererOptions = new ByteRendererOptions()
            {
                TargetHeight = 56,
                TargetWidth = 90,
                VerticalSkipAhead = 5,
                HorizontalSkipAhead = 5
            };

            // Preparing console


            // Setting up Renderer and BitmapProvider
            ByteRenderer byteRenderer = new ByteRenderer(byteRendererOptions);
            FolderBitmapSource provider = new FolderBitmapSource("./processed/", new GarbageCollectionController(100));

            // Setting up Translator and Encoder
            ByteToAsciiTranslator translator = new ByteToAsciiTranslator();
            ContentAwareEncoder<char> encoder = new ContentAwareEncoder<char>();

            var rendered = byteRenderer.ProcessAll(provider);
            var tran = translator.Translate(rendered, new object());
            
            // Packing stuff into the FrameSequence
            FrameSequence<char> sequence = new FrameSequence<char>(tran, new FrameSequenceMeta() { Height = 56, Width = 90, FrameRate = 30 });

            var encoded = encoder.EncodeFrames(sequence, new ContentAwareEncoderOptions() { Threshold = 0.3 });

            EncodedSequenceDisplay<char> display = new EncodedSequenceDisplay<char>(encoded, false, false);
            display.Start();

            Console.Read();
            Console.WriteLine("Done!");
        }
    }
}
