using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irc4Control
{
    public class IrcDataGridView : DataGridView
    {
        
        public IrcDataGridView()
        {

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right)
            {
                var dt = this.DataSource;
                if (dt != null && dt is MyDataTable)
                {
                    var hitTestInfo = this.HitTest(e.X, e.Y);

                    //右クリックした行のLogを取得する。
                    var myDt = (MyDataTable)dt;
                    var log = myDt.GetLog(hitTestInfo.RowIndex);
                }
            }
        }
    }
}
