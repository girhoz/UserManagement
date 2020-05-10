using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_T_BootCamp")]
    public class BootCamp
    {
        [ForeignKey("UserDetails")]
        public int UserId { get; set; }
        [ForeignKey("Batch")]
        public int BatchId { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        public UserDetails UserDetails { get; set; }
        public Batch Batch { get; set; }
        public Class Class { get; set; }
    }
}
