using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreAPI.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}