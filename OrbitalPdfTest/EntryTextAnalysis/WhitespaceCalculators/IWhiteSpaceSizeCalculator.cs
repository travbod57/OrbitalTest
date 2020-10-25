using OrbitalPdfTest.Helpers;

namespace OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators
{
    public interface IWhiteSpaceSizeCalculator
    {
        WhiteSpaceSize Convert(int whiteSpaceLength);
    }
}