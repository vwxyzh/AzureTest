using System.Collections.Generic;
using System.Web.Http;

namespace MyNamespace.Controllers
{
    public partial class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4" };
        }

        public object Get(string id)
        {
            // https://localhost/CoolWebApi/Values/12345
            // 1. client cert is optional
            // 2. if client has specified a certificate for authentication, it needs to pass IIS validation (issuer recognized by IIS)
            // 3. if client cert has been specified, gateway service will look up the corresponding SID
            // 4. authentication and authorization should be done once for each connection (TLS transport)
            // 5. client can also do token based authentication, in this way client cert is not required, the token will be included in HTTP request header
            // 6. if both token and client cert are specified, token wins, this would allow client impersonation which is more flexible
            // 7. cert will be converted into an internal token, all the internal communication will use token (similar as NT security)
            // 8. each token has a context policy and default context
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = null;
            if (System.Web.HttpContext.Current.Request.ClientCertificate.IsPresent)
            {
                cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    System.Web.HttpContext.Current.Request.ClientCertificate.Certificate);
            }
            var headers = System.Web.HttpContext.Current.Request.Headers;
            // originally I was using System.Collections.ArrayList, but it seems that .NET is NOT trying to keep the order of HTTP headers in NameValueCollection
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            foreach (var x in headers.AllKeys)
            {
                dict[x] = headers[x];
            }
            return new Dictionary<string, object>{
                { "Id", id },
                { "ClientCertificate", cert == null ? "<no client cert>" : cert.Subject },
                { "RequestHeaders", dict },
                { "User", System.Web.HttpContext.Current.User },
            };
        }

    }
}
