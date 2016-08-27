
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Extensions;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CommentsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Post);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName");
            ViewBag.Post_Id = new SelectList(db.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Text")] Comment comment,int? id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                        file.SaveAs(path);
                        comment.FileName = fileName;
                    }
                    ViewBag.Message = "Upload successful";
                   
                }
                catch
                {
                    ViewBag.Message = "Upload failed";
                   
                }

                Post post = db.Posts.Find(id);
                comment.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                comment.Post = post;
                post.CommentsCount++;
                db.Comments.Add(comment);
                db.Users.Find(comment.Author.Id).Comments.Add(comment);
                db.Posts.Find(id).Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("Коментарът е изпратен.", NotificationType.SUCCESS);
                return RedirectToAction("../Posts/Details/" + id);
            }
            this.AddNotification("Грешка! Коментарът трябва да съдържа поне един символ.", NotificationType.ERROR);
            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName", comment.Author_Id);
            ViewBag.Post_Id = new SelectList(db.Posts, "Id", "Title", comment.Post_Id);
            return RedirectToAction("/Create/" + id);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentComment = db.Comments.Single(c => c.Id == id);
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && (User.Identity.Name == currentComment.Author.UserName ||
               currentUser.Role == "Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = db.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName", comment.Author_Id);
                ViewBag.Post_Id = new SelectList(db.Posts, "Id", "Title", comment.Post_Id);

                return View(comment);
            }
            return RedirectToAction("../");
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Comment newComment, string text, string postId, int? id)
        {
            if (ModelState.IsValid)
            {
                var oldComment = db.Comments.Find(id);
                newComment = oldComment;
                newComment.Text = text;
                db.Entry(newComment).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Коментарът е редактиран.", NotificationType.SUCCESS);
                return RedirectToAction("../Posts/Details/"+ postId);
            }
            this.AddNotification("Грешка! Коментарът трябва да съдържа поне един символ.", NotificationType.ERROR);
            return RedirectToAction("/Edit/" + id);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            var currentComment = db.Comments.Single(c => c.Id == id);
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.Identity.IsAuthenticated && (User.Identity.Name == currentComment.Author.UserName ||
               currentUser.Role == "Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Comment comment = db.Comments.Find(id);
                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
            return RedirectToAction("../");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Comment comment = db.Comments.Find(id);
            var postId = comment.Post.Id;
            db.Posts.Find(postId).CommentsCount--;
            db.Posts.Find(postId).Comments.Remove(comment);
            db.Users.Find(comment.Author.Id).Comments.Remove(comment);
            db.Comments.Remove(comment);
            db.SaveChanges();
            this.AddNotification("Коментарът е изтрит.", NotificationType.INFO);
            return RedirectToAction("../Posts/Details/" + postId);
        }
    
        public FileResult Download(string ImageName)
        {
            return File("~/Files/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet);

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

