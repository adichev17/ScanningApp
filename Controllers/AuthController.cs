using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AppDbContext _context;

        public AuthController(UserManager<User> userManager, SignInManager<User> signinMgr, AppDbContext context)
        {
            _userManager = userManager;
            signInManager = signinMgr;
            _context = context;
        }

        [HttpPost("/DecryptingPassword")]
        public User DecryptingPassword(string userName, string password = "ADE$25103001_Ad")
        {
            //return _context.Users.SingleOrDefault(u => u.UserName == userName && u.PasswordHash == password);

            var user = _context.Users.SingleOrDefault(u => u.UserName == "Dimon123");
            if (user != null)
            {
                Microsoft.AspNetCore.Identity.PasswordHasher<User> hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
                var authUser = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (authUser != PasswordVerificationResult.Failed)
                {
                    return user;
                }
            }
            return null;
        }
    }
}