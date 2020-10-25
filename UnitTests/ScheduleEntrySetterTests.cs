using NUnit.Framework;
using FluentAssertions;
using OrbitalPdfTest;
using OrbitalPdfTest.Parsers.Notes;
using System.Collections.Generic;

namespace UnitTests
{
    public class ScheduleEntrySetterTests
    {
        [Test]
        public void ScheduleEntrySetter_Process_Valid()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009",
                    "NOTE: This is a note"
                }
            };

            string[] columns = new string[4] {
                "1 28.01.2009 tinted blue (part of)",
                "Transformer Chamber (Ground Floor)",
                "23.01.2009 99 years from 23.1.2009",
                "EGL551039"
            };

            ScheduleEntrySetter setter = new ScheduleEntrySetter(scheduleEntry);
            setter.Process(new DefaultNoteExtractor(), columns);

            scheduleEntry.IsValid.Should().BeTrue("because all columns have values and all data is set properly");
            scheduleEntry.EntryNumber.Should().Be(1, "because it is the first number in the first column text");
            scheduleEntry.RegistrationDateAndPlanRef.Should().Be("28.01.2009 tinted blue (part of)", "because it is the first column text minus the entry number");
            scheduleEntry.PropertyDescription.Should().Be("Transformer Chamber (Ground Floor)", "because it is the second column text");
            scheduleEntry.DateOfLeaseAndTerm.Should().Be("23.01.2009 99 years from 23.1.2009", "because it is the third column text");
            scheduleEntry.LesseeTitle.Should().Be("EGL551039", "because it is the fourth column text");
            scheduleEntry.Note.Should().Be("This is a note");
        }

        [Test]
        public void ScheduleEntrySetter_Process_NotEnoughColumns_Inalid()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009",
                    "NOTE: This is a note"
                }
            };

            string[] columns = new string[1] {
                "1 28.01.2009 tinted blue (part of)"
            };

            ScheduleEntrySetter setter = new ScheduleEntrySetter(scheduleEntry);
            setter.Process(new DefaultNoteExtractor(), columns);

            scheduleEntry.IsValid.Should().BeFalse("because there is only one column");
        }

        [Test]
        public void ScheduleEntrySetter_Process_TooManyColumns_Inalid()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009",
                    "NOTE: This is a note"
                }
            };

            string[] columns = new string[5] {
                "1 28.01.2009 tinted blue (part of)",
                "Transformer Chamber (Ground Floor)",
                "23.01.2009 99 years from 23.1.2009",
                "EGL551039",
                "Extra Column"
            };

            ScheduleEntrySetter setter = new ScheduleEntrySetter(scheduleEntry);
            setter.Process(new DefaultNoteExtractor(), columns);

            scheduleEntry.IsValid.Should().BeFalse("because there are too many columns");
        }

        [Test]
        public void ScheduleEntrySetter_Process_ColumnsAreNull_Inalid()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009",
                    "NOTE: This is a note"
                }
            };

            string[] columns = null;

            ScheduleEntrySetter setter = new ScheduleEntrySetter(scheduleEntry);
            setter.Process(new DefaultNoteExtractor(), columns);

            scheduleEntry.IsValid.Should().BeFalse("because the columns are null");
        }
    }
}