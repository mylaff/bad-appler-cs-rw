using System;
using System.Collections;
using System.Collections.Generic;

namespace BadAppler.Base
{
    [Serializable]
    class Sequence<T, Y> : IEnumerable<T>
    {
        /// <summary>
        /// Sequence is a container to store different types of iterable and logically bound data such as frames, chunks, etc paired with unique metadata classes.
        /// This class can be used on its own, though it is meant to be inherited to avoid generic-hell.
        /// 
        /// Count property *should* be overridden in different use cases, i.e. for FrameSequnece Count could return the number of frames in stored Content, whereas for 
        /// ChunkSequence it could return the overall amount of frames inside of chunks (instead of the number of chunks stored in content).
        /// </summary>
        public Y Meta { get; init; }

        public virtual int Count { get; protected set; }

        public IList<T> Content { get; init; }

        public Sequence() { }

        public Sequence(IList<T> content, Y meta)
        {
            Content = content;
            Meta = meta;
        }

        public IEnumerator<T> GetEnumerator() => Content.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
