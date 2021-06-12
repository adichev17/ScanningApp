using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = MVCJwtToken.AuthSchemes)]
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet("/GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == User.Identity.Name);
            if (user != null)
            {
                // установка куки
                await _signInManager.SignInAsync(user, false);
                return Json(user);
            }
            return StatusCode(404);
        }
    }
}