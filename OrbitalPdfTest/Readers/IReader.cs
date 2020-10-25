using System.Collections.Generic;

namespace OrbitalPdfTest.Readers
{
    public interface IReader
    {
        List<string> Read();
    }
}