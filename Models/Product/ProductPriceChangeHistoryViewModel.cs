using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class ProductPriceChangeHistoryViewModel
    {
        [Required]
        public string UPCEAN { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public byte IsSale { get; set; }
    }
}
