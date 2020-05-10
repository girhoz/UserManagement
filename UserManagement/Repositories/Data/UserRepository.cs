using API.Context;
using API.Models;
using API.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class UserRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        public UserRepository(MyContext myContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _myContext = myContext;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return entity;
            }
            _myContext.Set<User>().Remove(entity); ;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<User> Get()
        {
            return _myContext.Set<User>().ToList();
        }

        public async Task<User> Get(int id)
        {
            return await _myContext.Set<User>().FindAsync(id);
        }

        public async Task<User> Post(User entity)
        {
            await _myContext.Set<User>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Put(User entity)
        {
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public User GetByEmail(string email)
        {
            return _myContext.User.Where(s => s.Email == email).FirstOrDefault();
        }

        public async Task<IEnumerable<UserVM>> GetDetails()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_GetDetails_AllTable";
                var data = await connection.QueryAsync<UserVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<UserVM>> GetDetailsById(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_GetDetailsById_AllTable";
                parameters.Add("@Id", id);
                var data = await connection.QueryAsync<UserVM>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ChartVM>> GetUserApp()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_UserAppInfo_UserApplication";
                var data = await connection.QueryAsync<ChartVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ChartVM>> GetUserReligion()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_UserReligionInfo_UserUserDetailsReligion";
                var data = await connection.QueryAsync<ChartVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ChartVM>> GetUserBatch()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_UserBatchInfo_BootCampBatch";
                var data = await connection.QueryAsync<ChartVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<ChartVM>> GetUserClass()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_UserClassInfo_BootCampClass";
                var data = await connection.QueryAsync<ChartVM>(spName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public async Task<IEnumerable<BootCamp>> InsertBootCamp(int userId, int batchId, int classId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                var spName = "SP_Insert_BootCamp";
                parameters.Add("@UserId", userId);
                parameters.Add("@BatchId", batchId);
                parameters.Add("@ClassId", classId);
                var data = await connection.QueryAsync<BootCamp>(spName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        public UserVM GetBootCamp(int userId)
        {
            UserVM info = new UserVM();
            var bootcamp = _myContext.BootCamp.Where(b => b.UserId == userId).SingleOrDefault();
            if(bootcamp != null)
            {
                var batch = _myContext.Batch.Where(b => b.Id == bootcamp.BatchId).SingleOrDefault();
                var clas = _myContext.Class.Where(b => b.Id == bootcamp.ClassId).SingleOrDefault();
                info.BatchId = batch.Id;
                info.ClassId = clas.Id;
                info.BatchName = batch.Name;
                info.ClassName = clas.Name;
            }
            return info;
        }
    }
}
