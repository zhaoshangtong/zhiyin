using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace Rays.Model.Sys
{
    /// <summary>
    /// 返回JSON数据
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ApiStatusCode status { get; set; }
        /// <summary>
        /// 返回成功与否
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 返回信息，如果失败则是错误提示
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public object data { get; set; }
    }

    /// <summary>
    /// 返回分页JSON数据
    /// </summary>
    public class ApiPageResult : ApiResult
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 总数据数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageTotal
        {
            get
            {
                return pageSize != 0 ? totalCount % pageSize == 0 ? (totalCount / pageSize) : (totalCount / pageSize + 1) : 0;
            }
        }
    }

    /// <summary>
    /// 返回分页JSON数据
    /// </summary>
    public class ApiPageResult2 : ApiResult
    {
        /// <summary>
        /// 开始下标
        /// </summary>
        public int beginPoint { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 总数据数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 当前 Data 的 Count
        /// </summary>
        public int currentCount
        {
            get
            {
                if (data is DataTable)
                {
                    return ((DataTable)data)?.Rows?.Count ?? 0;
                }
                if (data is IEnumerable<object>)
                {
                    var data1 = data as IEnumerable<object>;
                    return (data1 != null && data1.Any()) ? data1.Count() : 0;
                }
                return 0;
            }
        }
    }

    /// <summary>
    /// 返回JSON数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public new T data { get; set; }
    }

    /// <summary>
    /// 返回分页JSON数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiPageResult<T> : ApiPageResult
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public new T data { get; set; }
    }
    #region 自定义返回状态值
    /// <summary>
    /// 自定义返回状态值（可自行扩展自己需要的返回码）
    /// </summary>
    public enum ApiStatusCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        OK = 0,
        /// <summary>
        /// 系统内部错误(代码错误)
        /// </summary>
        [Description("系统内部错误")]
        Error = 400,

        // 授权相关
        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("没有权限")]
        Unauthorized = 1001,
        /// <summary>
        /// 授权用户不存在！
        /// </summary>
        [Description("授权用户不存在！")]
        InvalidClient = 1002,
        /// <summary>
        /// Token过期
        /// </summary>
        [Description("Token过期")]
        InvalidToken = 1003,
        /// <summary>
        /// 签名错误
        /// </summary>
        [Description("签名错误")]
        InvalidSign = 1004,
        /// <summary>
        /// 无效的时间戳
        /// </summary>
        [Description("无效的时间戳")]
        InvalidTimeStamp = 1005,

        /// <summary>
        /// 访问次数超过限制
        /// </summary>
        [Description("访问次数超过限制")]
        LimitRequest = 1006,

        // 业务逻辑错误状态码 3000-3999
        /// <summary>
        /// 数据已存在
        /// </summary>
        [Description("数据已存在")]
        DataExisted = 3001,
        /// <summary>
        /// 数据不存在
        /// </summary>
        [Description("数据不存在")]
        NotFound = 3002,
        /// <summary>
        /// 参数错误(invalid parameter)
        /// </summary>
        [Description("参数错误")]
        InvalidParam = 3003,

        // 系统异常 5000-5999
        /// <summary>
        /// 内部系统异常
        /// </summary>
        [Description("内部系统异常")]
        InternalServerError = 5001,

        [Description("免费订单免支付")]
        FreeBuy = 1007

    }
    #endregion
}