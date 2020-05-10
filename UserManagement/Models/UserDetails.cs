using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_T_UserDetails")]
    public class UserDetails
    {
        [ForeignKey("User"), Key]
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Please enter correct name")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Please enter correct name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("Religion")]
        public int ReligionId { get; set; }
        [ForeignKey("State")]
        public int StateId { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        [ForeignKey("Zipcode")]
        public int ZipcodeId { get; set; }
        public bool WorkStatus { get; set; }
        public User User { get; set; }
        public Religion Religion { get; set; }
        public State State { get; set; }
        public District District { get; set; }
        public Zipcode Zipcode { get; set; }
    }
}
