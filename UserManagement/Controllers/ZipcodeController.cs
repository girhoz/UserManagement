using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Base;
using API.Models;
using API.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ZipcodeController : BaseController<Zipcode, ZipcodeRepository>
    {
        private readonly ZipcodeRepository _zipcodeRepository;

        public ZipcodeController(ZipcodeRepository repository, ZipcodeRepository zipcodeRepository) : base(repository)
        {
            this._zipcodeRepository = zipcodeRepository;
        }

        [HttpGet]
        [Route("GetByDistrictId/{id}")]
        public async Task<IEnumerable<Zipcode>> GetByDistrictId(int id)
        {
            return await _zipcodeRepository.GetByDistrictId(id);
        }
    }
}