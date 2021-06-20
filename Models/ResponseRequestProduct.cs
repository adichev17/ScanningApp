using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class ResponseRequestProduct
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
