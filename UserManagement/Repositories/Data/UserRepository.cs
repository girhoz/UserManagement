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
    }
}
