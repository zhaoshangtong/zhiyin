using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Model.DBModels;
using Rays.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Zhiyin.Common;

namespace ZAhiyin.Common.Weixin
{
    public class WeixinAPI
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId = null;// "wx35eeddd265cce37e";
                                   /// <summary>
                                   /// 小程序密钥
                                   /// </summary>
        public string AppSecret = null;//"d0bd38c0070a56a70c5f37c8832ddc70";
                                       /// <summary>
                                       /// 小程序token
                                       /// </summary>
        public string AccessToken = null;
        /// <summary>
        /// token过期时间
        /// </summary>
        public string AccessTokenTime = null;

        public bool rootSuccess = false;
        public string resource = "";
        public string rootErrMsg = "";
        /// <summary>
        /// 
        /// </summary>
        public static HttpContext httpcontext = null;

        public WeixinAPI()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_appid"></param>
        /// <param name="_appsecret"></param>
        public WeixinAPI(string _appid, string _appsecret)
        {
            AppId = _appid;
            AppSecret = _appsecret;

        }
        public WeixinAPI(string _appid, string _appsecret, string _access_token, string _access_token_time)
        {
            rootSuccess = true;
            AppId = _appid;
            AppSecret = _appsecret;
            AccessToken = _access_token;
            AccessTokenTime = _access_token_time;
            //检查授权有效期，如果凭证为空，或者已失效，则需要请求授权
            DateTime time = DateTime.Now;

            if (!string.IsNullOrEmpty(_access_token_time))
            {
                time = DateTime.Parse(_access_token_time);
            }

            int comtime = (int)time.Subtract(DateTime.Now).TotalMinutes;

            //凭证在5分钟以内将失效，则重新获取凭证
            if (comtime < 5)
            {

                string s = Util.MethodGET("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret, "UTF-8");
                try
                {
                    JObject jo = JObject.Parse(s);
                    AccessToken = jo["access_token"].ToString();
                    AccessTokenTime = System.DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString())).ToString();

                    //将获取的最新 AccessToken 保存到数据库中
                    BaseBLL<weixin> baseBLL = new BaseBLL<weixin>();
                    var weixin_1 = baseBLL.Find(x => x.appid == _appid&&x.appsecret==_appsecret);
                    if (weixin_1 != null)
                    {
                        weixin_1.access_token = AccessToken;
                        weixin_1.access_token_time = Convert.ToDateTime(AccessTokenTime);
                        baseBLL.Update(weixin_1);
                    }

                }
                catch (Exception e)
                {
                    rootSuccess = false;
                    rootErrMsg = e.Message;
                }
                resource = s;

            }
        }

        /// <summary>
        /// 获取公众号二维码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public string GetWeixinCode(string ticket)
        {
            string imgpath = "";
            string filename = sys.getRandomStr() + ".jpg";
            string filefolder = "~/download/weixin/";
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath(filefolder), filename);
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            string url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="+ HttpUtility.UrlEncode(ticket);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream sm = response.GetResponseStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(sm);
            img.Save(filePath);
            sm.Flush();
            sm.Close();

            string domain = ConfigurationManager.AppSettings["FILESERVERADDRESS"].ToString();
            string folder = "applet/";
            //上传文件到文件服务器并取得返回地址
            FileInfo file = new FileInfo(Path.Combine(HttpContext.Current.Server.MapPath(filefolder), filename));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("folder", folder);
            string returnstr = Util.HttpPostFile(domain, file, dic);
            Dictionary<string, object> returnobj = JsonConvert.DeserializeObject<Dictionary<string, object>>(returnstr);
            bool success = Convert.ToBoolean(returnobj["success"]);
            if (success)
            {
                imgpath = returnobj["path"].ToString();
            };
            return imgpath;
        }





        ///<summary>
        ///采用https协议访问网络
        ///</summary>
        ///<param name="URL">url地址</param>
        ///<param name="strPostdata">发送的数据</param>
        ///<returns></returns>
        public void MethodJsonPOST(string URL, string strPostdata, string strEncoding, string filename, string filefolder)
        {
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath(filefolder), filename);
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            Encoding encoding = System.Text.Encoding.GetEncoding(strEncoding);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "post";
            request.ContentType = "application/json;charset=" + strEncoding.ToUpper();
            byte[] buffer = encoding.GetBytes(strPostdata);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream sm = response.GetResponseStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(sm);
            img.Save(filePath);
            sm.Flush();
            sm.Close();
        }

    }
}