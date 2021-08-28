using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using BadAppler.Utils;


namespace BadAppler.Providers
{
    class FolderBitmapSourceEnumerator : IEnumerator<Bitmap>
    {
        public Bitmap Current => FolderBitmapSource.getBitmapFromPath(pathEnumerator.Current);

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            gcController.Iterate();

            return pathEnumerator.MoveNext();
        }

        public void Reset()
        {
            pathEnumerator.Reset();
            gcController.Reset();
        }

        private IEnumerator<string> pathEnumerator;

        private GarbageCollectionController gcController;

        public FolderBitmapSourceEnumerator(IEnumerator<string> pathEnum, GarbageCollectionController gcControll)
        {
            pathEnumerator = pathEnum;
            gcController = gcControll;
        }
    }
}
