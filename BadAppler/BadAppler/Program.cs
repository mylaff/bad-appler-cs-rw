using System;
using BadAppler.Utils;
using BadAppler.Render;
using BadAppler.Providers;
using BadAppler.Translator;
using BadAppler.Base;
using BadAppler.Encoder;
using BadAppler.Encoder.Base;
using BadAppler.Encoder.ContentAwareEncoder;
using System.Media;


namespace BadAppler
{
    class Program
    {
        static void SplitVideo(string source, int framerate)
        {
            IVideoHandler videoHandler = new FFMpegVideoHandler("./bin/ffmpeg.exe");
            videoHandler.ToFrames(source, "./processed/", new FrameSequenceMeta { FrameRate = framerate });
        }

        static void ExtractAudio(string source, string output)
        {
            IVideoHandler videoHandler = new FFMpegVideoHandler("./bin/ffmpeg.exe");
            videoHandler.ExtractAudio(source, output);
        }

        static FrameSequence<char> Translate(FrameSequence<byte> sequence)
        {
            ByteToAsciiTranslator translator = new ByteToAsciiTranslator();

            return translator.Translate(sequence, new object());
        }
        static FrameSequence<byte> RenderFolder(string path, int framerate, int height=56, int width=90)
        {
            ByteRendererOptions byteRendererOptions = new ByteRendererOptions()
            {
                TargetHeight = height,
                TargetWidth = width,
                VerticalSkipAhead = 5,
                HorizontalSkipAhead = 5
            };

            // Setting up pipeline
            ByteRenderer byteRenderer = new ByteRenderer(byteRendererOptions);
            FolderBitmapSource provider = new FolderBitmapSource("./processed/", new GarbageCollectionController(100));
            

            var rendered = byteRenderer.ProcessAll(provider);
            
            FrameSequence<byte> sequence = new FrameSequence<byte>(rendered, new FrameSequenceMeta() { 
                Height = byteRendererOptions.TargetHeight, 
                Width = byteRendererOptions.TargetWidth, 
                FrameRate = framerate
            });

            return sequence;
        }

        static EncodedChunkSequence<char, FrameSequenceMeta> EncodeContentAware(FrameSequence<char> sequence, ContentAwareEncoderOptions options)
        {
            ContentAwareEncoder<char> encoder = new ContentAwareEncoder<char>();

            return encoder.EncodeFrames(sequence, options);
        }

        static void DisplayEncoded(EncodedChunkSequence<char, FrameSequenceMeta> sequence, string soundPath="")
        {
            EncodedSequenceDisplay<char> display = new EncodedSequenceDisplay<char>(sequence, false, false);

            if (soundPath != "")
            {
                SoundPlayer sp = new SoundPlayer(soundPath);
                sp.Play();
            }
                
            display.Start();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            if (args[0] == "render") // BadAppler.exe render <framerate> <cachefile> <width> <height>
            {
                int framerate, height, width;
                FrameSequence<byte> rendered;

                if (!int.TryParse(args[1], out framerate))
                    return;

                if (args.Length > 4)
                {
                    if (int.TryParse(args[3], out width) && int.TryParse(args[4], out height))
                        rendered = RenderFolder("./processed", framerate, height, width);
                    else
                        rendered = RenderFolder("./processed", framerate);
                }
                else
                    rendered = RenderFolder("./processed", framerate);

                CacheManager.SaveRaw(rendered, args[2]);
            }
            else if (args[0] == "translate") // BadAppler.exe translate <rawfile> <translatedfile>
            {
                var raw = CacheManager.LoadRaw(args[1]);
                CacheManager.SaveTranslated(Translate(raw.Sequence), args[2]);
            }
            else if (args[0] == "encode") // BadAppler.exe encode <translatedfile> <threshold> <output> <musicsrc>
            {
                float threshold;

                if (!float.TryParse(args[2], out threshold))
                    return;

                var raw = CacheManager.LoadTranslated(args[1]);
                var encoded = EncodeContentAware(raw.Sequence, new ContentAwareEncoderOptions() { Threshold = threshold });
                CacheManager.SaveEncoded(encoded, args[3], args.Length > 4 ? args[4] : "");
            }
            else if (args[0] == "rencode") // BadAppler.exe rencode <framerate> <threshold> <cachefile> <musicsrc>
            {
                int framerate;
                float threshold;

                if (!int.TryParse(args[1], out framerate) || !float.TryParse(args[2], out threshold))
                    return;

                var rendered = RenderFolder("./processed", framerate);
                var translated = Translate(rendered);
                var encoded = EncodeContentAware(translated, new ContentAwareEncoderOptions() { Threshold = threshold });

                CacheManager.SaveEncoded(encoded, args[3], args.Length > 4 ? args[4] : "");
            }
            else if (args[0] == "extract") // BadAppler.exe extract <source> <output>
            {
                ExtractAudio(args[1], args[2]);
            }
            else if (args[0] == "split") // BadAppler.exe split <filename> <framerate>
            {
                int framerate;

                if (int.TryParse(args[2], out framerate))
                    SplitVideo(args[1], framerate);
            }
            else // BadAppler.exe <encodedfile>
            {
                Console.Clear();
                var cache = CacheManager.LoadEncoded(args[0]);
                DisplayEncoded(cache.Sequence, cache.MusicFile);
                Console.Read();
            }
            
            Console.WriteLine("Done!");
        }
    }
}
