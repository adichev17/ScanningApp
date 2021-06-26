using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class MVCJwtToken
    {
        public const string Issuer = "MVC";
        public const string Audience = "ApiUser";
        public const string Key = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IkphdmFJblVzZSIsImV4cCI6MTYyMjM4NzkyNiwiaWF0IjoxNjIyMzg3OTI2fQ.AQjJaMVFwzeRcGXW0TGpK0npHqGNbvNfeBXn3Ibe0H8";

        public const string AuthSchemes = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
