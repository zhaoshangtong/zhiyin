using Newtonsoft.Json.Linq;
using Rays.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zhiyin.Common.Weixin
{
    /// <summary>
    /// 微信小程序数据统计分析
    /// </summary>
    public class WeixinAppletDataCount
    {
        public string access_token { get; set; }
        public WeixinAppletDataCount(string _access_token)
        {
            access_token = _access_token;
        }
        /// <summary>
        ///  概况趋势--用户访问小程序的详细数据可从访问分析中获取，概况中提供累计用户数等部分指标数据。
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期，限定查询1天数据，end_date允许设置的最大值为昨日</param>
        /// <returns></returns>
        public string GetDailySummaryTrend(string startDate, string endDate)
        {
            string url ="https://api.weixin.qq.com/datacube/getweanalysisappiddailysummarytrend?access_token="+ access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return  HttpHelper.HttpPost(url, postData);
            
        }
        /// <summary>
        /// 访问趋势--日趋势
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期，限定查询1天数据，end_date允许设置的最大值为昨日</param>
        /// <returns></returns>
        public string GetDailyVisitTrend(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappiddailyvisittrend?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }

        /// <summary>
        /// 访问趋势--周趋势
        /// </summary>
        /// <param name="startDate">开始日期，为周一日期</param>
        /// <param name="endDate">结束日期，为周日日期，限定查询一周数据</param>
        /// <returns></returns>
        public string  GetWeeklyVisitTrend(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappidweeklyvisittrend?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }
        /// <summary>
        /// 访问趋势--月趋势
        /// </summary>
        /// <param name="startDate">开始日期，为自然月第一天</param>
        /// <param name="endDate">结束日期，为自然月最后一天，限定查询一个月数据</param>
        /// <returns></returns>
        public string GetMonthVisitTrend(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappidmonthlyvisittrend?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }
        /// <summary>
        /// 访问分布
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期，限定查询1天数据，end_date允许设置的最大值为昨日</param>
        /// <returns></returns>
        public string GetVisitDistribution(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappidvisitdistribution?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return  HttpHelper.HttpPost(url, postData);
        }

        /// <summary>
        /// 访问留存--日留存
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期，限定查询1天数据，end_date允许设置的最大值为昨日</param>
        /// <returns></returns>
        public string GetDailyRetainInfo(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappiddailyretaininfo?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }
        /// <summary>
        /// 访问留存--周留存
        /// </summary>
        /// <param name="startDate">开始日期，为周一日期</param>
        /// <param name="endDate">结束日期，为周日日期，限定查询一周数据</param>
        /// <returns></returns>
        public string GetWeeklyRetainInfo(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappidweeklyretaininfo?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }
        /// <summary>
        /// 访问留存--月留存
        /// </summary>
        /// <param name="startDate">开始日期，为周一日期</param>
        /// <param name="endDate">结束日期，为周日日期，限定查询一周数据</param>
        /// <returns></returns>
        public string GetMonthRetainInfo(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappidmonthlyretaininfo?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }
        /// <summary>
        /// 用户画像
        /// </summary>
        /// <param name="startDate">开始日期，为周一日期</param>
        /// <param name="endDate">结束日期，开始日期与结束日期相差的天数限定为0/6/29，分别表示查询最近1/7/30天数据，end_date允许设置的最大值为昨日</param>
        /// <returns></returns>
        public string GetUserPortrait(string startDate, string endDate)
        {
            string url = "https://api.weixin.qq.com/datacube/getweanalysisappiduserportrait?access_token=" + access_token;
            string postData = "{ \"begin_date\" :\"" + startDate + "\" ,\"end_date\" :\"" + endDate + "\"}";
            return HttpHelper.HttpPost(url, postData);
        }

    }
}