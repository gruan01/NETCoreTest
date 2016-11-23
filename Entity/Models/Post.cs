using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models {
    public partial class Post {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [ForeignKey("BlogID")]
        public int BlogId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime? ModifyOn { get; set; }

        public virtual Blog Blog { get; set; }

    }
}
