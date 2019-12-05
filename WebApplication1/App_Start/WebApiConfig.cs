using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //自定义路由：和MVC类似，增加action
            config.Routes.MapHttpRoute(
               name: "customRoute1",
               routeTemplate: "myapi/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

        }
    }
}
