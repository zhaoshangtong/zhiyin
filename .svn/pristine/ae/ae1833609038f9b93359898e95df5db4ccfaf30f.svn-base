using Quartz;
using Quartz.Impl;
using Rays.Utility.Redis;
using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Zhiyin
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new ApiHttpControllerSelector(GlobalConfiguration.Configuration));
            //EF预热加载
            using (var dbcontext = new Rays.Model.zhiyin())
            {
                var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (System.Data.Entity.Core.Mapping.StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(System.Data.Entity.Core.Metadata.Edm.DataSpace.CSSpace);
                mappingCollection.GenerateViews(new System.Collections.Generic.List<System.Data.Entity.Core.Metadata.Edm.EdmSchemaError>());
            }
        }
        public override void Init()
        {
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(
                System.Web.SessionState.SessionStateBehavior.Required);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var req = System.Web.HttpContext.Current.Request;
            if (req.HttpMethod == "OPTIONS")//过滤options请求，用于js跨域
            {
                Response.StatusCode = 200;
                Response.SubStatusCode = 200;
                Response.End();
            }
        }
        
    }
}
