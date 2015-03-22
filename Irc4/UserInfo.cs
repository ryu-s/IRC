using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ryu_s.MyCommon;
namespace Irc4
{
    delegate void ModeSetDelegate(char mode);
    /// <summary>
    /// ユーザ情報
    /// </summary>
    [Obsolete]
    public class UserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string NickName { set; get; }

        private string _mode = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Mode
        {
            get
            {
                return string.Format("+{0}", _mode);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Full
        {
            get
            {
                if (string.IsNullOrWhiteSpace(NickName) || string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(HostName))
                    return string.Empty;
                return string.Format(":{0}!{1}@{2}", NickName, LoginName, HostName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator ==(UserInfo c1, UserInfo c2)
        {
            //どちらもnull
            if (object.ReferenceEquals(c1, c2))
                return true;
            //どちらかがnull
            if ((object)c1 == null || ((object)c2) == null)
                return false;
            return (c1.NickName == c2.NickName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static bool operator !=(UserInfo c1, UserInfo c2)
        {
            return !(c1 == c2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool Equals(UserInfo u)
        {
            if ((object)u == null)
            {
                return false;
            }
            return (this.NickName == u.NickName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            UserInfo info = obj as UserInfo;
            if ((UserInfo)info == null)
                return false;
            return (info.NickName == this.NickName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.NickName.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname">fullnameまたはnickname</param>
        public UserInfo(string fullname)
        {
            Set(fullname);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="mode"></param>
        public UserInfo(string fullname, string mode)
        {
            Set(fullname);
            SetMode(mode);
        }
        /// <summary>
        /// 
        /// </summary>
        public UserInfo()
        {
            NickName = string.Empty;
            LoginName = string.Empty;
            HostName = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(string mode)
        {
            //mode like "+qov"
            if (mode[0] != '+' && mode[0] != '-')
                return;
            ModeSetDelegate setmode = null;
            for (int i = 0; i < mode.Length; i++)
            {
                switch (mode[i])
                {
                    case '+':
                        setmode = (c) =>
                        {
                            if (_mode.IndexOf(c) < 0)
                                _mode += c;
                        };
                        break;
                    case '-':
                        setmode = (c) =>
                        {
                            if (_mode.IndexOf(c) >= 0)
                                _mode = _mode.Replace(c + "", "");
                        };
                        break;
                    default:
                        setmode(mode[i]);
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public void Set(string sender)
        {
            Sender2UserInfo(sender);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void Sender2UserInfo(string sender)
        {
            NickName = string.Empty;
            LoginName = string.Empty;
            HostName = string.Empty;
            if (string.IsNullOrEmpty(sender))
                return;

            string pattern = @":?(?<nickname>[^\s]*?)!(?<loginname>[^\s]*?)@(?<hostname>[^\s]*?)$";
            var dic = MyRegex.MatchNamedCaptures(pattern, sender);

            if (dic != null)
            {
                NickName = dic["nickname"];
                LoginName = dic["loginname"];
                HostName = dic["hostname"];
            }
            else
            {
                // これで本当に大丈夫だろうか？
                NickName = sender;
            }

            return;
        }
    }
}
