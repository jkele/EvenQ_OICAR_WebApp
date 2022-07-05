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
using Firebase.Auth;

namespace Eveq_Oicar_web.Controllers
{
    public class EventController : Controller
    {

        public FirebaseAuthProvider auth;

        public EventController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyB6-iD9rlVsAQfOZKsmDBVPBlpEFpGrBa0"));
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            IEnumerable<Event> eventList;
            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event/UpcomingEvents").Result;
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
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Create(int id = 0)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (id == 0)
            {
                if (token != null)
                {
                    return View(new Event());
                }
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Event>().Result);
            }

          

        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Details(int id = 0)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event/" + id.ToString()).Result;
                if (token != null)
                {

                    Event result = response.Content.ReadAsAsync<Event>().Result;

                    string[] latlng = result.Location.Coordinates.Split(new string[] { "," }, StringSplitOptions.None);
                    string markers = "[";
                    markers += "{";
                    markers += string.Format("'lat': '{0}',", latlng[0].Trim());
                    markers += "},";
                    markers += "{";
                    markers += string.Format("'lng': '{0}'", latlng[1].Trim());
                    markers += "}";
                    markers += "];";

                    ViewBag.Marker = markers;
                    return View(result);
                }
                return RedirectToAction("Index");
            }

        }


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariable.WebApiClient.DeleteAsync("Event/" + id.ToString()).Result;
            return RedirectToAction("Index");
        }
    }
}
