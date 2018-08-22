using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rays.Model.Sys
{
    public class GloabManager
    {
        public const int BEGINPOINT = 1;
        public const int PAGEINDEX = 1;
        public const int PAGESIZE = 10;

        // 数据库连接
        public static readonly string DBCONNECTION;

        // 码书直播回调接口
        public static readonly string CODEBOOKLIVERECEIVE;
        // 蓝海直播回调接口
        public static readonly string BLUESEALIVERECEIVE;

        // 直播弹幕
        public static readonly string SDKAPPID;
        public static readonly string IDENTIFIER;
        public static readonly string ACCOUNTTYPE;

        // 腾讯云
        public static readonly string APPID;
        public static readonly string LIVEBIZID;
        public static readonly string LIVEPUSHKEY;
        public static readonly string LIVEAUTHENKEY;
        public static readonly string SECRETID;
        public static readonly string SECRETKEY;

        // 直播录制视频回调关联资源时的【时间偏移量（单位：小时）】
        public static readonly string BEGINLIVETIMEOFFSET;
        public static readonly string ENDLIVETIMEOFFSET;



        static GloabManager()
        {
            DBCONNECTION = ConfigurationManager.AppSettings.Get("db_live");
            SDKAPPID = ConfigurationManager.AppSettings.Get("sdkappid");
            APPID = ConfigurationManager.AppSettings.Get("AppID");
            LIVEBIZID = ConfigurationManager.AppSettings.Get("LiveBizid");
            CODEBOOKLIVERECEIVE = ConfigurationManager.AppSettings.Get("CodeBookLiveReceive");
            BLUESEALIVERECEIVE = ConfigurationManager.AppSettings.Get("BlueseaLiveReceive");
            LIVEPUSHKEY = ConfigurationManager.AppSettings.Get("LivePushKey");
            LIVEAUTHENKEY = ConfigurationManager.AppSettings.Get("LiveAuthenKey");
            SECRETID = ConfigurationManager.AppSettings.Get("SecretId");
            SECRETKEY = ConfigurationManager.AppSettings.Get("SecretKey");
            IDENTIFIER = ConfigurationManager.AppSettings.Get("identifier");
            ACCOUNTTYPE = ConfigurationManager.AppSettings.Get("accounttype");
            BEGINLIVETIMEOFFSET = ConfigurationManager.AppSettings.Get("BeginLiveTimeOffset");
            ENDLIVETIMEOFFSET = ConfigurationManager.AppSettings.Get("EndLiveTimeOffset");
        }
    }


    public static class GloabErrorMessage
    {
        public const string SELECTERRORMESSAGE = "查询发生错误，请稍后重试";
    }
}
