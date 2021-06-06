using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class User: IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(15)]
        public string DateBirth { get; set; }
    }
}