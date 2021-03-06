﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rays.Model.Sys;
using Zhiyin.Common.Weixin;
using Rays.Model.DBModels;
using Zhiyin.Common;
using Newtonsoft.Json.Linq;
using Rays.Utility;
using Rays.BLL;
using Rays.Utility.DataTables;
using System.Web;

namespace Zhiyin.Controllers.Applet
{
    /// <summary>
    /// 小程序二维码
    /// </summary>
    public class AppletController : ApiController
    {
        /// <summary>
        /// 生产小程序二维码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ApiResult CreateQrCode(dynamic data) 
        {
            ApiResult apiResult = new ApiResult();
            string path = data?.path;
            string appcode = data?.appcode;
            string _code_type = data?.code_type;
            var checkResult = Util.CheckParameters(
               new Parameter { Value = path, Msg = "path 不能为空值" },
               new Parameter { Value = appcode, Msg = "appcode 不能为空值" },
               new Parameter { Value = _code_type, Msg = "code_type 必须是数字类型", Regex = @"^[1-9]\d*$" },
               new Parameter { Value = _code_type, Msg = "code_type 不能为空值" }
               );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }
            int code_type = int.Parse(_code_type);
            if (code_type == 1)
            {
                apiResult.data = WeiXinHelper.CreateWxaCodeByPath(path, appcode);
            }
            else if (code_type == 2)
            {
                apiResult.data = WeiXinHelper.CreateCircleWxaCodeByPath(path, appcode);
            }
            else
            {
                return new ApiResult()
                {
                    success=false,
                    message="参数错误"
                };
            }
            apiResult.success = true;
            apiResult.message = "生成成功";
            return apiResult;
        }

        #region 登陆
        /// <summary>
        /// 微信登录通用方法
        /// </summary>
        /// <param name="data">小程序登录用户数据，JSON格式,code：授权code，userinfo：微信用户信息，appcode：小程序编码(口算ARITHMETIC) </param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult WeixinLogin(dynamic data)
        {
            ApiResult apiResult = new ApiResult();
            var checkResult = Util.CheckParameters(
                new Parameter { Value = data?.code, Msg = "code不能为空" },
                new Parameter { Value = data?.userinfo.ToString(), Msg = "userInfo不能为空" },
                new Parameter { Value = data?.appcode, Msg = "appcode不能为空！" }
                );
            if (!checkResult.OK)
            {
                apiResult.success = false;
                apiResult.status = ApiStatusCode.InvalidParam;
                apiResult.message = checkResult.Msg;
                return apiResult;
            }

            try
            {
                //根据code查找APPID与Secret，获取微信session、openid和unionid
                string appcode = data.appcode.ToString();
                BaseBLL<weixin_applet> weixinAppletBll = new BaseBLL<weixin_applet>();
                weixin_applet weixinApplet = weixinAppletBll.Find(x => x.appcode == appcode);
                WeixinXAPI weixinxapi = new WeixinXAPI(weixinApplet.appid, weixinApplet.secret);
                string str = weixinxapi.codeToSession(data.code.ToString());
                JObject session_json = JObject.Parse(str);
                if (session_json["errcode"].To<string>().IsNotNullAndEmpty())
                {
                    apiResult.success = false;
                    apiResult.status = ApiStatusCode.NotFound;
                    apiResult.message = str;
                    return apiResult;
                }
                string openid = session_json["openid"].ToString();
                string session_key = session_json["session_key"].ToString();
                string unionid = session_json["unionid"].To<string>();
                if (StringHelper.IsNullOrEmpty(unionid) && !StringHelper.IsNullOrEmpty(data?.encryptedData) && !StringHelper.IsNullOrEmpty(data?.iv))
                {
                    string info = DEncrypt.XCXDecrypt(data?.encryptedData.ToString(), session_key, data?.iv.ToString());
                    JObject userInfoJson = JObject.Parse(info);
                    unionid = userInfoJson["unionId"].To<string>();
                }

                weixin_user userInfo = new weixin_user
                {
                    openid = openid,
                    unionid = unionid,
                    nickname = data.userinfo["nickName"],
                    sex = data.userinfo["gender"],
                    language = data.userinfo["language"],
                    city = data.userinfo["city"],
                    province = data.userinfo["province"],
                    country = data.userinfo["country"],
                    headimgurl = data.userinfo["avatarUrl"],
                    source_code = appcode,
                    weixin_applet_id = weixinApplet.id
                };

                if (Util.isNotNull(userInfo.unionid))
                {
                    //查询当前openid的用户是否存在
                    //如果不存在则要创建，创建时，先创建 iuser ,再创建 weixin_user
                    BaseBLL<weixin_user> weixinUserBll = new BaseBLL<weixin_user>();
                    var weixinUser = weixinUserBll.Find(x => x.unionid == unionid);
                    bool first_login = false;
                    //可能是第一次登陆，在网页端登陆
                    if (weixinUser == null)
                    {
                        //微信开发平台的openid与小程序的openid不一致
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
                        LogHelper.Info("first_login:" + first_login + ",userInfo:" + Newtonsoft.Json.JsonConvert.SerializeObject(userInfo));
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
                        success=false,
                        message="unionid不能为空，小程序必须绑定开放平台"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                apiResult.success = false;
                apiResult.status = ApiStatusCode.Error;
            }

            return apiResult;

        }
        #endregion

        /// <summary>
        /// 获取公众号二维码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ApiResult GetWeixinCode(string path)
        {
            ApiResult apiResult = new ApiResult();
            string img_path=WeiXinHelper.CreateWeixinCode("wx7c4153b9f945ed5c", "ede0ea889b14d0e81b465db48d7c2ae3", path);
            if (Util.isNotNull(img_path))
            {
                return new ApiResult()
                {
                    success = true,
                    message = "获取公众号二维码",
                    data = img_path
                };
            }
            apiResult.success = false;
            apiResult.message = "获取公众号二维码失败";
            return apiResult;
        }
    }
}
