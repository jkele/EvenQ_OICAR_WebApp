using Eveq_Oicar_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eveq_Oicar_web.Controllers
{
    public class TicketController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Ticket> eventList;
            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Ticket").Result;
            eventList = response.Content.ReadAsAsync<IEnumerable<Ticket>>().Result;
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                return View(eventList);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }

        }
    }
}
