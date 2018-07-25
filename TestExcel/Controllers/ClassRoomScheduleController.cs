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
    public class ClassRoomScheduleController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: ClassRoomSchedule
        public ActionResult Building_62()
        {
            var model = db.BUILDINGs.Where(x => x.BUILDING_NAME == 62).ToList();
            return View(model);
        }
    }
}