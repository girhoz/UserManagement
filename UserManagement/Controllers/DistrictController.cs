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
    public class DistrictController : BaseController<District, DistrictRepository>
    {
        private readonly DistrictRepository _districtRepository;

        public DistrictController(DistrictRepository repository, DistrictRepository districtRepository) : base(repository)
        {
            this._districtRepository = districtRepository;
        }

        [HttpGet]
        [Route("GetByStateId/{id}")]
        public async Task<IEnumerable<District>> GetByStateId(int id)
        {
            return await _districtRepository.GetByStateId(id);
        }
    }
}