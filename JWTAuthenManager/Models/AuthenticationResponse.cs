using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthenManager.Models
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Role {  get; set; }

        public string JwtToken { get; set; }

        public int ExpireIn {  get; set; }
    }
}
