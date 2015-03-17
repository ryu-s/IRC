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
        string column2Name = "time";
        string column3Name = "command";
        string column4Name = "sender";

        /// <summary>
        /// 
        /// </summary>
        public MyDataTable()
        {
            this.Columns.Add(column1Name);
            this.Columns.Add(column2Name);
            this.Columns.Add(column3Name);
            this.Columns.Add(column4Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public void SetLog(Irc4.Log log)
        {
            var newRow = this.NewRow();
            newRow[column1Name] = log.Text;
            newRow[column2Name] = log.Time.ToString("HH:mm:ss");
            newRow[column3Name] = log.Command;
            newRow[column4Name] = log.SenderInfo.NickName;
            this.Rows.Add(newRow);
            logList.Add(log);
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
