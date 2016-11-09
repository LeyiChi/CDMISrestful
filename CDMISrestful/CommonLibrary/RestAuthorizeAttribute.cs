using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CDMISrestful.CommonLibrary
{ /// <summary>
    /// Token-based authentication for ASP .NET MVC REST web services.
    /// Copyright (c) 2015 Kory Becker
    /// http://primaryobjects.com/kory-becker
    /// License MIT
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RESTAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _securityToken = "token";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }

            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            //try
            //{
            //    //前端传入Token
            //    HttpRequestMessage request = actionContext.Request;

            //    var header_token = request.Headers.GetValues("token");
            //    string token = header_token.ElementAt(0);

            //    if (SecurityManager.IsTokenValid(token))
            //    {
            //        //Token未过期
            //        return true;
            //    }
            //    else
            //    {
            //        //Token已过期               
            //        return false;
            //    }
            //}
            //catch (Exception)
            //{
            //    //前端不传入Token
            //    return false;
            //}
            return true;
        }
    }
}