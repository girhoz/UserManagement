using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Application")]
    public class Application : IEntity
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Please enter correct name")]
        public string Name { get; set; }
    }
}
