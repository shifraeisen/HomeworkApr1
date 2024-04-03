﻿using HomeworkApr1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeworkApr1.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }

        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Message = TempData["Error"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "role"))).Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }

        public IActionResult IsEmailAvailable(string email)
        {
            var repo = new UserRepository(_connectionString);

            return Json(new
            {
                IsAvailable = repo.EmailAvailable(email)
            });
        }
    }
}
