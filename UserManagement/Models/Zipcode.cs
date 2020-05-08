using API.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Zipcode")]
    public class Zipcode : IEntity
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Please enter correct name")]
        public string Name { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
