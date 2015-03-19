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
    [Serializable]
    public class Channel : ChannelInfo, ISec
    {
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
        /// 
        /// </summary>
        /// <param name="log"></param>
        public void QuitHandler(Log log)
        {
            if (log == null || log.Command != Command.QUIT)
                throw new ArgumentException("log");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public void NickHandler(Log log)
        {
            if (log == null || log.Command != Command.NICK)
                throw new ArgumentException("log");

        }
        public void SetLog(Log log)
        {

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
