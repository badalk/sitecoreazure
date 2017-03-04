using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitecore.ContentModerator.Processors
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );*/
        }
    }

    public class WebApiProcessor
    {
        public void Process(PipelineArgs args)
        {
            var config = GlobalConfiguration.Configuration;
            Configure(config);
        }
        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("ModerationAPI", "sitecore/api/moderate/{controller}/{action}", new
            {
                controller = "moderation",
                action = "start"                
            });
        }
    }
}
