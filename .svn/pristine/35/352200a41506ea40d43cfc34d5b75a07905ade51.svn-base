using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 微信支付，统一订单支付的参数值对象
    /// </summary>
    public class WeixinPayNotify
    {
        /// <summary>
        /// //返回状态码
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// //返回信息
        /// </summary>
        public string return_msg { get; set; }


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
        public string openid { get; set; }//用户标识
        public string is_subscribe { get; set; }//是否关注公众账号

        public string trade_type { get; set; }//交易类型
        public string bank_type { get; set; }//付款银行
        public int total_fee { get; set; }//订单总金额，单位为分
        public int coupon_fee { get; set; }//现金券支付金额<=订单总金额，订单总金额-现金券金额为现金支付金额
        public string fee_type { get; set; }//货币种类
        public string transaction_id { get; set; }//微信支付订单号
        public string out_trade_no { get; set; }//商户订单号
        public string attach { get; set; }//商家数据包，原样返回
        public string time_end { get; set; }//支付完成时间，格式为yyyyMMddhhmmss
    }
}