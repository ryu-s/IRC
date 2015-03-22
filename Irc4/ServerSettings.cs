using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ryu_s.MyCommon;
namespace Irc4
{

    [DataContract]
    class ServerSettings : MySettingsBase<ServerSettings>
    {
        /// <summary>
        /// 実行されたくないからprivateにしたいけど、そうするとMySettingsBaseから呼び出せない。どうすればいいのか・・・。
        /// </summary>
        public ServerSettings()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        private List<ServerInfo> _serverInfoList;
        /// <summary>
        /// 
        /// </summary>
        public List<ServerInfo> ServerInfoList
        {
            get
            {
                return _serverInfoList;
            }
            set
            {
                _serverInfoList = value;
                OnChanged(Tool.GetName(() => ServerSettings.Instance.ServerInfoList));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void SetDefaultValues()
        {
            ServerInfoList = new List<ServerInfo>();
        }
    }
}
