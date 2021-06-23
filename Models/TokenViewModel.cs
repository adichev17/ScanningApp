using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class TokenViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
