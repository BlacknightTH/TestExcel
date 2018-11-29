using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Models;
using TestExcel.Utility;

namespace TestExcel.Controllers
{
    public class LoginController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: Login
        public ActionResult Index()
        {
            //var model = from a in db.SECTIONs
            //            group a by new
            //            {
            //                classroom = a.SECTION_CLASSROOM
            //            } into s
            //            select new Section_Subject{
            //                SECTION_CLASSROOM = s.Key.classroom
            //            };
            //var query = db.SECTIONs.GroupBy(x => new { classroom = x.SECTION_CLASSROOM }).Select(y => new Section_Subject { SECTION_CLASSROOM = y.Key.classroom });
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string Username = collection["Username"];
            string Password = collection["Password"];

            try
            {
                var obj = db.USERs.Where(x => x.USER_USERNAME == Username && x.USER_PASSWORD == Password).FirstOrDefault();
                if (obj != null)
                {
                    Session["Username"] = obj.USER_USERNAME.ToString();
                    Session["status"] = obj.USER_STATUS.ToString();
                    return RedirectToAction("DSchedule", "TimeSchedule");
                }
                else
                {
                    ViewBag.Message = "คุณไม่มีสิทธิเข้าใช้งานกรุณาติดต่อ admin";
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "ไม่มีชื่อผู้ใช้";
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}