using OrbitalPdfTest.Configuration;
using OrbitalPdfTest.EntryTextAnalysis;
using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Helpers;
using OrbitalPdfTest.Parsers.Notes;
using OrbitalPdfTest.Readers;
using System.Collections.Generic;

namespace OrbitalPdfTest
{
    public class ScheduleController
    {
        private List<string> _dataLines;
        private readonly IDataEngineColumnRuleEngine _ruleEngine;
        private readonly IWhiteSpaceSizeCalculator _whiteSpaceCalculator;
        private readonly INoteExtractor _noteExtractor;

        public ScheduleController(IConfigurationFacade configuration)
        {
            _ruleEngine = configuration.RuleEngine;
            _whiteSpaceCalculator = configuration.WhiteSpaceCalculator;
            _noteExtractor = configuration.NoteExtractor;

            ScheduleEntries = new List<ScheduleEntry>();
        }

        public void LoadData(IReader reader)
        {
            _dataLines = reader.Read();
        }

        public void CreateScheduleEntries()
        {
            // lopp through all data lines looking for header lines which determine a new Schedule Entry
            for (int index = 0; index < _dataLines.Count; index++)
            {
                if (ScheduleEntryLine.IsHeader(_dataLines[index]))
                {
                    int entryStartLineIndex = index;

                    // get the lines of text that constitute a schedule entry and intialise to a new ScheduleEntry
                    ScheduleEntryInitialiser scheduleEntryInitialiser = new ScheduleEntryInitialiser();
                    ScheduleEntry scheduleEntry = scheduleEntryInitialiser.Initialise(entryStartLineIndex, _dataLines);

                    // getting EntryText into a fixed column data structure
                    EntryTextColumnBuilder builder = new EntryTextColumnBuilder(_ruleEngine, _whiteSpaceCalculator, _noteExtractor);
                    string[] scheduleEntryColumns = builder.Build(scheduleEntry.EntryText);

                    // setting relevant properties on Schedule Entry by using colum data
                    ScheduleEntrySetter method = new ScheduleEntrySetter(scheduleEntry);
                    method.Process(_noteExtractor, scheduleEntryColumns);

                    // fast forward the loop to the already known next HEADER line
                    index = scheduleEntryInitialiser.EntryEndLineIndex - 1;

                    // add initialised entry to collection
                    ScheduleEntries.Add(scheduleEntry);
                }
            }
        }

        public List<ScheduleEntry> ScheduleEntries { get; }
    }
}