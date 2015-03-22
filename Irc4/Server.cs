using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ryu_s.Net;
using ryu_s.MyCommon;
namespace Irc4
{
    //[改善点]
    //自分が打った文字列が反映されない。
    //いつかServerとChannelをinternalにする。

    //[仕様]
    //サーバを切断したときにチャンネルが接続状態にあってもチャンネルのDisconnectedイベントは起こさない。
    //ChannelListは接続中でも更新可能。他のパラメータは接続時に更新された分は切断後に反映される。

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class Server : ServerInfo, ISec
    {

        SocketStreamAsync socketStream;
        /// <summary>
        /// 一旦切断して、次の接続時に使用する情報。接続中には変更できない値が幾つかあるため。
        /// 設定の変更は常にここに入れる。Connect()の最初で本体の情報を更新する。
        /// </summary>
        private ServerInfo infoModified;
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
        /// 行末文字。
        /// </summary>
        private string LineTerminator;
        /// <summary>
        /// 
        /// </summary>
        public Server()
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
            infoModified = new ServerInfo();
            LineTerminator = "\r\n";
            socketStream = new SocketStreamAsync(LineTerminator, this.Encoding);
            socketStream.ConnectSuccess += socketStream_ConnectSuccess;
            socketStream.Disconnected += socketStream_Disconnected;
            socketStream.Received += socketStream_Received;
            MyInfo = new UserInfo();
            IsDisconnectedExpected = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public void SetInfo(ServerInfo info)
        {
            infoModified.Clone(info);
            if (!IsConnected)
                this.Clone(info);

            //ここでChannnelListに登録する以外のAddChannel()と同様の処理をする。
            //共通化できれば良いのだが。
            foreach (var channel in info.ChannelList)
            {
                var c = (Channel)channel;
                c.Server = this;
                OnAddChannel(c);
            }
            var message = "設定変更完了。";
            if (IsConnected)
                message += "次回接続時に反映されます。";
            InfoHandler(LogLevel.notice, message);
        }
        internal Channel AddChannel(ChannelInfo channelInfo)
        {
            Channel newChannel = null;
            try
            {
                if (channelInfo == null)
                    throw new ArgumentNullException("channelInfo");
                if (!IrcTool.IsChannelName(channelInfo.DisplayName))
                    throw new Exception("チャンネル名が不正な形式");
                foreach (var channel in ChannelList)
                {
                    if (channelInfo.DisplayName == channel.DisplayName)
                        throw new Exception("このチャンネル名は既に登録済み。");
                }
                newChannel = new Channel();
                newChannel.Server = this;
                newChannel.SetInfo(channelInfo);

                //チャンネルリストに登録する。
                var list = base.ChannelList.ToList();
                list.Add(newChannel);
                base.ChannelList = list.ToArray();

                OnAddChannel(newChannel);
            }
            catch (Exception ex)
            {
                InfoHandler(LogLevel.error, ex.Message, ex);
            }
            return newChannel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        void OnAddChannel(Channel channel)
        {
            //TODO:チャンネルが追加されたら発生するイベントを追加する。
            channel.ConnectSuccess += channel_ConnectSuccess;
            channel.Disconnected += channel_Disconnected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServerInfo GetInfo()
        {
            var info = new ServerInfo();
            info.Clone(infoModified);
            info.ChannelList = base.ChannelList;
            return info;
        }
        /// <summary>
        /// 表示名
        /// </summary>
        /// <remarks>接続状態に関わらずいつでも変更可能</remarks>
        public new string DisplayName
        {
            get
            {
                return base.DisplayName;
            }
            set
            {
                base.DisplayName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public new string Hostname
        {
            get
            {
                if (IsConnected)
                    return base.Hostname;
                else
                    return infoModified.Hostname;
            }
            set
            {
                infoModified.Hostname = value;
                if (!IsConnected)
                    base.Hostname = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public new string Username
        {
            get
            {
                if (this.IsConnected)
                    return base.Username;
                else
                    return infoModified.Username;
            }
            set
            {
                infoModified.Username = value;
                if (!IsConnected)
                    base.Username = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public new int Port
        {
            get
            {
                if (this.IsConnected)
                    return base.Port;
                else
                    return infoModified.Port;
            }
            set
            {
                infoModified.Port = value;
                if (!IsConnected)
                    base.Port = value;
            }
        }


        void channel_Disconnected(object sender, IrcEventArgs e)
        {
            if (Disconnected != null && this.IsConnected)
            {
                Disconnected(sender, e);
            }
        }

        void channel_ConnectSuccess(object sender, IrcEventArgs e)
        {
            if (ConnectSuccess != null && this.IsConnected)
            {
                ConnectSuccess(sender, e);
            }
        }
        void newChannel_ReceiveEvent(object sender, IRCReceiveEventArgs e)
        {
            if (ReceiveEvent != null)
            {
                ReceiveEvent(sender, e);
            }
        }
        /// <summary>
        /// 接続済みか。
        /// </summary>
        public virtual bool IsConnected
        {
            get
            {
                return (socketStream != null && socketStream.IsConnected);
            }
        }
        /// <summary>
        /// 例外処理。
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        private void InfoHandler(LogLevel level, string message, Exception ex)
        {
            var additional = "";
            var logPath = @"C:\Irc4Exception.txt";
            if (ex is SocketException)
                additional = ((SocketException)ex).SocketErrorCode.ToString();

            var s = ""
                + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine
                + message + Environment.NewLine
                + additional + Environment.NewLine
                + ex.GetType() + Environment.NewLine
                + ex.Message + Environment.NewLine
                + ex.TargetSite + Environment.NewLine
                + ex.StackTrace + Environment.NewLine
                + "===================================================" + Environment.NewLine
                ;
            using (var sr = new System.IO.StreamWriter(logPath, true))
            {
                sr.Write(s);
            }
            if (ExceptionInfo != null)
            {
                var args = new IrcExceptionEventArgs();
                args.date = DateTime.Now;
                args.ex = ex;
                args.logLevel = level;
                args.IServerChannel = this;
                args.Message = message;
                ExceptionInfo(this, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        private void InfoHandler(LogLevel level, string message)
        {
            if (InfoEvent != null)
            {
                var args = new IrcInfoEventArgs();
                args.IServerChannel = this;
                args.Message = message;
                args.logLevel = level;
                args.date = DateTime.Now;
                InfoEvent(this, args);
            }
        }
        private bool isDisconnectedExpected;
        internal bool IsDisconnectedExpected
        {
            get
            {
                return isDisconnectedExpected;
            }
            private set
            {
                isDisconnectedExpected = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task Connect()
        {
            Console.WriteLine("Server::Connect():" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var fatalErrorOccured = false;

            //存在しないホスト名だった場合に例外を投げるが、接続試行中にホスト名を変更すると、catch内で間違っている方のホスト名が参照できない。
            //そこで、一旦コピーしておく。
            var hostname = this.Hostname;
            try
            {
                if (IsConnected)
                    throw new Exception("既に接続済み");
                IsDisconnectedExpected = false;
                //Update my info
                this.Clone(infoModified);
                await socketStream.Connect(hostname, this.Port);
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.HostNotFound:
                        message = "No such host is known：" + hostname;
                        break;
                    case SocketError.ConnectionRefused:
                        message = "接続が拒否された。サーバソフトが起動してないか、正常に機能してない。";
                        break;
                    case SocketError.TimedOut:
                        message = string.Format("Time out. HostもしくはPortが間違っているかも？ Host:{0} Port：{1}", this.Hostname, this.Port);
                        break;
                    case SocketError.IsConnected:
                        message = "このソケットは接続済み。";
                        break;
                    case SocketError.AddressAlreadyInUse:
                        message = "おそらく前回使用時にDisconnectAsync()を実行していない？";
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                InfoHandler(LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            catch (Exception ex)
            {
                var message = "Not Implemented";
                InfoHandler(LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            if (fatalErrorOccured)
            {
                //ConnectionFailed
                return;
            }

            //コマンドの送信に成功したらtrueが返ってくるから反転させる必要がある。
            fatalErrorOccured = !await this.SendCmd(string.Format("NICK {0}\r\nUSER {1} 8 * :{2}\r\n", this.Nickname, this.Username, this.Realname));
            if (fatalErrorOccured)
            {
                OnDisconnected();
                return;
            }

            try
            {
                await socketStream.Receive();
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                        message = "ネット回線が途切れた？" + ex.SocketErrorCode.ToString();
                        break;
                    case SocketError.ConnectionReset:
                        message = "おそらくサーバが落ちた";
                        break;
                    case SocketError.NotConnected:
                        message = "未接続、あるいはデータを受信中に切断した。";
                        break;
                    default:
                        message = "Not Implemented SocketErrorCode:" + ex.SocketErrorCode.ToString();
                        break;
                }
                if (!IsDisconnectedExpected)
                    InfoHandler(LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            catch (Exception ex)
            {
                var message = "Not Implemented";
                InfoHandler(LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            if (fatalErrorOccured)
            {
                //次回の接続に同じソケットを使うと"AdressAlreadyInUse"という例外が発生してしまう。それへの対処。
                socketStream.Reset();
                OnDisconnected();
                return;
            }
            await ForceDisconnect();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Quit()
        {
            await SendCmd("QUIT");
        }
        void OnDisconnected()
        {
            if (Disconnected != null && !IsConnected)
            {
                System.Diagnostics.Debug.Assert(!IsConnected);

                foreach (var channel in ChannelList)
                {
                    ((Channel)channel).OnDisconnected();
                }
                var args = new IrcEventArgs();
                args.date = DateTime.Now;
                args.logLevel = LogLevel.notice;
                args.IServerChannel = this;
                Disconnected(this, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void socketStream_Received(object sender, StringEventArgs e)
        {
            var log = new Irc4.Log(this, e.Message);
            await SetLog(log);
        }

        void socketStream_Disconnected(object sender, EventArgs e)
        {

        }

        void socketStream_ConnectSuccess(object sender, EventArgs e)
        {
            if (ConnectSuccess != null)
            {
                var args = new IrcEventArgs();
                args.date = DateTime.Now;
                args.logLevel = LogLevel.notice;
                args.IServerChannel = this;
                ConnectSuccess(this, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        async Task SetLog(Log log)
        {
            //QUITとNICKを全チャンネルに渡す必要がある。チャンネルからそれを飛ばしたユーザを外すため。


            //チャンネル名に何か入っていた場合の準備
            Channel channel = null;
            if (!string.IsNullOrWhiteSpace(log.ChannelName))
            {
                channel = (Channel)GetChannel(log.ChannelName);
                if (channel == null)
                {
                    if (log.Command == Command.JOIN || log.Command == Command.RPL_NAMREPLY)
                    {
                        var newChannelInfo = new ChannelInfo();
                        newChannelInfo.DisplayName = log.ChannelName;
                        channel = AddChannel(newChannelInfo);
                    }
                    else
                    {
                        //未登録のチャンネル。JOINだったら登録する処理か？どんなメッセージが来るかあんまり分からないから情報収集。
                        using (var sw = new System.IO.StreamWriter(@"C:\notSubscribedChannelMessage.txt", true))
                        {
                            var s = ""
                                + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine
                                + log.Command.ToString() + Environment.NewLine
                                + log.Raw + Environment.NewLine
                                + "==========================" + Environment.NewLine;
                            sw.Write(s);
                        }
                    }
                }
            }

            //受信通知 TODO:位置はここで良いか？サーバやチャンネル固有の処理をした後の方が良いか？
            if (ReceiveEvent != null)
            {
                var args = new IRCReceiveEventArgs();
                args.text = log.Text;//TODO:要らない
                args.log = log;
                args.IServerChannel = (channel == null) ? (ISec)this : channel;
                ReceiveEvent(this, args);
            }

            if (channel == null)
            {
                //サーバ向けのメッセージ

                if (log.Command == Command.PING)
                {
                    await Pong(log.Sender);
                }
                else if (log.Command == Command.QUIT)
                {

                }
                else if (log.Command == Command.NICK)
                {

                }
                else if (log.Command == Command.RPL_WELCOME)
                {
                    MyInfo.Set(log.Dictionary["myname"]);
                }
            }
            else
            {
                //チャンネル向けメッセージ

                channel.SetLog(log);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public UserInfo MyInfo { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public ISec GetChannel(string displayName)
        {
            foreach (var channel in this.ChannelList)
            {
                if (channel.DisplayName.ToLower() == displayName.ToLower())
                    return channel;
            }
            return null;
        }
        public async Task<bool> SendCmd(string str)
        {
            var s = str + "\r\n";
            var b = false;

            try
            {
                await socketStream.Send(s);
                b = true;
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.NotConnected:
                        message = "The socket is not connected";
                        break;
                    case SocketError.ConnectionReset:
                        message = "おそらくサーバが落ちた";
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                InfoHandler(LogLevel.error, message, ex);
            }
            return b;
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task Pong(string receiver)
        {
            await SendCmd("PONG " + receiver);
        }
        /// <summary>
        /// 強制的に切断する。IRC的にはQuit()を利用するほうが良いかも。
        /// </summary>
        /// <returns></returns>
        internal async Task ForceDisconnect()
        {
            IsDisconnectedExpected = true;
            try
            {
                await socketStream.Disconnect();
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.NotConnected:
                        message = "接続していないのに切断処理をしようとした。";
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                InfoHandler(LogLevel.error, message, ex);
            }
            catch (Exception ex)
            {
                var message = "Not Implemented";
                InfoHandler(LogLevel.error, message, ex);
            }
            finally
            {
                OnDisconnected();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            if (IsConnected)
            {
                IsDisconnectedExpected = true;
                await Quit();
            }
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
    ///// <summary>
    ///// 
    ///// </summary>
    //[Serializable]
    //internal class Server : ServerInfo, ISec
    //{        
    //    /// <summary>
    //    /// 一旦切断して、次の接続時に使用する情報。接続中には変更できない値が幾つかあるため。
    //    /// 設定の変更は常にここに入れる。Connect()の最初で本体の情報を更新する。
    //    /// </summary>
    //    private ServerInfo infoModified;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    private SocketAsync socket;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    private SplitBuffer splitBuffer;
    //    /// <summary>
    //    /// 接続に成功した。
    //    /// </summary>
    //    public event IrcEventHandler ConnectSuccess;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public event IrcEventHandler Disconnected;
    //    /// <summary>
    //    /// 何か受け取った。
    //    /// </summary>
    //    public event ReceiveEventHandler ReceiveEvent;
    //    /// <summary>
    //    /// 例外情報
    //    /// </summary>
    //    public event IrcExceptionHandler ExceptionInfo;
    //    /// <summary>
    //    /// 諸々の情報。例外はExceptionInfo。
    //    /// </summary>
    //    public event IrcInfoHandler InfoEvent;
    //    /// <summary>
    //    /// 行末文字。
    //    /// </summary>
    //    private string LineTerminator;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <remarks>デシリアライズした時にconstは初期化されるんだろうか</remarks>
    //    private const int BUFSIZ = 2048;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Server()
    //    {      
    //        Initialize();
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <remarks>シリアライザで読み込まれた場合にはコンストラクタが実行されない代わりにこれが呼ばれる。</remarks>
    //    /// <param name="c"></param>
    //    [System.Runtime.Serialization.OnDeserializing]
    //    private void OnDeserializing(System.Runtime.Serialization.StreamingContext c)
    //    {
    //        Initialize();
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    private void Initialize()
    //    {
    //        infoModified = new ServerInfo();
    //        socket = new SocketAsync();
    //        LineTerminator = "\r\n";
    //        MyInfo = new UserInfo();
    //        IsDisconnectedExpected = false;
    //        splitBuffer = new SplitBuffer(this, LineTerminator);
    //        splitBuffer.AddedEvent += splitBuffer_AddedEvent; 
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="info"></param>
    //    public void SetInfo(ServerInfo info)
    //    {
    //        infoModified.Clone(info);
    //        if (!IsConnected)
    //            this.Clone(info);

    //        //ここでChannnelListに登録する以外のAddChannel()と同様の処理をする。
    //        //共通化できれば良いのだが。
    //        foreach (var channel in info.ChannelList)
    //        {
    //            var c = (Channel)channel;
    //            c.Server = this;
    //            OnAddChannel(c);
    //        }
    //        var message = "設定変更完了。";
    //        if (IsConnected)
    //            message += "次回接続時に反映されます。";
    //        InfoHandler(LogLevel.notice, message);
    //    }
    //    internal Channel AddChannel(ChannelInfo channelInfo)
    //    {
    //        Channel newChannel = null;
    //        try
    //        {
    //            if (channelInfo == null)
    //                throw new ArgumentNullException("channelInfo");
    //            if (!IrcTool.IsChannelName(channelInfo.DisplayName))
    //                throw new Exception("チャンネル名が不正な形式");
    //            foreach(var channel in ChannelList)
    //            {
    //                if (channelInfo.DisplayName == channel.DisplayName)
    //                    throw new Exception("このチャンネル名は既に登録済み。");
    //            }
    //            newChannel = new Channel();
    //            newChannel.Server = this;
    //            newChannel.SetInfo(channelInfo);

    //            //チャンネルリストに登録する。
    //            var list = base.ChannelList.ToList();
    //            list.Add(newChannel);
    //            base.ChannelList = list.ToArray();

    //            OnAddChannel(newChannel);
    //        }
    //        catch (Exception ex)
    //        {
    //            InfoHandler(LogLevel.error, ex.Message, ex);
    //        }
    //        return newChannel;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="channel"></param>
    //    void OnAddChannel(Channel channel)
    //    {
    //        //TODO:チャンネルが追加されたら発生するイベントを追加する。
    //        channel.ConnectSuccess += channel_ConnectSuccess;
    //        channel.Disconnected += channel_Disconnected;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public ServerInfo GetInfo()
    //    {
    //        var info = new ServerInfo();
    //        info.Clone(infoModified);
    //        info.ChannelList = base.ChannelList;
    //        return info;
    //    }
    //    /// <summary>
    //    /// 表示名
    //    /// </summary>
    //    /// <remarks>接続状態に関わらずいつでも変更可能</remarks>
    //    public new string DisplayName
    //    {
    //        get
    //        {
    //            return base.DisplayName;
    //        }
    //        set
    //        {
    //            base.DisplayName = value;
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public new string Hostname
    //    {
    //        get
    //        {
    //            if (IsConnected)
    //                return base.Hostname;
    //            else
    //                return infoModified.Hostname;
    //        }
    //        set
    //        {
    //            infoModified.Hostname = value;
    //            if (!IsConnected)
    //                base.Hostname = value;
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public new string Username
    //    {
    //        get
    //        {
    //            if (this.IsConnected)
    //                return base.Username;
    //            else
    //                return infoModified.Username;
    //        }
    //        set
    //        {
    //            infoModified.Username = value;
    //            if (!IsConnected)
    //                base.Username = value;
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public new int Port
    //    {
    //        get
    //        {
    //            if (this.IsConnected)
    //                return base.Port;
    //            else
    //                return infoModified.Port;
    //        }
    //        set
    //        {
    //            infoModified.Port = value;
    //            if (!IsConnected)
    //                base.Port = value;
    //        }
    //    }


    //    void channel_Disconnected(object sender, IrcEventArgs e)
    //    {
    //        if (Disconnected != null && this.IsConnected)
    //        {
    //            Disconnected(sender, e);
    //        }
    //    }

    //    void channel_ConnectSuccess(object sender, IrcEventArgs e)
    //    {
    //        if (ConnectSuccess != null && this.IsConnected)
    //        {
    //            ConnectSuccess(sender, e);
    //        }
    //    }
    //    void newChannel_ReceiveEvent(object sender, IRCReceiveEventArgs e)
    //    {
    //        if (ReceiveEvent != null)
    //        {
    //            ReceiveEvent(sender, e);
    //        }
    //    }
    //    /// <summary>
    //    /// 接続済みか。
    //    /// </summary>
    //    public virtual bool IsConnected
    //    {
    //        get
    //        {
    //            return (socket != null && socket.IsConnected);
    //        }
    //    }
    //    /// <summary>
    //    /// 例外処理。
    //    /// </summary>
    //    /// <param name="level"></param>
    //    /// <param name="message"></param>
    //    /// <param name="ex"></param>
    //    private void InfoHandler(LogLevel level, string message, Exception ex)
    //    {
    //        var additional = "";
    //        var logPath = @"C:\Irc4Exception.txt";
    //        if (ex is SocketException)
    //            additional = ((SocketException)ex).SocketErrorCode.ToString();

    //        var s = ""
    //            + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine
    //            + message + Environment.NewLine
    //            + additional + Environment.NewLine
    //            + ex.GetType() + Environment.NewLine
    //            + ex.Message + Environment.NewLine
    //            + ex.TargetSite + Environment.NewLine
    //            + ex.StackTrace + Environment.NewLine
    //            + "===================================================" + Environment.NewLine
    //            ;
    //        using (var sr = new System.IO.StreamWriter(logPath, true))
    //        {
    //            sr.Write(s);
    //        }
    //        if (ExceptionInfo != null)
    //        {
    //            var args = new IrcExceptionEventArgs();
    //            args.date = DateTime.Now;
    //            args.ex = ex;
    //            args.logLevel = level;
    //            args.IServerChannel = this;
    //            args.Message = message;
    //            ExceptionInfo(this, args);
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="level"></param>
    //    /// <param name="message"></param>
    //    private void InfoHandler(LogLevel level, string message)
    //    {
    //        if (InfoEvent != null)
    //        {
    //            var args = new IrcInfoEventArgs();
    //            args.IServerChannel = this;
    //            args.Message = message;
    //            args.logLevel = level;
    //            args.date = DateTime.Now;
    //            InfoEvent(this, args);
    //        }
    //    }
    //    private bool isDisconnectedExpected;
    //    internal bool IsDisconnectedExpected
    //    {
    //        get
    //        {
    //            return isDisconnectedExpected;
    //        }
    //        private set
    //        {
    //            isDisconnectedExpected = value;
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public async Task Connect()
    //    {
    //        Console.WriteLine("Server::Connect():" + System.Threading.Thread.CurrentThread.ManagedThreadId);
    //        var fatalErrorOccured = false;

    //        //存在しないホスト名だった場合に例外を投げるが、接続試行中にホスト名を変更すると、catch内で間違っている方のホスト名が参照できない。
    //        //そこで、一旦コピーしておく。
    //        var hostname = this.Hostname;
    //        try
    //        {
    //            if (IsConnected)
    //                throw new Exception("既に接続済み");
    //            IsDisconnectedExpected = false;
    //            //Update my info
    //            this.Clone(infoModified);
    //            await socket.ConnectAsync(hostname, this.Port);
    //        }
    //        catch (SocketException ex)
    //        {
    //            string message = "";
    //            switch (ex.SocketErrorCode)
    //            {
    //                case SocketError.HostNotFound:
    //                    message = "No such host is known：" + hostname;
    //                    break;
    //                case SocketError.ConnectionRefused:
    //                    message = "接続が拒否された。サーバソフトが起動してないか、正常に機能してない。";
    //                    break;
    //                case SocketError.TimedOut:
    //                    message = string.Format("Time out. HostもしくはPortが間違っているかも？ Host:{0} Port：{1}", this.Hostname,this.Port);
    //                    break;
    //                case SocketError.IsConnected:
    //                    message = "このソケットは接続済み。";
    //                    break;
    //                case SocketError.AddressAlreadyInUse:
    //                    message = "おそらく前回使用時にDisconnectAsync()を実行していない？";
    //                    break;
    //                default:
    //                    message = "Not Implemented";
    //                    break;
    //            }
    //            InfoHandler(LogLevel.error, message, ex);
    //            fatalErrorOccured = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            var message = "Not Implemented";
    //            InfoHandler(LogLevel.error, message, ex);
    //            fatalErrorOccured = true;
    //        }
    //        if (fatalErrorOccured)
    //        {
    //            //ConnectionFailed
    //            return;
    //        }

    //        //コマンドの送信に成功したらtrueが返ってくるから反転させる必要がある。
    //        fatalErrorOccured = !await this.SendCmd(string.Format("NICK {0}\r\nUSER {1} 8 * :{2}\r\n", this.Nickname, this.Username, this.Realname));
    //        if (fatalErrorOccured)
    //        {
    //            OnDisconnected();
    //            return;
    //        }
    //        if (ConnectSuccess != null)
    //        {
    //            var args = new IrcEventArgs();
    //            args.date = DateTime.Now;
    //            args.logLevel = LogLevel.notice;
    //            args.IServerChannel = this;
    //            ConnectSuccess(this, args);
    //        }            
    //        var buffer = new byte[BUFSIZ];
    //        var hostEnc = this.Encoding;
    //        var sb = new StringBuilder();
    //        int n = 0;
    //        do
    //        {
    //            try
    //            {
    //                n = await socket.ReceiveAsync(buffer, 0, buffer.Length, System.Net.Sockets.SocketFlags.None);
    //            }
    //            catch (SocketException ex)
    //            {
    //                string message = "";
    //                switch (ex.SocketErrorCode)
    //                {
    //                    case SocketError.ConnectionAborted:
    //                        message = "ネット回線が途切れた？" + ex.SocketErrorCode.ToString();
    //                        break;
    //                    case SocketError.ConnectionReset:
    //                        message = "おそらくサーバが落ちた";
    //                        break;
    //                    case SocketError.NotConnected:
    //                        message = "未接続、あるいはデータを受信中に切断した。";
    //                        break;
    //                    default:
    //                        message = "Not Implemented SocketErrorCode:" + ex.SocketErrorCode.ToString();
    //                        break;
    //                }
    //                if(!IsDisconnectedExpected)
    //                    InfoHandler(LogLevel.error, message, ex);
    //                fatalErrorOccured = true;
    //            }
    //            catch (Exception ex)
    //            {
    //                var message = "Not Implemented";
    //                InfoHandler(LogLevel.error, message, ex);
    //                fatalErrorOccured = true;
    //            }
    //            if (fatalErrorOccured)
    //            {
    //                //次回の接続に同じソケットを使うと"AdressAlreadyInUse"という例外が発生してしまう。それへの対処。
    //                socket.Reset();
    //                OnDisconnected();
    //                return;
    //            }
    //            var s = hostEnc.GetString(buffer, 0, n);
    //            sb.Append(s);
    //            splitBuffer.Add(s.Replace("\0", ""));
    //        } while (n > 0);
    //        try
    //        {
    //            await socket.DisconnectAsync();
    //        }
    //        catch (SocketException ex)
    //        {
    //            string message = "";
    //            switch (ex.SocketErrorCode)
    //            {
    //                case SocketError.NotConnected:
    //                    message = "接続していないのに切断処理をしようとした。";
    //                    break;
    //                default:
    //                    message = "Not Implemented";
    //                    break;
    //            }
    //            InfoHandler(LogLevel.error, message, ex);
    //            fatalErrorOccured = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            var message = "Not Implemented";
    //            InfoHandler(LogLevel.error, message, ex);
    //            fatalErrorOccured = true;
    //        }
    //        finally
    //        {
    //            OnDisconnected();
    //        }
    //        if (!fatalErrorOccured)
    //            Console.WriteLine("正常終了");
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task Quit()
    //    {
    //        await SendCmd("QUIT");
    //    }
    //    void OnDisconnected()
    //    {
    //        if (Disconnected != null && !IsConnected)
    //        {
    //            System.Diagnostics.Debug.Assert(!IsConnected);

    //            foreach (var channel in ChannelList)
    //            {
    //                ((Channel)channel).OnDisconnected();
    //            }
    //            var args = new IrcEventArgs();
    //            args.date = DateTime.Now;
    //            args.logLevel = LogLevel.notice;
    //            args.IServerChannel = this;
    //            Disconnected(this, args);
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    async void splitBuffer_AddedEvent(object sender, AddedEventArgs e)
    //    {
    //        var log = new Irc4.Log(this, e.AddedString);
    //        await SetLog(log);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="log"></param>
    //    async Task SetLog(Log log)
    //    {
    //        //QUITとNICKを全チャンネルに渡す必要がある。チャンネルからそれを飛ばしたユーザを外すため。


    //        //チャンネル名に何か入っていた場合の準備
    //        Channel channel = null;
    //        if (!string.IsNullOrWhiteSpace(log.ChannelName))
    //        {
    //            channel = (Channel)GetChannel(log.ChannelName);
    //            if (channel == null)
    //            {
    //                if (log.Command == Command.JOIN || log.Command == Command.RPL_NAMREPLY)
    //                {
    //                    var newChannelInfo = new ChannelInfo();
    //                    newChannelInfo.DisplayName = log.ChannelName;
    //                    channel = AddChannel(newChannelInfo);
    //                }
    //                else
    //                {
    //                    //未登録のチャンネル。JOINだったら登録する処理か？どんなメッセージが来るかあんまり分からないから情報収集。
    //                    using (var sw = new System.IO.StreamWriter(@"C:\notSubscribedChannelMessage.txt", true))
    //                    {
    //                        var s = ""
    //                            + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine
    //                            + log.Command.ToString() + Environment.NewLine
    //                            + log.Raw + Environment.NewLine
    //                            + "==========================" + Environment.NewLine;
    //                        sw.Write(s);
    //                    }
    //                }
    //            }
    //        }

    //        //受信通知 TODO:位置はここで良いか？サーバやチャンネル固有の処理をした後の方が良いか？
    //        if (ReceiveEvent != null)
    //        {
    //            var args = new IRCReceiveEventArgs();
    //            args.text = log.Text;//TODO:要らない
    //            args.log = log;
    //            args.IServerChannel = (channel == null) ? (ISec)this : channel;
    //            ReceiveEvent(this, args);
    //        }

    //        if(channel == null)
    //        {
    //            //サーバ向けのメッセージ

    //            if (log.Command == Command.PING)
    //            {
    //                await Pong(log.Sender);
    //            }
    //            else if (log.Command == Command.QUIT)
    //            {

    //            }
    //            else if (log.Command == Command.NICK)
    //            {

    //            }else if(log.Command == Command.RPL_WELCOME)
    //            {
    //                MyInfo.Set(log.Dictionary["myname"]);
    //            }
    //        }
    //        else
    //        {
    //            //チャンネル向けメッセージ

    //            channel.SetLog(log);
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public UserInfo MyInfo { get; private set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="displayName"></param>
    //    /// <returns></returns>
    //    public ISec GetChannel(string displayName)
    //    {
    //        foreach(var channel in this.ChannelList)
    //        {
    //            if (channel.DisplayName.ToLower() == displayName.ToLower())
    //                return channel;
    //        }
    //        return null;
    //    }
    //    public async Task<bool> SendCmd(string str)
    //    {
    //        var s = str + "\r\n";
    //        var b = false;
    //        Encoding enc = Encoding.UTF8;
    //        try
    //        {
    //            await socket.SendAsync(enc.GetBytes(s));
    //            b = true;
    //        }
    //        catch (SocketException ex)
    //        {
    //            string message = "";
    //            switch (ex.SocketErrorCode)
    //            {
    //                case SocketError.NotConnected:
    //                    message = "The socket is not connected";
    //                    break;
    //                case SocketError.ConnectionReset:
    //                    message = "おそらくサーバが落ちた";
    //                    break;
    //                default:
    //                    message = "Not Implemented";
    //                    break;
    //            }
    //            InfoHandler(LogLevel.error, message, ex);
    //        }
    //        return b;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public async Task Pong(string receiver)
    //    {
    //        await SendCmd("PONG " + receiver);
    //    }
    //    /// <summary>
    //    /// 強制的に切断する。IRC的にはQuit()を利用するほうが良いかも。
    //    /// </summary>
    //    /// <returns></returns>
    //    internal async Task ForceDisconnect()
    //    {
    //        IsDisconnectedExpected = true;
    //        try
    //        {
    //            await socket.DisconnectAsync();
    //        }
    //        catch (SocketException ex)
    //        {
    //            string message = "";
    //            switch (ex.SocketErrorCode)
    //            {
    //                case SocketError.NotConnected:
    //                    message = "未接続の状態で切断処理をしようとした。";
    //                    break;
    //                default:
    //                    message = "Not Implemented";
    //                    break;
    //            }
    //            InfoHandler(LogLevel.error, message, ex);
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task Disconnect()
    //    {
    //        if (IsConnected)
    //        {
    //            IsDisconnectedExpected = true;
    //            await Quit();
    //        }
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return this.DisplayName;
    //    }
    //}
}
