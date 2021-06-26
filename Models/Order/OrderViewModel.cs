using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class OrderViewModel
    {
        public DateTime DateTime { get; set; }
        [Required]
        public double TotalСost { get; set; }
        [Required]
        public List<OrderBodyFromRequest> UPCEANProducts { get; set; }
    }
}
