using Eveq_Oicar_web.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Controllers
{
    public class MemberController : Controller
    {
        public FirebaseAuthProvider auth;
        public MemberController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyB6-iD9rlVsAQfOZKsmDBVPBlpEFpGrBa0"));
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            Member member;
            var token = HttpContext.Session.GetString("_UserToken");

            User user = auth.GetUserAsync(token).Result;
            string uid = user.LocalId;

            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Member/" + uid).Result;
            member = response.Content.ReadAsAsync<Member>().Result;

            if (token != null)
            {
                return View(member);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Edit()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            User user = auth.GetUserAsync(token).Result;
            string id = user.LocalId;
            if (token != null)
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Member/" + id).Result;

                return View(response.Content.ReadAsAsync<Member>().Result);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }

        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult Edit(Member member)
        {
            HttpResponseMessage response = GlobalVariable.WebApiClient.PutAsJsonAsync("Member/Admin/" + member.UID, member).Result;

            return RedirectToAction("Index");
        }

        public ActionResult RedirectToDelete()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            User user = auth.GetUserAsync(token).Result;
            string id = user.LocalId;
            if (token != null)
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Member/" + id).Result;

                return View(response.Content.ReadAsAsync<Member>().Result);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }

        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Member member)
        {
            HttpResponseMessage response = GlobalVariable.WebApiClient.DeleteAsync("Member/" + member.UID).Result;
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index", "Home");
        }

    }
}
