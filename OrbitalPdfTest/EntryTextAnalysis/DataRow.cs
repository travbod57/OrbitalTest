namespace OrbitalPdfTest.EntryTextAnalysis
{
    public class DataRow
    {
        private string[] columns;

        public DataRow()
        {
            columns = new string[4];
        }

        public void SetColumns(string column1 = null, string column2 = null, string column3 = null, string column4 = null)
        {
            this[0] = !string.IsNullOrWhiteSpace(column1) ? column1.Trim() : null;
            this[1] = !string.IsNullOrWhiteSpace(column2) ? column2.Trim() : null;
            this[2] = !string.IsNullOrWhiteSpace(column3) ? column3.Trim() : null;
            this[3] = !string.IsNullOrWhiteSpace(column4) ? column4.Trim() : null;
        }

        public string this[int i]
        {
            get { return columns[i]; }
            set { columns[i] = value; }
        }
    }
}