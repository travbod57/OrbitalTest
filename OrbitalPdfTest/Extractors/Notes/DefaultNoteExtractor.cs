using System.Collections.Generic;
using System.Text;

namespace OrbitalPdfTest.Parsers.Notes
{
    public class DefaultNoteExtractor : INoteExtractor
    {
        private const string NOTE_IDENTIFIER_TEXT = "NOTE: ";

        public string Extract(List<string> entryText)
        {
            int noteStartIndex = -1;

            for (int i = 0; i < entryText.Count; i++)
            {
                if (IsNotesLine(entryText[i]))
                {
                    noteStartIndex = i;
                    break;
                }
            }

            if (noteStartIndex == -1)
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();

            for (int i = noteStartIndex; i < entryText.Count; i++)
            {
                builder.AppendFormat("{0} ", entryText[i]);
            }

            return builder.ToString().Replace(NOTE_IDENTIFIER_TEXT, string.Empty).Trim();
        }

        public bool IsNotesLine(string line)
        {
            return !string.IsNullOrWhiteSpace(line) && line.StartsWith(NOTE_IDENTIFIER_TEXT);
        }
    }
}