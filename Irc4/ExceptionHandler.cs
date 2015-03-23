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
    public static class MessageHandler
    {
        public delegate void ExceptionOccuredEventHandler(object sender, ExceptionOccuredEventArgs e);
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        public static event ExceptionOccuredEventHandler ExceptionOccured;
        public static event MessageEventHandler MessageEvent;

        public static void OnExceptionOccured(IInfo serverChannel, Log log, Exception ex)
        {
            if (ExceptionOccured != null)
            {
                var args = new ExceptionOccuredEventArgs();
                args.DateTime = DateTime.Now;
                args.Exception = ex;
                ExceptionOccured(serverChannel, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        public static void OnExceptionOccured(object sender, Exception ex, string message = "")
        {
            if (ExceptionOccured != null)
            {
                var args = new ExceptionOccuredEventArgs();
                args.DateTime = DateTime.Now;
                args.Exception = ex;
                args.Message = message;
                ExceptionOccured(sender, args);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public static void OnMessageEvent(object sender, string message)
        {
            if (MessageEvent != null)
            {
                var args = new MessageEventArgs();
                args.DateTime = DateTime.Now;
                args.Message = message;
                MessageEvent(sender, args);
            }
        }
    }
    public class MessageEventArgs : EventArgs
    {
        public DateTime DateTime;
        public string Message;
    }
    public class ExceptionOccuredEventArgs : MessageEventArgs
    {
        public Exception Exception;
    }
}
