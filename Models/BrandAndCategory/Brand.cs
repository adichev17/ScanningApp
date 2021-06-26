using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Display(Name = "Brand")]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
