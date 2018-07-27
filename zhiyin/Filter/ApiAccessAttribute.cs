using Rays.Model.Sys;
using Rays.Utility.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Zhiyin.Common;
using Zhiyin.Models;

namespace Zhiyin.Filter
{
    public class ApiAccessAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// action执行前调用的方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ApiMonitorLog MonLog = new ApiMonitorLog();
            MonLog.ExecuteStartTime = DateTime.Now;
            //获取Action 参数
            MonLog.ActionParams = actionContext.ActionArguments;
            MonLog.ActionName = actionContext.ActionDescriptor.ActionName;
            MonLog.ControllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            MonLog.IP = Util.GetUserIp();
            MonLog.HttpRequestHeaders = actionContext.Request.Headers.Host;
            MonLog.HttpMethod = actionContext.Request.Method.Method;


            //#region 同一接口，同一IP，同一请求在10分钟内访问次数不超过300次，1个小时内不超过2000次，在当天不超过10000次
            bool isValid = SignToken.CheckRequestIsValid(MonLog);
            if (isValid)
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                var resultMsg = new ApiResult { success = false, status = ApiStatusCode.LimitRequest, message = ApiStatusCode.LimitRequest.GetEnumDesc() };
                actionContext.Response = actionContext.Request.CreateResponse(resultMsg);
            }
            //#endregion

        }
    }
}