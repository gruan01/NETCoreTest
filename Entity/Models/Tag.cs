using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models {
    public class Tag {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
