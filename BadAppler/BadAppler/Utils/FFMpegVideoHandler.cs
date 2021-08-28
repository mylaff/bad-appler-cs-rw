using System.IO;
using System.Diagnostics;
using BadAppler.Base;

namespace BadAppler.Utils
{
    class FFMpegVideoHandler : IVideoHandler
    {
        private string binaryPath;

        public FFMpegVideoHandler(string binaryPath)
        {
            EnsureFileExist(binaryPath, "ffmpeg.exe");

            this.binaryPath = binaryPath;
        }

        private void ExecuteCommand(string command) => Process.Start(new ProcessStartInfo(binaryPath, command));

        private void EnsureFileExist(string path, string desc="target")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Required {desc} file wasn't found (at: {path})");
        }

        public void ExtractAudio(string source, string output)
        {
            EnsureFileExist(source, "source");
            ExecuteCommand($"-i {source} -q:a 0 -map a {output}");
        }

        /// <TODO>
        /// Do a better workaround than to use FrameSequenceMeta.
        /// </TODO>
        /// <param name="settings">ffmpeg settings. In sake of my laziness I left FrameSequenceMeta class as a class for settings as it contains both resolution and framerate parameters.</param>
        public void ToFrames(string source, string outputFolder, FrameSequenceMeta settings)
        {
            EnsureFileExist(source, "source");

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            ExecuteCommand($"-i {source} -r {settings.FrameRate} {outputFolder}/frame-%07d.png");
        }
    }
}
