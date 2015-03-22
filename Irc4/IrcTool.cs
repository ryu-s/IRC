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
    public static class IrcTool
    {
        /// <summary>
        /// コマンド名の文字列をenum Commandに変換する。
        /// </summary>
        /// <remarks>まず文字列が数字かどうか見て、数字なら数値に変換してから対応するCommandに変換。
        /// 文字なら表示名がそれのCommandに変換。</remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Command GetCommand(string command)
        {
            // commandが数字だったら、それを数値に変換したもの
            int num = 0;
            if (int.TryParse(command, out num))
            {
                // commandが数字
                if (Enum.IsDefined(typeof(Command), num))
                {
                    Command c = (Command)Enum.ToObject(typeof(Command), num);
                    return c;
                }
            }
            else
            {
                // 
                Command enumCommand;
                if (Enum.TryParse<Command>(command, out enumCommand))
                {
                    return enumCommand;
                }
            }
            return Command.UNKNOWN;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string GetCommandStr(Command command)
        {
            var name = Enum.GetName(typeof(Command), command);
            var str = "";
            if(name.StartsWith("ERR_") || name.StartsWith("RPL_"))
            {
                str = string.Format("{0:000}", (int)command);
            }
            else
            {
                str = name;
            }
            return str;
        }
        /// <summary>
        /// 文字列がチャンネル名か（RFCのチャンネル名に適合しているか）
        /// </summary>
        /// <param name="channelName"></param>
        public static bool IsChannelName(string channelName)
        {
            if (string.IsNullOrWhiteSpace(channelName))
                return false;
            switch (channelName[0])
            {
                case '&': //LOCAL。作られたサーバでのみ存在。
                case '#': //NETWORK。全サーバから接続可能。
                case '!': //NETWORK_SAFE。
                case '+': //NETWORK_UNMODERATED
                    return true;
                default:
                    return false;
            }
        }
    }
}
