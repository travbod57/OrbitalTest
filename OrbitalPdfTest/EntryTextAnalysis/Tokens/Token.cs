using System.Text.RegularExpressions;

namespace OrbitalPdfTest.EntryTextAnalysis.Tokens
{
    public abstract class Token
    {
        public Token(Match match)
        {
            StartIndex = match.Index;
            EndIndex = match.Index + match.Value.Length;
            Value = match.Value;
        }

        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public string Value { get; private set; }
    }
}