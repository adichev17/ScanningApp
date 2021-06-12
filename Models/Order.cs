using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public HistoryOrders HistoryOrders { get; set; }
    }
}
