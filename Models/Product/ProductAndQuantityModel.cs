using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class ProductAndQuantityModel
    {
        public Product Product { get; set; }
        public int Count { get; set; }

    }
}
