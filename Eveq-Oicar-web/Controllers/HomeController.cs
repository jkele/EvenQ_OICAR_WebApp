using Eveq_Oicar_web.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

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
            await auth.SendEmailVerificationAsync(token);
            if (fbAuthLink.User.IsEmailVerified)
            {

                
                
            
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(Login userModel)
        {
            //log in the user
            var fbAuthLink = await auth.SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;
            if (!fbAuthLink.User.IsEmailVerified)
            {

                
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
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

            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn");
        }
    }
}
