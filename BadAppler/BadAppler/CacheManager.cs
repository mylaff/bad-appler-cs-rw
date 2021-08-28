using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BadAppler.Base;
using BadAppler.Encoder.Base;

namespace BadAppler
{
    [Serializable]
    class Cached<T, Y> 
    {
        public string MusicFile { get; set; }

        public Sequence<T, Y> Sequence { get; set; } // Not sure that shadowing will be good here, but for now that doesn't really matter
    }

    class CachedFrames<T> : Cached<Frame<T>, FrameSequenceMeta> where T : struct
    {
        public new FrameSequence<T> Sequence { get; set; } 
    }

    class CachedEncoded<T, Y> : Cached<EncodedChunk<T>, Y> where T : struct
    {
        public new EncodedChunkSequence<T, Y> Sequence { get; set; }
    }

    class CacheManager
    {
        public static CachedFrames<byte> LoadRaw(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            CachedFrames<byte> rawFile;

            using (FileStream fs = new FileStream(path, FileMode.Open))
                rawFile = (CachedFrames<byte>)formatter.Deserialize(fs);

            return rawFile;
        }

        public static CachedFrames<char> LoadTranslated(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            CachedFrames<char> rawFile;

            using (FileStream fs = new FileStream(path, FileMode.Open))
                rawFile = (CachedFrames<char>)formatter.Deserialize(fs);

            return rawFile;
        }

        public static CachedEncoded<char, FrameSequenceMeta> LoadEncoded(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            CachedEncoded<char, FrameSequenceMeta> rawFile;

            using (FileStream fs = new FileStream(path, FileMode.Open))
                rawFile = (CachedEncoded<char, FrameSequenceMeta>)formatter.Deserialize(fs);

            return rawFile;
        }
    }
}
