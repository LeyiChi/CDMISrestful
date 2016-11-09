using CDMISrestful.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CDMISrestful
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Filters.Add(new RESTAuthorizeAttribute());

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services

            // Web API routes
            //属性路由
            config.MapHttpAttributeRoutes();

            //基于公约的路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Api/v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.EnableQuerySupport();
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
