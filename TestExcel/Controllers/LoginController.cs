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
            var professor = (from e1 in db.SECTIONs
                        join e2 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e2.PROFESSOR_SHORTNAME
                        where e1.SEMESTER.Contains("1") && e1.YEAR.Contains("2560")
                        select new Section_Professor
                        {
                           PROFESSOR_ID = e2.PROFESSOR_ID,
                           SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME
                        }).OrderBy(x => x.SECTION_PROFESSOR_SHORTNAME);
            ViewBag.ddl_Professor = new SelectList(professor.Select(x => new { x.PROFESSOR_ID,x.SECTION_PROFESSOR_SHORTNAME }).Distinct(), "PROFESSOR_ID", "SECTION_PROFESSOR_SHORTNAME");
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
                    return RedirectToAction("data", "Report");
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