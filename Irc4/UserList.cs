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
    public class UserList
    {
        private List<UserInfo> list = new List<UserInfo>();
        /// <summary>
        /// 
        /// </summary>
        public UserList()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<UserInfo> GetEnumerator()
        {
            foreach (UserInfo user in this.list)
            {
                yield return user;
            }
        }
        /// <summary>
        /// ユーザの数
        /// </summary>
        public int Count { get { return list.Count; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        public void Add(UserInfo userInfo)
        {
            UserInfo find = Get(userInfo);
            if (find == null)
                list.Add(userInfo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        public void Add(string nickname)
        {
            UserInfo find = Get(nickname);
            if (find == null)
                list.Add(new UserInfo(nickname));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public UserInfo Get(string nickname)
        {
            return list.Find(
                user =>
                {
                    return user.NickName == nickname;
                });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public UserInfo Get(UserInfo userInfo)
        {
            return this.Get(userInfo.NickName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nickname"></param>
        public void Remove(string nickname)
        {
            UserInfo find = Get(nickname);
            if (find != null)
                list.Remove(find);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        public void Remove(UserInfo userInfo)
        {
            this.Remove(userInfo.NickName);
        }
        /// <summary>
        /// nickNameをNickNameに持つユーザがいるか。
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public bool Contains(string nickName)
        {
            return (this.GetUserInfo(nickName) != null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool Contains(UserInfo userInfo)
        {
            return this.Contains(userInfo.NickName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UserInfo this[int index]
        {
            get
            {
                return list[index];
            }
        }
        /// <summary>
        /// nickNameをNickNameに持つユーザを取得。
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns>存在しなかったらnull</returns>
        public UserInfo GetUserInfo(string nickName)
        {
            UserInfo userInfo = null;
            for (int i = 0; i < this.Count; ++i)
            {
                if (this[i].NickName == nickName)
                {
                    userInfo = this[i];
                    break;
                }
            }
            return userInfo;
        }
        /// <summary>
        /// Nicknameを変更する。
        /// </summary>
        /// <param name="oldNick"></param>
        /// <param name="newNick"></param>
        public void ChangeNickname(string oldNick, string newNick)
        {
            UserInfo user = GetUserInfo(oldNick);
            if (user != null)
            {
                user.NickName = newNick;

            }
            else
            {
            }

        }


    }
}
