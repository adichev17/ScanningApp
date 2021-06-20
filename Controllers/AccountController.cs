using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Route("/reg")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Name = model.Name, PhoneNumber = model.Number, UserName = model.UserLogin };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return Json(user);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    return StatusCode(501);
                }
            }
            return View(model);
        }

        private User AuthenticateUser(string userName, string password)
        {
            //return _context.Users.SingleOrDefault(u => u.UserName == userName && u.PasswordHash == password);
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
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

        [HttpPost("/CreateToken")]
        public IActionResult CreateToken(JwtTokenViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = AuthenticateUser(model.UserName, model.Password);
                if (user != null)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MVCJwtToken.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName)
                    };
                    var token = new JwtSecurityToken(
                        MVCJwtToken.Issuer,
                        MVCJwtToken.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: creds
                        );

                    var results = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Created("", results);
                } else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPatch("/UpdateProfile/{UserId}")]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileModel model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                try
                {
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Name = model.Name;
                    user.DateBirth = model.BirthDate;
                }
                catch (Exception)
                {
                    return StatusCode(501);
                }
                await _context.SaveChangesAsync();
                return Json(user);
            }
            return Unauthorized();
        }
    }
}