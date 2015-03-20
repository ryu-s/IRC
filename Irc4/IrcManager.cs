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
        public IrcManager()
        {
            Load();
        }
        /// <summary>
        /// 
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
                        var task = server.Disconnect();
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
                ExceptionHandler.OnExceptionOccured(this, ex);
            }
            Console.WriteLine("Save(),4:" + System.Threading.Thread.CurrentThread.ManagedThreadId);
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
        public Server AddServer(ServerInfo info)
        {
            //同じDisplayNameがあったらダメ
            foreach (var server in serverList)
            {
                if (server.DisplayName == info.DisplayName)
                    return null;
            }
            var newServer = new Server();
            newServer.ConnectSuccess += newServer_ConnectSuccess;
            newServer.Disconnected += newServer_Disconnected;
            newServer.ReceiveEvent += newServer_ReceiveEvent;
            newServer.InfoEvent += newServer_InfoEvent;
            newServer.ExceptionInfo += newServer_ExceptionInfo;
            newServer.SetInfo(info);
            serverList.Add(newServer);

            return newServer;
        }

        void newServer_Disconnected(object sender, IrcEventArgs e)
        {
            if (Disconnected != null)
            {
                Disconnected(sender, e);
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

        public List<Server> ServerList
        {
            get
            {
                return serverList;
            }
        }
    }
}
