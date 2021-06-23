using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Users
{
    public interface IAccountManager
    {
        public Task<User> Register(RegisterViewModel model);
        public TokenViewModel CreateToken(JwtTokenViewModel model);
        public Task<User> UpdateProfile(UserUpdateProfileModel model, string UserId);
    }
}
