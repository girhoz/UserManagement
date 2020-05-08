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
    public class ZipcodeRepository : GeneralRepository<Zipcode, MyContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        public ZipcodeRepository(MyContext myContext, IConfiguration configuration, MyContext MyContext) : base(myContext)
        {
            _configuration = configuration;
            _myContext = MyContext;
        }

        public Zipcode GetByName(string name)
        {
            return _myContext.Zipcode.Where(s => s.Name == name).FirstOrDefault();
        }

        public async Task<IEnumerable<Zipcode>> GetByDistrictId(int Id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_GetByDistrictId_Zipcode";
                parameters.Add("@Id", Id);
                var data = await connection.QueryAsync<Zipcode>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
