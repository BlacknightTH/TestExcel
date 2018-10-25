using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Models;
using System.Text;
using TestExcel.Utility;

namespace TestExcel.Controllers
{
    public class DataController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: Data
        public ActionResult Section()
        {
            var model = db.SECTIONs.ToList();
            return View(model);
        }
        public ActionResult Subject()
        {
            var model = db.SUBJECTs.ToList();
            return View(model);
        }
        public ActionResult Member()
        {
            var model = db.USERs.ToList();
            return View(model);
        }
        public ActionResult Professor()
        {
            var model = db.PROFESSORs.ToList();
            return View(model);
        }
        public ActionResult Department()
        {
            var model = db.DEPARTMENTs.ToList();
            return View(model);
        }
        public ActionResult Building()
        {
            var model = db.BUILDINGs.ToList();
            return View(model);
        }
        public ActionResult Branch()
        {
            var model = db.BRANCHes.ToList();
            return View(model);
        }
    }
}