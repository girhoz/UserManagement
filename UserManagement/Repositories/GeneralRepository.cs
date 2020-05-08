using API.Base;
using API.Context;
using API.Repositories.Interface;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class GeneralRepository<TEntity, TContext> : IRepository<TEntity>
            where TEntity : class, IEntity
            where TContext : MyContext
    {
        private readonly MyContext _myContext;

        public GeneralRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return entity;
            }
            _myContext.Set<TEntity>().Remove(entity); ;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _myContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Get(int id)
        {
            return await _myContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Post(TEntity entity)
        {
            await _myContext.Set<TEntity>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Put(TEntity entity)
        {
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public TEntity GetByName(string name)
        {
            return _myContext.Set<TEntity>().SingleOrDefault(e => e.Name == name);
        }
    }
}
