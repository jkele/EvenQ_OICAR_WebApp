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
using Newtonsoft.Json;

namespace Eveq_Oicar_web.Controllers
{
    public class AdminController : Controller
    {
        public FirebaseAuthProvider auth;
        public int locationID = 0;
        public AdminController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyB6-iD9rlVsAQfOZKsmDBVPBlpEFpGrBa0"));
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            IEnumerable<Event> eventList;
            HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event").Result;
            eventList = response.Content.ReadAsAsync<IEnumerable<Event>>().Result;
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                User user = auth.GetUserAsync(token).Result;
                string uid = user.LocalId;
                HttpResponseMessage Adminresponse = GlobalVariable.WebApiClient.GetAsync("Member/Admin/" + uid.ToString()).Result;
                bool Admin = Adminresponse.IsSuccessStatusCode;
                if (Admin == true)
                {
                    return View(eventList);
                }
                else
                {
                    return RedirectToAction("SignIn", "Home", new { area = "" });
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
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
                    return View(response.Content.ReadAsAsync<Event>().Result);
                }
                return RedirectToAction("Index");
            }

        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult CreateLocation(int id = 0)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (id == 0)
            {
                if (token != null)
                {
                    return View(new Location());
                }
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Location/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Location>().Result);
            }

        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult CreateLocation(Location location)
        {
            if (location.IDLocation == 0) 
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.PostAsJsonAsync("Location", location).Result;
                response.EnsureSuccessStatusCode();
            }
            else
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.PutAsJsonAsync("Location/" + location.IDLocation, location).Result;
            }

            return View();

        }
        
      

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Edit(int id = 0)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                HttpResponseMessage response = GlobalVariable.WebApiClient.GetAsync("Event/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Event>().Result);
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult Edit(Event eventm, IFormFile images)
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

            return RedirectToAction("Index");
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
        [HttpPost]
        public ActionResult Create(Event eventm, IFormFile images)
        {
            int LocationIdCounter = GlobalVariable.Counter;
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
                Location location = new Location { City = eventm.Location.City, Street = eventm.Location.Street, Coordinates = eventm.Location.Coordinates };
                HttpResponseMessage responseLocation = GlobalVariable.WebApiClient.PostAsJsonAsync("Location", location).Result;
                GlobalVariable.Increment();
                HttpResponseMessage requestLocation = GlobalVariable.WebApiClient.GetAsync("Location/" + LocationIdCounter.ToString()).Result;
                var content = requestLocation.Content.ReadAsStringAsync().Result;
                string jsonString = JsonConvert.SerializeObject(content);
                var dese = JsonConvert.DeserializeObject<dynamic>(jsonString);
                location = JsonConvert.DeserializeObject<Location>(dese);
                eventm.LocationId = location.IDLocation;
                eventm.Location.IDLocation = location.IDLocation;
                HttpResponseMessage response = GlobalVariable.WebApiClient.PostAsJsonAsync("Event", eventm).Result;
                response.EnsureSuccessStatusCode(); 

            }
            else 
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

