using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Irc4
{
    /// <summary>
    /// IRCサーバから受信した文字列を解析する。
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 処理前のサーバから受け取ったそのままの文字列
        /// </summary>
        public string Raw { get; private set; }
        private string _sender;
        /// <summary>
        /// 送信者。これにセットすれば、自動的にSenderInfoにも渡される。
        /// </summary>
        public string Sender
        {
            get { return _sender; }
            private set
            {
                _sender = value;
                if (SenderInfo != null)
                    SenderInfo.Set(_sender);
            }
        }
        /// <summary>
        /// 送信者情報
        /// </summary>
        public UserInfo SenderInfo { get; private set; }
        /// <summary>
        /// コマンド（文字列）。廃止予定。
        /// </summary>
        public string StrCommand { get; private set; }

        /// <summary>
        /// コマンド
        /// </summary>
        public Command Command { get; private set; }

        /// <summary>
        /// 受信者
        /// </summary>
        public string Receiver { get; private set; }
        /// <summary>
        /// 受信者情報
        /// </summary>
        public UserInfo ReceiverInfo { get; private set; }
        /// <summary>
        /// 割りと親切な表示
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// メッセージの宛先のチャンネル名
        /// </summary>
        public string ChannelName { get; private set; }
        /// <summary>
        /// 文字列をspaceを区切り文字として配列化したもの
        /// </summary>
        public string[] Arr { get; private set; }
        /// <summary>
        /// 文字列を受信した時刻
        /// </summary>
        public DateTime Time { get; private set; }
        /// <summary>
        /// 受信したサーバ
        /// </summary>
        public Server Server { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Dictionary { get; private set; }
        struct Reply
        {
            public string Name { get; set; }
            public string Pattern { get; set; }
            public Action<Log, string> Process { get; set; }
        }
        static Reply[] replyList = new Reply[]{
            new Reply{
				Name="PING",
				Pattern=@"(?<command>[^\s]*) :?(?<sender>[^\s]*)$",
				Process=(log, raw)=>{
                    //PING :sjc-chat09.ustream.tv
                    log.Text = log.Dictionary["sender"];
				}
			},
            new Reply{
				Name="PONG",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<from>[^\s]*) ?:?(?<to>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("PONG {0} => {1}", log.Dictionary["from"], log.Dictionary["to"]);
				}
			},
            new Reply{
				Name="ERROR",
				Pattern=@"(?<command>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //ERROR :Ping timeout: 20 seconds
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="PRIVMSG",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) ?:(?<body>.*)$",
				Process=(log, raw)=>{
                    if (IrcTool.IsChannelName(log.Dictionary["receiver"]))
                        log.ChannelName = log.Dictionary["receiver"];
                    else
                        log.Receiver = log.Dictionary["receiver"]; // TALK, etc...
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="NOTICE",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) ?:(?<body>.*)$",
				Process=(log, raw)=>{
                    if (IrcTool.IsChannelName(log.Dictionary["receiver"]))
                        log.ChannelName = log.Dictionary["receiver"];
                    else
                        log.Receiver = log.Dictionary["receiver"]; // TALK
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="QUIT",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} ({1})", log.SenderInfo.NickName, log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="JOIN",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.ChannelName = log.Dictionary["body"];
                    log.Text = log.SenderInfo.NickName + " " + log.ChannelName + " (" + log.SenderInfo.LoginName + "@" + log.SenderInfo.HostName + ")";
				}
			},
            new Reply{
                Name = "PART",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<channel>[^\s]*).*$",
                Process=(log, raw)=>{
                    log.ChannelName = log.Dictionary["channel"];
                    log.Text = string.Format("{0}", log.SenderInfo.NickName);                         
                }
            },
            new Reply{
                Name="KICK",
                Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<channel>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
                Process=(log, raw)=>{
                    log.Text = string.Format("{0} KICK {1}({2})", log.SenderInfo.NickName, log.Receiver, log.Dictionary["body"]);
                }
            },
            new Reply{
                Name="NICK",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) :?(?<newNick>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} → {1}", log.SenderInfo.NickName, log.Dictionary["newNick"]);
				}
			},
            new Reply{
				Name="TOPIC",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<channel>[^\s]*) :(?<topic>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("({0}) {1}", log.SenderInfo.NickName, log.Dictionary["topic"]);
				}
			},
            new Reply{
				Name="MODE",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                        // :ustreamer-91836 MODE ustreamer-91836 :+wx [sender][command][receiver][body]
                        // :chat13.ustream.tv MODE #numazu2525 +ntf [3t]:5 [sender][command][channel][body]
                        // :numazu2525!~numazu252@224.198.192.61.east.global.alpha-net.ne.jp MODE numazu2525 :+i [sender][command][receiver][body]
                        // :chat17.ustream.tv MODE #NORIYUKI8591 +v Gachigaru [sender][command][channel][body]
                        // :namazu2525!namazu2525@dev-88E85483.east.global.alpha-net.ne.jp MODE #namazu2525 +o ustreamer-91838
                        
                        if (IrcTool.IsChannelName(log.Dictionary["receiver"]))
                            log.ChannelName = log.Dictionary["receiver"];
                        else
                            log.Receiver = log.Dictionary["receiver"];
                        string name = (!string.IsNullOrEmpty(log.SenderInfo.NickName)) ? log.SenderInfo.NickName : log.Sender;
                        log.Text = string.Format("{0} {1}",name ,log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="KILL",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<body>.*)$",
				Process=(log, raw)=>{
                            //ネットワークから強制的に追い出す。KICKはチャンネルから。
                            //:sjc-chat01.ustream.tv KILL IG_91835 :chat12!sjc-chat01!sjc-chat01.ustream.tv (Forced nick collision)
                            string pattern2 = @"(?<killedBy>[^\s]*) (?<comments>.*)$";
                            var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                            log.Dictionary.Add("killedBy", dictionary2["killedBy"]);
                            log.Dictionary.Add("comments", dictionary2["comments"]);
                            log.Text = string.Format("{0} KILL {1} {2}", log.Dictionary["killedBy"], log.Dictionary["receiver"], log.Dictionary["comments"]);
				}
			},
            new Reply{
				Name="INVITE",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*)$",
				Process=(log, raw)=>{
                    log.Text = log.Receiver + "は" + log.Sender + "から" + log.ChannelName + "への招待を受けました。";
				}
			},
            new Reply{
				Name="WALLOPS",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) :?(?<message>.*)$",
				Process=(log, raw)=>{
                        log.Text = log.Dictionary["message"];
				}
			},
            new Reply{
				Name="001",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<mynick>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    string pattern2 = @"^.* (?<myname>[^\s]+![^\s]+@[^\s]+).*$";
                            var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                            if (dictionary2 != null)
                            { 
                                log.Dictionary.Add("myname", dictionary2["myname"]);
                            }
                            else
                            {
                                string pattern3 = @".* (?<myname>[^\s]+)$";
                                var dictionary3 = MyLibrary.MyRegex.MatchNamedCaptures(pattern3, log.Dictionary["body"]);
                                if(dictionary3 != null)
                                    log.Dictionary.Add("myname", dictionary3["myname"]);
                            }

                            log.ReceiverInfo = new UserInfo();
                            log.ReceiverInfo.Set(log.Dictionary["myname"]);
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="002",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                            string pattern2 = @".* (?<myhost>[^\s,]*),.* (?<version>[^\s]*)$";
                            var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                            log.Dictionary.Add("myhost", dictionary2["myhost"]);
                            log.Dictionary.Add("version", dictionary2["version"]);

                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="003",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="004",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<server_name>[^\s]*) (?<version>[^\s]*) (?<user_modes>[^\s]*) (?<chan_modes>[^\s]*)$",
				Process=(log, raw)=>{
                            log.Text = string.Format("{0} {1} {2} {3}", log.Dictionary["server_name"], log.Dictionary["version"], log.Dictionary["user_modes"], log.Dictionary["chan_modes"]);

				}
			},
            new Reply{
				Name="005",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<parameters>.*) :(?<message>.*)$",
				Process=(log, raw)=>{
                            //:sjc-chat11.ustream.tv 005 IG_91835 CMDS=KNOCK,MAP,DCCALLOW,USERIP UHNAMES NAMESX SAFELIST HCN MAXCHANNELS=20 CHANLIMIT=#:20 MAXLIST=b:60,e:60,I:60 NICKLEN=30 CHANNELLEN=32 TOPICLEN=307 KICKLEN=307 AWAYLEN=307 :are supported by this server
                            //:sjc-chat11.ustream.tv 005 IG_91835 MAXTARGETS=20 WALLCHOPS WATCH=128 WATCHOPTS=A SILENCE=15 MODES=12 CHANTYPES=# PREFIX=(qaohv)~&@%+ CHANMODES=beI,kfL,ljZ,psmntirRcOAQKVCuzNSMTGYUP NETWORK=Ustream CASEMAPPING=ascii EXTBAN=~,cqnrT ELIST=MNUCT :are supported by this server
                            //:sjc-chat11.ustream.tv 005 IG_91835 STATUStext=~&@%+ EXCEPTS INVEX :are supported by this server

                            //:verne.freenode.net 005 Ryu91835 CHANTYPES=# EXCEPTS INVEX CHANMODES=eIbq,k,flj,CFLMPQScgimnprstz CHANLIMIT=#:120 PREFIX=(ov)@+ MAXLIST=bqeI:100 MODES=4 NETWORK=freenode KNOCK STATUStext=@+ CALLERID=g :are supported by this server
                            //:verne.freenode.net 005 Ryu91835 CASEMAPPING=rfc1459 CHARSET=ascii NICKLEN=16 CHANNELLEN=50 TOPICLEN=390 ETRACE CPRIVlog.text CNOTICE DEAF=D MONITOR=100 FNC TARGMAX=NAMES:1,LIST:1,KICK:1,WHOIS:1,PRIVtext:4,NOTICE:4,ACCEPT:,MONITOR: :are supported by this server
                            //:verne.freenode.net 005 Ryu91835 EXTBAN=$,arxz WHOX CLIENTVER=3.0 SAFELIST ELIST=CTU :are supported by this server

                            //:irc.example.net 005 nickname RFC2812 IRCD=ngIRCd CASEMAPPING=ascii PREFIX=(ov)@+ CHANTYPES=#&+ CHANMODES=beI,k,l,imnOPRstz CHANLIMIT=#&+:10 :are supported on this server
                            //:irc.example.net 005 nickname CHANNELLEN=50 NICKLEN=9 TOPICLEN=490 AWAYLEN=127 KICKLEN=400 MODES=5 MAXLIST=beI:50 EXCEPTS=e INVEX=I PENALTY :are supported on this server

                            var parameters = log.Dictionary["parameters"].Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();
                            foreach (var param in parameters)
                            {
                                int equalIdx = param.IndexOf('=');
                                string key = "";
                                string value = "";
                                if (equalIdx >= 0)
                                {
                                    key = param.Substring(0, equalIdx);
                                    value = param.Substring(equalIdx + 1);
                                    log.Dictionary.Add(key, value);
                                }
                                else
                                {
                                    key = param;
                                    log.Dictionary.Add(param, "");
                                }
                            }
                            log.Text = string.Format("{0} {1}", log.Dictionary["parameters"], log.Dictionary["message"]);

                            //このコマンドは2～3回送られてくる。
                            //parametersに各パラメータの生データが入っている。これらを構造体に入れるかなんかしてServer側に渡さないと。
                            //おそらくだけど、dictionaryで渡すのは良い方法では無いと思う。もっとうまいやり方を考えないと。
                            //メンバにobject型の変数を持たせて構造体を入れようか。
				}
			},
            new Reply{
				Name="020",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:irc.livedoor.ne.jp 020 * :Please wait while we process your connection.
                    log.Text = string.Format("{0}", log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="042",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<id>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:irc.media.kyoto-u.ac.jp 042 ryu91835 392ZA3U9W :your unique ID
                    log.Text = string.Format("{0} :{1}", log.Dictionary["id"], log.Dictionary["body"]);
				}
			},
            new Reply{
                Name = "219",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<what>[^\s]*) :?(?<body>.*)$",
                Process=(log,raw)=>{
                    //:irc.2ch.net 219 ryu91835 * :End of STATS report
                    log.Text = log.Dictionary["body"];
                }
            },
            new Reply{
				Name="250",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    // :irc.example.net 250 ircClient :Highest connection count: 5 (159 connections received)
                    // 接続しすぎぃ↑ってことかな？
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="251",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat12.ustream.tv 251 IG_91835 :There are 49455 users and 181 invisible on 16 servers
                    //:irc.example.net 251 nickname :There are 3 users and 0 services on 1 servers
                    log.Text = log.Dictionary["body"];
                    string pattern2 = @"^.* (?<users>[\d]*) .* (?<invisible>[\d]*) .* (?<servers>[\d]*) .*$";
                    var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                    log.Dictionary.Add("users", dictionary2["users"]);
                    log.Dictionary.Add("invisible", dictionary2["invisible"]);
                    log.Dictionary.Add("servers", dictionary2["servers"]);
				}
			},
            new Reply{
				Name="252",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<operators>[\d]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat07.ustream.tv 252 IG_91835 2 :operator(s) online
                    log.Text = string.Format("{0} {1}", log.Dictionary["operators"], log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="253",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<unknownConnections>[\d]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} {1}", log.Dictionary["unknownConnections"], log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="254",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channels>[\d]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} {1}", log.Dictionary["channels"], log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="255",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                            log.Text = string.Format("{0}", log.Dictionary["body"]);

                            // clients, serversを抽出しないと。
				}
			},
            new Reply{
				Name="265",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0}", log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="266",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0}", log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="301",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) :?(?<message>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0}({1})", log.Dictionary["message"], log.Dictionary["nick"]);
				}
			},
            new Reply{
				Name="302",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<host>.*)$",
				Process=(log, raw)=>{
                    //:irc.2ch.net 302 ryu91835 :ryu91835=+~mTahdLYx3o@245.48.0.110.ap.yournet.ne.jp
                    log.Text = string.Format("{0}", log.Dictionary["host"]);
				}
			},
            new Reply{
				Name="311",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) (?<user>[^\s]*) (?<host>[^\s]*) \* :?(?<real_name>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("nick=\"{0}\" user=\"{1}\" host=\"{2}\" real_name=\"{3}\"", log.Dictionary["nick"], log.Dictionary["user"], log.Dictionary["host"], log.Dictionary["real_name"]);
                }
			},
            new Reply{
				Name="312",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) (?<server>[^\s]*) :?(?<server_info>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("nick='{0}'server='{1}'server_info='{2}'", log.Dictionary["nick"], log.Dictionary["server"], log.Dictionary["server_info"]);
				}
			},
            new Reply{
				Name="313",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["who"] + " " + log.Dictionary["body"];
				}
			},
            new Reply{
				Name="314",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) (?<user>[^\s]*) (?<host>[^\s]*) \* :(?<real_name>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("nick=\"{0}\" user=\"{1}\" host=\"{2}\" real_name=\"{3}\"", log.Dictionary["nick"], log.Dictionary["user"], log.Dictionary["host"], log.Dictionary["real_name"]);
				}
			},
            new Reply{
				Name="315",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="317",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) (?<idleseconds>[^\s]*) (?<signontime>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    DateTime signonTime = MyLibrary.MyTime.FromUnixTime(long.Parse(log.Dictionary["signontime"]));
                    log.Text = string.Format("{0} seconds idle, signon time={1}", log.Dictionary["idleseconds"], signonTime.ToString("yyyy/MM/dd HH:mm:ss"));
				}
			},
            new Reply{
				Name="318",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="319",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];

                    var chanArr = ((string)log.Dictionary["body"]).Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToList();
                    for (int i = 0; i < chanArr.Count; i++)
                        log.Dictionary.Add("channel" + i, chanArr[i]);
				}
			},
            new Reply{
				Name="322",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="323",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="324",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="328",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<channel>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="329",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<createTime>[\d]*)$",
				Process=(log, raw)=>{
                    long unixTime;
                    long.TryParse(log.Dictionary["createTime"], out unixTime);
                    DateTime createTime = MyLibrary.MyTime.FromUnixTime(unixTime);
                    log.Text = "チャンネル作成日時：" + createTime.ToString("yyyy/MM/dd HH:mm:ss");
				}
			},
            new Reply{
				Name="331",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["info"];
				}
			},
            new Reply{
				Name="332",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<topic>.*)$",
				Process=(log, raw)=>{
                    log.Text = log.Dictionary["topic"];
				}
			},
            new Reply{
				Name="333",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<nick>[^\s]*) (?<time>[^\s]*)$",
				Process=(log, raw)=>{
                    log.Dictionary["nick"] = (new UserInfo(log.Dictionary["nick"]).NickName);
                    DateTime topicTime = MyLibrary.MyTime.FromUnixTime(long.Parse(log.Dictionary["time"]));
                    log.Text = string.Format("{0} {1}", log.Dictionary["nick"], topicTime.ToString("yyyy/MM/dd HH:mm:ss"));
                }
			},
            new Reply{
				Name="341",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) (?<channel>[^\s]*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} {1}", log.Dictionary["nick"], log.Dictionary["channel"]);
				}
			},
            new Reply{
				Name="346",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<invitemask>[^\s]*) (?<adduser>[^\s]*) (?<addtime>[^\s]*)$",
				Process=(log, raw)=>{
                    DateTime addTime = MyLibrary.MyTime.FromUnixTime(long.Parse(log.Dictionary["addtime"]));
                    log.Text = string.Format("{0} {1} {2}", log.Dictionary["invitemask"], log.Dictionary["adduser"], addTime.ToString("yyyy/MM/dd HH:mm:ss"));
				}
			},
            new Reply{
				Name="347",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0}", log.Dictionary["info"]);
				}
			},
            new Reply{
				Name="348",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<exceptionmask>[^\s]*) (?<adduser>[^\s]*) (?<addtime>[^\s]*)$",
				Process=(log, raw)=>{
                    DateTime addTime = MyLibrary.MyTime.FromUnixTime(long.Parse(log.Dictionary["addtime"]));
                    log.Text = string.Format("{0} {1} {2}", log.Dictionary["exceptionmask"], log.Dictionary["adduser"], addTime.ToString("yyyy/MM/dd HH:mm:ss"));
				}
			},
            new Reply{
				Name="349",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0}", log.Dictionary["info"]);
				}
			},
            new Reply{
				Name="351",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<version>[^\s]*) (?<server>[^\s]*) :?(?<comments>.*)$",
				Process=(log, raw)=>{
                    log.Text = string.Format("{0} {1} {2}", log.Dictionary["version"], log.Dictionary["server"], log.Dictionary["comments"]);
				}
			},
            new Reply{
				Name="352",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<user>[^\s]*) (?<host>[^\s]*) (?<server>[^\s]*) (?<nickname>[^\s]*) (?<hg>[H|G][\*]?[@|+]?) :(?<hopcount>[\d]*) (?<realname>.*)$",
				Process=(log, raw)=>{
                                                //RPL_WHOREPLY
                            //"<channel> <user> <host> <server> <nickname> <H|G>[*][@|+] :<hopcount> <real name>"
                            //:sjc-chat11.ustream.tv 352 IG_91835 #wgmmma-min IG_91835 dev-6937823.ap.yournet.ne.jp sjc-chat11.ustream.tv IG_91835 H :0 IG_91835
                            //:sjc-chat04.ustream.tv 352 IG_91835 #noriradi NORIRADI dev-EF5F9847.saitama.ocn.ne.jp sjc-chat03.ustream.tv NORIRADI H :2 NORIRADI
                            //:sjc-chat04.ustream.tv 352 IG_91835 #noriradi IG_ma-mi dev-871C9FA5.bbtec.net sjc-chat08.ustream.tv IG_ma-mi H :2 IG_ma-mi
                            log.Text = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", log.Dictionary["channel"], log.Dictionary["user"], log.Dictionary["host"], log.Dictionary["server"], log.Dictionary["nickname"], log.Dictionary["hg"], log.Dictionary["hopcount"], log.Dictionary["realname"]);

				}
			},
            new Reply{
				Name="353",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<unknown>.) (?<channel>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat04.ustream.tv 353 IG_91835 @ #NORIRADI :IG_91835 +IG_72776 +IG_12080 +IG_06068 +IG_00913 +IG_13902 +IG_44566 +IG_10903 +IG_06630 +IG_93801 +IG_69797 +IG_15479 +IG_46367 +IG_15278 +IG_65726 +IG_83885 +IG_24699 +IG_19093 +akakakakakakakakakak +spopopo +IG_61214 +komokomo222 +IG_45353 +IG_25783 +IG_74750 +IG_48967 +IG_68879 +IG_81250 @samon8591 +IG_62501
                    //:leguin.freenode.net 353 Ryu91835 * #haskell :IanKelling cognominal nodogbite Baughn Rodya_ _ikke_ kfish hng apaku codesoup c_wraith pterygota mawuli n4l [swift] srhb predator117 no-n mikeizbicki wjm carter maroloccio2 luzie applybot Cory wchun jedws dropdrive Guest81354 WraithM psquid mister_m rieper meretrix george2 pyon Turl edwardk ec glowcoil khushildep jml DexterLB c74d jedai42 wollw jaimef creichert dsantiago zerokarmaleft wagle hpc favetelinguis tensorpudding Mon_Ouie funfunctor xinming yac
                    string[] list = log.Dictionary["body"].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                    for (int i = 0; i < list.Length; i++)
                    {
                       log.Dictionary.Add("user" + i, list[i]);
                    }
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
                Name = "364",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<host1>[^\s]*) (?<host2>[^\s]*) :?(?<body>.*)$",
                Process=(log,raw)=>{
                    //LINKSのリプライ?
                    //:irc.2ch.net 364 ryu91835 irc.2ch.net irc.2ch.net :0 2ch.net and www.alt-r.com
                    log.Text = string.Format("{0} {1} {2}", log.Dictionary["host1"], log.Dictionary["host2"], log.Dictionary["body"]);
                }
            },
            new Reply{
                Name = "365",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<what>[^\s]*) :?(?<body>.*)$",
                Process=(log,raw)=>{
                    //:irc.2ch.net 365 ryu91835 * :End of LINKS list.
                    log.Text = log.Dictionary["body"];
                }
            },
            new Reply{
				Name="366",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat07.ustream.tv 366 IG_91835 #NORIRADI :End of /NAMES list.
                            log.Text = string.Format("{0}", log.Dictionary["info"]);
				}
			},
            new Reply{
				Name="367",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) (?<banid>[^\s]*)[\s]?(?<adduser>[^\s]*)[\s]?:?(?<addtime>.*)$",
				Process=(log, raw)=>{
                                                //:irc.example.net 367 nick #test Ryu!*@*
                            //<channel> <banid> [<time_left> :<reason>] 
                            string addtime = string.Empty;
                            string adduser = string.Empty;
                            log.Dictionary.TryGetValue("addtime", out addtime);
                            log.Dictionary.TryGetValue("adduser", out adduser);
                            if (!string.IsNullOrEmpty(addtime))
                            {
                                DateTime addDateTime = MyLibrary.MyTime.FromUnixTime(long.Parse(addtime));
                                log.Text = string.Format("{0} {1} {2}", log.Dictionary["banid"], addDateTime.ToString("yyyy/MM/dd HH:mm:ss"), adduser);
                            }
                            else
                                log.Text = string.Format("{0}", log.Dictionary["banid"]);
				}
			},
            new Reply{
				Name="368",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                                                //:irc.example.net 368 nick #test :End of channel ban list
                            log.Text = string.Format("{0}", log.Dictionary["info"]);
				}
			},
            new Reply{
				Name="369",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat09.ustream.tv 369 IG_91835 IG_23680 :End of WHOWAS
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="371",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat10.ustream.tv 371 IG_91835 :=-=-=-= Unreal3.2.8.1 =-=-=-=
                            //:sjc-chat10.ustream.tv 371 IG_91835 :| This release was brought to you by the following people:
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="372",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                                                int len = log.Arr[0].Length + log.Arr[1].Length + log.Arr[2].Length + 3 + 1;//最後の+1はコロン
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="374",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<info>.*)$",
				Process=(log, raw)=>{
                            //:sjc-chat12.ustream.tv 374 IG_91835 :End of /INFO list.
                            log.Text = log.Dictionary["info"];
				}
			},
            new Reply{
				Name="375",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<info>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat01.ustream.tv 375 IG_91835 :- sjc-chat01.ustream.tv Message of the Day - 
                    log.Text = log.Dictionary["info"];
				}
			},
            new Reply{
				Name="376",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat13.ustream.tv 376 IG_91835 :End of /MOTD command.
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="377",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :?(?<info>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat01.ustream.tv 477 ustreamer-91835 #NORIRADI :You need a registered nick to join that channel.
                    log.Text = log.Dictionary["info"];
				}
			},
            new Reply{
				Name="378",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat08.ustream.tv 378 IG_91835 IG_91835 :is connecting from *@119.95.240.49.ap.yournet.ne.jp 49.240.95.119
                            log.Text = log.Dictionary["who"] + " " + log.Dictionary["body"];
                            string pattern2 = @".* \*@(?<userhost>[^\s]*) (?<ipaddr>[^\s]*)$";
                            var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                            log.Dictionary.Add("userhost", dictionary2["userhost"]);
                            log.Dictionary.Add("ipaddr", dictionary2["ipaddr"]);
				}
			},
            new Reply{
				Name="379",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:irc.example.net 379 nick nick :is using modes +
                            log.Text = log.Dictionary["who"] + " " + log.Dictionary["body"];
				}
			},
            new Reply{
				Name="381",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<info>.*)$",
				Process=(log, raw)=>{
                                                //:irc.example.net 381 nick :You are now an IRC Operator
                            log.Text = string.Format("{0}", log.Dictionary["info"]);
				}
			},
            new Reply{
				Name="391",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<server>[^\s]*) :?(?<severlocaltime>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat11.ustream.tv 391 IG_91835 sjc-chat11.ustream.tv :Thursday May 8 2014 -- 09:58 -07:00
                            log.Text = log.Dictionary["severlocaltime"];
				}
			},
            new Reply{
				Name="401",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<nick>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    // 存在しないチャンネルやユーザに向けてコマンドを打った。
                    //:irc.example.net 401 nick irc.example.net :No such nick or channel name
                    log.Text = string.Format("{0} ({1})", log.Dictionary["reason"], log.Dictionary["nick"]);
				}
			},
            new Reply{
				Name="403",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:irc2.2ch.net 403 namazu252 !_P2PQuake :No such channel
                    log.Text = string.Format("{0} :{1}", log.Dictionary["channel"], log.Dictionary["reason"]);
				}
			},
            new Reply{
				Name="404",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:irc.example.net 404 nanashi #test :Cannot send to channel
                    // チャンネルが存在しているけど、JOINしてない時にPRIVlog.textを打ったら出た。
                    log.Text = string.Format("{0} ({1})", log.Dictionary["reason"], log.Dictionary["channel"]);
				}
			},
            new Reply{
				Name="406",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<who>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat09.ustream.tv 406 IG_91835 IG_23680 :There was no such nickname
                            log.Text = string.Format("{0} ({1})", log.Dictionary["body"], log.Dictionary["who"]);
				}
			},
            new Reply{
				Name="409",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                                                //:sjc-chat06.ustream.tv 409 IG_91835 :No origin specified
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="412",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat01.ustream.tv 412 IG_91835 :No log.text to send
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="421",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<unknowncommand>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    // :irc.example.net 421 nickname /join :Unknown command
                    int len = log.Arr[0].Length + log.Arr[1].Length + log.Arr[2].Length + 3;
                    log.Text = string.Format("{0} :{1}", log.Dictionary["unknowncommand"], log.Dictionary["reason"]);
				}
			},
            new Reply{
				Name="431",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                            //:sjc-chat08.ustream.tv 431 IG_91835 :No nickname given
                            log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="433",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) . (?<nick>[^/s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:irc.example.net 433 * user1 :Nickname already in use
                    log.Text = string.Format("{0} :{1}", log.Dictionary["nick"], log.Dictionary["body"]);


				}
			},
            new Reply{
				Name="439",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:irc.lolipower.org 439 * :Please wait while we process your connection.
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="442",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :?(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:chat15.ustream.tv 442 ustreamer-91838 #namazu2525 :You're not on that channel
                    log.Text = string.Format("{0} ({1})", log.Dictionary["reason"], log.ChannelName);
				}
			},
            new Reply{
				Name="445",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat12.ustream.tv 445 IG_91835 :SUMMON has been disabled
                    log.Text = log.Dictionary["reason"];
				}
			},
            new Reply{
                Name = "446",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
                Process=(log,raw)=>{
                    log.Text = log.Dictionary["body"];
                }
            },
            new Reply{
				Name="451",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<what>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat14.ustream.tv 451 JOIN :You have not registered
                    log.Text = string.Format("{0} :{1}", log.Dictionary["what"], log.Dictionary["body"]);
				}
			},
            new Reply{
				Name="461",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<inputcommand>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:irc2.2ch.net 461 namazu JOIN :Not enough parameters
                    log.Text = string.Format("入力されたコマンド:{0} 理由:{1}", log.Dictionary["inputcommand"], log.Dictionary["reason"]);
				}
			},
            new Reply{
                Name="470",
                Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<body>.*)$",
                Process=(log, raw)=>{
                    //:chat12.ustream.tv 470 ustreamer-91838 [Link] #noriyuki8591 has become full, so you are automatically being transferred to the linked channel #noriyuki8591_1
                    log.Text = log.Dictionary["body"];
                    //2014/12/29 パターンが上手く適合していなかったため修正
                    string pattern2 = @".* ?(?<channel_from>[#|&|!][^\s]*) ?[^#&!]* ?(?<channel_to>[#|&|!][^\s]*) ?.*";
                    var dictionary2 = MyLibrary.MyRegex.MatchNamedCaptures(pattern2, log.Dictionary["body"]);
                    log.Dictionary.Add("channel_from", dictionary2["channel_from"]);
                    log.Dictionary.Add("channel_to", dictionary2["channel_to"]);
                }
            },
            new Reply{
				Name="471",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //":chat15.ustream.tv 471 ustreamer-91838 #namazu2525 :Cannot join channel (+l)"
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="472",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<char>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:irc.example.net 472 nick r :is unknown mode char for #test
                    log.Text = string.Format("{0} {1}", log.Dictionary["char"], log.Dictionary["reason"]);
				}
			},
            new Reply{
				Name="473",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:irc.example.net 473 nickname #test :Cannot join channel (+i) -- Invited users only
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
                Name = "474",
                Pattern = @":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<body>.*)$",
                Process = (log,raw) => {
                    //:irc.example.net 474 nick2 #test :Cannot join channel (+b) -- You are banned
                    log.Text = log.Dictionary["body"];
                }
            },
            new Reply{
				Name="481",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :?(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat04.ustream.tv 481 IG_91835 :Permission Denied- You do not have the correct IRC operator privileges
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="482",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) (?<channel>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //:irc.example.net 482 nick #test :You are not channel operator
                    log.Text = log.Dictionary["reason"];
				}
			},
            new Reply{
				Name="487",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<body>.*)$",
				Process=(log, raw)=>{
                    //:sjc-chat04.ustream.tv 487 IG_91835 :ERROR is a server only command
                    log.Text = log.Dictionary["body"];
				}
			},
            new Reply{
				Name="501",
				Pattern=@":?(?<sender>[^\s]*) (?<command>[^\s]*) (?<receiver>[^\s]*) :(?<reason>.*)$",
				Process=(log, raw)=>{
                    //ERR_UMODEUNKNOWNFLAG 
                    //:irc.example.net 501 nickname :Unknown mode
                    log.Text = log.Dictionary["reason"];
				}
			},



        };

        #region override
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Raw;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="log"></param>
        public Log(Server server, string log)
        {
            Sender = string.Empty;
            SenderInfo = new UserInfo();
            StrCommand = string.Empty;
            Receiver = string.Empty;
            ReceiverInfo = null;
            Text = string.Empty;
            ChannelName = string.Empty;
            Dictionary = new Dictionary<string, string>();
            Set(server, log);
        }

        /// <summary>
        /// IRCサーバから送られてきたログを解析する。
        /// </summary>
        /// <param name="server"></param>
        /// <param name="raw"></param>
        private void Set(Server server, string raw)
        {
            try
            {
                this.Server = server;
                Raw = raw;
                Time = DateTime.Now;
                Arr = raw.Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToArray();

                // set command
                if (raw.IndexOf("PING ") == 0)
                    StrCommand = Arr[0];
                else if (raw.IndexOf("ERROR ") == 0)
                    StrCommand = Arr[0];
                else if (raw.IndexOf("NOTICE") == 0)
                    StrCommand = Arr[0];
                else
                    StrCommand = Arr[1];
                Command = IrcTool.GetCommand(StrCommand);
                // whereの仕様上複数が帰ってくることも考慮するようなコードになってしまう。
                // あと、登録してないCommandだった場合のことも考えてこんな感じにしてみた。
                var replys = replyList.Where(reply => reply.Name == StrCommand);
                if (replys.Count() > 0)
                {
                    var replyStruct = replys.First();
                    string pattern = replyStruct.Pattern;
                    Dictionary = MyLibrary.MyRegex.MatchNamedCaptures(pattern, raw);
                    if (Dictionary == null)
                        throw new Exception("パターンにマッチしなかった！ raw=" + raw);
                    if (Dictionary.ContainsKey("sender"))
                        Sender = Dictionary["sender"];
                    if (Dictionary.ContainsKey("receiver"))
                        Receiver = Dictionary["receiver"];
                    if (Dictionary.ContainsKey("channel"))
                        ChannelName = Dictionary["channel"];
                    //リプライ固有の処理
                    replyStruct.Process(this, raw);



                    //大文字小文字混ざってると面倒だから小文字に統一
                    ChannelName = ChannelName.ToLower();
                }
                else
                {
                    // 登録してないCommandだったら
                    StrCommand = "000";
                    using (StreamWriter sw = new StreamWriter(@"C:\log\" + StrCommand + ".txt", true))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + Raw);
                    }
                }




            }
            catch (Exception ex)
            {
                //TODO:
                //IRCExceptionHandler.WriteLog(ex, raw);
            }
            // enum Commnad
            if (Command == Command.UNKNOWN && StrCommand != "000")
            {
                using (StreamWriter sw = new StreamWriter(@"C:\log\unknownCommand.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + StrCommand);
                }
            }
            return;
        }
    }
}
