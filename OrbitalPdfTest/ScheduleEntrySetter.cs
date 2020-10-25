using OrbitalPdfTest.Helpers;
using OrbitalPdfTest.Parsers.Notes;

namespace OrbitalPdfTest
{
    public class ScheduleEntrySetter
    {
        private ScheduleEntry _scheduleEntry;

        public ScheduleEntrySetter(ScheduleEntry scheduleEntry)
        {
            _scheduleEntry = scheduleEntry;
        }

        public void Process(INoteExtractor noteExtractor, string[] columns)
        {
            if (columns != null && columns.Length == 4)
            {
                _scheduleEntry.SetEntryNumber(ScheduleEntryLine.GetEntryNumber(columns[0]));
                _scheduleEntry.SetRegistrationDateAndPlanRef(columns[0]);
                _scheduleEntry.SetPropertyDescription(columns[1]);
                _scheduleEntry.SetDateOfLeaseAndTerm(columns[2]);
                _scheduleEntry.SetLesseeTitle(columns[3]);
                _scheduleEntry.SetNote(noteExtractor);
            }
        }
    }
}