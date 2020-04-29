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
            else
            {
                User user = new User();
                //Generate pass with bcrypt 
                //var guiPass = Guid.NewGuid().ToString();
                var guiPass = userVM.Password;
                var salt = BCryptHelper.GenerateSalt(12);

                user.Email = userVM.Email;
                user.Password = BCryptHelper.HashPassword(guiPass, salt);
                user.App_Type = userVM.App_Type;
                var result = await _userRepository.Post(user);
                if (result != null)
                {
                    //Adding role member to the user 
                    await _roleRepository.InsertUserRoles(user.Id, userVM.RoleId);
                    //Adding user detail to the user
                    UserDetails userDetails = new UserDetails();
                    userDetails.Id = user.Id;
                    if(userVM.FullName == null)
                    {
                        userVM.FullName = userVM.FirstName + " " + userVM.LastName;
                    }
                    userDetails.FullName = userVM.FullName;
                    userDetails.FirstName = userVM.FirstName;
                    userDetails.LastName = userVM.LastName;
                    userDetails.Address = userVM.Address;
                    userDetails.BirthDate = userVM.BirthDate;
                    userDetails.PhoneNumber = userVM.PhoneNumber;
                    if (userVM.ReligionId != 0)
                    {
                        userDetails.ReligionId = userVM.ReligionId;
                    }
                    else
                    {
                        userDetails.ReligionId = 1;
                    }                  
                    await _userDetailsRepository.Post(userDetails);
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
                var check = BCryptHelper.CheckPassword(userVM.Password, getUser.Password);
                if (check == false)
                {
                    return BadRequest("Password Wrong!"); ;
                }
                else
                {
                    //Get Role From User Login
                    var dataRole = await _roleRepository.GetRole(getUser.Id);
                    foreach (Role item in dataRole)
                    {
                        userVM.RoleName = item.Name;
                    }
                    //Build JWToken
                    var claims = new List<Claim>
                        {
                            new Claim("Email", userVM.Email),
                            new Claim("Role", userVM.RoleName),
                            new Claim("App", getUser.App_Type.ToString())
                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, UserVM userVM)
        {
            if (userVM.Email != null || userVM.Password != null || userVM.App_Type != 0)
            {
                //Update User
                var user = await _userRepository.Get(id);
                if (userVM.Password != null)
                {
                    var pass = userVM.Password;
                    var salt = BCryptHelper.GenerateSalt(12);
                    user.Password = BCryptHelper.HashPassword(pass, salt);
                }
                if (userVM.App_Type != 0)
                {
                    user.App_Type = userVM.App_Type;
                }
                await _userRepository.Put(user);
            }
            //Update User Details
            var userDetails = await _userDetailsRepository.Get(id);
            if (userVM.FullName != null)
            {
                userDetails.FullName = userVM.FullName;
            }
            if (userVM.FirstName != null)
            {
                userDetails.FirstName = userVM.FirstName;
            }
            if (userVM.LastName != null)
            {
                userDetails.LastName = userVM.LastName;
            }
            if (userVM.Address != null)
            {
                userDetails.Address = userVM.Address;
            }
            if (userVM.BirthDate != null)
            {
                userDetails.BirthDate = userVM.BirthDate;
            }
            if (userVM.PhoneNumber != null)
            {
                userDetails.PhoneNumber = userVM.PhoneNumber;
            }
            if (userVM.ReligionId != 0)
            {
                userDetails.ReligionId = userVM.ReligionId;
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
    }
}