using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Irc4
{
    public class IrcManager
    {
        string settingFilePath = System.Environment.CurrentDirectory + @"\Server.config";
        List<Server> serverList = new List<Server>();

        /// <summary>
        /// 何か受け取った。
        /// </summary>
        public event ReceiveEventHandler ReceiveEvent;

        public IrcManager()
        {
            Load();
        }
        public async void Save()
        {
            //切断しないと変更後の設定値を取得できない。
            foreach (var server in serverList)
            {
                if (server.IsConnected)
                    await server.Disconnect();
            }
            ServerSettings.Instance.ServerInfoList.Clear();
            foreach (var server in serverList)
            {
                ServerSettings.Instance.ServerInfoList.Add(server.GetInfo());
            }
            
            ServerSettings.SaveToXmlFile(settingFilePath);
        }
        public void Load()
        {
            ServerSettings.LoadFromXmlFile(settingFilePath);
            foreach (var serverInfo in ServerSettings.Instance.ServerInfoList)
            {
                this.AddServer(serverInfo);
            }
        }
        public Server AddServer(ServerInfo info)
        {
            //同じDisplayNameがあったらダメ
            foreach (var server in serverList)
            {
                if (server.DisplayName == info.DisplayName)
                    return null;
            }
            var newServer = new Server();
            newServer.ReceiveEvent += newServer_ReceiveEvent;
            newServer.SetInfo(info);
            serverList.Add(newServer);

            return newServer;
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
