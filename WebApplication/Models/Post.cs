using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Post
    {   
        public Post()
        {
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
            this.Date = DateTime.Now;
            
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        public string Author_Id { get; set; }
        
        [ForeignKey("Author_Id")]
        public virtual ApplicationUser Author { get; set; }
        
        public virtual ICollection<Comment> Comments  { get; set; }

        public virtual int CommentsCount { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public  Category Category { get; set; }
    }
}