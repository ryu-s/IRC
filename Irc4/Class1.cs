using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Irc4
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISec
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task Connect();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task Disconnect();
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IInfo
    {
        /// <summary>
        /// 
        /// </summary>
        string DisplayName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ServerInfo : IInfo
    {
        public string DisplayName { get; set; }

        public string Hostname { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// コードページ。Encodingだとシリアライズできない。確か。
        /// </summary>
        public int CodePage { get; set; }
        
        public string Password { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Realname { get; set; }
        public List<Channel> ChannelList { get; set; }
        public ServerInfo()
        {
            ChannelList = new List<Channel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ServerInfo Clone()
        {
            var work = new ServerInfo();
            work.Clone(this);
            return work;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        public void Clone(ServerInfo src)
        {
            this.DisplayName = src.DisplayName;
            this.Hostname = src.Hostname;
            this.Port = src.Port;
            this.CodePage = src.CodePage;
            this.Password = src.Password;
            this.Nickname = src.Nickname;
            this.Username = src.Username;
            this.Realname = src.Realname;
            this.ChannelList = src.ChannelList;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ChannelInfo : IInfo
    {
        public string DisplayName { get; set; }

        public ChannelInfo Clone()
        {
            var work = new ChannelInfo();
            work.Clone(this);
            return work;
        }
        public void Clone(ChannelInfo src)
        {
            this.DisplayName = src.DisplayName;
        }
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
        public IInfo serverChannel;
        /// <summary>
        /// 
        /// </summary>
        public MyLibrary.LogLevel logLevel;
    }
    public class IrcInfoEventArgs : IrcEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message;
    }
    public class IrcExceptionEventArgs : IrcInfoEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Exception ex;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void IrcEventHandler(object sender, IrcEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ReceiveEventHandler(object sender, IRCReceiveEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void IrcExceptionHandler(object sender, IrcExceptionEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void IrcInfoHandler(object sender, IrcInfoEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class Parser
    {

    }
}
