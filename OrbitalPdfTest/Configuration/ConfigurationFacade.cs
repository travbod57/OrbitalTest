using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Parsers.Notes;

namespace OrbitalPdfTest.Configuration
{
    public class ConfigurationFacade : IConfigurationFacade
    {
        public IDataEngineColumnRuleEngine RuleEngine { get; set; }
        public IWhiteSpaceSizeCalculator WhiteSpaceCalculator { get; set; }
        public INoteExtractor NoteExtractor { get; set; }
    }
}