using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_User")]
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FailCount { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public Application Application { get; set; }
        [ForeignKey("Application")]
        public int App_Type { get; set; }
    }
}
