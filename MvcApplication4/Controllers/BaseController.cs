using Divisasx.Security.Entities;
using Divisasx.Security.Services;
using MvcApplication4.FilterAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MvcApplication4.Controllers
{
    [RESTAuthorizeAttribute]
    public class BaseController : ApiController
    {
       private TokenEntity _token;

       public TokenEntity Token
       {
           get
           {
               return _token = _token ?? (_token = GetTokenFromRequest());
           }
       }

       private TokenEntity GetTokenFromRequest()
       {
           var token = HttpUtility.UrlDecode(Request.Headers.Authorization.Scheme);
           return new TokenManager().GetTokenEntityFromHash(token);
       }
    }
}
