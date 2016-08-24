
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApplication.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Posts = new HashSet<Post>();

        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual HashSet<Post> Posts { get; set; }
    }
}