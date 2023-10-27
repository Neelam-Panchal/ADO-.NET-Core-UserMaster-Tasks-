using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserMaster.Application.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserMaster.Core.ViewModels;// Include the correct namespace for your view models

namespace UserMaster.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserMasterService _userMasterService;

        public RegisterController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Nationalities = _userMasterService.PopulateNationalities();
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {

                    // Check if the username already exists
                    if (_userMasterService.IsUsernameExists(model.UserName))
                    {
                        ModelState.AddModelError("UserName", "Username is already taken. Please choose a different one.");
                        ViewBag.Error = "Please choose another one.";
                        return View(model);
                    }

                    // Register the user using the DatabaseManager
                    if (_userMasterService.RegisterUser(model))
                    {
                        TempData["Message"] = "Registration successful.";
                        return RedirectToAction("RegisterSuccess");
                    }
                    else
                    {
                        ViewBag.Nationalities = _userMasterService.PopulateNationalities();
                        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                        return View(model);
                }
            }
            else
            {
                ViewBag.Nationalities = _userMasterService.PopulateNationalities();
                return View(model);
            }

            // If ModelState is not valid or the insertion fails, return to the registration page.
           
        }
       /* public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Nationalities = _userMasterService.PopulateNationalities();
                return View(model);
            }

            bool registrationResult = _userMasterService.RegisterUser(model);

            if (registrationResult)
            {
                return RedirectToAction("RegisterSuccess");
            }
            else
            {
                ViewBag.Nationalities = _userMasterService.PopulateNationalities();
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                return View(model);
            }
        }*/

        public ActionResult RegisterSuccess()
        {
            // Return a success view or redirect to a login page
            return View();
        }
    }
}
