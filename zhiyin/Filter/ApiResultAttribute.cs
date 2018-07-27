using Rays.Model.Sys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Zhiyin.Common;
using Zhiyin.Models;

namespace Zhiyin.Filter
{
    /// <summary>
    /// 封装返回值
    /// </summary>
    public class ApiResultAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 统一回传格式
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                return;
            }

            base.OnActionExecuted(actionExecutedContext);

            //统一回传格式
            ApiResult result = new ApiResult();
            // 取得由 API 返回的状态代码
            result.status = ApiStatusCode.OK;
            result.success = true;
            result.message = "";
            // 取得由 API 返回的资料
            result.data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
            // 重新封装回传格式
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(actionExecutedContext.ActionContext.Response.StatusCode, result);

        }
    }
}