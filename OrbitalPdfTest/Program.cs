using OrbitalPdfTest.Readers;
using System;
using System.Diagnostics;
using System.Linq;
using OrbitalPdfTest.Configuration;
using OrbitalPdfTest.Parsers.Notes;
using OrbitalPdfTest.EntryTextAnalysis.DataColumnRuleEngines;
using OrbitalPdfTest.EntryTextAnalysis.WhitespaceCalculators;
using System.IO;

namespace OrbitalPdfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Official_Copy_Register_EGL363613.pdf");

            Stopwatch stopWatch = Stopwatch.StartNew();

            // Configuration allows swapping out implementations of parts of the system
            IConfigurationFacade configuration = new ConfigurationFacade()
            {
                RuleEngine = new DefaultDataColumnRuleEngine(),
                NoteExtractor = new DefaultNoteExtractor(),
                WhiteSpaceCalculator = new DefaultWhiteSpaceSizeCalculator()
            };

            ScheduleController controller = new ScheduleController(configuration);
            controller.LoadData(new PdfReader(filePath));
            controller.CreateScheduleEntries();

            stopWatch.Stop();

            // Reporting

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Processed { controller.ScheduleEntries.Count() } entries in { stopWatch.ElapsedMilliseconds }ms");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Valid Entries: { controller.ScheduleEntries.Where(e => e.IsValid).Count() }");

            int invalidEntryCount = controller.ScheduleEntries.Where(e => !e.IsValid).Count();

            if (invalidEntryCount > 0) {

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Total Invalid Entries: { invalidEntryCount }");

                Console.WriteLine();
                Console.WriteLine("===============");
                Console.WriteLine("Invalid Entries");
                Console.WriteLine("===============");
                Console.WriteLine();

                foreach (var scheduleEntry in controller.ScheduleEntries.Where(e => !e.IsValid))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var text in scheduleEntry.EntryText)
                    {
                        Console.WriteLine(text);
                    }
                    
                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }
    }
}