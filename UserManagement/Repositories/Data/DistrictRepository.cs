using API.Context;
using API.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class DistrictRepository : GeneralRepository<District, MyContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        public DistrictRepository(MyContext myContext, IConfiguration configuration, MyContext MyContext) : base(myContext)
        {
            _configuration = configuration;
            _myContext = MyContext;
        }

        public async Task<IEnumerable<District>> GetByStateId(int Id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_GetByStateId_District";
                parameters.Add("@Id", Id);
                var data = await connection.QueryAsync<District>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
