using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiaB.IdentityServer.Models
{
    public class RegisterResult
    {
        public bool IsSuccess { get; set; }

        public string Token { get; set; }
        
        public int RemainingRequestCount { get; set; }

        public string Message { get; set; }
    }
}
