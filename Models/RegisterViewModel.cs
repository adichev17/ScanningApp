﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserLogin")]
        public string UserLogin { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Required]
        //[UIHint("password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
