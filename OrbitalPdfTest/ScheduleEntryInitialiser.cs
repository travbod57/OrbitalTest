using OrbitalPdfTest.Helpers;
using System.Collections.Generic;

namespace OrbitalPdfTest
{
    public class ScheduleEntryInitialiser
    {
        private readonly ScheduleEntry _scheduleEntry;

        public ScheduleEntryInitialiser()
        {
            _scheduleEntry = new ScheduleEntry();
        }

        public int EntryEndLineIndex { get; private set; }

        /// <summary>
        /// Creates a new Schedule Entry record and extract all the lines relevant for the Entry Text
        /// </summary>
        /// <param name="entryStartLineIndex">new header line index</param>
        /// <param name="lines">all the data lines</param>
        /// <returns></returns>
        public ScheduleEntry Initialise(int entryStartLineIndex, List<string> lines)
        {
            EntryEndLineIndex = lines.Count;

            // add in the found header row text
            _scheduleEntry.EntryText.Add(lines[entryStartLineIndex]);

            // starting at the line after the HEADER, add rows of text up until you reach the next HEADER
            for (int i = entryStartLineIndex + 1; i < lines.Count; i++)
            {
                if (ScheduleEntryLine.IsHeader(lines[i]))
                {
                    EntryEndLineIndex = i;
                    break;
                }
                else
                {
                    _scheduleEntry.EntryText.Add(lines[i]);
                }
            }

            return _scheduleEntry;
        }
    }
}