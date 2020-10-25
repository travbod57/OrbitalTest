using Moq;
using NUnit.Framework;
using OrbitalPdfTest;
using OrbitalPdfTest.Configuration;
using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using OrbitalPdfTest.Parsers.Notes;
using OrbitalPdfTest.Readers;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;

namespace UnitTests
{
    public class ScheduleControllerTests
    {
        private Mock<IReader> _mockFileReader;
        private List<string> _dataLines;
        private IConfigurationFacade _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationFacade()
            {
                RuleEngine = new DefaultDataColumnRuleEngine(),
                NoteExtractor = new DefaultNoteExtractor(),
                WhiteSpaceCalculator = new DefaultWhiteSpaceSizeCalculator()
            };

            _dataLines = new List<string>();

            _mockFileReader = new Mock<IReader>();
            _mockFileReader.Setup(x => x.Read()).Returns(_dataLines);
        } 

        [Test]
        public void ScheduleController_CreateScheduleEntries_Valid()
        {
            _dataLines.AddRange(new List<string>()
            {
                "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                "tinted blue     Floor)                        99 years from",
                "(part of)                                     23.1.2009",
                "2 09.07.2009      Endeavour House, 47 Cuba      06.07.2009      EGL557357",
                "Edged and       Street, London                125 years from",
                "numbered 2 in                                 1.1.2009",
                "blue (part of)"
            });

            ScheduleController controller = new ScheduleController(_configuration);

            controller.LoadData(_mockFileReader.Object);
            controller.CreateScheduleEntries();

            controller.ScheduleEntries.Count().Should().Be(2, "because there are 2 entries supplied via the data lines");
            controller.ScheduleEntries.Where(e => e.IsValid).Count().Should().Be(2, "because both entries are valid");
        }

        [Test]
        public void ScheduleController_CreateScheduleEntries_OneInvalid()
        {
            _dataLines.AddRange(new List<string>()
            {
                "1 28.01.2009      Transformer Chamber (Ground   23.01.2009      EGL551039",
                "tinted blue     Floor)                        99 years from",
                "(part of)                                     23.1.2009",
                "2 09.07.2009      Endeavour   House, 47 Cuba      06.07.2009      EGL557357",
                "Edged and       Street, London                125 years from",
                "numbered 2 in                                 1.1.2009",
                "blue (part of)"
            });

            ScheduleController controller = new ScheduleController(_configuration);

            controller.LoadData(_mockFileReader.Object);
            controller.CreateScheduleEntries();

            controller.ScheduleEntries.Count().Should().Be(2, "because there are 2 entries supplied via the data lines");
            controller.ScheduleEntries.Where(e => !e.IsValid).Count().Should().Be(1, "because 1 entries is invalid");
            controller.ScheduleEntries.Where(e => e.IsValid).Count().Should().Be(1, "because 1 entries is valid");
        }
    }
}