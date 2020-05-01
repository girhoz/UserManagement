using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ClassesController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44365/api/")
        };

        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            ViewBag.PacketText = HttpContext.Session.GetString("Name");
            if (role == "Admin")
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        public JsonResult LoadClass()
        {
            IEnumerable<Class> classes = null;
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("Class"); //Access data from employees API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<Class>>(); //Get all the data from the API
                readTask.Wait();
                classes = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(classes);
        }

        public JsonResult InsertOrUpdate(Class cls)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(cls);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (cls.Id == 0)
            {
                var result = client.PostAsync("Class", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("Class/" + cls.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            Class cls = null;
            var responseTask = client.GetAsync("Class/" + Id); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                cls = JsonConvert.DeserializeObject<Class>(json); //Tampung setiap data didalam departments
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(cls);
        }

        public JsonResult Delete(int Id)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var result = client.DeleteAsync("Class/" + Id).Result;
            return Json(result);
        }
    }
}