using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class OrderBodyFromRequest
    {
        public string UPCEAN { get; set; }
        public int Count { get; set; }
    }
}
