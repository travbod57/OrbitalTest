using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Helpers;
using System.Text.RegularExpressions;

namespace OrbitalPdfTest.EntryTextAnalysis.Tokens
{
    public class WhiteSpaceToken : Token
    {
        public WhiteSpaceToken(Match match, IWhiteSpaceSizeCalculator converter) : base(match)
        {
            WhiteSpaceSize = converter.Convert(match.Length);
        }

        public WhiteSpaceSize WhiteSpaceSize { get; set; }
    }
}