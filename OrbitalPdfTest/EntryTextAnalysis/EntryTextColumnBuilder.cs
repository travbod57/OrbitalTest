using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Parsers.Notes;
using System.Collections.Generic;

namespace OrbitalPdfTest.EntryTextAnalysis
{
    public partial class EntryTextColumnBuilder
    {
        private readonly IWhiteSpaceSizeCalculator _whiteSpaceCalculator;
        private readonly INoteExtractor _noteExtractor;
        private readonly IDataEngineColumnRuleEngine _dataEngineColumnRuleEngine;

        public EntryTextColumnBuilder(IDataEngineColumnRuleEngine dataEngineColumnRuleEngine, IWhiteSpaceSizeCalculator whiteSpaceCalculator, INoteExtractor noteExtractor)
        {
            _dataEngineColumnRuleEngine = dataEngineColumnRuleEngine;
            _whiteSpaceCalculator = whiteSpaceCalculator;
            _noteExtractor = noteExtractor;
        }

        /// <summary>
        /// Change unstructured entry text into a data structure representing the actual text for each Schedule Entry column
        /// </summary>
        /// <param name="entryText"></param>
        /// <returns></returns>
        public string[] Build(List<string> entryText)
        {
            Matrix matrix = new Matrix();

            for (int entryIndex = 0; entryIndex < entryText.Count; entryIndex++)
            {
                string line = entryText[entryIndex];

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (_noteExtractor.IsNotesLine(line))
                {
                    // notes are always at the end of the schedule entry
                    break;
                }

                // Put the unstructured EntryText values into a fixed DataRow structure and add to a matrix
                DataRow dataRow = _dataEngineColumnRuleEngine.Run(line, _whiteSpaceCalculator);

                if (dataRow != null)
                {
                    matrix.AddRow(dataRow);
                }
                else
                {
                    return null;
                }
            }

            // Sslice vertically through all data rows to produce columns and append the contents of each column togetther
            return matrix.PivotToColumns();
        }
    }
}