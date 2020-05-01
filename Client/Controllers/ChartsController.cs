using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class ChartsController : Controller
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

        public JsonResult GetUserApp()
        {
            return GetChart("User/UserAppInfo");
        }

        public JsonResult GetUserReligion()
        {
            return GetChart("User/UserReligionInfo");
        }

        public JsonResult GetUserBatch()
        {
            return GetChart("User/UserBatchInfo");
        }

        public JsonResult GetUserClass()
        {
            return GetChart("User/UserClassInfo");
        }

        public JsonResult GetChart(string address)
        {
            IEnumerable<ChartVM> chartInfo = null;
            List<ChartVM> chartData = new List<ChartVM>();
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync(address); //Access data from employees API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<ChartVM>>(); //Get all the data from the API
                readTask.Wait();
                chartInfo = readTask.Result;
                foreach (var item in chartInfo)
                {
                    ChartVM data = new ChartVM();
                    data.label = item.label;
                    data.value = item.Total;
                    chartData.Add(data);
                }
                var json = JsonConvert.SerializeObject(chartData, Formatting.Indented);
                return Json(json);
            }
            return Json("internal server error");
        }
    }
}