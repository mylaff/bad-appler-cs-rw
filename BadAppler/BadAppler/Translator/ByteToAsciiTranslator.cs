using BadAppler.Render;
using System.Collections.Generic;

namespace BadAppler.Translator
{
    class ByteToAsciiTranslator : ITranslator<char, byte>
    {
        static private char solveAscii(byte luminosity)
        {
            if (luminosity == 0)
                return ' ';
            else if (0 < luminosity && luminosity <= 30)
                return '.';
            else if (30 < luminosity && luminosity <= 80)
                return ';';
            else if (80 < luminosity && luminosity <= 100)
                return '/';
            else if(100 < luminosity && luminosity <= 140)
                return '%';
            else if (140 < luminosity && luminosity <= 170)
                return '&';
            else if (170 < luminosity && luminosity <= 210)
                return '@';
            return '#';
        }

        private Frame<char> frameToAscii(Frame<byte> frame) 
        {
            Frame<char> asciiFrame = new Frame<char>(frame.Width, frame.Height);

            for (int y = 0; y < frame.Height; y++)
                for (int x = 0; x < frame.Width; x++)
                    asciiFrame.SetPixel(x, y, solveAscii(frame.GetPixel(x, y)));

            return asciiFrame;
        }

        public List<Frame<char>> Translate(List<Frame<byte>> frames, object settings)
        {
            List<Frame<char>> translated = new List<Frame<char>>();

            foreach (Frame<byte> frame in frames)
                translated.Add(frameToAscii(frame));

            return translated;
        }
    }
}
