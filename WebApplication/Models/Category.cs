
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection <Post> Posts { get; set; }
    }

}