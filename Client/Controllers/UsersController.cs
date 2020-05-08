using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
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
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            ViewBag.PacketText = HttpContext.Session.GetString("Name");
            if (role == "Admin")
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Users");
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
            if (userVM.Id == 0)
            {
                var password = Guid.NewGuid().ToString(); //generate password with guid
                userVM.Password = password; // changes the password with new password
            }         
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (userVM.Id == 0)
            {
                var result = client.PostAsync("User/Register", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    SendPassword(userVM, "Password New Account");
                }
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
            UserVM userVM = null;
            var responseTask = client.GetAsync("User/" + Id); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<UserVM>>(); //Get all the data from the API
                readTask.Wait();
                userVM = readTask.Result[0];
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
            //Remember Me
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Password = HttpContext.Session.GetString("Password");
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserVM userVM)
        {
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //Check Status LockedOut Account
            var checkLock = client.GetAsync("User/GetUserByEmail/" + userVM.Email);
            checkLock.Wait(); //Waits for the Task to complete execution.
            var statusLock = checkLock.Result;
            var readTask = statusLock.Content.ReadAsAsync<User>(); //Get all the data from the API
            readTask.Wait();
            var lockedTime = readTask.Result.LockoutEnd;
            if (DateTime.Now < lockedTime)
            {
                return RedirectToAction("LockedOut", "Users");
            }
            else
            {
                //Login
                var result = client.PostAsync("User/Login", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    //Get token and role from token
                    var data = result.Content.ReadAsStringAsync().Result;
                    var token = "Bearer " + data;
                    var info = GetTokenInfo(data);
                    //Add token to session and role to session
                    HttpContext.Session.SetString("Id", info[0]);
                    HttpContext.Session.SetString("Role", info[1]);
                    HttpContext.Session.SetString("App", info[2]);
                    HttpContext.Session.SetString("Name", info[3]);
                    //Remember Me
                    if (userVM.checkRemember == "true")
                    {
                        HttpContext.Session.SetString("Email", info[4]);
                        HttpContext.Session.SetString("Password", userVM.Password);
                    }
                    else
                    {
                        HttpContext.Session.Remove("Email");
                        HttpContext.Session.Remove("Password");
                    }
                    HttpContext.Session.SetString("JWToken", token);
                    if (info[1] == "Admin")
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        return RedirectToAction("EditAccount", "Users");
                    }
                }
                ModelState.AddModelError("Email", "Email or Password is Wrong!");
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("App");
            HttpContext.Session.Remove("Name");
            return RedirectToAction("Login", "Users");
        }

        public IActionResult EditAccount()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            ViewBag.PacketText = HttpContext.Session.GetString("Name");
            if (role != null)
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Users");
        }

        public JsonResult LoadProfile()
        {
            var Id = Int32.Parse(HttpContext.Session.GetString("Id"));
            //Get the session with token and set authorize bearer token to API header
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            UserVM userVM = null;
            var responseTask = client.GetAsync("User/" + Id); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<UserVM>>(); //Get all the data from the API
                readTask.Wait();
                userVM = readTask.Result[0];
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(userVM);
        }

        protected List<string> GetTokenInfo(string token)
        {
            List<string> result = new List<string>();
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
            result.Add(data.SingleOrDefault(p => p.Type == "Id").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Role").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "App").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Name").Value);
            result.Add(data.SingleOrDefault(p => p.Type == "Email").Value);
            return result;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult LockedOut()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(UserVM userVM)
        {
            var password = Guid.NewGuid().ToString(); //generate password with guid
            userVM.Password = password; // changes the password with new password
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("User/ForgotPassword", byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                SendPassword(userVM, "Pasword Recovery");
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

        public void SendPassword(UserVM userVM, string message)
        {
            MailMessage mm = new MailMessage("projectbootcamp35@gmail.com", userVM.Email);
            string today = DateTime.Now.ToString();
            mm.Subject = message + " (" + today + ")";
            mm.Body = string.Format("Hi {0},<br /><br />Your password is: <br />{1}<br /><br />Thank You.", userVM.Email, userVM.Password);
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            //Definition of sender
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "projectbootcamp35@gmail.com";
            NetworkCred.Password = "girhoz16!";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
        }
    }
}