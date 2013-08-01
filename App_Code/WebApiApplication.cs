using System;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MyNamespace
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var configuration = GlobalConfiguration.Configuration;

            var formatters = configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            configuration.EnableQuerySupport();

            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
/*
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = null;
            var current = System.Web.HttpContext.Current;
            if (current.Request.ClientCertificate.IsPresent)
            {
                cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    current.Request.ClientCertificate.Certificate);
                current.User = new GenericPrincipal(
                    new GenericIdentity("test"), new string[] { "Test" });
            }
        }
*/
    }
}
