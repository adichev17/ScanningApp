using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class PriceChangeHistory
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        public byte IsSale { get; set; }
    }
}
