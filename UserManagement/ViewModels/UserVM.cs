using API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UserVM : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int App_Type { get; set; }
        public int Role_Id { get; set; }
        public string App_Name { get; set; }
        public string Role_Name { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public int Religion { get; set; }
        public string Religion_Name { get; set; }
        public bool WorkStatus { get; set; }
    }
}
