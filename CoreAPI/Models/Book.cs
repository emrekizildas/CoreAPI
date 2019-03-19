using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreAPI.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }

    }
}
