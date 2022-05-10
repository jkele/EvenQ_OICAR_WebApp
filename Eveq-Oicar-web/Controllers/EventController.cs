using Eveq_Oicar_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Eveq_Oicar_web.Controllers
{
    public class EventController : Controller
    {
       
        public IActionResult Index()
        {
            IEnumerable<Event> eventList;
            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event").Result;
            eventList = response.Content.ReadAsAsync<IEnumerable<Event>>().Result;
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

        public ActionResult Create(int id = 0)
        {

            if (id == 0)
            {
                return View(new Event());
            }
            else
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Event>().Result);
            }

          

        }

        [HttpPost]
        public ActionResult Create(Event eventm, IFormFile images)
        {
    
            if (eventm.IDEvent == 0) // Create event
            {
                if (images != null)
                {
                    if (images.Length > 0)

                    {
                        byte[] p1 = null;
                        using (var fs1 = images.OpenReadStream())
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                        eventm.PosterImage = p1;

                    }
                }
                HttpResponseMessage response = GlobalVariable.WebApiClient.PostAsJsonAsync("Event", eventm).Result;
                response.EnsureSuccessStatusCode();
            }
            else // update event
            {
                if (images != null)
                {
                    if (images.Length > 0)

                    {
                        byte[] p1 = null;
                        using (var fs1 = images.OpenReadStream())
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                        eventm.PosterImage = p1;

                    }
                }
                HttpResponseMessage response = GlobalVariable.WebApiClient.PutAsJsonAsync("Event/" + eventm.IDEvent, eventm).Result;
            }


            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariable.WebApiClient.DeleteAsync("Event/" + id.ToString()).Result;
            return RedirectToAction("Index");
        }
    }
}
