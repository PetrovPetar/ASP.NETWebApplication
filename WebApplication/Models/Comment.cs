using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
           
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        
        public DateTime Date { get; set; }
        public string Author_Id { get; set; }

        [ForeignKey("Author_Id")]
        public virtual ApplicationUser Author { get; set; }
        public int Post_Id { get; set; }

        [ForeignKey("Post_Id")]
        public virtual Post Post { get; set; }
    }
}