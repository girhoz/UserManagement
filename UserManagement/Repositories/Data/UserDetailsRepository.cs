using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class UserDetailsRepository
    {
        private readonly MyContext _myContext;

        public UserDetailsRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<UserDetails> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == null)
            {
                return entity;
            }
            _myContext.Set<UserDetails>().Remove(entity); ;
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<UserDetails> Get()
        {
            return _myContext.Set<UserDetails>().ToList();
        }

        public async Task<UserDetails> Get(int id)
        {
            return await _myContext.Set<UserDetails>().FindAsync(id);
        }

        public async Task<UserDetails> Post(UserDetails entity)
        {
            await _myContext.Set<UserDetails>().AddAsync(entity);
            await _myContext.SaveChangesAsync();
            return entity;
        }

        public async Task<UserDetails> Put(UserDetails entity)
        {
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }
    }
}
