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
    public static class ExceptionHandler
    {
        public delegate void ExceptionOccuredEventHandler(object sender, ExceptionOccuredEventArgs e);
        public static event ExceptionOccuredEventHandler ExceptionOccured;
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
    }

    public class ExceptionOccuredEventArgs : EventArgs
    {
        public DateTime DateTime;
        public Exception Exception;
        public string Message;
    }
}
