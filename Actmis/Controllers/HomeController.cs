using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Actmis.Models;

namespace Actmis.Controllers
{
    public class HomeController : Controller
    {
        ActmisDBEntities db = new ActmisDBEntities();

        //【Index 首頁】
        public ActionResult Index()
        {
            var events = db.tEvents.ToList();
            if (Session["Member"] == null)
            {
                return View("Index", "_Layout", events);
            }
            return View("Index", "_LayoutMember", events);
            //return View();
        }

        //【Filter 活動篩選頁】
        public ActionResult Filter()
        {
            var act = db.tEvents.OrderBy(m => m.fType_no).ThenBy(m => m.fPId).ToList();
            if (Session["Member"] == null)
            {
                return View("Filter", "_Layout", act);
            }
            return View("Filter", "_LayoutMember", act);
        }

    }

}