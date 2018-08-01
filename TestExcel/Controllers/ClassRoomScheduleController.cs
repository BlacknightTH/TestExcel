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
        public ActionResult Building()
        {
            //ViewBag.Confirm = false;
            var query = from e1 in db.SECTIONs
                        join e2 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e2.CLASSROOM_NAME
                        join e3 in db.SUBJECTs on e1.SUBJECT_ID equals e3.SUBJECT_ID
                        join e4 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e4.PROFESSOR_SHORTNAME
                        where e2.BUILDING_NAME == 62
                        select new Building_Classroom
                        {
                            CLASSROOM_NAME = e2.CLASSROOM_NAME,
                            SUBJECT_ID = e3.SUBJECT_ID,
                            SUBJECT_NAME = e3.SUBJECT_NAME,
                            SUBJECT_CREDIT = e3.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e4.PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                            SEMESTER = e3.SEMESTER,
                            YEAR = e3.YEAR
                        };
            ViewBag.DDLSelected = 1;
            ViewBag.DDLPSelected = 62;
            ViewBag.ddl_Branch = new SelectList(db.PROFESSORs.ToList(), "ID", "PROFESSOR_SHORTNAME");
            //var model = db.BUILDINGs.Where(x => x.BUILDING_NAME == 62).ToList();
            //ViewBag.Model = model;
            //return View(query);
            return View();
        }
        [HttpPost]
        public ActionResult Building(FormCollection collection)
        {
            //ViewBag.Confirm = true;
            int P = int.Parse(collection["DDL_P"]);
            int PA = int.Parse(collection["DDL_PA"]);
            string P_NAME = db.PROFESSORs.Where(x => x.ID == P).First().PROFESSOR_SHORTNAME;
            var query = from e1 in db.SECTIONs
                        join e2 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e2.CLASSROOM_NAME
                        join e3 in db.SUBJECTs on e1.SUBJECT_ID equals e3.SUBJECT_ID
                        join e4 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e4.PROFESSOR_SHORTNAME
                        where e2.BUILDING_NAME == PA && e1.SECTION_PROFESSOR_SHORTNAME.Contains(P_NAME)
                        select new Building_Classroom
                        {
                            CLASSROOM_NAME = e2.CLASSROOM_NAME,
                            SUBJECT_ID = e3.SUBJECT_ID,
                            SUBJECT_NAME = e3.SUBJECT_NAME,
                            SUBJECT_CREDIT = e3.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                            SEMESTER = e3.SEMESTER,
                            YEAR = e3.YEAR
                        };
            ViewBag.DDLSelected = P;
            ViewBag.ddl_Branch = new SelectList(db.PROFESSORs.ToList(), "ID", "PROFESSOR_SHORTNAME");
            var model = db.BUILDINGs.Where(x => x.BUILDING_NAME == PA).ToList();
            ViewBag.Model = model;
            return PartialView("Results",query);
        }
        //public JsonResult AjaxMethod(int DDL_P)
        //{
        //    ViewBag.Confirm = true;
        //    string P_NAME = db.PROFESSORs.Where(x => x.ID == DDL_P).First().PROFESSOR_SHORTNAME;
        //    var query = from e1 in db.SECTIONs
        //                join e2 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e2.CLASSROOM_NAME
        //                join e3 in db.SUBJECTs on e1.SUBJECT_ID equals e3.SUBJECT_ID
        //                join e4 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e4.PROFESSOR_SHORTNAME
        //                where e2.BUILDING_NAME == 62 && e1.SECTION_PROFESSOR_SHORTNAME.Contains(P_NAME)
        //                select new Building_Classroom
        //                {
        //                    CLASSROOM_NAME = e2.CLASSROOM_NAME,
        //                    SUBJECT_ID = e3.SUBJECT_ID,
        //                    SUBJECT_NAME = e3.SUBJECT_NAME,
        //                    SUBJECT_CREDIT = e3.SUBJECT_CREDIT,
        //                    SECTION_NUMBER = e1.SECTION_NUMBER,
        //                    SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
        //                    SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
        //                    SECTION_DATE = e1.SECTION_DATE,
        //                    SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
        //                    SECTION_TIME_START = e1.SECTION_TIME_START,
        //                    SECTION_TIME_END = e1.SECTION_TIME_END,
        //                    SEMESTER = e3.SEMESTER,
        //                    YEAR = e3.YEAR
        //                };
        //    ViewBag.DDLSelected = DDL_P;
        //    ViewBag.ddl_Branch = new SelectList(db.PROFESSORs.ToList(), "ID", "PROFESSOR_SHORTNAME");
        //    var model = db.BUILDINGs.Where(x => x.BUILDING_NAME == 62).ToList();
        //    ViewBag.Model = model;
        //    return Json(query);
        //}
    }
}