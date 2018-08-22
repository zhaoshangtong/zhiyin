using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Zhiyin.Filter;
using Newtonsoft.Json.Converters;
using Listening.Filter;

namespace Zhiyin
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //log4.net
            log4net.Config.XmlConfigurator.Configure();
            ////自定义监控
            config.Filters.Add(new ApiMonitorAttribute());
            //自定义异常处理
            config.Filters.Add(new ApiErrorHandleAttribute());
            //权限认证
            config.Filters.Add(new ApiAuth2Attribute());
            //移除xml输出方式
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //只有一级编目
            config.Routes.MapHttpRoute(
                            name: "AreaApi",
                            routeTemplate: "api/{area}/{controller}/{action}"
                            );

            //包含二级编目
            config.Routes.MapHttpRoute(
                            name: "AreaCategoryApi",
                            routeTemplate: "api/{area}/{category}/{controller}/{action}"
                           );
            
            //配置返回的时间类型数据格式
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                }
            );
        }
    }
}
