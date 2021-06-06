using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string UPCEAN { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }
        public int BrandID { get; set; }
        [ForeignKey(nameof(BrandID))]
        public Brand Brand { get; set; }
        public int Price { get; set; }
        public int UnitOfAccountID { get; set; }
        [ForeignKey(nameof(UnitOfAccountID))]
        public UnitOfAccount UnitOfAccount { get; set; }
    }
}
