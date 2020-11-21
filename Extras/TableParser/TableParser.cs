using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Extras.TableParser
{
    public class TableParser
    {
        public static string ConvertToHtml(DataTable dt)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table BORDER ='1'>");
            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<th align = 'center'>{0}</th>", dc.ColumnName);
            }

            foreach (DataRow dr in dt.Rows)

            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    sb.AppendFormat("<td>{0}</ td>", cellValue);
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}
