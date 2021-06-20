using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class SelectionForProducts
    {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }

        [ForeignKey(nameof(ProductID))]
        public Product Product { get; set; }

        public int AdjacentID { get; set; }
        [ForeignKey(nameof(AdjacentID))]
        public Product AdjacentProduct { get; set; }
        public int Count { get; set; }
    }
}
