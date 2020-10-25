using OrbitalPdfTest.Parsers.Notes;
using System.Collections.Generic;

namespace OrbitalPdfTest
{
    public class ScheduleEntry
    {
        public ScheduleEntry()
        {
            EntryText = new List<string>();
        }

        public int EntryNumber { get; private set; }

        public List<string> EntryText { get; set; }

        public string RegistrationDateAndPlanRef { get; private set; }

        public string PropertyDescription { get; private set; }

        public string DateOfLeaseAndTerm { get; private set; }
   
        public string LesseeTitle { get; private set; }

        public string Note { get; private set; }

        public void SetEntryNumber(string entryNumber)
        {
            if (int.TryParse(entryNumber, out int number))
            {
                EntryNumber = number;
            }
            else
            {
                EntryNumber = 0;
            }
        }

        public void SetRegistrationDateAndPlanRef(string registrationDateAndPlanRef)
        {
            string entryNumberAsString = EntryNumber.ToString();
            int index = registrationDateAndPlanRef.IndexOf(entryNumberAsString);

            RegistrationDateAndPlanRef = registrationDateAndPlanRef.Remove(index, entryNumberAsString.Length).TrimStart();
        }
        public void SetPropertyDescription(string propertyDescription)
        {
            PropertyDescription = propertyDescription;
        }
        public void SetDateOfLeaseAndTerm(string dateOfLeaseAndTerm)
        {
            DateOfLeaseAndTerm = dateOfLeaseAndTerm;
        }
        public void SetLesseeTitle(string lesseeTitle)
        {
            LesseeTitle = lesseeTitle;
        }

        public void SetNote(INoteExtractor noteExtractor)
        {
            Note = noteExtractor.Extract(EntryText);
        }

        public bool IsValid 
        { 
            get
            {
                return IsSet(EntryNumber)
                    && IsSet(RegistrationDateAndPlanRef)
                    && IsSet(PropertyDescription)
                    && IsSet(DateOfLeaseAndTerm)
                    && IsSet(LesseeTitle);
            }
        }

        private bool IsSet(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        private bool IsSet(int value)
        {
            return value > 0;
        }
    }
}