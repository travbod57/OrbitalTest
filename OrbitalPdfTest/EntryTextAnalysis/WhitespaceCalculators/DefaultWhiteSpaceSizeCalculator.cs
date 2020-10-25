using OrbitalPdfTest.Helpers;

namespace OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators
{
    public class DefaultWhiteSpaceSizeCalculator : IWhiteSpaceSizeCalculator
    {
        public WhiteSpaceSize Convert(int whiteSpaceLength)
        {
            // whiteSpaceLength important here to determine the whitespace between columns is a standard column gap or tha a column is empty
            if (whiteSpaceLength <= 20)
            {
                return WhiteSpaceSize.Small;
            }
            else
            {
                return WhiteSpaceSize.Large;
            }
        }
    }
}