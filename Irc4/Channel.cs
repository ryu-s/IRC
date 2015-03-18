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
        /// <returns></returns>
        public async Task Connect()
        {
            //TODO:
            await Task.Delay(100);
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
        public void SetInfo(ChannelInfo info)
        {

        }
    }
}
