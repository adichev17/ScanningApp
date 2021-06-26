using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class HistoryOrderViewModel
    {
        public DateTime DateTime { get; set; }
        public double TotalСost { get; set; }
        public List<ProductAndQuantityModel> ProductsIncludeOrder { get; set; }
    }
}
