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
    public enum Command
    {
        /// <summary>
        /// 
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 
        /// </summary>
        ADMIN = 1001,
        /// <summary>
        /// 
        /// </summary>
        AWAY = 1002,
        /// <summary>
        /// 
        /// </summary>
        CNOTICE = 1003,
        /// <summary>
        /// 
        /// </summary>
        CPRIVMSG = 1004,
        /// <summary>
        /// 
        /// </summary>
        CONNECT = 1005,
        /// <summary>
        /// 
        /// </summary>
        DIE,
        /// <summary>
        /// 
        /// </summary>
        ENCAP,
        /// <summary>
        /// 
        /// </summary>
        ERROR,
        /// <summary>
        /// 
        /// </summary>
        HELP,
        /// <summary>
        /// 
        /// </summary>
        INFO,
        /// <summary>
        /// 
        /// </summary>
        INVITE,
        /// <summary>
        /// 
        /// </summary>
        ISON,
        /// <summary>
        /// 
        /// </summary>
        JOIN,
        /// <summary>
        /// 
        /// </summary>
        KICK,
        /// <summary>
        /// 
        /// </summary>
        KILL,
        /// <summary>
        /// 
        /// </summary>
        KNOCK,
        /// <summary>
        /// 
        /// </summary>
        LINKS,
        /// <summary>
        /// 
        /// </summary>
        LIST,
        /// <summary>
        /// 
        /// </summary>
        LUSERS,
        /// <summary>
        /// 
        /// </summary>
        MODE,
        /// <summary>
        /// 
        /// </summary>
        MOTD,
        /// <summary>
        /// 
        /// </summary>
        NAMES,
        /// <summary>
        /// 
        /// </summary>
        NAMESX,
        /// <summary>
        /// 
        /// </summary>
        NICK,
        /// <summary>
        /// 
        /// </summary>
        NOTICE,
        /// <summary>
        /// 
        /// </summary>
        OPER,
        /// <summary>
        /// 
        /// </summary>
        PART,
        /// <summary>
        /// 
        /// </summary>
        PASS,
        /// <summary>
        /// 
        /// </summary>
        PING,
        /// <summary>
        /// 
        /// </summary>
        PONG,
        /// <summary>
        /// 
        /// </summary>
        PRIVMSG,
        /// <summary>
        /// 
        /// </summary>
        QUIT,
        /// <summary>
        /// 
        /// </summary>
        REHASH,
        /// <summary>
        /// 
        /// </summary>
        RESTART,
        /// <summary>
        /// 
        /// </summary>
        RULES,
        /// <summary>
        /// 
        /// </summary>
        SERVER,
        /// <summary>
        /// 
        /// </summary>
        SERVICE,
        /// <summary>
        /// 
        /// </summary>
        SERVLIST,
        /// <summary>
        /// 
        /// </summary>
        SQUERY,
        /// <summary>
        /// 
        /// </summary>
        SETNAME,
        /// <summary>
        /// 
        /// </summary>
        SILENCE,
        /// <summary>
        /// 
        /// </summary>
        STATS,
        /// <summary>
        /// 
        /// </summary>
        SUMMON,
        /// <summary>
        /// 
        /// </summary>
        TIME,
        /// <summary>
        /// 
        /// </summary>
        TOPIC,
        /// <summary>
        /// 
        /// </summary>
        TRACE,
        /// <summary>
        /// 
        /// </summary>
        UHNAMES,
        /// <summary>
        /// 
        /// </summary>
        USER,
        /// <summary>
        /// 
        /// </summary>
        USERHOST,
        /// <summary>
        /// 
        /// </summary>
        USERIP,
        /// <summary>
        /// 
        /// </summary>
        USERS,
        /// <summary>
        /// 
        /// </summary>
        VERSION,
        /// <summary>
        /// 
        /// </summary>
        WALLOPS,
        /// <summary>
        /// 
        /// </summary>
        WATCH,
        /// <summary>
        /// 
        /// </summary>
        WHO,
        /// <summary>
        /// 
        /// </summary>
        WHOIS,
        /// <summary>
        /// 
        /// </summary>
        WHOWAS,
        /// <summary>
        /// 1
        /// </summary>
        RPL_WELCOME = 1,
        /// <summary>
        /// 2
        /// </summary>
        RPL_YOURHOST = 2,
        /// <summary>
        /// 3
        /// </summary>
        RPL_CREATED = 3,
        /// <summary>
        /// 4
        /// </summary>
        RPL_MYINFO = 4,
        /// <summary>
        /// 5
        /// </summary>
        RPL_BOUNCE = 5,
        /// <summary>
        /// 200
        /// </summary>
        RPL_TRACELINK = 200,
        /// <summary>
        /// 201
        /// </summary>
        RPL_TRACECONNECTING = 201,
        /// <summary>
        /// 202
        /// </summary>
        RPL_TRACEHANDSHAKE = 202,
        /// <summary>
        /// 203
        /// </summary>
        RPL_TRACEUNKNOWN = 203,
        /// <summary>
        ///  204 
        /// </summary>
        RPL_TRACEOPERATOR = 204,
        /// <summary>
        ///  205 
        /// </summary>
        RPL_TRACEUSER = 205,
        /// <summary>
        ///  206 
        /// </summary>
        RPL_TRACESERVER = 206,
        /// <summary>
        ///  207 
        /// </summary>
        RPL_TRACESERVICE = 207,
        /// <summary>
        ///  208 
        /// </summary>
        RPL_TRACENEWTYPE = 208,
        /// <summary>
        ///  209 
        /// </summary>
        RPL_TRACECLASS = 209,
        /// <summary>
        ///  210 
        /// </summary>
        RPL_TRACERECONNECT = 210,
        /// <summary>
        ///  211 
        /// </summary>
        RPL_STATSLINKINFO = 211,
        /// <summary>
        ///  212 
        /// </summary>
        RPL_STATSCOMMANDS = 212,
        /// <summary>
        ///  213 
        /// </summary>
        RPL_STATSCLINE = 213,
        /// <summary>
        ///  214 
        /// </summary>
        RPL_STATSNLINE = 214,
        /// <summary>
        ///  215 
        /// </summary>
        RPL_STATSILINE = 215,
        /// <summary>
        ///  216 
        /// </summary>
        RPL_STATSKLINE = 216,
        /// <summary>
        ///  217 
        /// </summary>
        RPL_STATSQLINE = 217,
        /// <summary>
        ///  218 
        /// </summary>
        RPL_STATSYLINE = 218,
        /// <summary>
        ///  219 
        /// </summary>
        RPL_ENDOFSTATS = 219,
        /// <summary>
        ///  221 
        /// </summary>
        RPL_UMODEIS = 221,
        /// <summary>
        ///  231 
        /// </summary>
        RPL_SERVICEINFO = 231,
        /// <summary>
        ///  232 
        /// </summary>
        RPL_ENDOFSERVICES = 232,
        /// <summary>
        ///  233 
        /// </summary>
        RPL_SERVICE = 233,
        /// <summary>
        ///  234 
        /// </summary>
        RPL_SERVLIST = 234,
        /// <summary>
        ///  235 
        /// </summary>
        RPL_SERVLISTEND = 235,
        /// <summary>
        ///  240 
        /// </summary>
        RPL_STATSVLINE = 240,
        /// <summary>
        ///  241 
        /// </summary>
        RPL_STATSLLINE = 241,
        /// <summary>
        ///  242 
        /// </summary>
        RPL_STATSUPTIME = 242,
        /// <summary>
        ///  243 
        /// </summary>
        RPL_STATSOLINE = 243,
        /// <summary>
        ///  244 
        /// </summary>
        RPL_STATSHLINE = 244,
        /// <summary>
        ///  246 
        /// </summary>
        RPL_STATSPING = 246,
        /// <summary>
        ///  247 
        /// </summary>
        RPL_STATSBLINE = 247,
        /// <summary>
        ///  250 
        /// </summary>
        RPL_STATSDLINE = 250,

        /// <summary>
        /// 250
        /// </summary>
        RPL_STATSCONN = 250,
        /// <summary>
        /// 251
        /// </summary>
        RPL_LUSERCLIENT = 251,
        /// <summary>
        /// 252
        /// </summary>
        RPL_LUSEROP = 252,
        /// <summary>
        /// 253
        /// </summary>
        RPL_LUSERUNKNOWN = 253,
        /// <summary>
        /// 254
        /// </summary>
        RPL_LUSERCHANNELS = 254,
        /// <summary>
        /// 255
        /// </summary>
        RPL_LUSERME = 255,
        /// <summary>
        /// 256
        /// </summary>
        RPL_ADMINME = 256,
        /// <summary>
        /// 257
        /// </summary>
        RPL_ADMINLOC1 = 257,
        /// <summary>
        /// 258
        /// </summary>
        RPL_ADMINLOC2 = 258,
        /// <summary>
        /// 259
        /// </summary>
        RPL_ADMINEMAIL = 259,
        /// <summary>
        /// 261
        /// </summary>
        RPL_TRACELOG = 261,
        /// <summary>
        /// 262
        /// </summary>
        RPL_TRACEEND = 262,
        /// <summary>
        /// 263
        /// </summary>
        RPL_TRYAGAIN = 263,
        /// <summary>
        /// 265
        /// </summary>
        RPL_LOCALUSERS = 265,
        /// <summary>
        /// 266
        /// </summary>
        RPL_GLOBALUSERS = 266,
        /// <summary>
        /// 300
        /// </summary>
        RPL_NONE = 300,
        /// <summary>
        /// 301
        /// </summary>
        RPL_AWAY = 301,
        /// <summary>
        /// 302
        /// </summary>
        RPL_USERHOST = 302,
        /// <summary>
        /// 303
        /// </summary>
        RPL_ISON = 303,
        /// <summary>
        /// 305
        /// </summary>
        RPL_UNAWAY = 305,
        /// <summary>
        /// 306
        /// </summary>
        RPL_NOWAWAY = 306,
        /// <summary>
        /// 311
        /// </summary>
        RPL_WHOISUSER = 311,
        /// <summary>
        /// 312
        /// </summary>
        RPL_WHOISSERVER = 312,
        /// <summary>
        /// 313
        /// </summary>
        RPL_WHOISOPERATOR = 313,
        /// <summary>
        /// 314
        /// </summary>
        RPL_WHOWASUSER = 314,
        /// <summary>
        /// 315
        /// </summary>
        RPL_ENDOFWHO = 315,
        /// <summary>
        /// 317
        /// </summary>
        RPL_WHOISIDLE = 317,
        /// <summary>
        /// 318
        /// </summary>
        RPL_ENDOFWHOIS = 318,
        /// <summary>
        /// 319
        /// </summary>
        RPL_WHOISCHANNELS = 319,
        /// <summary>
        /// 321
        /// </summary>
        RPL_LISTSTART = 321,
        /// <summary>
        /// 322
        /// </summary>
        RPL_LIST = 322,
        /// <summary>
        /// 323
        /// </summary>
        RPL_LISTEND = 323,
        /// <summary>
        /// 324
        /// </summary>
        RPL_CHANNELMODEIS = 324,
        /// <summary>
        /// 328
        /// </summary>
        RPL_CHANNEL_URL = 328,
        /// <summary>
        /// 329
        /// </summary>
        RPL_CREATIONTIME = 329,
        /// <summary>
        /// 331
        /// </summary>
        RPL_NOTOPIC = 331,
        /// <summary>
        /// 332
        /// </summary>
        RPL_TOPIC = 332,
        /// <summary>
        /// 333
        /// </summary>
        RPL_TOPICWHOTIME = 333,
        /// <summary>
        /// 341
        /// </summary>
        RPL_INVITING = 341,
        /// <summary>
        /// 342
        /// </summary>
        RPL_SUMMONING = 342,
        /// <summary>
        /// 346
        /// </summary>
        RPL_INVITELIST = 346,
        /// <summary>
        /// 347
        /// </summary>
        RPL_ENDOFINVITELIST = 347,
        /// <summary>
        /// 348
        /// </summary>
        RPL_EXCEPTLIST = 348,
        /// <summary>
        /// 349
        /// </summary>
        RPL_ENDOFEXCEPTLIST = 349,
        /// <summary>
        /// 351
        /// </summary>
        RPL_VERSION = 351,
        /// <summary>
        /// 352
        /// </summary>
        RPL_WHOREPLY = 352,
        /// <summary>
        /// 353
        /// </summary>
        RPL_NAMREPLY = 353,
        /// <summary>
        /// 361
        /// </summary>
        RPL_KILLDONE = 361,
        /// <summary>
        /// 362
        /// </summary>
        RPL_CLOSING = 362,
        /// <summary>
        /// 363
        /// </summary>
        RPL_CLOSEEND = 363,
        /// <summary>
        /// 364
        /// </summary>
        RPL_LINKS = 364,
        /// <summary>
        /// 365
        /// </summary>
        RPL_ENDOFLINKS = 365,
        /// <summary>
        /// 366
        /// </summary>
        RPL_ENDOFNAMES = 366,
        /// <summary>
        /// 367
        /// </summary>
        RPL_BANLIST = 367,
        /// <summary>
        /// 368
        /// </summary>
        RPL_ENDOFBANLIST = 368,
        /// <summary>
        /// 369
        /// </summary>
        RPL_ENDOFWHOWAS = 369,
        /// <summary>
        /// 371
        /// </summary>
        RPL_INFO = 371,
        /// <summary>
        /// 372
        /// </summary>
        RPL_MOTD = 372,
        /// <summary>
        /// 374
        /// </summary>
        RPL_ENDOFINFO = 374,
        /// <summary>
        /// 375
        /// </summary>
        RPL_MOTDSTART = 375,
        /// <summary>
        /// 376
        /// </summary>
        RPL_ENDOFMOTD = 376,
        /// <summary>
        /// 377
        /// </summary>
        RPL_KICKEXPIRED = 377,
        /// <summary>
        /// 378
        /// </summary>
        RPL_WHOISHOST = 378,
        /// <summary>
        /// 379
        /// </summary>
        RPL_WHOISMODES = 379,
        /// <summary>
        /// 381
        /// </summary>
        RPL_YOUREOPER = 381,
        /// <summary>
        /// 382
        /// </summary>
        RPL_REHASHING = 382,
        /// <summary>
        /// 391
        /// </summary>
        RPL_TIME = 391,
        /// <summary>
        /// 392
        /// </summary>
        RPL_USERSSTART = 392,
        /// <summary>
        /// 393
        /// </summary>
        RPL_USERS = 393,
        /// <summary>
        /// 394
        /// </summary>
        RPL_ENDOFUSERS = 394,
        /// <summary>
        /// 395
        /// </summary>
        RPL_NOUSERS = 395,
        /// <summary>
        /// 401
        /// </summary>
        ERR_NOSUCHNICK = 401,
        /// <summary>
        /// 402
        /// </summary>
        ERR_NOSUCHSERVER = 402,
        /// <summary>
        /// 403
        /// </summary>
        ERR_NOSUCHCHANNEL = 403,
        /// <summary>
        /// 404
        /// </summary>
        ERR_CANNOTSENDTOCHAN = 404,
        /// <summary>
        /// 405
        /// </summary>
        ERR_TOOMANYCHANNELS = 405,
        /// <summary>
        /// 406
        /// </summary>
        ERR_WASNOSUCHNICK = 406,
        /// <summary>
        /// 407
        /// </summary>
        ERR_TOOMANYTARGETS = 407,
        /// <summary>
        /// 409
        /// </summary>
        ERR_NOORIGIN = 409,
        /// <summary>
        /// 411
        /// </summary>
        ERR_NORECIPIENT = 411,
        /// <summary>
        /// 412
        /// </summary>
        ERR_NOTEXTTOSEND = 412,
        /// <summary>
        /// 413
        /// </summary>
        ERR_NOTOPLEVEL = 413,
        /// <summary>
        /// 414
        /// </summary>
        ERR_WILDTOPLEVEL = 414,
        /// <summary>
        /// 421
        /// </summary>
        ERR_UNKNOWNCOMMAND = 421,
        /// <summary>
        /// 422
        /// </summary>
        ERR_NOMOTD = 422,
        /// <summary>
        /// 423
        /// </summary>
        ERR_NOADMININFO = 423,
        /// <summary>
        /// 424
        /// </summary>
        ERR_FILEERROR = 424,
        /// <summary>
        /// 431
        /// </summary>
        ERR_NONICKNAMEGIVEN = 431,
        /// <summary>
        /// 432
        /// </summary>
        ERR_ERRONEUSNICKNAME = 432,
        /// <summary>
        /// 433
        /// </summary>
        ERR_NICKNAMEINUSE = 433,
        /// <summary>
        /// 436
        /// </summary>
        ERR_NICKCOLLISION = 436,
        /// <summary>
        /// 441
        /// </summary>
        ERR_USERNOTINCHANNEL = 441,
        /// <summary>
        /// 442
        /// </summary>
        ERR_NOTONCHANNEL = 442,
        /// <summary>
        /// 443
        /// </summary>
        ERR_USERONCHANNEL = 443,
        /// <summary>
        /// 444
        /// </summary>
        ERR_NOLOGIN = 444,
        /// <summary>
        /// 445
        /// </summary>
        ERR_SUMMONDISABLED = 445,
        /// <summary>
        /// 446
        /// </summary>
        ERR_USERSDISABLED = 446,
        /// <summary>
        /// 447
        /// </summary>
        ERR_NONICKCHANGE = 447,
        /// <summary>
        /// 449
        /// </summary>
        ERR_NOTIMPLEMENTED = 449,
        /// <summary>
        /// 451
        /// </summary>
        ERR_NOTREGISTERED = 451,
        /// <summary>
        /// 461
        /// </summary>
        ERR_NEEDMOREPARAMS = 461,
        /// <summary>
        /// 462
        /// </summary>
        ERR_ALREADYREGISTRED = 462,
        /// <summary>
        /// 463
        /// </summary>
        ERR_NOPERMFORHOST = 463,
        /// <summary>
        /// 464
        /// </summary>
        ERR_PASSWDMISMATCH = 464,
        /// <summary>
        /// 465
        /// </summary>
        ERR_YOUREBANNEDCREEP = 465,
        /// <summary>
        /// 467
        /// </summary>
        ERR_KEYSET = 467,
        /// <summary>
        /// 470
        /// </summary>
        ERR_KICKEDFROMCHAN = 470,
        /// <summary>
        /// 471
        /// </summary>
        ERR_CHANNELISFULL = 471,
        /// <summary>
        /// 472
        /// </summary>
        ERR_UNKNOWNMODE = 472,
        /// <summary>
        /// 473
        /// </summary>
        ERR_INVITEONLYCHAN = 473,
        /// <summary>
        /// 474
        /// </summary>
        ERR_BANNEDFROMCHAN = 474,
        /// <summary>
        /// 475
        /// </summary>
        ERR_BADCHANNELKEY = 475,
        /// <summary>
        /// 476
        /// </summary>
        ERR_BADCHANMASK = 476,
        /// <summary>
        /// 481
        /// </summary>
        ERR_NOPRIVILEGES = 481,
        /// <summary>
        /// 482
        /// </summary>
        ERR_CHANOPRIVSNEEDED = 482,
        /// <summary>
        /// 483
        /// </summary>
        ERR_CANTKILLSERVER = 483,
        /// <summary>
        /// 491
        /// </summary>
        ERR_NOOPERHOST = 491,
        /// <summary>
        /// 501
        /// </summary>
        ERR_UMODEUNKNOWNFLAG = 501,
        /// <summary>
        /// 502
        /// </summary>
        ERR_USERSDONTMATCH = 502,
    }
}
