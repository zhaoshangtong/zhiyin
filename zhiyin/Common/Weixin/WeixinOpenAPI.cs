using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zhiyin.Common;

namespace Zhiyin.Common.Weixin
{
    public class WeixinOpenAPI
    {
        public string appid { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
        public string access_token_time { get; set; }//过期时间
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public WeixinOpenAPI(string _appid,string _secret)
        {
            appid = _appid;
            secret = _secret;
        }


        #region 微信扫码登陆
        /// <summary>
        /// 第一步：请求CODE
        /// </summary>
        /// <param name="redirect_uri">回调域名</param>
        ///  <param name="state"></param>
        public  string GetCode(string redirect_uri,string state)
        {
            string url = "https://open.weixin.qq.com/connect/qrconnect?appid=" + appid + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_login&state=" + state + "#wechat_redirect";
            string s = Util.MethodGET(url);
            return s;
        }
        /// <summary>
        /// 第二步：获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public void GetAccessToken(string code)
        {
            string s = Util.MethodGET("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code", "UTF-8");
            try
            {
                LogHelper.Info("code换取access_token:" + s);
                JObject jo = JObject.Parse(s);
                access_token = jo["access_token"].ToString();
                access_token_time = System.DateTime.Now.AddSeconds(double.Parse(jo["expires_in"].ToString())).ToString();
                refresh_token = jo["refresh_token"].ToString();
                openid = jo["openid"].ToString();
                //将获取的最新 AccessToken 保存到数据库中  （反正每次都要请求新的，就不存了）
                //BaseBLL<weixin_open> baseBLL = new BaseBLL<weixin_open>();
                //var weixin_open = baseBLL.Find(x => x.appid == appid && x.secret == secret);
                //if (weixin_open != null)
                //{
                //    weixin_open.access_token = access_token;
                //    weixin_open.access_token_time = Convert.ToDateTime(access_token_time);
                //    weixin_open.refresh_token = refresh_token;
                //    baseBLL.Update(weixin_open);
                //}
            }
            catch (Exception e)
            {
                LogHelper.Error("获取微信开发平台access_token失败：" + e.Message);
            }
        }
        /// <summary>
        /// 刷新或续期access_token使用
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public string GetOpenId(string appid,string refresh_token)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid="+appid+"&grant_type=refresh_token&refresh_token="+ refresh_token;
            return Util.MethodGET(url, "UTF-8");
        }
        /// <summary>
        /// 第三步：获取用户信息
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo(string openid)
        {
            string url = "https://api.weixin.qq.com/sns/userinfo?access_token="+access_token+"&openid="+openid;
            return Util.MethodGET(url, "UTF-8");
        }
        #endregion
    }
}