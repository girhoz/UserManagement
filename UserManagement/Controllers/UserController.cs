using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DevOne.Security.Cryptography.BCrypt;
using API.Base;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly UserDetailsRepository _userDetailsRepository;
        public IConfiguration _configuration;

        public UserController(UserRepository userRepository, RoleRepository roleRepository, UserDetailsRepository userDetailsRepository, IConfiguration configuration)
        {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._userDetailsRepository = userDetailsRepository;
            this._configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(UserVM userVM)
        {
            var checkEmail = _userRepository.GetByEmail(userVM.Email);
            //Check if email is used or not 
            if (checkEmail != null)
            {
                return BadRequest("Email Already Used!");
            }
            else if (!(userVM.Email.Contains("@gmail.com") || userVM.Email.Contains("@yahoo.com")))
            {
                return BadRequest("Invalid Email Address");
            }
            else
            {
                User user = new User();
                //Generate pass with guid and bcrypt 
                userVM.Password = Guid.NewGuid().ToString();
                var guiPass = userVM.Password;
                var salt = BCryptHelper.GenerateSalt(12);

                user.Email = userVM.Email;
                user.Password = BCryptHelper.HashPassword(guiPass, salt);
                user.App_Type = userVM.App_Type;
                var result = await _userRepository.Post(user);
                if (result != null)
                {
                    //Adding role to the user 
                    await _roleRepository.InsertUserRoles(user.Id, userVM.RoleId);
                    //Adding user detail to the user
                    UserDetails userDetails = new UserDetails();
                    userDetails.Id = user.Id;
                    userDetails.FirstName = userVM.FirstName;
                    userDetails.LastName = userVM.LastName;
                    userDetails.Address = userVM.Address;
                    if (CheckDate(userVM.BirthDate) == false)
                    {
                        return BadRequest("Date Not Valid");
                    }
                    userDetails.BirthDate = userVM.BirthDate;
                    userDetails.PhoneNumber = userVM.PhoneNumber;
                    if (userVM.ReligionId == 0)
                    {
                        userVM.ReligionId = 1;
                    }
                    userDetails.ReligionId = userVM.ReligionId;
                    if (userVM.BatchId == 0)
                    {
                        userVM.BatchId = 1;
                    }
                    userDetails.BatchId = userVM.BatchId;
                    if (userVM.ClassId == 0)
                    {
                        userVM.ClassId = 1;
                    }
                    userDetails.ClassId = userVM.ClassId;
                    if (userVM.StateId == 0)
                    {
                        userVM.StateId = 1;
                    }
                    userDetails.StateId = userVM.StateId;
                    if (userVM.DistrictId == 0)
                    {
                        userVM.DistrictId = 1;
                    }
                    userDetails.DistrictId = userVM.DistrictId;
                    if (userVM.ZipcodeId == 0)
                    {
                        userVM.ZipcodeId = 3;
                    }
                    if (userVM.Gender == null)
                    {
                        userVM.Gender = "Unspecified";
                    }
                    userDetails.Gender = userVM.Gender;
                    userDetails.ZipcodeId = userVM.ZipcodeId;
                    await _userDetailsRepository.Post(userDetails);
                    //Send Password To Email
                    SendPassword(userVM, "Password New Account");
                    return Ok("Register Succesfull!");
                }
                else
                {
                    return BadRequest("Failed to Register User");
                }
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(UserVM userVM)
        {
            var getUser = _userRepository.GetByEmail(userVM.Email);
            if (getUser == null)
            {
                return BadRequest("Email Wrong!");
            }
            else
            {
                //Lockout account
                if (getUser.LockoutEnd != null && DateTime.Now < getUser.LockoutEnd)
                {
                    return BadRequest("Your Account is Locked, Please Try Again Later or Reset Your Password");
                }
                else
                {
                    var check = BCryptHelper.CheckPassword(userVM.Password, getUser.Password);
                    if (check == false)
                    {
                        //Lockout function
                        await LockedOut(getUser);
                        return BadRequest("Password Wrong!");
                    }
                    else
                    {
                        //Reset lockedout account count after succesfull login
                        getUser.FailCount = 0;
                        await _userRepository.Put(getUser);

                        //Get Role From User Login
                        var dataRole = await _roleRepository.GetRole(getUser.Id);
                        foreach (Role item in dataRole)
                        {
                            userVM.RoleName = item.Name;
                        }
                        //Get Data From User Detail
                        var detailUser = await _userDetailsRepository.Get(getUser.Id);

                        //Build JWToken
                        var claims = new List<Claim>
                        {
                            new Claim("Id", getUser.Id.ToString()),
                            new Claim("Email", userVM.Email),
                            new Claim("Role", userVM.RoleName),
                            new Claim("App", getUser.App_Type.ToString()),
                            new Claim("Name", detailUser.FirstName + " " + detailUser.LastName)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                }
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IEnumerable<UserVM>> Details()
        {
            return await _userRepository.GetDetails();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IEnumerable<UserVM>> DetailsById(int id)
        {
            return await _userRepository.GetDetailsById(id);
        }

        [HttpGet]
        [Route("GetUserByEmail/{email}")]
        public User GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut]
        [Route("ChangePassword/{id}")]
        public async Task<ActionResult> ChangePassword(int id, ChangePassVM passVM)
        {
            var user = await _userRepository.Get(id);
            var check = BCryptHelper.CheckPassword(passVM.CurrentPassword, user.Password);
            if (check == true)
            {
                if (passVM.NewPassword.Length < 6)
                {
                    return BadRequest("Password Must Contain At Least Six Characters!");
                }
                var re = new Regex(@"[0-9]+");
                if (!re.IsMatch(passVM.NewPassword))
                {
                    return BadRequest("Password Must Contain At Least One Number (0-9)!");
                }
                re = new Regex(@"[a-z]+"); ;
                if (!re.IsMatch(passVM.NewPassword))
                {
                    return BadRequest("Password Must Contain At Least One Lowercase Letter (a-z)!");
                }
                re = new Regex(@"[A-Z]+");
                if (!re.IsMatch(passVM.NewPassword))
                {
                    return BadRequest("Password Must Contain At Least One Uppercase Letter (A-Z)!");
                }
                re = new Regex(@"[@$!%*#?&]");
                if (!re.IsMatch(passVM.NewPassword))
                {
                    return BadRequest("Password Must Contain At Least One Special Character (@$!%*#?&)!");
                }
                var pass = passVM.NewPassword;
                var salt = BCryptHelper.GenerateSalt(12);
                user.Password = BCryptHelper.HashPassword(pass, salt);
                await _userRepository.Put(user);
                return Ok("Change Password Succesfull");
            }
            return BadRequest("Current Password Wrong");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, UserVM userVM)
        {
            if (userVM.App_Type != 0)
            {
                //Update User
                var user = await _userRepository.Get(id);
                if (userVM.App_Type != user.App_Type && userVM.App_Type != 0)
                {
                    user.App_Type = userVM.App_Type;
                }
                await _userRepository.Put(user);
            }
            //Update User Details
            var userDetails = await _userDetailsRepository.Get(id);
            if (userVM.FirstName != null)
            {
                userDetails.FirstName = userVM.FirstName;
            }
            if (userVM.LastName != null)
            {
                userDetails.LastName = userVM.LastName;
            }
            if (userVM.Gender != null)
            {
                userDetails.Gender = userVM.Gender;
            }
            if (userVM.Address != null)
            {
                userDetails.Address = userVM.Address;
            }
            if (userVM.BirthDate != null)
            {
                if (CheckDate(userVM.BirthDate) == false)
                {
                    return BadRequest("Date Not Valid");
                }
                userDetails.BirthDate = userVM.BirthDate;
            }
            if (userVM.PhoneNumber != null)
            {
                userDetails.PhoneNumber = userVM.PhoneNumber;
            }
            if (userVM.ReligionId != userDetails.ReligionId && userVM.ReligionId != 0)
            {
                userDetails.ReligionId = userVM.ReligionId;
            }
            if (userVM.BatchId != userDetails.BatchId && userVM.BatchId != 0)
            {
                userDetails.BatchId = userVM.BatchId;
            }
            if (userVM.ClassId != userDetails.ClassId && userVM.ClassId != 0)
            {
                userDetails.ClassId = userVM.ClassId;
            }
            if (userVM.StateId != userDetails.StateId && userVM.StateId != 0)
            {
                userDetails.StateId = userVM.StateId;
            }
            if (userVM.DistrictId != userDetails.DistrictId && userVM.DistrictId != 0)
            {
                userDetails.DistrictId = userVM.DistrictId;
            }
            if (userVM.ZipcodeId != userDetails.ZipcodeId && userVM.ZipcodeId != 0)
            {
                userDetails.ZipcodeId = userVM.ZipcodeId;
            }
            if (userVM.WorkStatus != userDetails.WorkStatus)
            {
                userDetails.WorkStatus = userVM.WorkStatus;
            }
            var result = await _userDetailsRepository.Put(userDetails);
            if (result != null)
            {
                return Ok("Update Succesfull");
            }
            else
            {
                return BadRequest("Update Failed");
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("UserAppInfo")]
        public async Task<IEnumerable<ChartVM>> UserAppInfo()
        {
            return await _userRepository.GetUserApp();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("UserReligionInfo")]
        public async Task<IEnumerable<ChartVM>> UserReligionInfo()
        {
            return await _userRepository.GetUserReligion();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("UserBatchInfo")]
        public async Task<IEnumerable<ChartVM>> UserBatchInfo()
        {
            return await _userRepository.GetUserBatch();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("UserClassInfo")]
        public async Task<IEnumerable<ChartVM>> UserClassInfo()
        {
            return await _userRepository.GetUserClass();
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(UserVM userVM)
        {
            var user = _userRepository.GetByEmail(userVM.Email);
            var pass = userVM.Password;
            var salt = BCryptHelper.GenerateSalt(12);
            user.Password = BCryptHelper.HashPassword(pass, salt);
            //Reset Lockedout account count and time
            user.FailCount = 0;
            user.LockoutEnd = null;
            var result = await _userRepository.Put(user);
            if (result != null)
            {
                //Send Password To Email
                SendPassword(userVM, "Pasword Recovery");
                return Ok("Password Updated and Sended to Email");
            }
            return BadRequest("Password Update Unsucsessfull");
        }

        public async Task<ActionResult> LockedOut(User user)
        {
            var getUser = await _userRepository.Get(user.Id);
            if (getUser.FailCount + 1 != 3)
            {
                getUser.FailCount = getUser.FailCount + 1;
            }
            else
            {
                getUser.FailCount = 0;
                getUser.LockoutEnd = DateTime.Now.AddMinutes(5);
            }
            await _userRepository.Put(getUser);
            return Ok("Failed " + getUser.FailCount.ToString());
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

        public bool CheckDate(DateTime? date)
        {
            DateTime today = DateTime.Now;
            DateTime minDate = today.AddYears(-50);
            if ((date >= today) || (date < minDate))
            {
                return false;
            }
            return true;
        }
    }
}