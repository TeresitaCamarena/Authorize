using Divisasx.Security.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace MvcApplication4.FilterAttributes
{
    public class RESTAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (Authorize(filterContext))
            {
                return;
            }
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Request.Headers.Authorization == null )
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            string encodedToken = actionContext.ControllerContext.Request.Headers.Authorization.Scheme;
            string token = HttpUtility.UrlDecode(encodedToken);

            if (!new TokenManager().IsValidTocken(token))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return true;
        }
    }
}