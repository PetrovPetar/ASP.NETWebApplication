using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class PostTags
    {
        public Post Post { get; set; }

        public Tag Tag { get; set; }
    }
}