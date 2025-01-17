﻿using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using TelgeProject.Entity;
using TelgeProject.Interface;

namespace TelgeProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAuthRepository _userRepository;

        public AuthController(IAuthService authService, IAuthRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _authService.Authenticate(username, password);
            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            // Generate token and save it in the database
            var token = _authService.GenerateToken(user);
            _userRepository.SaveToken(new TblUsertoken
            {
                UserId = user.UserId,
                IsDelete = 0,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            });

            // Store token in session or cookie
            HttpContext.Session.SetString("AuthToken", token);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            return RedirectToAction("Projects", "Projects");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}