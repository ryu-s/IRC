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
        public void SetInfo(ChannelInfo info)
        {

        }
    }
}
