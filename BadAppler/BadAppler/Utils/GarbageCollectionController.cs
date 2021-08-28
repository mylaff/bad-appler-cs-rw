using System;


namespace BadAppler.Utils
{
    class GarbageCollectionController
    {
        private uint frequency;

        public uint Frequency
        {
            get => frequency;
            set
            {
                iteration = 0;
                frequency = value;
            }
        }

        private uint iteration;

        public void Reset() => iteration = 0;

        public void Iterate()
        {
            if (++iteration == Frequency)
            {
                GC.Collect();
                iteration = 0;
            }
        }

        public GarbageCollectionController(uint frequency) => Frequency = frequency;
    }
}
