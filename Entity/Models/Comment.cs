using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entity.Models {
    public class Comment {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CommentID { get; set; }

        [StringLength(10)]
        public string User { get; set; }

        public int? UserID { get; set; }

        [StringLength(500)]
        public string Ctx { get; set; }

        public DateTime CreatedOn { get; set; }

        [ForeignKey("PostID")]
        public int PostID { get; set; }

        public virtual Post Post { get; set; }
    }
}
