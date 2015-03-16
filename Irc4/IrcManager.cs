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
        }
        public bool AddServer(ServerInfo info)
        {
            //同じDisplayName
            foreach (var server in ServerSettings.Instance.ServerInfoList)
            {
                if (server.DisplayName == info.DisplayName)
                    return false;
            }
            var newServer = new Server();
            newServer.SetInfo(info);
            serverList.Add(newServer);

            return true;
        }
    }
}
