using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;

namespace OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines
{
    public interface IDataEngineColumnRuleEngine
    {
        DataRow Run(string line, IWhiteSpaceSizeCalculator whiteSpaceCalculator);
    }
}