using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Irc4
{
    [Serializable]
    public class Server : ServerInfo, ISec
    {
        /// <summary>
        /// 一旦切断して、次の接続時に使用する情報。接続中には変更できない値が幾つかあるため。
        /// 設定の変更は常にここに入れる。Connect()の最初で本体の情報を更新する。
        /// </summary>
        private ServerInfo infoModified;

        private MyLibrary.MySocket.SocketAsync socket;

        private MyLibrary.MySocket.SplitBuffer splitBuffer;

        public event ReceiveEventHandler ReceiveEvent;

        private string NewLine = "\r\n";
        /// <summary>
        /// 
        /// </summary>
        public Server()
        {
            socket = new MyLibrary.MySocket.SocketAsync();
            splitBuffer = new MyLibrary.MySocket.SplitBuffer(this, NewLine);
            splitBuffer.AddedEvent += splitBuffer_AddedEvent;
            infoModified = new ServerInfo();
        }

        public void SetInfo(ServerInfo info)
        {
            CopyInfo(info, infoModified);
        }
        public void CopyInfo(ServerInfo src, ServerInfo dst)
        {
            dst.DisplayName = src.DisplayName;
            dst.Hostname = src.Hostname;
            dst.Port = src.Port;
        }
        public new string DisplayName
        {
            get
            {
                return base.DisplayName;
            }
            set
            {
                infoModified.DisplayName = value;
            }
        }
        public bool IsConnected
        {
            get
            {
                return (socket != null && socket.IsConnected);
            }
        }
        public void ExceptionHandler(MyLibrary.LogLevel level, string message, Exception ex)
        {
            var additional = "";
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
            using (var sr = new System.IO.StreamWriter(@"C:\Irc4Exception.txt", true))
            {
                sr.Write(s);
            }
        }
        public event EventHandler ConnectSuccess;
        /// <summary>
        /// 
        /// </summary>
        public async Task Connect()
        {
            var fatalErrorOccured = false;
            //Update my info
            CopyInfo(infoModified, this);
            try
            {
                await socket.ConnectAsync(this.Hostname, this.Port);
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.HostNotFound:
                        message = "No such host is known：" + this.Hostname;
                        break;
                    case SocketError.ConnectionRefused:
                        message = "接続が拒否された。サーバソフトが起動してないか、正常に機能してない。";
                        break;
                    case SocketError.TimedOut:
                        message = "Time out. Portが間違っているかも？";
                        break;
                    case SocketError.IsConnected:
                        message = "このソケットは接続済み。";
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            catch (Exception ex)
            {
                var message = "Not Implemented";
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            if (fatalErrorOccured)
                return;

            //コマンドの送信に成功したらtrueが返ってくるから反転させる必要がある。
            fatalErrorOccured = !await this.SendCmd("NICK mycktai\r\nUSER hosu 8 * :ryruid\r\n");
            if (fatalErrorOccured)
                return;
            if (ConnectSuccess != null)
            {
                ConnectSuccess(this, EventArgs.Empty);
            }
            var buffer = new byte[2048];
            var hostEnc = Encoding.UTF8;
            var sb = new StringBuilder();
            int n = 0;
            do
            {
                try
                {
                    n = await socket.ReceiveAsync(buffer, 0, buffer.Length, System.Net.Sockets.SocketFlags.None);
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
                    ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                    fatalErrorOccured = true;
                }
                catch (Exception ex)
                {
                    var message = "Not Implemented";
                    ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                    fatalErrorOccured = true;
                }
                if (fatalErrorOccured)
                    return;
                var s = hostEnc.GetString(buffer, 0, n);
                sb.Append(s);
                splitBuffer.Add(s.Replace("\0", ""));
            } while (n > 0);
            try
            {
                await socket.DisconnectAsync();
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.NotConnected:
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            catch (Exception ex)
            {
                var message = "Not Implemented";
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
                fatalErrorOccured = true;
            }
            if (!fatalErrorOccured)
                Console.WriteLine("正常終了");
        }
        async void splitBuffer_AddedEvent(object sender, MyLibrary.MySocket.AddedEventArgs e)
        {
            var log = new Irc4.Log(this, e.AddedString);
            if (ReceiveEvent != null)
            {

                var args = new IRCReceiveEventArgs();
                args.text = e.AddedString;
                args.log = log;
                ReceiveEvent(this, args);
            }
            if (log.Command == Command.PING)
            {
                await Pong(log.Sender);
            }
        }
        public async Task<bool> SendCmd(string str)
        {
            var s = str + "\r\n";
            var b = false;
            Encoding enc = Encoding.UTF8;
            try
            {
                await socket.SendAsync(enc.GetBytes(s));
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
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {

            try
            {
                await socket.DisconnectAsync();
            }
            catch (SocketException ex)
            {
                string message = "";
                switch (ex.SocketErrorCode)
                {
                    case SocketError.NotConnected:
                        message = "未接続の状態で切断処理をしようとした。";
                        break;
                    default:
                        message = "Not Implemented";
                        break;
                }
                ExceptionHandler(MyLibrary.LogLevel.error, message, ex);
            }
        }
    }
    [Serializable]
    public class Channel : ChannelInfo, ISec
    {
        public async Task Connect()
        {
            //TODO:
            await Task.Delay(100);
        }
        public void SetInfo(ChannelInfo info)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ISec
    {
        Task Connect();

    }
    public interface IInfo
    {
        /// <summary>
        /// 
        /// </summary>
        string DisplayName { get; set; }
    }
    [Serializable]
    public class ServerInfo : IInfo
    {
        public string DisplayName { get; set; }

        public string Hostname { get; set; }
        public int Port { get; set; }
    }
    [Serializable]
    public class ChannelInfo : IInfo
    {
        public string DisplayName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IRCStringEventArgs : IrcEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string text;
    }
    /// <summary>
    /// 
    /// </summary>
    public class IRCReceiveEventArgs : IRCStringEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Log log;
    }
    public class IrcEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime date;
        /// <summary>
        /// 
        /// </summary>
        //        public IServerChannel serverChannel;
        /// <summary>
        /// 
        /// </summary>
        public MyLibrary.LogLevel logLevel;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReceiveEventHandler(object sender, IRCReceiveEventArgs e);

    public class Parser
    {

    }
}
