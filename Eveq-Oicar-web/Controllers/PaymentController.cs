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
    public class PaymentController : Controller
    {
        public FirebaseAuthProvider auth;

        DateTime timer;
        public PaymentController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyB6-iD9rlVsAQfOZKsmDBVPBlpEFpGrBa0"));
        }

        public IActionResult IndexTicket(int id, int ticketPrice)
        {
            ViewBag.TicketPrice = ticketPrice;
            var token = HttpContext.Session.GetString("_UserToken");
            ViewBag.EventId = id;

            User user = auth.GetUserAsync(token).Result;
            string uid = user.LocalId;

            IEnumerable<Payment> payment;

            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Payment/" + uid).Result;
            if (response.IsSuccessStatusCode)
            {
                payment = response.Content.ReadAsAsync<IEnumerable<Payment>>().Result;

                foreach (var item in payment)
                {
                    if (item.IsMembership == true)
                    {
                        timer = item.DateBought.AddYears(1);
                    }
                }
            }






            
            if (token != null && DateTime.Compare(timer, DateTime.Now) > 0)
            {
                return View(id);
            }
            else if (token != null)
            {
                //return RedirectToAction("Index", "Event");
                return View(id);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public IActionResult IndexMembership()
        {


            return View();
        }




        public IActionResult PayEndTicket(int id)
        {
            var token = HttpContext.Session.GetString("_UserToken");


            User user = auth.GetUserAsync(token).Result;
            string uid = user.LocalId;
            Ticket eventList = new Ticket(id + ":" + uid, uid, id, true);

            HttpResponseMessage response = GlobalVariable.WebApiClient.PostAsJsonAsync("Ticket", eventList).Result;
            response.EnsureSuccessStatusCode();

            Payment payment = new Payment(DateTime.Now, false, uid);
            HttpResponseMessage response1 = GlobalVariable.WebApiClient.PostAsJsonAsync("Payment/", payment).Result;
            response1.EnsureSuccessStatusCode();

            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }



        }





        public IActionResult PayEndMembership()
        {
            var token = HttpContext.Session.GetString("_UserToken");

            User user = auth.GetUserAsync(token).Result;
            string uid = user.LocalId;




            Payment payment = new Payment(DateTime.Now, DateTime.Now.AddYears(1), true, uid);
            HttpResponseMessage response1 = GlobalVariable.WebApiClient.PostAsJsonAsync("Payment/", payment).Result;
            response1.EnsureSuccessStatusCode();

            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Member/" + uid).Result;
            Member m = response.Content.ReadAsAsync<Member>().Result;

            if (response1.IsSuccessStatusCode)
            {
                m.MembershipValid = true;
            }

            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }



        }


    }
}
