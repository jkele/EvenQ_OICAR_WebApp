using Eveq_Oicar_web.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Controllers
{
    public class HomeController : Controller
    {
        public FirebaseAuthProvider auth;
        public HomeController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyB6-iD9rlVsAQfOZKsmDBVPBlpEFpGrBa0"));
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                return RedirectToAction("Member");
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userModel)
        {
            //create the user
            await auth.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password);

            //log in the new user
            var fbAuthLink = await auth
                            .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;

            //empty user za dohavacanje UIDa
            User user = auth.GetUserAsync(token).Result;

            //Upis u bazu
            Member memberm = new Member(user.LocalId, userModel.FirstName, userModel.LastName, userModel.ReferralCode, false, 0, false);

            HttpResponseMessage response = GlobalVariable.WebApiClient.PostAsJsonAsync("Member", memberm).Result;

            await auth.SendEmailVerificationAsync(token);
            if (fbAuthLink.User.IsEmailVerified)
            {

                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index", "Event", new { area = "" });
                }
                else
                {
                    return View("Member");
                }
            }
            else
            {
                TempData["AlertMessage"] = "Registration is successful";
                return View("Index");
            }
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> SignIn(Login userModel)
        {
            //log in the user
            var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;
            User user = auth.GetUserAsync(token).Result;
            string uid = user.LocalId;
            HttpResponseMessage Adminresponse = GlobalVariable.WebApiClient.GetAsync("Member/Admin/" + uid.ToString()).Result;
            bool Admin = Adminresponse.IsSuccessStatusCode;
            if (!fbAuthLink.User.IsEmailVerified)
            {

                
                //saving the token in a session variable
                if ((token != null) && (Admin == false))
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index", "Event", new { area = "" });
                }
                else if ((token != null) && (Admin == true))
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    return RedirectToAction("Index", "Admin", new { area = "" });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(Login login)
        {
            await auth.SendPasswordResetEmailAsync(login.Email);

            return RedirectToAction("ForgotPasswordSent", "Home", new { area = "" });
        }


        public IActionResult ForgotPasswordSent()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Member()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            
            return RedirectToAction("SignIn");
        }
    }
}
