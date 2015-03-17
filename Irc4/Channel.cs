using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Irc4
{
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
}
