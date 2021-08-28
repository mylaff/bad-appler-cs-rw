using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using BadAppler.Utils;


namespace BadAppler.Providers
{
    class FolderBitmapSource : IReadOnlyList<Bitmap>
    {
        private List<string> imageFiles;

        private GarbageCollectionController gcController;

        public Bitmap this[int index] => getBitmapFromPath(imageFiles[index]);

        public int Count => imageFiles.Count;

        public static Bitmap getBitmapFromPath(string path)
        {
            if (!File.Exists(path))
                throw new Exception("Unable to get ordered image as folder was corrupted");

            return new Bitmap(Image.FromFile(path));
        }

        public IEnumerator<Bitmap> GetEnumerator() => new FolderBitmapSourceEnumerator(imageFiles.GetEnumerator(), gcController);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private bool isSupportedFile(string filePath)
        {
            string[] supportedTypes = { ".png", ".jpg" };
            string extension = Path.GetExtension(filePath);

            return supportedTypes.Any(ext => extension == ext);
        }

        public FolderBitmapSource(string folderPath, GarbageCollectionController gcControll)
        {
            imageFiles = Directory.GetFiles(folderPath).Where(f => isSupportedFile(f)).ToList();
            gcController = gcControll;
        }

        public FolderBitmapSource(List<string> imageFiles, GarbageCollectionController gcControll)
        {
            this.imageFiles = imageFiles;
            gcController = gcControll;
        }
    }
}
