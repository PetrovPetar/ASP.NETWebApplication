
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
        // GET: Tags
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
       
        public ActionResult Create()
        {
           
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && 
               currentUser.Role == "Admin")
            {
                ViewBag.Posts = db.Posts.ToList();
                return View();
            }
            return RedirectToAction("../");
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create([Bind(Include = "Id,Name")] Tag tag, string [] post)
        {
          
            if (ModelState.IsValid)
            {
                
                foreach (var p in post)
                {
                    var id = Convert.ToInt32(p);
                    tag.Posts.Add(db.Posts.Single(x => x.Id == id));
                    
                    db.Posts.Find(id).Tags.Add(tag);
                }
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("/Details/"+ tag.Id);
            }
            
            return View(tag);
        }

        // GET: Tags/Edit/5
       
        public ActionResult Edit(int? id)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated &&
               currentUser.Role == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tag tag = db.Tags.Find(id);
                if (tag == null)
                {
                    return HttpNotFound();
                }
                return View(tag);
            }
            return RedirectToAction("../");
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
       
        public ActionResult Delete(int? id)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated &&
               currentUser.Role == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tag tag = db.Tags.Find(id);
                if (tag == null)
                {
                    return HttpNotFound();
                }
                return View(tag);
            }
            return RedirectToAction("../");
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
      
        public ActionResult DeleteConfirmed(int id)
        {
            
            Tag tag = db.Tags.Find(id);
            foreach (var post in tag.Posts.ToList())
            {
                var postId = post.Id;
                db.Posts.Find(postId).Tags.Remove(tag);
            }
            db.Tags.Remove(tag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}

