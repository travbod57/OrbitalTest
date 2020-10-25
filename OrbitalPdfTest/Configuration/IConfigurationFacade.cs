using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Parsers.Notes;

namespace OrbitalPdfTest.Configuration
{
    public interface IConfigurationFacade
    {
        IDataEngineColumnRuleEngine RuleEngine { get; set; }
        IWhiteSpaceSizeCalculator WhiteSpaceCalculator { get; set; }
        INoteExtractor NoteExtractor { get; set; }
    }
}