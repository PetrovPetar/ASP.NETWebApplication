
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
            //ViewBag.Categories = db.Categories.ToList();
            ViewBag.Tags = db.Tags.ToList();
            var posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date).Take(5);
            return View(posts.ToList());
        }

    }

}