using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Utility;

namespace TestExcel.Controllers
{
    public class LoginController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: Login
        public ActionResult Index()
        {
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