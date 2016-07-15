using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication4.DTO
{
    public class AutenticationResponse
    {
        public AutenticationResponse()
        {

        }
        public bool Autenticated { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}