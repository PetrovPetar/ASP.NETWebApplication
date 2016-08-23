<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();
            var posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(5);
            return View(posts.ToList());
        }

    }
=======
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();
            var posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(5);
            return View(posts.ToList());
        }

    }
>>>>>>> d792e6801b2b6d3c9a4d9063835e002fabf139f1
}