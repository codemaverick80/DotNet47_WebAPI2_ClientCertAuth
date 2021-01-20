using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPI2.Filters
{
    public class RequireHttpsAttribute : AuthorizeAttribute // AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            X509Certificate2 cert = actionContext.RequestContext.ClientCertificate;

            if (cert == null)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "HTTPS Required"
                };
                return;
            }

            if (!IsValidClientCertificate(cert))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    //ReasonPhrase = cert.Subject + " - " + cert.SubjectName.Name + " - " + cert.NotAfter.ToString()
                    ReasonPhrase = "Invalid Client Certificate"
                };
                return;
            }

        }



        private bool IsValidClientCertificate(X509Certificate2 certificate)
        {
            //// check time validaty of certificate
            if (certificate.NotAfter < DateTime.Now)
            {
                return false;
            }

            bool foundSubject = false;
            string[] certSubjectData = certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            //// web.config <appSettings><add key="AllowedClients" value="OU=TESTAPP;OU=Client_OU" /></appSettings>
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["AllowedClients"]))
            {
                throw new ArgumentNullException("AllowedClients");
            }
            string[] allowedClients = ConfigurationManager.AppSettings["AllowedClients"].ToUpper().Split(';');

            foreach (string s in certSubjectData)
            {
                if (allowedClients.Contains(s.Trim().ToUpper()))
                {
                    foundSubject = true;
                    break;
                }
            }
            if (!foundSubject) return false;


            ////// check issuer name of certificate
            //bool foundIssuer = false;
            //string[] certIssuerData = certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string s in certIssuerData)
            //{
            //    if (string.Compare(s.Trim(),"[Issuer]")==0)
            //    {
            //        foundIssuer = true;
            //        break;
            //    }
            //}
            //if (!foundIssuer) return false;

            return true;
        }



    }
}