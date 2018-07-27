using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Model.DBModels;
using Rays.Model.Sys;
using Rays.Utility;
using Rays.Utility.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Zhiyin.Common.Weixin;
using Zhiyin.Common;
using Zhiyin.Common.Weixin;

namespace Zhiyin.Controllers.WebLogin
{
    /// <summary>
    /// 客户端扫码登陆
    /// </summary>
    public class LoginController : ApiController
    {
        /// <summary>
        /// 扫码登陆请求
        /// 第一步获取返回的code
        /// </summary>
        [HttpGet]
        public void ScanLogin()
        {
            BaseBLL<weixin_open> bll = new BaseBLL<weixin_open>();
            var weixin_open = bll.Find(o => o.appid != null && o.secret != null);
            string appid = weixin_open.appid;
            string secret = weixin_open.secret;
            string server = Util.getServerPath();
            string redirect_uri = System.Web.HttpUtility.UrlEncode("http://zhiyin.whtlkj.net/");
            LogHelper.Info("redirect_uri:" + redirect_uri);
            string state = sys.getRandomCode(16);
            //缓存state
            HttpContext.Current.Session["weixin_login_state"] = state;
            WeixinOpenAPI api = new WeixinOpenAPI(appid, secret);
            LogHelper.Info(api.GetCode("登陆的code:" + redirect_uri, state));
            string result = api.GetCode(redirect_uri, state);
            result = result.Replace("/connect/qrcode/", "https://open.weixin.qq.com/connect/qrcode/");
            HttpContext.Current.Response.Write(result);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 扫码登陆请求(前端生成二维码)
        /// </summary>
        public ApiResult ScanLoginByJs()
        {
            ApiResult apiResult = new ApiResult();
            BaseBLL<weixin_open> bll = new BaseBLL<weixin_open>();
            var weixin_open = bll.Find(o => o.appid != null && o.secret != null);
            string appid = weixin_open.appid;
            string secret = weixin_open.secret;
            string state = sys.getRandomCode(16);
            //使用session存state
            HttpContext.Current.Session["session_weixin_login_state"] = state;
            CookieHelper.SetCookie("cookie_weixin_login_state", state);
            apiResult.success = true;
            apiResult.message = "";
            apiResult.data = new { appid, state };
            return apiResult;
        }
        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="code">请求微信返回的code</param>
        /// <param name="state">请求微信的参数state</param>
        /// <returns></returns>
        public ApiResult LoginReturn(string code, string state)
        {
            ApiResult apiResult = new ApiResult();
            LogHelper.Info("code:" + code + ",state:" + state);
            //必须用cookie或者session
            var session = HttpContext.Current.Session["session_weixin_login_state"];
            string session_state = session == null ? "" : session.ToString();
            string cookie_state = CookieHelper.GetCookieValue("cookie_weixin_login_state");
            LogHelper.Info("session_state:" + session_state + ",cookie_state:" + cookie_state);
            //if (state == _state)
            //{
                BaseBLL<weixin_open> bll = new BaseBLL<weixin_open>();
                var weixin_open = bll.Find(o => o.appid != null && o.secret != null);
                string appid = weixin_open.appid;
                string secret = weixin_open.secret;

                WeixinOpenAPI api = new WeixinOpenAPI(appid, secret);
                //string access_token = weixin_open.access_token;
                //string access_token_time = weixin_open.access_token_time == null ? "" : weixin_open.access_token_time.Value.ToString();

                api.GetAccessToken(code);
                LogHelper.Info("access_token:" + api.access_token);
                string user_json = api.GetUserInfo(api.openid);
                LogHelper.Info("user_json:" + user_json);
                JObject obj = JObject.Parse(user_json);
                string openid = obj["openid"] == null ? "" : obj["openid"].ToString();
                LogHelper.Info("openid:" + openid);
                BaseBLL<weixin_applet> weixinAppletBll = new BaseBLL<weixin_applet>();
                weixin_applet weixinApplet = weixinAppletBll.Find(x => x.appcode == "ZHIYIN");
                weixin_user userInfo = new weixin_user
                {
                    openid = obj["openid"] == null ? "" : obj["openid"].ToString(),
                    unionid = obj["unionid"] == null ? "" : obj["unionid"].ToString(),
                    nickname = obj["nickname"] == null ? "" : obj["nickname"].ToString(),
                    sex = obj["sex"] == null ? 0 : int.Parse(obj["sex"].ToString()),
                    language = obj["language"] == null ? "" : obj["language"].ToString(),
                    city = obj["city"] == null ? "" : obj["city"].ToString(),
                    province = obj["province"] == null ? "" : obj["province"].ToString(),
                    country = obj["country"] == null ? "" : obj["country"].ToString(),
                    headimgurl = obj["headimgurl"] == null ? "" : obj["headimgurl"].ToString(),
                    source_code = weixinApplet.appcode,
                    weixin_applet_id = weixinApplet.id
                };
                if (!Util.isNotNull(openid))
                {
                    return new ApiResult()
                    {
                        success = false,
                        message = "openid为空"
                    };
                }
                #region 微信登陆，保存信息
                //如果不存在则要创建，创建时，先创建 iuser ,再创建 weixin_user
                bool first_login = false;
                BaseBLL<weixin_user> weixinUserBll = new BaseBLL<weixin_user>();
                if (Util.isNotNull(userInfo.unionid))
                {
                    var weixinUser = weixinUserBll.Find(o => o.unionid == userInfo.unionid);
                    //可能是第一次登陆，在网页端登陆
                    if (weixinUser == null)
                    {
                        //微信开发平台的openid与小程序的openid不一致
                        //var _weixin_user = weixinUserBll.Find(o => o.nickname == userInfo.nickname);
                        first_login = true;
                        //先存iuser
                        var iuser = new iuser();
                        BaseBLL<iuser> iuserBll = new BaseBLL<iuser>();
                        iuser.random = sys.getRandomStr();
                        iuser.createtime = DateTime.Now;
                        iuser.updatetime = DateTime.Now;
                        iuser = iuserBll.Add(iuser);
                        //再存weixin_user
                        userInfo.uid = iuser.id;
                        userInfo.sub_time = DateTime.Now;
                        userInfo.first_sub_time = DateTime.Now;
                        LogHelper.Info("first_login:" + first_login + ",userInfo:" +Newtonsoft.Json.JsonConvert.SerializeObject(userInfo));
                        weixinUser = weixinUserBll.Add(userInfo);
                    }
                    else
                    {
                        weixinUser.nickname = userInfo.nickname;
                        weixinUser.headimgurl = userInfo.headimgurl;
                        LogHelper.Info("first_login:" + first_login + ",userInfo:" + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo));
                        weixinUserBll.Update(weixinUser);
                    }

                    apiResult.success = true;
                    apiResult.data = new { first_login = first_login, weixinUser = weixinUser };
                    apiResult.status = ApiStatusCode.OK;
                }
                else
                {
                    return new ApiResult()
                    {
                        success = false,
                        message = "微信开发平台未获取到unionid"
                    };
                }
                #endregion
            //}
            //else
            //{
            //    return new ApiResult()
            //    {
            //        success = false,
            //        message = "请求超时"
            //    };
            //}
            return apiResult;
        }
    }
}
