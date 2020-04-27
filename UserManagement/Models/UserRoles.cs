using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_T_UserRoles")]
    public class UserRoles
    {
        public User User { get; set; }
        public Role Role { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
    }
}
