using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Users
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AccountManager(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
        public TokenViewModel CreateToken(JwtTokenViewModel model)
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

                    var results = new TokenViewModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };
                    return results;
                }
                else
                {
                    return null;
                }
        }

        public async Task<User> Register(RegisterViewModel model)
        {
            User user = new User { Name = model.Name, PhoneNumber = model.Number, UserName = model.UserLogin };
            // добавляем пользователя
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // установка куки
                await _signInManager.SignInAsync(user, false);
                return user;
            }
            else
                return null;
        }

        public async Task<User> UpdateProfile(UserUpdateProfileModel model, string UserId)
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
                    return null;
                }
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }
    }
}
