using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI2.Filters;

namespace WebAPI2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
           config.Filters.Add(new RequireHttpsAttribute());
            config.Filters.Add(new ModelValidationAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
