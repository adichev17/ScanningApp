using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserLogin")]
        public string UserLogin { get; set; }

        [Required]
        //[UIHint("password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }
}
