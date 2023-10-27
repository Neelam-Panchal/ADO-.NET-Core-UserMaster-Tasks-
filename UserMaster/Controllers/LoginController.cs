using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMaster.ViewModels;
using UserMaster.Application.Services;
using Microsoft.AspNetCore.Http;

namespace UserMaster.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserMasterService _userMasterService;

        public LoginController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                LoginViewModel loginResult = _userMasterService.LoginUser(model);
                

                if (loginResult != null)
                {
                    int userId = loginResult.IdUser;
                    string username = loginResult.UserName;

                    HttpContext.Session.SetInt32("IdUser", userId);
                    HttpContext.Session.SetString("UserName", username);

                    TempData["LoginSuccess"] = "Login Successful";
                    return RedirectToAction("GetTask1", "Task");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Remove the session value
            HttpContext.Session.Remove("IdUser");

            // You can also redirect to a different page or perform other logout actions.

            return RedirectToAction("Login", "Login"); // Redirect to the home page after logout.
        }

    }
}
