using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 小程序支付
    /// </summary>
    public class WeixinAppletPay
    {
        public string AppId = null;
        public string AppSecret = null;
        public string AccessToken = null;
        public string AccessTokenTime = null;
        public static HttpContext httpcontext = null;
        public string MchId = null;
        public string PartnerKey = null;
        public string CertPath = null;
        public string CertPassword = null;


        public WeixinAppletPay(HttpContext context, string _appid, string _secret, string _mch_id, string _partner_key)
        {
            httpcontext = context;
            AppId = _appid;
            AppSecret = _secret;
            MchId = _mch_id;
            PartnerKey = _partner_key;
        }

        /// <summary>
        /// 返回预支付订单号
        /// 统一支付接口,可受 JSAPI/NATIVE/APP 下预支付订单,返回预支付订单号
        /// </summary>
        public UnifiedOrderReturn UnifiedOrder(UnifiedOrder order)
        {

            //初始化的一些固定的值
            order.appid = AppId;
            order.mch_id = MchId;

            //设置可能中文的字段的UTF8编码格式
            //ordervo.body = HttpUtility.UrlEncode(ordervo.body, System.Text.Encoding.GetEncoding("UTF-8"));

            //设置数据签名
            Hashtable sign_hashtable = Util.voToHashtable(order);
            order.sign = Sign(sign_hashtable);


            //发起远程请求，得到XML数据
            string action = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string data = Util.voToXML(order);
            string xmlData = Util.MethodPOST(action, data, "UTF-8");

            //读取XML文档
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlData);

            return (UnifiedOrderReturn)Util.xmlToVO(xml, "Zhiyin.Common.Weixin.UnifiedOrderReturn");
        }



        /// <summary>
        /// 生成签名
        /// </summary>
        public string Sign(Hashtable parameter)
        {
            string sign_str = "";

            //步骤  a **************************
            //key 排序
            ArrayList arraylist = new ArrayList(parameter.Keys);
            arraylist.Sort();

            int index = 0;
            //遍历已经排序的 数组(keys)
            foreach (string str in arraylist)
            {
                if (Util.isNotNull(parameter[str]))
                {

                    if (index > 0)
                    {
                        sign_str += "&";
                    }

                    sign_str += str + "=" + parameter[str];
                    index++;
                }
            }

            //步骤 b **************************
            sign_str += "&key=" + PartnerKey;
            sign_str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign_str, "md5");
            sign_str = sign_str.ToUpper();

            return sign_str;
        }

        /// <summary>
        /// 返回预支付消息，由创建支付订单时的 notify_url 接收微信支付成功通知    
        /// </summary>
        public static WeixinPayNotify getPayNotify(HttpRequest req)
        {
            WeixinPayNotify notifyvo = new WeixinPayNotify();
            StreamReader reader = new StreamReader(req.InputStream, System.Text.Encoding.GetEncoding("UTF-8"));
            String xmlData = reader.ReadToEnd();
            LogHelper.Info("getPayNotify:" + xmlData);
            if (Util.isNotNull(xmlData))
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlData);
                notifyvo = (WeixinPayNotify)Util.xmlToVO(xml, "Zhiyin.Common.Weixin.WeixinPayNotify");
            }

            return notifyvo;
        }
    }
}