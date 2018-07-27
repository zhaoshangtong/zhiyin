using Rays.Model.Sys;
using Rays.Utility.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Zhiyin.Common;
using Zhiyin.Models;

namespace Zhiyin.Filter
{
    /// <summary>
    /// 错误处理
    /// </summary>
    public class ApiErrorHandleAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常错误处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //记录日志
            string Msg = $@"请求【{actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName}Controller】的【{actionExecutedContext
                    .ActionContext.ActionDescriptor.ActionName}】产生异常：
                   客户端IP：{Util.GetUserIp()},
                   HttpMethod:{actionExecutedContext.Request.Method.Method}，
                   异常信息：{actionExecutedContext.Exception.Message}";
            LogHelper.Error(Msg, actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
            var result = new ApiResult
            {
                status = ApiStatusCode.Error,
                success = false,
                message = actionExecutedContext.Exception is Rays.Utility.CommonException ? actionExecutedContext.Exception.Message : ApiStatusCode.Error.GetEnumDesc()
            };
            // 重新打包回传的讯息
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, result);

        }
    }
}