using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irc4Control
{
    /// <summary>
    /// 
    /// </summary>
    public class IrcDataGridView : DataGridView
    {
        /// <summary>
        /// 
        /// </summary>
        public IrcDataGridView()
        {
            //ダブルバッファ
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

           this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToResizeRows = false;
            this.RowHeadersVisible = false;

            string column1Name = "time";
            string column2Name = "sender";
            string column3Name = "command";
            string column4Name = "text";
            this.Columns.Add(column1Name, column1Name);
            this.Columns[column1Name].DataPropertyName = column1Name;
            this.Columns.Add(column2Name, column2Name);
            this.Columns[column2Name].DataPropertyName = column2Name;
            this.Columns.Add(column3Name, column3Name);
            this.Columns[column3Name].DataPropertyName = column3Name;
            this.Columns.Add(column4Name, column4Name);
            this.Columns[column4Name].DataPropertyName = column4Name;
            this.Columns[column4Name].Width = 400;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
                    if (hitTestInfo.RowIndex >= 0)
                    {
                        var log = myDt.GetLog(hitTestInfo.RowIndex);

                        var itemNickname = new ToolStripMenuItem();
                        itemNickname.Text = log.SenderInfo.NickName;
                        itemNickname.Enabled = false;

                        var itemText = new ToolStripMenuItem();
                        itemText.Text = "Copy Text";
                        itemText.MouseUp += (sender, arg) =>
                        {
                            Clipboard.SetText(log.Text);
                        };

                        contextMenuStrip1.Items.Clear();
                        contextMenuStrip1.Items.Add(itemNickname);
                        contextMenuStrip1.Items.Add("-");
                        contextMenuStrip1.Items.Add(itemText);
                        contextMenuStrip1.Show(this,e.X, e.Y);
                    }
                }
            }
        }
        private ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
    }
}
