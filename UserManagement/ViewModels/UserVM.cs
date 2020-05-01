using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class UserVM : IEntity
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int App_Type { get; set; }
        [Required]
        public int RoleId { get; set; }

        public string AppName { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public int ReligionId { get; set; }
        public string ReligionName { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public bool WorkStatus { get; set; }
    }
}
