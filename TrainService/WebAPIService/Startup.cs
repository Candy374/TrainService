using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebAPIService.Startup))]

namespace WebAPIService
{
    public partial class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host.             
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // https://msdn.microsoft.com/zh-cn/magazine/dn532203.aspx
            config.EnableCors();

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            appBuilder.UseWebApi(config);
            Jobs.Start();
        }
    }
}
