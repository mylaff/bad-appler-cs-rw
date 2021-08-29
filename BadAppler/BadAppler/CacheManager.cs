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

    [Serializable]
    class CachedFrames<T> : Cached<Frame<T>, FrameSequenceMeta> where T : struct
    {
        public new FrameSequence<T> Sequence { get; set; } 
    }

    [Serializable]
    class CachedEncoded<T, Y> : Cached<EncodedChunk<T>, Y> where T : struct
    {
        public new EncodedChunkSequence<T, Y> Sequence { get; set; }
    }

    class CacheManager
    {
        private static T loadCached<T>(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            T rawFile;

            using (FileStream fs = new FileStream(path, FileMode.Open))
                rawFile = (T)formatter.Deserialize(fs);

            return rawFile;
        }

        public static CachedFrames<byte> LoadRaw(string path)
        {
            return loadCached<CachedFrames<byte>>(path);
        }

        public static CachedFrames<char> LoadTranslated(string path)
        {
            return loadCached<CachedFrames<char>>(path);
        }

        public static CachedEncoded<char, FrameSequenceMeta> LoadEncoded(string path)
        {
            return loadCached<CachedEncoded<char, FrameSequenceMeta>>(path);
        }

        private static void saveCache<T, Y>(Cached<T, Y> cache, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path, FileMode.Create))
                formatter.Serialize(fs, cache);
        }

        public static void SaveRaw(FrameSequence<byte> sequence, string outputPath)
        {
            saveCache(new CachedFrames<byte>() { Sequence = sequence, MusicFile = "" }, outputPath);
        }

        public static void SaveTranslated<T>(FrameSequence<T> translated, string outputPath) where T : struct
        {
            saveCache(new CachedFrames<T>() { Sequence = translated, MusicFile = "" }, outputPath);
        }

        public static void SaveEncoded<T, Y>(EncodedChunkSequence<T, Y> encoded, string outputPath, string musicPath="") where T : struct
        {
            saveCache(new CachedEncoded<T, Y>() { Sequence = encoded, MusicFile = musicPath }, outputPath);
        }
    }
}
