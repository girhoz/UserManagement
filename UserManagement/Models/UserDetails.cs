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
    public class UserDetails : IEntity
    {
        [ForeignKey("User"), Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        [ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        [ForeignKey("Batch")]
        public int? BatchId { get; set; }
        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public bool WorkStatus { get; set; }
        public User User { get; set; }
        public Religion Religion { get; set; }
        public Batch Batch { get; set; }
        public Class Class { get; set; }
    }
}
