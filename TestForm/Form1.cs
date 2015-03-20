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
        Irc4.IrcManager ircManager = new Irc4.IrcManager();
        Irc4.Server current;
        Irc4.Channel currentChannel;
        Dictionary<Irc4.IInfo, Irc4Control.MyDataTable> tableDic = new Dictionary<Irc4.IInfo, Irc4Control.MyDataTable>();
        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            ircManager.ConnectSuccess += ircManager_ConnectSuccess;
            ircManager.Disconnected += ircManager_Disconnected;
            ircManager.ReceiveEvent += ircManager_ReceiveEvent;
            ircManager.InfoEvent += ircManager_InfoEvent;
            ircManager.ExceptionInfo += ircManager_ExceptionInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var server = comboBox1.SelectedItem as Irc4.Server;
            ChangeCurrent(server);
        }
        private void comboBoxCurrentChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var channel = comboBoxCurrentChannel.SelectedItem as Irc4.Channel;
            if (channel != null)
            {
                txtChannelDisplayName.Text = channel.DisplayName;
                ircDataGridView2.DataSource = tableDic[channel];
                currentChannel = channel;
            }
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

                comboBoxCurrentChannel.Items.Clear();
                foreach (var channel in server.ChannelList)
                {
                    comboBoxCurrentChannel.Items.Add(channel);
                }
                if (comboBoxCurrentChannel.Items.Count > 0)
                    comboBoxCurrentChannel.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        void SetServerTextbox(Irc4.ServerInfo server)
        {
            var b = InvokeRequired;
            txtDisplayName.Text = server.DisplayName;
            txtNickname.Text = server.Nickname;
            txtHost.Text = server.Hostname;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //設定値の読み込み。
            ircManager.Load();
            foreach(Irc4.Server server in ircManager.ServerList)
            {
                AddServer(server);
                //var channelInfo = new Irc4.ChannelInfo();
                //channelInfo.DisplayName = "channame";
                //server.AddChannel(channelInfo);
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
            foreach (var channel in server.ChannelList)
            {
                AddChannel(channel);
            }
        }
        public void AddChannel(Irc4.Channel channel)
        {
            tableDic.Add(channel, new Irc4Control.MyDataTable());
        }
        /// <summary>
        /// 
        /// </summary>
        bool isSavedWhenClosing = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnClosing(CancelEventArgs e)
        {            
            //OnClosing()でawaitをする場合、以下のようにすれば問題無さそう。
            if (!isSavedWhenClosing)
            {
                e.Cancel = true;
                await ircManager.Save();
                isSavedWhenClosing = true;
                this.Close();
            }
            else
            {
                base.OnClosing(e);
            }
        }
        void ircManager_ExceptionInfo(object sender, Irc4.IrcExceptionEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += string.Format("{0} [{1}] {2}",e.date.ToString("HH:mm:ss"), e.serverChannel.DisplayName, e.Message) + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();

                if (e.serverChannel is Irc4.Server)
                {
                    var server = (Irc4.Server)e.serverChannel;
                    btnConnect.Enabled = !server.IsConnected;
                    btnDisconnect.Enabled = server.IsConnected;
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        void ircManager_InfoEvent(object sender, Irc4.IrcInfoEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += string.Format("[{0}] {1}", e.serverChannel.DisplayName, e.Message) + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ircManager_ConnectSuccess(object sender, Irc4.IrcEventArgs e)
        {
            Action action = () =>
            {
                if (e.serverChannel is Irc4.Server)
                {
                    var server = (Irc4.Server)e.serverChannel;
                    if (server == current)
                    {
                        btnConnect.Enabled = !server.IsConnected;
                        btnDisconnect.Enabled = server.IsConnected;
                    }
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        void ircManager_Disconnected(object sender, Irc4.IrcEventArgs e)
        {
            Action action = () =>
            {
                if (e.serverChannel is Irc4.Server)
                {
                    var server = (Irc4.Server)e.serverChannel;
                    if (server == current)
                    {
                        btnConnect.Enabled = !server.IsConnected;
                        btnDisconnect.Enabled = server.IsConnected;
                    }
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        if(ircDataGridView1.Rows.Count > 0)
                            ircDataGridView1.FirstDisplayedScrollingRowIndex = ircDataGridView1.Rows.Count - 1;
                    }
                }
                else if (e.serverChannel is Irc4.Channel)
                {
                    var channel = (Irc4.Channel)e.serverChannel;
                    if (!tableDic.ContainsKey(channel))
                    {
                        AddChannel(channel);
                        comboBoxCurrentChannel.Items.Add(channel);
                    }
                    var dt = tableDic[channel];
                    dt.SetLog(e.log);
                    if (channel == currentChannel)
                    {
                        if (ircDataGridView2.Rows.Count > 0)
                            ircDataGridView2.FirstDisplayedScrollingRowIndex = ircDataGridView2.Rows.Count - 1;
                    }
                }
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
            if (current != null)
            {
                await current.SendCmd(this.textBox2.Text);
                textBox2.Text = "";
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                await current.Disconnect();
            }
            
        }

        private void btnUpdateHost_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                var serverInfo = CreateServerInfo(txtDisplayName.Text, txtHost.Text, txtNickname.Text);
                current.SetInfo(serverInfo);
                for(int i = 0; i < comboBox1.Items.Count ; i++)
                {
                    var item = comboBox1.Items[i];
                    if (item != null && item is Irc4.Server && (Irc4.Server)item == current)
                    {
                        //上手く動くか自信ない。
                        comboBox1.Items.Remove(item);
                        var newIndex = comboBox1.Items.Add(item);
                        comboBox1.SelectedIndex = newIndex;//今変更したアイテムが選択される。
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var displayName = txtDisplayName.Text;
            var host = txtHost.Text;
            var nickname = txtNickname.Text;
            var serverInfo = CreateServerInfo(displayName, host, nickname);
            var server = ircManager.AddServer(serverInfo);
            if(server != null)
                AddServer(server);
        }
        private Irc4.ServerInfo CreateServerInfo(string displayName, string hostname, string nickname)
        {
            var serverInfo = new Irc4.ServerInfo();
            serverInfo.DisplayName = displayName;
            serverInfo.Nickname = nickname;
            serverInfo.Hostname = hostname;
            //面倒だから以下固定値。
            serverInfo.Port = 6667;
            serverInfo.CodePage = Encoding.UTF8.CodePage;
            serverInfo.Realname = "Irc Test";
            serverInfo.Username = nickname;
            serverInfo.Password = "";
            return serverInfo;
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

        private async void btnJoin_Click(object sender, EventArgs e)
        {
            if (currentChannel != null)
            {
                await currentChannel.Connect();
            }
        }

        private async void btnPart_Click(object sender, EventArgs e)
        {
            if (currentChannel != null)
            {
                await currentChannel.Disconnect();
            }
        }


    }
}
