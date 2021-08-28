using BadAppler.Base;

namespace BadAppler.Utils
{
    interface IVideoHandler
    {
        public void ExtractAudio(string source, string output);

        public void ToFrames(string source, string outputFolder, FrameSequenceMeta settings);
    }
}
