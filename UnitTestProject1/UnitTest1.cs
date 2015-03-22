using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        string displayName_original = "DisplayName_original";
        string displayName_modified = "DisplayName_modified";
        string hostname_original = "chat1.ustream.tv";
        string hostname_modified = "123.456.789";
        int port_original = 6667;
        int port_modified = 8000;
        /// <summary>
        /// 接続の前後で設定値が正常に反映されるかテスト
        /// </summary>
        [TestMethod]
        public async Task TestMethod1()
        {            
            var server = new Irc4.Server();
            var privateObj = new PrivateObject(server);
            server.SetInfo(GetServerInfo());
            server.ConnectSuccess += async (sender, e) =>
            {
                //単に接続しただけ。値が変化するはずは無い。
                Assert.AreEqual<string>(displayName_original, server.DisplayName);
                Assert.AreEqual<string>(hostname_original, server.Hostname);
                Assert.AreEqual<int>(port_original, server.Port);

                //値の変更
                server.DisplayName = displayName_modified;
                server.Hostname = hostname_modified;
                server.Port = port_modified;

                //接続中であるため、DisplayName以外は変更できないはず。
                Assert.AreEqual<string>(displayName_modified, server.DisplayName);//DisplayNameは接続状態に関係なく変更していい。
                Assert.AreEqual<string>(hostname_original, server.Hostname);
                Assert.AreEqual<int>(port_original, server.Port);

                //切断
                await server.Disconnect();
            };
            server.Disconnected += (sender, e) =>
            {
                //切断後。変更が反映される。
                Assert.AreEqual<string>(displayName_modified, server.DisplayName);
                Assert.AreEqual<string>(hostname_modified, server.Hostname);
                Assert.AreEqual<int>(port_modified, server.Port);
            };
            Assert.AreEqual<string>(displayName_original, server.DisplayName);
            Assert.AreEqual<string>(hostname_original, server.Hostname);
            Assert.AreEqual<int>(port_original, server.Port);

            //接続
            await server.Connect();
        }

        private Irc4.ServerInfo GetServerInfo()
        {
            var info = new Irc4.ServerInfo();
            info.DisplayName = displayName_original;
            info.Hostname = hostname_original;
            info.Nickname = "Nickname_original";
            info.Password = "Password_original";
            info.Port = port_original;
            info.Realname = "Realname_original";
            info.Username = "Username_original";
            info.ChannelList = new Irc4.ISec[0];
            info.CodePage = Encoding.UTF8.CodePage;

            return info;
        }
    }
}
