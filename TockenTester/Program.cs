using Divisasx.Security.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TockenTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string tocken = new TokenManager().GenerateToken(1,"john", "password");
            new TokenManager().IsValidTocken(tocken);
            var encoded=HttpUtility.UrlEncode(tocken);


        }
    }
}
