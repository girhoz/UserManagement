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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<User, UserRepository>
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        public IConfiguration _configuration;

        public UserController(UserRepository userRepository, RoleRepository roleRepository, IConfiguration configuration) : base(userRepository)
        {
            this._roleRepository = roleRepository;
            this._userRepository = userRepository;
            this._configuration = configuration;
        }

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
                    await _roleRepository.InsertUserRoles(userVM.Id, userVM.Role_Id);
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
            var check = BCryptHelper.CheckPassword(userVM.Password, getUser.Password);
            if (getUser == null || check == false)
            {
                return BadRequest("Username or Email Wrong!");
            }
            else
            {
                //Get Role From User Login
                var dataRole = await _roleRepository.GetRole(getUser.Id);
                foreach (Role item in dataRole)
                {
                    userVM.Role_Name = item.Name;
                }
                //Build JWToken
                var claims = new List<Claim>
                        {
                            new Claim("Email", userVM.Email),
                            new Claim("Role", userVM.Role_Name),
                            new Claim("App", getUser.App_Type.ToString())
                        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

        }
    }
}