using System.Text.RegularExpressions;

namespace OrbitalPdfTest.Helpers
{
    public static class ScheduleEntryLine
    {
        private const string HEADER_IDENTIFIER_PATTERN = @"\d+\s\d{2}.\d{2}.\d{4}";
        private const string ENTRY_NUMBER_PATTERN = @"\d+\b";

        /// <summary>
        /// Matches the start of a new schedule entry e.g. 1 28.01.2009, 2 09.07.2009 or 3 25.09.2009 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsHeader(string line)
        {
            return Regex.IsMatch(line, HEADER_IDENTIFIER_PATTERN);
        }

        public static string GetEntryNumber(string line)
        {
            Match match = Regex.Match(line, ENTRY_NUMBER_PATTERN);

            if (match.Success)
            {
                return match.Value.Trim();
            }

            return string.Empty;
        }
    }
}