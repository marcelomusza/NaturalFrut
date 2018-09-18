using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using NaturalFrut.App_Start;
using AutoMapper;
using Naturalfrut.Helpers;

namespace NaturalFrut
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ModelBinders.Binders[typeof(float)] = new SingleModelBinder();
            //ModelBinders.Binders[typeof(double)] = new DoubleModelBinder();
            //ModelBinders.Binders[typeof(decimal)] = new DecimalModelBinder();

        }
    }
}
