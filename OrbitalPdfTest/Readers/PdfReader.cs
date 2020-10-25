using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrbitalPdfTest.Readers
{
    public class PdfReader : ReaderBase, IReader
    {
        private const int TABLE_TITLE_LINE_COUNT = 3;
        private const string PAGE_INDICATOR_PATTERN = @"\d+\sof\s\d+";

        public PdfReader(string filePath) : base(filePath)
        {

        }

        public List<string> Read()
        {
            List<string> dataLines = new List<string>();

            try
            {
                using (iText.Kernel.Pdf.PdfReader reader = new iText.Kernel.Pdf.PdfReader(new FileInfo(FilePath)))
                using (PdfDocument pdfDocument = new PdfDocument(reader))
                {
                    for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                    {
                        var page = pdfDocument.GetPage(pageNumber);

                        ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();

                        string unencodedText = PdfTextExtractor.GetTextFromPage(page, strategy);

                        if (TextContainsScheduleEntryHeader(unencodedText))
                        {
                            string encodedText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(unencodedText)));

                            List<string> lines = encodedText.Split('\n').ToList();

                            int entryStartIndex = GetScheduleEntriesStartLineIndex(lines);

                            for (int i = entryStartIndex; i < lines.Count; i++)
                            {
                                string line = lines[i];

                                if (IsPageNumberIndicator(line))
                                {
                                    continue;
                                }

                                if (IsEndOfRegister(line))
                                {
                                    break;
                                }

                                dataLines.Add(line.Trim());
                            }
                        }
                    }
                }
         
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return dataLines;
        }

        private bool IsPageNumberIndicator(string text)
        {
            return Regex.IsMatch(text, PAGE_INDICATOR_PATTERN);
        }

        private int GetScheduleEntriesStartLineIndex(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (TextContainsScheduleEntryHeader(lines[i]))
                {
                    // the actual entries start on the next line AFTER the table title lines
                    return i + TABLE_TITLE_LINE_COUNT + 1;
                }
            }

            return -1;
        }

        private bool TextContainsScheduleEntryHeader(string text)
        {
            return text.IndexOf("Schedule of notices of leases", StringComparison.OrdinalIgnoreCase) > -1;
        }

        private bool IsEndOfRegister(string text)
        {
            return text.IndexOf("End of register", StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}