using NUnit.Framework;
using FluentAssertions;
using OrbitalPdfTest;
using OrbitalPdfTest.Parsers.Notes;
using System.Collections.Generic;

namespace UnitTests
{
    public class DefaultNoteExtractorTests
    {
        [Test]
        public void DefaultNoteExtractor_Extract_SingleLineNote_HasNote()
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

            DefaultNoteExtractor noteExtractor = new DefaultNoteExtractor();

            string note = noteExtractor.Extract(scheduleEntry.EntryText);

            note.Should().Be("This is a note", "because the note spans 1 line only");
        }

        [Test]
        public void ScheduleEntrySetter_Process_MultipleLineNote_HasNote()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009",
                    "NOTE: This is a note",
                    "that spans two lines"
                }
            };

            DefaultNoteExtractor noteExtractor = new DefaultNoteExtractor();

            string note = noteExtractor.Extract(scheduleEntry.EntryText);

            note.Should().Be("This is a note that spans two lines", "because the note spans 2 lines");
        }

        [Test]
        public void ScheduleEntrySetter_Process_NoNote()
        {
            ScheduleEntry scheduleEntry = new ScheduleEntry()
            {
                EntryText = new List<string>()
                {
                    "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                    "tinted blue     Floor)                        99 years from",
                    "(part of)                                     23.1.2009"
                }
            };

            DefaultNoteExtractor noteExtractor = new DefaultNoteExtractor();

            string note = noteExtractor.Extract(scheduleEntry.EntryText);

            note.Should().BeNull("because there is no note");
        }
    }
}