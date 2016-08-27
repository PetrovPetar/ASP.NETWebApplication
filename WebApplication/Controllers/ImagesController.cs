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
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Images
        public ActionResult Index()
        {
            var images = db.Images.ToList();
            return View(images);
        }

        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "Id")] Image image, HttpPostedFileBase file)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (file.ContentLength > 0)
                {
                    foreach (var img in db.Images.ToList())
                    {
                        if (img.Name == file.FileName)
                        {
                            this.AddNotification("Изображението вече го има!", NotificationType.WARNING);
                            return RedirectToAction("Index");
                        }
                    }
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Gallery/"), fileName);
                    file.SaveAs(path);
                    image.Name = fileName;

                    db.Images.Add(image);
                    db.SaveChanges();
                    this.AddNotification("Успешно качихте изображението.", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                this.AddNotification("Грешка! Файлът не се качи.", NotificationType.ERROR);
            }

            this.AddNotification("Грешка! Първо трябва да се впишете с потребителски профил.", NotificationType.ERROR);
            return RedirectToAction("../");
           
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Content")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult Download(string ImageName)
        {
            return File("~/Gallery/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet);

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
