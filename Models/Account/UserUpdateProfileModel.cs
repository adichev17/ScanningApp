using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Models
{
    public class UserUpdateProfileModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
    }
}
