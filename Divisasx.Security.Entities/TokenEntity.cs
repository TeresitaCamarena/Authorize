using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Divisasx.Security.Entities
{
    public class TokenEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
