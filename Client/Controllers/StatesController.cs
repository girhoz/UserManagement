using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class StatesController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44365/api/")
        };

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadState()
        {
            IEnumerable<State> state = null;
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("State"); //Access data from employees API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<State>>(); //Get all the data from the API
                readTask.Wait();
                state = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(state);
        }
    }
}