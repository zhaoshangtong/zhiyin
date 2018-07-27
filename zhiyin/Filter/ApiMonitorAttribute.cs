using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Rays.BLL;
using Rays.Model;
using Zhiyin.Common;
using Zhiyin.Models;
using Rays.Model.Sys;

namespace Zhiyin.Filter
{
    /// <summary>
    /// 监控action
    /// </summary>
    public class ApiMonitorAttribute : ActionFilterAttribute
    {
        private readonly string Key = "_thisWebApiOnActionMonitorLog_";

        /// <summary>
        /// action执行前调用的方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            ApiMonitorLog MonLog = new ApiMonitorLog();
            MonLog.ExecuteStartTime = DateTime.Now;
            //获取Action 参数
            MonLog.ActionParams = actionContext.ActionArguments;
            MonLog.ActionName = actionContext.ActionDescriptor.ActionName;
            MonLog.ControllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            MonLog.IP = Util.GetUserIp();
            MonLog.HttpRequestHeaders = actionContext.Request.Headers.Referrer?.ToString();
            MonLog.Host = actionContext.Request.Headers.Host;
            MonLog.HttpMethod = actionContext.Request.Method.Method;
            actionContext.Request.Properties[Key] = MonLog;

        }
        //public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        //{
        //    //记录监控日志
        //    ApiMonitorLog MonLog = actionExecutedContext.Request.Properties[Key] as ApiMonitorLog;
        //    MonLog.ExecuteEndTime = DateTime.Now;
        //    //LogHelper.Monitor(MonLog.GetLoginfo());
        //    //记录API调用记录到数据库
        //    try
        //    {
        //        BaseBLL<api_monitor_log> apiMonitorLogBLL = new BaseBLL<api_monitor_log>();
        //        string req_params = MonLog.GetCollections(MonLog.ActionParams);
        //        api_monitor_log log = new api_monitor_log
        //        {
        //            controll_name = MonLog.ControllerName,
        //            action_name = MonLog.ActionName,
        //            start_time = MonLog.ExecuteStartTime,
        //            end_time = MonLog.ExecuteEndTime,
        //            total_second = (MonLog.ExecuteEndTime - MonLog.ExecuteStartTime).TotalSeconds,
        //            ip = MonLog.IP,
        //            http_method = MonLog.HttpMethod,
        //            source_url = MonLog.Host,
        //            params_str = req_params.Length > 4000 ? req_params.Substring(0, 4000) : req_params,
        //            request_referrer = MonLog.HttpRequestHeaders
        //        };
        //        apiMonitorLogBLL.Add(log);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("往数据记录调用接口失败：" + ex.Message);
        //    }

        //    if (actionExecutedContext.Exception != null)
        //    {
        //        return;
        //    }

        //    base.OnActionExecuted(actionExecutedContext);
        //}

    }
}