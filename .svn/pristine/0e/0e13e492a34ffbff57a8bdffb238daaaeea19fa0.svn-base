using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using Rays.Model.Sys;
using Zhiyin.Common;

namespace Listening.Filter
{
    /// <summary>
    /// 安全授权过滤器------2018-07-13
    /// </summary>
    public class ApiAuth2Attribute : ActionFilterAttribute
    {
        /// <summary>
        /// 是否需要验证
        /// </summary>
        public bool isCheck { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="is_check"></param>
        public ApiAuth2Attribute(bool is_check = false)
        {
            isCheck = is_check;
        }
        /// <summary>
        /// 在请求头中加入 sign,timestamp,appcode
        /// sign:方法名与方法参数的集合排序后通过&连接的字符串
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //如果不需要检测直接返回
            if (!isCheck)
                return;
            ApiResult resultMsg = new ApiResult();
            List<string> list_params = new List<string>();
            string action_name = actionContext.ActionDescriptor.ActionName;
            list_params.Add("method=" + action_name);
            //参数在queryparam上
            var action_params = actionContext.ActionArguments;
            foreach (var param in action_params)
            {
                list_params.Add(param.Key + "=" + param.Value);
            }
            var headers = actionContext.Request.Content.Headers;
            //如果是multipart/form-data请求
            if (actionContext.Request.Content.IsMimeMultipartContent())
            {
                var formdata = actionContext.Request.Content.ReadAsMultipartAsync().Result.Contents;
                foreach (var form in formdata)
                {
                    //文件不加入到加密字符串中
                    if (Util.isNotNull(form.Headers.ContentDisposition.FileName))
                    {
                        continue;
                    }
                    var key = form.Headers.ContentDisposition.Name.Replace("\"", "");
                    string val = form.ReadAsStringAsync().Result;
                    list_params.Add(key + "=" + val);
                }
            }
            if (Util.isNotNull(headers.ContentType))
            {
                //各种请求类型(目前只支持下面三种和上面的formdata)
                if (headers.ContentType.ToString() == "application/json")
                {
                    var result = actionContext.Request.Content.ReadAsStringAsync().Result;
                    list_params.Add(result);
                }
                else if (headers.ContentType.ToString() == "text/plain; charset=UTF-8")
                {
                    var result = actionContext.Request.Content.ReadAsStringAsync().Result;
                    list_params.Add(result);
                }
                else if (headers.ContentType.ToString() == "application/x-www-form-urlencoded")
                {
                    var result = actionContext.Request.Content.ReadAsStringAsync().Result;
                    var _params = result.Split('&');
                    list_params.AddRange(_params);
                }
            }

            //加时间戳
            long timespan_now = Util.GetTimestamp(DateTime.Now);
            list_params.Add("timestamp=" + timespan_now / (1000 * 60));
            list_params.Sort(); //排序
            //需要加密的字段
            string sign_string = string.Join("&", list_params);
            //从header中读取sign,timestamp.sign是方法名与方法参数的集合排序后通过&连接的字符串
            string sign = SignToken.GetSignByHeader(actionContext.Request.Headers, "sign");
            //string timestamp = SignToken.GetSignByHeader(actionContext.Request.Headers, "timestamp");
            //判断请求头是否包含以下参数
            if (string.IsNullOrEmpty(sign))
            {
                resultMsg = new ApiResult { success = false, status = ApiStatusCode.InvalidParam, message = "请求头中缺少参数数据！" };
            }
            else
            {
                //服务端加密后的参数
                string server_sign = Util.MD5Encrypt(sign_string).ToLower();
                if (server_sign != sign)
                {
                    resultMsg = new ApiResult { success = false, status = ApiStatusCode.Unauthorized, message = "签名或者密钥错误！" };
                }
                else
                {
                    resultMsg.success = true;
                }

            }
            if (!resultMsg.success)
            {
                //如果验证不通过，则返回授权错误，并且写入错误原因
                actionContext.Response = actionContext.Request.CreateResponse(resultMsg);
            }
            else
            {

                base.OnActionExecuting(actionContext);
            }
        }

    }
}