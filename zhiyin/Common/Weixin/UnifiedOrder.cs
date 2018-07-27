using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 微信支付，统一订单支付的参数值对象
    /// </summary>
    public class UnifiedOrder
    {
        /// <summary>
        /// //公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// //商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// //设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// //随机字符串
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// //签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// //商品描述
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// //附加数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// //商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// //总金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// //终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// //交易起始时间
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// //交易结束时间
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// //商品标记
        /// </summary>
        public string goods_tag { get; set; }
        /// <summary>
        /// //通知地址
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// //交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// //用户标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// //商品ID
        /// </summary>
        public string product_id { get; set; }
    }
}