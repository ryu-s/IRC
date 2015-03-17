using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Irc4Control
{
    /// <summary>
    /// 
    /// </summary>
    public class MyDataTable : DataTable
    {
        List<Irc4.Log> logList = new List<Irc4.Log>();
        string column1Name = "text";

        /// <summary>
        /// 
        /// </summary>
        public MyDataTable()
        {
            this.Columns.Add(column1Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public void SetLog(Irc4.Log log)
        {
            var newRow = this.NewRow();
            newRow[column1Name] = log.Text;
            this.Rows.Add(newRow);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public Irc4.Log GetLog(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= logList.Count)
                throw new ArgumentException("rowIndex");
            return logList[rowIndex];
        }
    }
}
