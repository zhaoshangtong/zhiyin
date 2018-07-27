using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Model.DBModels;
using Rays.Utility;
using Zhiyin.Common;
using System.Data;
using Rays.DAL;

/// <summary>
/// 微信小程序 API 调用
/// </summary>
public class WeixinXAPI
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_appid"></param>
    /// <param name="_appsecret"></param>
    public WeixinXAPI(string _appid, string _appsecret)
    {
        AppId = _appid;
        AppSecret = _appsecret;

    }
    public WeixinXAPI(string _appid, string _appsecret, string _access_token, string _access_token_time)
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
                BaseBLL<weixin_applet> baseBLL = new BaseBLL<weixin_applet>();
                var weixin_applet = baseBLL.Find(x => x.appid== AppId&&x.secret== AppSecret);
                if (weixin_applet != null)
                {
                    weixin_applet.access_token = AccessToken;
                    weixin_applet.access_token_time = Convert.ToDateTime(AccessTokenTime);
                    baseBLL.Update(weixin_applet);
                }
                else//有可能是另一个数据库的小程序
                {
                    DataHelper db = new DataHelper();
                    DataTable dt = db.GetDataTable($@"select * from weixin_applet where appid='{AppId}' and secret='{AppSecret}'");
                    if (dt.Rows.Count > 0)
                    {
                        string update_sql = $@"update weixin_applet set access_token='{AccessToken}',access_token_time='{Convert.ToDateTime(AccessTokenTime)}' where appid='{AppId}' and secret='{AppSecret}' ";
                        db.ExcuteSQL(update_sql);
                    }
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

    public WeixinXAPI(string _appid, string _appsecret, string _access_token, string _access_token_time, int _weixin_applet_id)
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
                BaseBLL<weixin_applet> baseBLL = new BaseBLL<weixin_applet>();
                var weixin_applet = baseBLL.Find(x => x.id == _weixin_applet_id);
                if (weixin_applet != null)
                {
                    weixin_applet.access_token = AccessToken;
                    weixin_applet.access_token_time = Convert.ToDateTime(AccessTokenTime);
                    baseBLL.Update(weixin_applet);
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
    /// code 换取 session_key
    /// </summary>
    public string codeToSession(string jscode)
    {
        string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + AppId + "&secret=" + AppSecret + "&js_code=" + jscode + "&grant_type=authorization_code";
        // return MethodGET(url, "UTF-8");
        return HttpHelper.HttpGet(url, "");
    }

    /// <summary>
    /// 创建小程序二维码
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public string CreateQRCode(string path)
    {
        string imgpath = "";
        try
        {
            Dictionary<string, object> datadic = new Dictionary<string, object>();
            datadic.Add("path", path);
            datadic.Add("width", 430);
            string postdata = JsonConvert.SerializeObject(datadic);
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/wxaapp/createwxaqrcode?access_token={0}",
                AccessToken);
            string filename = sys.getRandomStr()+ ".jpg";
            string filefolder = "~/download/applet/";

            MethodJsonPOST(url, postdata, "utf-8", filename, filefolder);


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
        }
        catch (Exception ex)
        {
            rootSuccess = false;
            rootErrMsg = ex.Message;
        }
        return imgpath;
    }

    /// <summary>
    /// 创建圆形小程序码
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns></returns>
    public string CreateWxaCode(string path)
    {
        string imgpath = "";
        try
        {
            Dictionary<string, object> datadic = new Dictionary<string, object>();
            datadic.Add("path", path);
            datadic.Add("width", 430);
            string postdata = JsonConvert.SerializeObject(datadic);
            string url = string.Format("https://api.weixin.qq.com/wxa/getwxacode?access_token={0}",
                AccessToken);
            string filename = sys.getRandomStr() + ".jpg";
            string filefolder = "~/download/applet/";

            MethodJsonPOST(url, postdata, "utf-8", filename, filefolder);


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
        }
        catch (Exception ex)
        {
            rootSuccess = false;
            rootErrMsg = ex.Message;
        }
        return imgpath;
    }

    ///<summary>
    ///采用https协议访问网络
    ///</summary>
    ///<param name="URL">url地址</param>
    ///<param name="strPostdata">发送的数据</param>
    ///<returns></returns>
    public static void MethodJsonPOST(string URL, string strPostdata, string strEncoding, string filename, string filefolder)
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

    ///// <summary>
    ///// UrlEncode
    ///// </summary>
    //public string encode(string content)
    //{
    //    return httpcontext.Server.UrlEncode(content);
    //}


    /////<summary>
    /////采用https协议访问网络
    /////</summary>
    /////<param name="URL">url地址</param>
    /////<param name="strPostdata">发送的数据</param>
    /////<returns></returns>
    //public string MethodPOST(string URL, string strPostdata, string strEncoding)
    //{
    //    Encoding encoding = System.Text.Encoding.GetEncoding(strEncoding);
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
    //    request.Method = "post";
    //    request.Accept = "text/html, application/xhtml+xml, */*";
    //    request.ContentType = "application/x-www-form-urlencoded";
    //    byte[] buffer = encoding.GetBytes(strPostdata);
    //    request.ContentLength = buffer.Length;
    //    request.GetRequestStream().Write(buffer, 0, buffer.Length);
    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //    using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
    //    {
    //        return reader.ReadToEnd();
    //    }
    //}


    ///// <summary>
    ///// 获得本地或远程请求的页面HTML源代码
    ///// </summary>
    //public string MethodGET(string url, string encoding)
    //{

    //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
    //    httpcontext.Response.ContentEncoding = System.Text.Encoding.GetEncoding(encoding);
    //    System.Net.WebResponse response = request.GetResponse();
    //    System.IO.Stream resStream = response.GetResponseStream();
    //    System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(encoding));
    //    string str = sr.ReadToEnd();
    //    resStream.Close();
    //    sr.Close();
    //    return str;
    //}

    ///// <summary>
    ///// 获得时间戳
    ///// </summary>
    //public long getLongTime()
    //{
    //    DateTime timeStamp = new DateTime(1970, 1, 1); //得到1970年的时间戳
    //    long a = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000; //注意这里有时区
    //    return a;
    //}


    //发送模板消息
    public string sendTemplate(string postData)
    {
        LogHelper.Info("sendTemplate:" + postData);
        string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + AccessToken;
        return HttpHelper.HttpPost(url, postData);
    }


}