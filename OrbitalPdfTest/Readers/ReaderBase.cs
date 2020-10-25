using System;
using System.IO;

namespace OrbitalPdfTest.Readers
{
    public abstract class ReaderBase
    {
        public ReaderBase(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"A valid file path was not supplied");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"There is no file at the supplied location: { filePath }");
            }

            FilePath = filePath;
        }

        protected string FilePath { get; private set; }
    }
}