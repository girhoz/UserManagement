using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class UsersController : Controller
    {
        private HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44365/api/")
        };
        
        public IActionResult Index()
        {
            ViewBag.PacketText = HttpContext.Session.GetString("Name");
            return View();
        }

        public JsonResult LoadUser()
        {
            IEnumerable<UserVM> userVM = null;
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("User"); //Access data from employees API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<UserVM>>(); //Get all the data from the API
                readTask.Wait();
                userVM = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(userVM);
        }

        public JsonResult InsertOrUpdate(UserVM userVM)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (userVM.Id == 0)
            {
                var result = client.PostAsync("User/Register", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("User/" + userVM.Id, byteContent).Result;
                return Json(result);
            }
        }

        public JsonResult GetById(int Id)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            IEnumerable<UserVM> userVM = null;
            var responseTask = client.GetAsync("User/" + Id); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<UserVM>>(); //Get all the data from the API
                readTask.Wait();
                userVM = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(userVM);
        }

        public JsonResult Delete(int Id)
        {
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var result = client.DeleteAsync("User/" + Id).Result;
            return Json(result);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserVM userVM)
        {
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("User/Login", byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                //Get token and role from token
                var data = result.Content.ReadAsStringAsync().Result;
                var token = "Bearer " + data;
                var info = GetTokenInfo(data);
                //Add token to session and role to session
                HttpContext.Session.SetString("Email", info[0]);
                HttpContext.Session.SetString("Role", info[1]);
                HttpContext.Session.SetString("App", info[2]);
                HttpContext.Session.SetString("Name", info[3]);
                HttpContext.Session.SetString("JWToken", token);
                if (info[1] == "Admin")
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return RedirectToAction("Index", "Users");
                }

            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("App");
            HttpContext.Session.Remove("Name");
            return RedirectToAction("Login", "Users");
        }


        protected string[] GetTokenInfo(string token)
        {
            string[] result = new string[4];
            string secret = "sdfsdfsjdbf78sdyfssdfsdfbuidfs98gdfsdbf";
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            IEnumerable<Claim> data = claims.Claims;
            result[0] = data.SingleOrDefault(p => p.Type == "Email").Value;
            result[1] = data.SingleOrDefault(p => p.Type == "Role").Value;
            result[2] = data.SingleOrDefault(p => p.Type == "App").Value;
            result[3] = data.SingleOrDefault(p => p.Type == "Name").Value;
            return result;
        }

        
    }
}