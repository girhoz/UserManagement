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
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public int App_Type { get; set; }
        public int RoleId { get; set; }
        public string AppName { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Please enter correct name")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Please enter correct name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
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
        public string checkRemember { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int ZipcodeId { get; set; }
        public string ZipcodeName { get; set; }
    }

    public class ChangePassVM
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
