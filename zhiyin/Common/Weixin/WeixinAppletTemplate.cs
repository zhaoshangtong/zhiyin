using Newtonsoft.Json.Linq;
using Rays.BLL;
using Rays.Model.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhiyin.Common.Weixin
{
    public class WeixinAppletTemplate
    {
        /// <summary>
        /// 组织发送模板消息内容
        /// </summary>
        /// <param name="weixinApplet">小程序信息</param>
        /// <param name="templateCode">模板编码</param>
        /// <param name="openid">用户openid</param>
        /// <param name="form_id">表单id或支付prepay_id</param>
        /// <param name="data">发送数据，不同内容用;分隔</param>
        /// <param name="path">跳转路径</param>
        /// <returns></returns>
        public static string sendWeixinTemplate(weixin_applet weixinApplet, string templateCode, string openid, string form_id, string data, string path)
        {
            BaseBLL<weixin_applet_template> templateBll = new BaseBLL<weixin_applet_template>();
            weixin_applet_template appletTemplate = templateBll.Find(x => x.template_code == templateCode && x.weixin_applet_id == weixinApplet.id);

            //组织发送的数据
            string[] valueArray = data.Split(';');
            JObject jobject = JObject.Parse("{}");
            for (int i = 0; i < valueArray.Length; i++)
            {
                JObject subObject = new JObject(
                new JProperty("value", valueArray[i]),
                new JProperty("color", "#333")
                );
                jobject.Add("keyword" + (i + 1), subObject);
            }

            WeixinXAPI weixinXApi = new WeixinXAPI(weixinApplet.appid, weixinApplet.secret, weixinApplet.access_token, weixinApplet.access_token_time.ToString(), weixinApplet.id);
            JObject postData = JObject.Parse("{}");
            postData.Add("touser", openid);
            postData.Add("template_id", appletTemplate.template_id);
            postData.Add("page", path);
            postData.Add("form_id", form_id);
            postData.Add("data", jobject);
            return weixinXApi.sendTemplate(postData.ToString());
        }
    }
}