using API.Context;
using API.Models;
using API.ViewModels;
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
    public class UserRepository : GeneralRepository<User, MyContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        public UserRepository(MyContext myContext, IConfiguration configuration, MyContext MyContext) : base(myContext)
        {
            _configuration = configuration;
            _myContext = MyContext;
        }

        public User GetByEmail(string email)
        {
            return _myContext.User.Where(s => s.Email == email).FirstOrDefault();
        }

        public async Task<IEnumerable<UserVM>> GetDetails()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_GetDetails_AllTable";
                var data = await connection.QueryAsync<UserVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<UserVM>> GetDetailsById(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var spName = "SP_GetDetailsById_AllTable";
                parameters.Add("@Id", id);
                var data = await connection.QueryAsync<UserVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
