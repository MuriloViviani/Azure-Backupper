using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure_BackUpper.Model
{
    public class Table
    {
        public List<Column> Columns = new List<Column>();
        public string Table_Name { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("CREATE TABLE ");
            sb.Append(Table_Name);
            sb.Append(" (");

            for (int i = 0; i < Columns.Count-2; i++)
            {
                sb.Append(Columns[i].Column_Name);
                sb.Append(" ");
                sb.Append(Columns[i].Data_Type);
                sb.Append("(");
                sb.Append(Columns[i].Size);
                sb.Append("),");
            }

            sb.Append(Columns.Last().Column_Name);
            sb.Append(" ");
            sb.Append(Columns.Last().Data_Type);
            sb.Append("(");
            sb.Append(Columns.Last().Size);
            sb.Append("));");

            return sb.ToString();
        }
    }
}
