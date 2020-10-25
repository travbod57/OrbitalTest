using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbitalPdfTest.EntryTextAnalysis
{
    public class Matrix
    {
        private readonly List<DataRow> _dataRows;

        public Matrix()
        {
            _dataRows = new List<DataRow>();
        }

        public void AddRow(DataRow dataRow)
        {
            _dataRows.Add(dataRow);
        }

        public string[] PivotToColumns()
        {
            string[] columns = new string[4];

            for (int columnIndex = 0; columnIndex < columns.Count(); columnIndex++)
            {
                StringBuilder builder = new StringBuilder();

                foreach (DataRow row in _dataRows)
                {
                    builder.AppendFormat("{0} ", row[columnIndex]);
                }

                columns[columnIndex] = builder.ToString().Trim();
            }

            return columns;
        }
    }
}