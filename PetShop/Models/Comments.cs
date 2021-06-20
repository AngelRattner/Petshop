using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetShop.Models
{
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }
        public int AnimalId { get; set; }
        public string Comment { get; set; }
        public virtual Animal Animal { get; set; }
    }
}
