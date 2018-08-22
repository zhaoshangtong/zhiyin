using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 微信支付，统一订单支付的参数值对象
    /// </summary>
    public class UnifiedOrderReturn
    {
        public string return_code { get; set; }//返回状态码
        public string return_msg { get; set; }//返回信息


        //以下字段在return_code为SUCCESS的时候有返回
        public string appid { get; set; }//公众账号ID
        public string mch_id { get; set; }//商户号
        public string device_info { get; set; }//设备号
        public string nonce_str { get; set; }//随机字符串        
        public string sign { get; set; }//签名
        public string result_code { get; set; }//业务结果

        public string err_code { get; set; }//错误代码
        public string err_code_des { get; set; }//错误代码描述


        //以下字段在return_code 和result_code都为SUCCESS的时候有返回        
        public string trade_type { get; set; }//交易类型
        public string prepay_id { get; set; }//预支付ID
        public string code_url { get; set; }//二维码链接
    }
}