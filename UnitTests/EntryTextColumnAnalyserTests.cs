using NUnit.Framework;
using OrbitalPdfTest;
using OrbitalPdfTest.EntryTextAnalysis;
using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Parsers.Notes;
using System.Collections.Generic;

namespace UnitTests
{
    public class EntryTextColumnAnalyserTests
    {
        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums1234_Valid()
        {
            List<string> lines = new List<string>()
            {
                "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("1 28.01.2009", columns[0]);
            Assert.AreEqual("Transformer Chamber (Ground", columns[1]);
            Assert.AreEqual("23.01.2009", columns[2]);
            Assert.AreEqual("EGL551039", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums123_Valid()
        {
            List<string> lines = new List<string>()
            {
                "tinted blue     Floor)                        99 years from"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("tinted blue", columns[0]);
            Assert.AreEqual("Floor)", columns[1]);
            Assert.AreEqual("99 years from", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums12_Valid()
        {
            List<string> lines = new List<string>()
            {
                "tinted blue     Floor)"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("tinted blue", columns[0]);
            Assert.AreEqual("Floor)", columns[1]);
            Assert.AreEqual("", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums1_Valid()
        {
            List<string> lines = new List<string>()
            {
                "blue (part of)"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("blue (part of)", columns[0]);
            Assert.AreEqual("", columns[1]);
            Assert.AreEqual("", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums23_Valid()
        {
            List<string> lines = new List<string>()
            {
                "                Floor)                        99 years from"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("", columns[0]);
            Assert.AreEqual("Floor)", columns[1]);
            Assert.AreEqual("99 years from", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums3_Valid()
        {
            List<string> lines = new List<string>()
            {
                "                                              99 years from"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("", columns[0]);
            Assert.AreEqual("", columns[1]);
            Assert.AreEqual("99 years from", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums13_Valid()
        {
            List<string> lines = new List<string>()
            {
                "(part of)                                     23.1.2009"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("(part of)", columns[0]);
            Assert.AreEqual("", columns[1]);
            Assert.AreEqual("23.1.2009", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums2_Valid()
        {
            List<string> lines = new List<string>()
            {
                "                Floor)"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.AreEqual("", columns[0]);
            Assert.AreEqual("Floor)", columns[1]);
            Assert.AreEqual("", columns[2]);
            Assert.AreEqual("", columns[3]);
        }

        [Test]
        public void Analyse_DefaultDataColumnRuleEngine_Colums12345_Invalid()
        {
            List<string> lines = new List<string>()
            {
                "1 28.01.2009      Transformer     Chamber (Ground   23.01.2009      EGL551039"
            };

            ScheduleEntry scheduleEntry = new ScheduleEntry
            {
                EntryText = lines
            };

            EntryTextColumnBuilder builder = new EntryTextColumnBuilder(new DefaultDataColumnRuleEngine(), new DefaultWhiteSpaceSizeCalculator(), new DefaultNoteExtractor());
            string[] columns = builder.Build(scheduleEntry.EntryText);

            Assert.IsNull(columns);
            Assert.IsFalse(scheduleEntry.IsValid);
        }

    }
}