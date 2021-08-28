using System.Collections.Generic;

namespace BadAppler.Base
{
    class Sequence<T, Y>
    {   
        /// <summary>
        /// Sequence is a container to store different types of iterable and logically bound data such as frames, chunks etc paired with uniqie classes with metadata needed.
        /// This class can be used on it's own, though it is realy meant to be inherited to avoid generic-hell.
        /// 
        /// Count property *should* be overrided in different use cases, i.e. for FrameSequnece count could return amount of frames in Content, though as for 
        /// ChunkSequence it could return overall amount of frames inside of chunks (instead of amount of chunks in content).
        /// 
        /// </summary>
        public Y Meta { get; init; }

        public virtual int Count { get; protected set; }

        public List<T> Content { get; init; }

        public Sequence() { }

        public Sequence(List<T> content, Y meta)
        {
            Content = content;
            Meta = meta;
        }
    }
}
