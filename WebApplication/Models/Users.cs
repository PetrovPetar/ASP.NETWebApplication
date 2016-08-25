using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class Users : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Users = db.Users.ToList();
        }
    }
}