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
using ScanningProductsApp.Manager.Users;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IAccountManager _AccountManager;

        public AccountController(IAccountManager accountManager)
        {
            _AccountManager = accountManager;
        }

        [Route("/reg")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _AccountManager.Register(model);

                if (user != null)
                {
                    return Json(user);
                }
                else
                {
                    return StatusCode(501);
                }
            }
            return View(model);
        }

        [HttpPost("/CreateToken")]
        public IActionResult CreateToken(JwtTokenViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = _AccountManager.CreateToken(model);
                if (user != null)
                    return Created("", user);
                else
                    return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch("/UpdateProfile/{UserId}")]
        public async Task<IActionResult> UpdateProfile(UserUpdateProfileModel model, string UserId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _AccountManager.UpdateProfile(model, UserId);
                if (user != null)
                {
                    return Json(user);
                };
            }
            return Unauthorized();
        }
    }
}