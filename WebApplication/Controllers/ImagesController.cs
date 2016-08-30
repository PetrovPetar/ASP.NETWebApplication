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
                    image.Author = db.Users.Single(u => u.UserName == User.Identity.Name);
                    db.Users.Find(image.Author.Id).Images.Add(image);
                    db.Images.Add(image);
                    db.SaveChanges();
                    this.AddNotification("Успешно качихте изображението.", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                this.AddNotification("Грешка! Файлът не се качи.", NotificationType.ERROR);
            }

            this.AddNotification("Първо трябва да се впишете с потребителски профил.", NotificationType.WARNING);
            return RedirectToAction("Index");
           
        }


        // GET: Images/Edit/5
       
        // GET: Images/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var currentImage = db.Images.Find(id);

            if(currentImage.Author.UserName == User.Identity.Name ||
                currentUser.Role == "Admin")
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
            return RedirectToAction("Index");
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            
            var image = db.Images.Find(id);
            if(image != null)
            {
                db.Images.Remove(image);
                db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Images.Remove(image);
                db.SaveChanges();
                this.AddNotification("Изображението е изтрито.", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            this.AddNotification("Грешка! Изображението не съществува!", NotificationType.ERROR);
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
