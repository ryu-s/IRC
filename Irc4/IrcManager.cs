using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Irc4
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>目標はライブラリ利用側がServerやChannelを直接扱わなくても良いようにすること。何をするにもinterface経由で済むように。</remarks>
    public class IrcManager
    {
        string settingFilePath = System.Environment.CurrentDirectory + @"\Server.config";
        List<Server> serverList = new List<Server>();


        /// <summary>
        /// 接続に成功した。
        /// </summary>
        public event IrcEventHandler ConnectSuccess;
        /// <summary>
        /// 
        /// </summary>
        public event IrcEventHandler Disconnected;
        /// <summary>
        /// 何か受け取った。
        /// </summary>
        public event ReceiveEventHandler ReceiveEvent;
        /// <summary>
        /// 例外情報
        /// </summary>
        public event IrcExceptionHandler ExceptionInfo;
        /// <summary>
        /// 諸々の情報。例外はExceptionInfo。
        /// </summary>
        public event IrcInfoHandler InfoEvent;
        /// <summary>
        /// 
        /// </summary>
        private IrcManager()
        {
        }
        private static IrcManager instance = new IrcManager();
        /// <summary>
        /// 
        /// </summary>
        public static IrcManager Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// 設定を保存する。
        /// </summary>
        public async Task Save()
        {
            try
            {
                //切断しないと変更後の設定値を取得できない。
                foreach (var server in serverList)
                {
                    if (server.IsConnected)
                    {
                        var task = server.ForceDisconnect();
                        await task;
                    }
                }
                
                ServerSettings.Instance.ServerInfoList.Clear();
                foreach (var server in serverList)
                {
                    ServerSettings.Instance.ServerInfoList.Add(server.GetInfo());
                }

                ServerSettings.SaveToXmlFile(settingFilePath);
            }
            catch (Exception ex)
            {
                MessageHandler.OnExceptionOccured(this, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            ServerSettings.LoadFromXmlFile(settingFilePath);
            foreach (var serverInfo in ServerSettings.Instance.ServerInfoList)
            {
                this.AddServer(serverInfo);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ISec AddServer(ServerInfo info)
        {
            Server newServer = null;
            try
            {
                if (info == null)
                    throw new ArgumentNullException("info");
                if (string.IsNullOrWhiteSpace(info.DisplayName))
                    throw new ArgumentException("DisplayNameがnullもしくは空白");
                if (string.IsNullOrWhiteSpace(info.Hostname))
                    throw new ArgumentException("Hostnameがnullもしくは空白");
                if (string.IsNullOrWhiteSpace(info.Nickname))
                    throw new ArgumentException("Nicknameがnullもしくは空白");
                //同じDisplayNameがあったらダメ
                foreach (var server in serverList)
                {
                    if (server.DisplayName == info.DisplayName)
                        throw new ArgumentException("このDisplayNameは既に登録済み。");
                }
                newServer = new Server();
                newServer.ConnectSuccess += newServer_ConnectSuccess;
                newServer.Disconnected += newServer_Disconnected;
                newServer.ReceiveEvent += newServer_ReceiveEvent;
                newServer.InfoEvent += newServer_InfoEvent;
                newServer.ExceptionInfo += newServer_ExceptionInfo;
                newServer.SetInfo(info);
                reconnectStateDic.Add(newServer, new ReconnectState());
                serverList.Add(newServer);
            }
            catch (Exception ex)
            {
                MessageHandler.OnExceptionOccured(this, ex, ex.Message);
            }
            return newServer;
        }

        async void newServer_Disconnected(object sender, IrcEventArgs e)
        {
            //Debug
            if (e.IServerChannel.Type == ServerChannelType.SERVER)
            {
                var server = (Server)e.IServerChannel;
                Console.WriteLine("IsDisconnectedExpected:" + server.IsDisconnectedExpected.ToString());
            }

            if (Disconnected != null)
            {
                Disconnected(sender, e);
            }
            if (e.IServerChannel.Type == ServerChannelType.SERVER)
            {
                var server = (Server)e.IServerChannel;
                await TryReconnect(server);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// AddServerでServerを追加する。
        /// </remarks>
        Dictionary<Server, ReconnectState> reconnectStateDic = new Dictionary<Server, ReconnectState>();
        class ReconnectState
        {
            /// <summary>
            /// 再接続を試みた回数
            /// </summary>
            public int trialCounter;
            /// <summary>
            /// 
            /// </summary>
            public System.Timers.Timer Timer;
            /// <summary>
            /// 
            /// </summary>
            public Server Server;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>意図しない切断と接続に失敗した際に使うつもりで作った</remarks>
        /// <param name="server"></param>
        /// <returns></returns>
        private async Task TryReconnect(Server server)
        {
            if (!server.IsDisconnectedExpected)
            {                
                Func<ReconnectState, Task> Reconnect = async (s) =>
                {
                    s.trialCounter++;
                    await s.Server.Connect();
                    MessageHandler.OnMessageEvent(this, "再接続");
                };
                //再接続が必要。
                var state = reconnectStateDic[server];
                if (state.Timer == null)
                {
                    int reconnectIntervalMin = 1;
                    state.Server = server;
                    state.Timer = new System.Timers.Timer();
                    state.Timer.Interval = reconnectIntervalMin * 60 * 1000;
                    state.Timer.Elapsed += async (sender1, e1) =>
                    {
                        await Reconnect(state);
                    };
                    state.Timer.Enabled = true;
                    state.Timer.Start();
                }
                await Reconnect(state);
            }
        }
        void newServer_ExceptionInfo(object sender, IrcExceptionEventArgs e)
        {
            if (ExceptionInfo != null)
            {
                ExceptionInfo(sender, e);
            }
        }

        void newServer_InfoEvent(object sender, IrcInfoEventArgs e)
        {
            if (InfoEvent != null)
            {
                InfoEvent(sender, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newServer_ConnectSuccess(object sender, IrcEventArgs e)
        {
            //自動再接続用のコンテキストを無効化。
            if (e.IServerChannel.Type == ServerChannelType.SERVER)
            {
                var state = reconnectStateDic[(Server)e.IServerChannel];
                if (state.Timer != null)
                {
                    state.Timer.Stop();
                    state.trialCounter = 0;
                    state.Timer = null;
                }
            }

            var name = "";
            if (e.IServerChannel.Type == ServerChannelType.CHANNEL)
            {
                var channel = (Channel)e.IServerChannel;
                name = string.Format("{0}({1})", channel.DisplayName, channel.Server.DisplayName);
            }
            else
            {
                name = e.IServerChannel.DisplayName;
            }
            MessageHandler.OnMessageEvent(this, "接続完了:" + name);

            //接続成功のイベントを起こす。
            if (ConnectSuccess != null)
            {
                ConnectSuccess(sender, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newServer_ReceiveEvent(object sender, IRCReceiveEventArgs e)
        {
            if (ReceiveEvent != null)
            {
                ReceiveEvent(sender, e);
            }
        }

        public List<ISec> ServerList
        {
            get
            {
                var list = new List<ISec>();
                list.AddRange(serverList);
                return list;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public Task<bool> SendCmd(ISec target, string text)
        {
            if(target == null)            
                throw new ArgumentException("target");            
            if(string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("text");
            Server server = null;
            if (target.Type == ServerChannelType.SERVER)
                server = (Server)target;
            else
                server = ((Channel)target).Server;
            return server.SendCmd(text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SetInfo(ISec server, ServerInfo info)
        {
            var ret = false;
            if (server.Type == ServerChannelType.SERVER)
            {
                var s = (Server)server;
                s.SetInfo(info);
                ret = true;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SetInfo(ISec channel, ChannelInfo info)
        {
            var ret = false;
            if (channel.Type == ServerChannelType.CHANNEL)
            {
                var c = (Channel)channel;
                c.SetInfo(info);
                ret = true;
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public ISec AddChannel(ISec server, ChannelInfo info)
        {
            ISec channel = null;
            if (server.Type == ServerChannelType.SERVER)
            {
                var s = (Server)server;
                channel = s.AddChannel(info);
            }
            return channel;
        }

        public List<ISec> GetChannelList(ISec server)
        {
            var list = new List<ISec>();
            var s = (Server)server;
            list.AddRange(s.ChannelList);
            return list;
        }

    }
}
