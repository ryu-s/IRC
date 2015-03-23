using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ryu_s.MyCommon;
namespace Irc4
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class Channel : ChannelInfo, ISec
    {
        /// <summary>
        /// 接続に成功した。
        /// </summary>
        [field: NonSerialized()]
        public event IrcEventHandler ConnectSuccess;
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized()]
        public event IrcEventHandler Disconnected;
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized()]
        private Server server_;
        public Server Server
        {
            get
            {
                return server_;
            }
            set
            {
                server_ = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized()]
        private bool isConnected;
        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected { get { return isConnected; } }
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized()]
        private UserList userList_;
        /// <summary>
        /// 
        /// </summary>
        public UserList UserList { get { return userList_; } private set { userList_ = value; } }
        /// <summary>
        /// 
        /// </summary>
        public UserInfo MyInfo
        {
            get
            {
                UserInfo user = UserList.Get(this.Server.MyInfo.NickName);
                if (user != null)
                    return user;
                else
                    return new UserInfo(this.Server.MyInfo.NickName);//こんな感じでいいだろうか？
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Channel()
        {
            Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>シリアライザで読み込まれた場合にはコンストラクタが実行されない代わりにこれが呼ばれる。</remarks>
        /// <param name="c"></param>
        [System.Runtime.Serialization.OnDeserializing]
        private void OnDeserializing(System.Runtime.Serialization.StreamingContext c)
        {
            Initialize();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            UserList = new UserList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            if (Server == null)
                throw new Exception("プロパティServerがnull。");
            await Server.SendCmd("JOIN " + this.DisplayName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            if (Server == null)
                throw new Exception("プロパティServerがnull。");
            await Server.SendCmd("PART " + this.DisplayName);
        }

        /// <summary>
        /// Quitコマンドを受け取る。
        /// </summary>
        /// <param name="log"></param>
        public void QuitHandler(Log log)
        {
            if (log == null || log.Command != Command.QUIT)
                throw new ArgumentException("log");
        }
        /// <summary>
        /// NICKコマンドを受け取る。
        /// </summary>
        /// <param name="log"></param>
        public void NickHandler(Log log)
        {
            if (log == null || log.Command != Command.NICK)
                throw new ArgumentException("log");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public void SetLog(Log log)
        {
            if(log.Command == Command.JOIN)
            {
                if (log.SenderInfo.NickName == this.MyInfo.NickName)
                {
                    OnConnectSuccess();
                }
            }
            else if(log.Command == Command.RPL_NAMREPLY)
            {
                //OnConnectSuccess();
            }
            else if (log.Command == Command.PART)
            {
                if (log.SenderInfo.NickName == this.MyInfo.NickName)
                {
                    OnDisconnected();
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        void OnConnectSuccess()
        {
            if (!this.isConnected)
            {
                this.isConnected = true;
                if (ConnectSuccess != null)
                {
                    var args = new IrcEventArgs();
                    args.IServerChannel = this;
                    args.logLevel = LogLevel.notice;
                    args.date = DateTime.Now;
                    ConnectSuccess(this, args);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>サーバがDisconnect時にが呼ぶことがあるためinternal
        /// </remarks>
        internal void OnDisconnected()
        {
            if (this.isConnected)
            {
                isConnected = false;
                if (Disconnected != null)
                {
                    var args = new IrcEventArgs();
                    args.IServerChannel = this;
                    args.logLevel = LogLevel.notice;
                    args.date = DateTime.Now;
                    Disconnected(this, args);
                }
            }
        }
        public void SetInfo(ChannelInfo info)
        {
            this.Clone(info);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
