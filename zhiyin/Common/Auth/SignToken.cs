using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.Controllers;
using ICSharpCode.SharpZipLib.BZip2;
using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Utility;
using Zhiyin.Models;
using Rays.Model.Sys;
using Rays.DAL;

namespace Zhiyin.Common
{
    /// <summary>
    /// 授权
    /// </summary>
    public class SignToken
    {
        /// <summary>
        /// 判断签名是否正确
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="appsecret"></param>
        /// <param name="token"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static bool AuthValidate(string sign, string appsecret, string token, string methodName)
        {
            string websign = appsecret + token + methodName;
            bool pass = sign == Encrypt.EncryptSign(websign);
            return pass;
        }
        /// <summary>
        /// 判断appid是否有效
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        internal static bool CheckAppid(string appid, out string appsecret)
        {
            if (!string.IsNullOrEmpty(appid))
            {
                DataHelper db = new DataHelper("db_codebook");
                object obj = db.GetObject($"select top 1 appsecret from api_author where appid='{appid}'");
                appsecret = obj?.ToString() ?? string.Empty;
                return !string.IsNullOrEmpty(appsecret);
            }
            else
            {
                appsecret = string.Empty;
                return false;
            }

        }
        /// <summary>
        /// 判断appid是否有效
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        internal static bool CheckAppid(string appid)
        {
            if (!string.IsNullOrEmpty(appid))
            {
                DataHelper db = new DataHelper("db_codebook");
                int count = db.ExcuteSQL($"select count(id) from api_author where appid='{appid}'");
                return count > 0;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// //判断token是否有效
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="signtoken"></param>
        /// <returns></returns>
        internal static bool CheckToken(string appid, out string signtoken)
        {
            //判断token是否有效
            signtoken = string.Empty;
            Token token = (Token)HttpRuntime.Cache.Get(appid);

            if (token != null && token.ExpireTime >= DateTime.Now)
            {
                signtoken = token.SignToken.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取sign参数
        /// </summary>
        /// <param name="actionArguments"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSignByParam(Dictionary<string, object> actionArguments, HttpMethod type, string key)
        {
            var sign = "";
            try
            {
                if (type == HttpMethod.Get)
                {
                    var getsign = actionArguments[key];
                    sign = getsign?.ToString() ?? string.Empty;
                }
                else if (type == HttpMethod.Post)
                {
                    foreach (var value in actionArguments.Values)
                    {
                        if (value.GetType() == typeof(JObject))
                        {
                            var jobj = JObject.Parse(value.ToString());
                            if (jobj.Property(key) != null)
                            {
                                sign = jobj.Property(key).Value.ToString();
                            }
                        }
                        else
                        {
                            sign = value.GetType().GetProperty(key) == null
                                ? GetSignByParam(actionArguments, HttpMethod.Get, key)
                                : value.GetType().GetProperty(key).GetValue(value).ToString();
                        }
                    }
                }
                else
                {
                    throw new Exception("暂未开放其它访问方式!");
                }
            }
            catch (Exception)
            {

            }

            return sign;
        }

        /// <summary>
        /// 获取header里的信息
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSignByHeader(HttpRequestHeaders headers, string key)
        {
            string sign = null;
            //用户sign信息，用户sign信息来自调用发起方  
            if (headers.Contains(key))
            {
                sign = headers.GetValues(key).FirstOrDefault();
            }
            return sign;
        }
        /// <summary>
        /// 判断签名是否正确
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static bool AuthValidateV1(string key, string sign)
        {
            DataHelper db = new DataHelper("db_codebook");
            bool pass = false;
            Object obj = db.GetObject($@"select top 1 appsecret from api_author where appid='{key}'");
            if (obj != null)
            {
                string secret = obj?.ToString();
                string websign = Encrypt.Md5Hash(sign);
                pass = websign == secret;
            }
            return pass;
        }

        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval">差值（分钟）</param>
        /// <returns></returns>
        public static bool IsTime(long time, double interval)
        {
            DateTime dt = GetDateTimeFrom1970Ticks(time);
            //取现在时间
            DateTime dt1 = DateTime.Now.AddMinutes(interval);
            DateTime dt2 = DateTime.Now.AddMinutes(interval * -1);
            if (dt > dt2 && dt < dt1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="interval">差值（分钟）</param>
        /// <returns></returns>
        public static bool IsTime2(long time, double interval)
        {
            DateTime dt = GetDateTimeFrom1970Ticks2(time);
            //取现在时间
            DateTime dt1 = DateTime.Now.AddMinutes(interval);
            DateTime dt2 = DateTime.Now.AddMinutes(interval * -1);
            if (dt > dt2 && dt < dt1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTimeFrom1970Ticks(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(timeStamp);
        }
        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTimeFrom1970Ticks2(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddMilliseconds(timeStamp);
        }
        #region 限制接口访问的频率
        public static bool CheckRequestIsValid(ApiMonitorLog monitorLog)
        {
            bool isValid = true;
            DataHelper db = new DataHelper("db_codebook");
            string reqprams = monitorLog.GetCollections(monitorLog.ActionParams);
            string ip = monitorLog.IP;
            string actionname = monitorLog.ActionName;
            string controllname = monitorLog.ControllerName;
            /*同一接口，同一IP，同一请求在10分钟内访问次数不超过500次，
             * 1个小时内不超过4000次，在当天不超过20000次*/

            //判断10分钟内是否超过限制
            int perMinCount = Convert.ToInt32(ConfigurationManager.AppSettings["apiPerTenminCount"]);
            perMinCount = perMinCount == 0 ? 500 : perMinCount;
            //调试数据
            //perMinCount = 6;
            int apiMinCount = db.ExcuteScalarSQL($@"select count(id) from api_monitor_log where 
                 action_name='{actionname}' and controll_name='{controllname}' and params_str='{reqprams}'
                 and ip='{ip}' and datediff(mi,start_time,getdate())<=10 ");
            if (apiMinCount >= perMinCount)
            {
                SignToken.AddInvalidRequest(monitorLog, 0);
                return false;
            }
            //判断1小时内是否超过限制
            int perHourCount = Convert.ToInt32(ConfigurationManager.AppSettings["apiPerHourCount"]);
            perHourCount = perHourCount == 0 ? 4000 : perHourCount;
            //调试数据
            //perHourCount = 6;
            int apiHourCount = db.ExcuteScalarSQL($@"select count(id) from api_monitor_log where 
                 action_name='{actionname}' and controll_name='{controllname}' and params_str='{reqprams}'
                 and ip='{ip}' and datediff(hour,start_time,getdate())<=1 ");
            if (apiHourCount >= perHourCount)
            {
                SignToken.AddInvalidRequest(monitorLog, 1);
                return false;
            }
            //判断当天是否超过限制
            int perDayCount = Convert.ToInt32(ConfigurationManager.AppSettings["apiPerDayCount"]);
            perDayCount = perDayCount == 0 ? 20000 : perDayCount;
            ////调试数据
            //perDayCount = 6;
            int apiDayCount = db.ExcuteScalarSQL($@"select count(id) from api_monitor_log where 
                 action_name='{actionname}' and controll_name='{controllname}' and params_str='{reqprams}'
                 and ip='{ip}' and datediff(day,start_time,getdate())=0 ");
            if (apiDayCount >= perDayCount)
            {
                SignToken.AddInvalidRequest(monitorLog, 2);
                return false;
            }

            return true;
        }

        private static void AddInvalidRequest(ApiMonitorLog monitorLog, int forbid_type)
        {
            DataHelper db = new DataHelper("db_codebook");
            string controll_name = monitorLog.ControllerName;
            string action_name = monitorLog.ActionName;
            string ip = monitorLog.IP;
            string req_params = monitorLog.GetCollections(monitorLog.ActionParams);
            string params_str = req_params.Length > 4000 ? req_params.Substring(0, 4000) : req_params;
            string sql =
                $@"select count(id) from api_invalid_request where 
                 action_name='{action_name}' and controll_name='{controll_name}' and params_str='{params_str}'
                 and ip='{ip}' and forbid_type={forbid_type}";
            if (forbid_type == 0)
            {
                sql += " and datediff(mi,create_time,getdate())<=10 ";
            }
            else if (forbid_type == 1)
            {
                sql += " and datediff(hour,create_time,getdate())<=1 ";
            }
            else if (forbid_type == 2)
            {
                sql += " and datediff(day,create_time,getdate())=0 ";
            }
            int count = db.ExcuteScalarSQL(sql);
            if (count == 0)
            {
                db.Insert("api_invalid_request", new
                {
                    action_name,
                    controll_name,
                    ip,
                    params_str,
                    forbid_type,
                    create_time = DateTime.Now
                });
            }
        }
        #endregion

    }
}