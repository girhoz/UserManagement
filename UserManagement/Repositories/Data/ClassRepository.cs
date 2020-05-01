using API.Context;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories.Data
{
    public class ClassRepository : GeneralRepository<Class, MyContext>
    {
        public ClassRepository(MyContext myContext) : base(myContext)
        {
        }
    }
}
