
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        
        public ActionResult Index()
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && currentUser.Role == "Admin")
            {
                return View(db.Categories.ToList());
            }
            return RedirectToAction("../");
        }

        // GET: Categories/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
      
        public ActionResult Create()
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            ViewBag.Posts = db.Posts.ToList();
            if (User.Identity.IsAuthenticated && currentUser.Role == "Admin")
            {
                return View();
            }
            return RedirectToAction("../");
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Name = name;
                db.Categories.Add(category);
                db.SaveChanges();
                this.AddNotification("Category is created.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            this.AddNotification("!", NotificationType.ERROR);
            return View();
        }

        // GET: Categories/Edit/5
       
        public ActionResult Edit(int? id)
        {
            ViewBag.Posts = db.Posts.ToList();
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && currentUser.Role == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category category = db.Categories.Find(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            return RedirectToAction("../");
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(string name, string [] posts, int? id)
        {
            if (ModelState.IsValid)
            {
                var oldCategory = db.Categories.Find(id);
                var newCategory = oldCategory;
                newCategory.Name = name;

                if(posts != null)
                {
                    foreach (var p in posts)
                    {
                        var postId = Convert.ToInt32(p);
                        var currentPost = db.Posts.Find(postId);
                        if (!newCategory.Posts.Contains(currentPost))
                        {
                            newCategory.Posts.Add(currentPost);
                            var previousCategory = db.Posts.Find(postId).Category;
                            db.Categories.Find(previousCategory.Id).Posts.Remove(currentPost);
                            db.Posts.Find(postId).Category = newCategory;
                        }
                    }
                }
              
                db.Entry(newCategory).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Category is edited!", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            this.AddNotification("", NotificationType.ERROR);
            return View();
        }

        // GET: Categories/Delete/5
        
        public ActionResult Delete(int? id)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && currentUser.Role == "Admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category category = db.Categories.Find(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            return RedirectToAction("../");
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(int id)
        {

            Category category = db.Categories.Find(id);
            var categoryOther = db.Categories.Find(21);
            foreach (var post in category.Posts.ToList())
            {
                db.Posts.Find(post.Id).Category = categoryOther;
                categoryOther.Posts.Add(post);
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            this.AddNotification("Category is deleted!", NotificationType.INFO);
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

