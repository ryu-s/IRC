using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irc4TestForm
{
    public partial class Form1 : Form
    {
        Irc4.Server server;

        Irc4.IrcManager ircManager = new Irc4.IrcManager();
        Irc4.Server current;
        Dictionary<Irc4.Server, Irc4Control.MyDataTable> tableDic = new Dictionary<Irc4.Server, Irc4Control.MyDataTable>();
        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            ircManager.ReceiveEvent += ircManager_ReceiveEvent;
            
            server = new Irc4.Server();
            server.ConnectSuccess += server_ConnectSuccess;
            server.ReceiveEvent += server_ReceiveEvent;
            server.ExceptionInfo += server_ExceptionInfo;
        }



        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var server = comboBox1.SelectedItem as Irc4.Server;
            ChangeCurrent(server);
        }
        void ChangeCurrent(Irc4.Server server)
        {
            if (server != null)
            {
                SetServerTextbox(server);
                current = server;
                ircDataGridView1.DataSource = tableDic[server];
                btnConnect.Enabled = !server.IsConnected;
                btnDisconnect.Enabled = server.IsConnected;
            }
        }
        void SetServerTextbox(Irc4.ServerInfo server)
        {
            var b = InvokeRequired;
            txtDisplayName.Text = server.DisplayName;
            txtNickname.Text = server.Nickname;
            txtHost.Text = server.Hostname;
        }
        protected override void OnLoad(EventArgs e)
        {
            //設定値の読み込み。
            ircManager.Load();
            foreach(Irc4.Server server in ircManager.ServerList)
            {
                AddServer(server);
            }
            btnCancelCreateNewServer.Enabled = false;
            btnAddServer.Enabled = false;


            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            base.OnLoad(e);
        }
        public void AddServer(Irc4.Server server)
        {
            comboBox1.Items.Add(server);
            tableDic.Add(server, new Irc4Control.MyDataTable());
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            ircManager.Save();
            base.OnClosing(e);
        }
        void server_ExceptionInfo(object sender, Irc4.IrcExceptionEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += e.Message + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        void server_ConnectSuccess(object sender, EventArgs e)
        {

        }
        void ircManager_ReceiveEvent(object sender, Irc4.IRCReceiveEventArgs e)
        {
            Action action = () =>
            {
                if(e.serverChannel is Irc4.Server)
                {
                    var server = (Irc4.Server)e.serverChannel;
                    var dt = tableDic[server];
                    dt.SetLog(e.log);
                    if(server == current)
                    {
                        ircDataGridView1.FirstDisplayedScrollingRowIndex = ircDataGridView1.Rows.Count - 1;
                    }                        
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        void server_ReceiveEvent(object sender, Irc4.IRCReceiveEventArgs e)
        {
            Action action = () =>
            {
                if(e.serverChannel == current)
                {

                }
                this.textBox1.Text += e.log.Text + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        /// <summary>
        /// 接続ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                btnConnect.Enabled = false;
                await current.Connect();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            await server.SendCmd(this.textBox2.Text);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await server.Disconnect();
        }

        private void btnUpdateHost_Click(object sender, EventArgs e)
        {
            server.Hostname = txtHost.Text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var serverInfo = new Irc4.ServerInfo();
            serverInfo.DisplayName = txtDisplayName.Text;
            serverInfo.Nickname = txtNickname.Text;
            serverInfo.Hostname = txtHost.Text;
            //面倒だから以下固定値。
            serverInfo.Port = 6667;
            serverInfo.CodePage = Encoding.UTF8.CodePage;
            serverInfo.Realname = "Irc Test";
            serverInfo.Username = txtNickname.Text;
            serverInfo.Password = "";
            var server = ircManager.AddServer(serverInfo);
            AddServer(server);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateNewServer_Click(object sender, EventArgs e)
        {
            btnCreateNewServer.Enabled = false;
            btnCancelCreateNewServer.Enabled = true;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
            btnAddServer.Enabled = true;
            current = null;
            txtDisplayName.Text = "";
            txtHost.Text = "";
            txtNickname.Text = "";
        }

        private void btnCancelCreateNewServer_Click(object sender, EventArgs e)
        {
            btnCreateNewServer.Enabled = true;
            btnAddServer.Enabled = false;
            btnCancelCreateNewServer.Enabled = false;
            if (comboBox1.Items.Count > 0)
            {
                if (comboBox1.Items.Count == 1)
                {
                    //アイテムが一つしか無い場合、コンボボックスのSelectedIndexChangedが起こらないため、必要な処理をする。
                    ChangeCurrent((Irc4.Server)comboBox1.Items[0]);
                } else {
                    comboBox1.SelectedIndex = 0;
                }
            }
        }
    }
}
