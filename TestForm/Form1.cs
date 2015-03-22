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
        Irc4.ISec currentServer_;
        Irc4.ISec currentServer
        {
            get
            {
                var s = (currentServer_ == null) ? "null" : currentServer_.DisplayName;
                toolStripStatusLabel1.Text = "server:" + s;
                return currentServer_;
            }
            set
            {
                currentServer_ = value;
            }
        }
        Irc4.ISec currentChannelInterface_;
        Irc4.ISec currentChannelInterface
        {
            get
            {
                return currentChannelInterface_;
            }
            set
            {
                var s = (currentChannelInterface_ == null) ? "null" : currentChannelInterface_.DisplayName;
                toolStripStatusLabel2.Text = "channel:" + s;
                currentChannelInterface_ = value;
            }
        }
        Dictionary<Irc4.ISec, Irc4Control.MyDataTable> tableDic2 = new Dictionary<Irc4.ISec, Irc4Control.MyDataTable>();
        public Form1()
        {
            InitializeComponent();
            
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            Irc4.MessageHandler.ExceptionOccured += ExceptionHandler_ExceptionOccured;
            Irc4.MessageHandler.MessageEvent += MessageHandler_MessageEvent;

            Irc4.IrcManager.Instance.ConnectSuccess += ircManager_ConnectSuccess;
            Irc4.IrcManager.Instance.Disconnected += ircManager_Disconnected;
            Irc4.IrcManager.Instance.ReceiveEvent += ircManager_ReceiveEvent;
            Irc4.IrcManager.Instance.InfoEvent += ircManager_InfoEvent;
            Irc4.IrcManager.Instance.ExceptionInfo += ircManager_ExceptionInfo;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var server1 = comboBox1.SelectedItem as Irc4.ISec;
            ChangeCurrent(server1);
        }
        private void comboBoxCurrentChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var channel = comboBoxCurrentChannel.SelectedItem as Irc4.ISec;
            ChangeCurrentChannel(channel);
        }
        void ChangeCurrent(Irc4.ISec server)
        {
            if (server != null && server.Type == Irc4.ServerChannelType.SERVER)
            {
                var info = (Irc4.ServerInfo)server;
                SetServerTextbox(info);
                currentServer = server;
                ircDataGridView1.DataSource = tableDic2[server];
                btnConnect.Enabled = !server.IsConnected;
                btnDisconnect.Enabled = server.IsConnected;

                comboBoxCurrentChannel.Items.Clear();
                if (Irc4.IrcManager.Instance.GetChannelList(server).Count > 0)
                {
                    comboBoxCurrentChannel.Enabled = true;
                    foreach (var channel in Irc4.IrcManager.Instance.GetChannelList(server))
                    {
                        comboBoxCurrentChannel.Items.Add(channel);
                    }
                    SetComboBoxSelectedIndex(comboBoxCurrentChannel, 0);
                }
                else
                {
                    ChangeCurrentChannel(null);
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="index"></param>
        void SetComboBoxSelectedIndex(ComboBox comboBox, int index)
        {
            if (comboBox.Items.Count > 0)
            {
                if (comboBox.Items.Count == 1)
                {
                    //アイテムが一つしか無い場合、コンボボックスのSelectedIndexChangedが起こらないため、必要な処理をする。
                    var item = comboBox.Items[0];
                    var isec = (Irc4.ISec)item;
                    if (isec.Type == Irc4.ServerChannelType.CHANNEL)
                        ChangeCurrentChannel(isec);
                    else
                        ChangeCurrent(isec);
                    comboBox.SelectedIndex = 0;
                    comboBox.Text = item.ToString();
//                    comboBox.SelectedItem = item;
//                    comboBox.SelectedText = item.ToString();
                }
                else
                {
                    comboBox.SelectedIndex = index;
                }
            }
        }
        void ChangeCurrentChannel(Irc4.ISec channel)
        {
            if (channel != null && channel.Type == Irc4.ServerChannelType.CHANNEL)
            {
                var info = (Irc4.ChannelInfo)channel;
                txtChannelDisplayName.Text = channel.DisplayName;
                ircDataGridView2.DataSource = tableDic2[channel];
                currentChannelInterface = channel;
                if (currentServer.IsConnected)
                {
                    btnJoin.Enabled = !channel.IsConnected;
                    btnPart.Enabled = channel.IsConnected;
                }
                else
                {
                    btnJoin.Enabled = false;
                    btnPart.Enabled = false;
                }
            }
            else if (channel == null)
            {
                txtChannelDisplayName.Text = "";
                comboBoxCurrentChannel.Text = "";
                ircDataGridView2.DataSource = null;
                currentChannelInterface = null;
                btnJoin.Enabled = false;
                btnPart.Enabled = false;
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
            Irc4.IrcManager.Instance.Load();

            foreach(Irc4.ISec server in Irc4.IrcManager.Instance.ServerList)
            {
                AddServer(server);
            }
            SetComboBoxSelectedIndex(comboBox1, 0);
            btnCancelCreateNewServer.Enabled = false;
            btnAddServer.Enabled = false;
            btnCancelCreateNewChannel.Enabled = false;
            btnAddChannel.Enabled = false;


            base.OnLoad(e);
        }

        public void AddServer(Irc4.ISec server)
        {
            comboBox1.Items.Add(server);
            tableDic2.Add(server, new Irc4Control.MyDataTable());
            if (currentServer == null)
                currentServer = server;
            foreach (var channel in Irc4.IrcManager.Instance.GetChannelList(server))
            {
                AddChannel(channel);
            }
        }
        public void AddChannel(Irc4.ISec channel)
        {
            var index = comboBoxCurrentChannel.Items.Add(channel);
            tableDic2.Add(channel, new Irc4Control.MyDataTable());
            comboBoxCurrentChannel.SelectedIndex = index;
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
                try
                {
                    await Irc4.IrcManager.Instance.Save();
                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                }
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
                this.textBox1.Text += string.Format("{0} [{1}] {2}",e.date.ToString("HH:mm:ss"), e.IServerChannel.DisplayName, e.Message) + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();

                if (e.IServerChannel.Type == Irc4.ServerChannelType.SERVER)
                {
                    var server = e.IServerChannel;
                    btnConnect.Enabled = !server.IsConnected;
                    btnDisconnect.Enabled = server.IsConnected;
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        void ExceptionHandler_ExceptionOccured(object sender, Irc4.ExceptionOccuredEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += string.Format("{0} [{1}] {2}", e.DateTime.ToString("HH:mm:ss"), sender.ToString(), e.Message) + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
        void MessageHandler_MessageEvent(object sender, Irc4.MessageEventArgs e)
        {
            Action action = () =>
            {
                this.textBox1.Text += string.Format("{0} [{1}] {2}", e.DateTime.ToString("HH:mm:ss"), sender.ToString(), e.Message) + Environment.NewLine;
                this.textBox1.SelectionStart = this.textBox1.TextLength - 1;
                this.textBox1.ScrollToCaret();
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
                this.textBox1.Text += string.Format("[{0}] {1}", e.IServerChannel.DisplayName, e.Message) + Environment.NewLine;
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
                if (e.IServerChannel.Type == Irc4.ServerChannelType.SERVER)
                {
                    if (e.IServerChannel == currentServer)
                    {
                        btnConnect.Enabled = !e.IServerChannel.IsConnected;
                        btnDisconnect.Enabled = e.IServerChannel.IsConnected;
                        btnJoin.Enabled = true;
                    }
                }
                else if(e.IServerChannel.Type == Irc4.ServerChannelType.CHANNEL)
                {
                    if(e.IServerChannel == currentChannelInterface)
                    {
                        btnJoin.Enabled = !e.IServerChannel.IsConnected;
                        btnPart.Enabled = e.IServerChannel.IsConnected;
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
                if (e.IServerChannel.Type == Irc4.ServerChannelType.SERVER)
                {
                    if (e.IServerChannel == currentServer)
                    {
                        btnConnect.Enabled = !e.IServerChannel.IsConnected;
                        btnDisconnect.Enabled = e.IServerChannel.IsConnected;
                        btnJoin.Enabled = false;
                        btnPart.Enabled = false;
                    }
                }
                else if (e.IServerChannel.Type == Irc4.ServerChannelType.CHANNEL)
                {
                    if (e.IServerChannel == currentChannelInterface)
                    {
                        btnJoin.Enabled = !e.IServerChannel.IsConnected;
                        btnPart.Enabled = e.IServerChannel.IsConnected;
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
                if (e.IServerChannel.Type == Irc4.ServerChannelType.SERVER)
                {
                    var server = e.IServerChannel;
                    if(server == currentServer)
                    {
                        if (ircDataGridView1.Rows.Count > 0)
                            ircDataGridView1.FirstDisplayedScrollingRowIndex = ircDataGridView1.Rows.Count - 1;
                    }
                }
                else if(e.IServerChannel.Type == Irc4.ServerChannelType.CHANNEL)
                {
                    var channel = e.IServerChannel;
                    if(!tableDic2.ContainsKey(channel))
                    {
                        AddChannel(channel);
                        
                        if (channel == currentChannelInterface)
                        {
                            if (ircDataGridView2.Rows.Count > 0)
                                ircDataGridView2.FirstDisplayedScrollingRowIndex = ircDataGridView2.Rows.Count - 1;
                        }
                    }
                }
                var dt = tableDic2[e.IServerChannel];
                dt.SetLog(e.log);
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
            if (currentServer != null)
            {
                btnConnect.Enabled = false;
                await currentServer.Connect();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            if (currentServer != null)
            {
                await Irc4.IrcManager.Instance.SendCmd(currentServer, this.textBox2.Text);
                textBox2.Text = "";
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (currentServer != null)
            {
                await currentServer.Disconnect();
            }
            
        }

        private void btnUpdateHost_Click(object sender, EventArgs e)
        {
            if (currentServer != null)
            {
                var serverInfo = CreateServerInfo(txtDisplayName.Text, txtHost.Text, txtNickname.Text);
                Irc4.IrcManager.Instance.SetInfo(currentServer, serverInfo);
                for(int i = 0; i < comboBox1.Items.Count ; i++)
                {
                    var item = comboBox1.Items[i];
                    if (item != null && item is Irc4.ISec && (Irc4.ISec)item == currentServer)
                    {
                        //上手く動くか自信ない。
                        comboBox1.Items.Remove(item);
                        var newIndex = comboBox1.Items.Add(item);
                        comboBox1.SelectedIndex = newIndex;//今変更したアイテムが選択される。
                    }
                }
            }
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
        private Irc4.ChannelInfo CreateChannelInfo(string displayName)
        {
            var channelInfo = new Irc4.ChannelInfo();
            channelInfo.DisplayName = displayName;
            return channelInfo;
        }

        private async void btnJoin_Click(object sender, EventArgs e)
        {
            if (currentChannelInterface != null)
            {
                await currentChannelInterface.Connect();
            }
        }

        private async void btnPart_Click(object sender, EventArgs e)
        {
            if (currentChannelInterface != null)
            {
                await currentChannelInterface.Disconnect();
            }
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
//            currentServer = null;
            txtDisplayName.Text = "";
            txtHost.Text = "";
            txtNickname.Text = "";
        }

        private void btnCancelCreateNewServer_Click(object sender, EventArgs e)
        {
            btnCreateNewServer.Enabled = true;
            btnAddServer.Enabled = false;
            btnCancelCreateNewServer.Enabled = false;
            ChangeCurrent(currentServer);
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
            var server = Irc4.IrcManager.Instance.AddServer(serverInfo);
            if (server != null)
            {                
                btnCreateNewChannel.Enabled = true;
                btnAddChannel.Enabled = false;
                btnCancelCreateNewChannel.Enabled = false;
                AddServer(server);
                ChangeCurrent(server);
            }
        }

        private void btnCreateNewChannel_Click(object sender, EventArgs e)
        {
            btnCreateNewChannel.Enabled = false;
            btnAddChannel.Enabled = true;
            btnCancelCreateNewChannel.Enabled = true;
            txtChannelDisplayName.Text = "";
//            ChangeCurrentChannel(null);
        }

        private void btnAddChannel_Click(object sender, EventArgs e)
        {
            if (currentServer != null)
            {
                var displayName = txtChannelDisplayName.Text;
                var channelInfo = CreateChannelInfo(displayName);

                var currentChan = currentChannelInterface;
                Irc4.ISec channel = Irc4.IrcManager.Instance.AddChannel(currentServer, channelInfo);
                if (channel != null)
                {
                    btnCreateNewChannel.Enabled = true;
                    btnAddChannel.Enabled = false;
                    btnCancelCreateNewChannel.Enabled = false;
                    AddChannel(channel);
                    ChangeCurrentChannel(channel);
                }
                else
                {
                    ChangeCurrentChannel(currentChan);
                }
            }
        }

        private void btnCancelCreateNewChannel_Click(object sender, EventArgs e)
        {
            btnCreateNewChannel.Enabled = true;
            btnAddChannel.Enabled = false;
            btnCancelCreateNewChannel.Enabled = false;
            SetComboBoxSelectedIndex(comboBoxCurrentChannel, 0);
        }


    }
}
