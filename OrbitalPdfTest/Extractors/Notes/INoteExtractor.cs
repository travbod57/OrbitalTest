using System.Collections.Generic;

namespace OrbitalPdfTest.Parsers.Notes
{
    public interface INoteExtractor
    {
        string Extract(List<string> entryText);
        bool IsNotesLine(string line);
    }
}