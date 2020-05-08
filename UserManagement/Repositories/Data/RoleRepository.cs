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
    public class RoleRepository : GeneralRepository<Role, MyContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        public RoleRepository(MyContext myContext, IConfiguration configuration, MyContext MyContext) : base(myContext)
        {
            _configuration = configuration;
            _myContext = MyContext;
        }

        public async Task<IEnumerable<UserRoles>> InsertUserRoles(int userId, int roleId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_Insert_UserRoles";
                parameters.Add("@UserId", userId);
                parameters.Add("@RoleId", roleId);
                var data = await connection.QueryAsync<UserRoles>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<Role>> GetRole(int userId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_GetRole_RoleUserRoles";
                parameters.Add("@UserId", userId);
                var data = await connection.QueryAsync<Role>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }
    }
}
