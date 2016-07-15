using Divisasx.Security.Services;
using MvcApplication4.DTO;
using System;
using System.Web;
using System.Web.Http;

namespace MvcApplication4.Controllers
{
    public class SecurityController : System.Web.Http.ApiController
    {
        [ActionName("Authenticate")]
        [HttpPost]
        public AutenticationResponse Authenticate([FromBody]AutenticationRequest model)
        {
            try
            {
                if (model.UserName == "john" && model.Password == "password")// if authentication passed
                {
                    string token = new TokenManager().GenerateToken(1, model.UserName, model.Password);

                    return new AutenticationResponse
                    {
                        Autenticated = true,
                        Token = HttpUtility.UrlEncode(token)
                    };
                }

                return new AutenticationResponse { Autenticated = false, Message = "Invalid user name or password" };
            }
            catch (Exception ex)
            {
                return  new AutenticationResponse { Autenticated = false, Message = "Internal Error, please try again later" };
            }
        }

       
        public string Get()
        {
            return "aldo";
        }

        [ActionName("PostTest")]
        [HttpPost]
        public void PostTest([FromUri]string id)
        {

        }

    }
}
