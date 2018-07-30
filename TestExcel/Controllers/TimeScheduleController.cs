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
    [adminauthen]
    public class TimeScheduleController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();

        // GET: TimeSchedule
        public ActionResult Index()
        {
            var BRANCH_NAME = db.DEPARTMENTs.Select(x => x.BRANCH_NAME).First();
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(BRANCH_NAME)
                        select new Section_Subject
                        {
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END
                        };
            ViewBag.BRANCH_NAME = BRANCH_NAME;
            ViewBag.DDLSelected = 1;
            var rr = query.Where(x => x.SECTION_TIME_START <= 15.00 && x.SECTION_DATE == "M").Any();
            ViewBag.ddl_Branch = new SelectList(db.DEPARTMENTs.ToList(), "ID", "BRANCH_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int Branch_id = int.Parse(collection["DDL_BRANCH"]);
            var BRANCH_NAME = db.DEPARTMENTs.Where(x => x.ID == Branch_id).First().DEPARTMENT_NAME;
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(BRANCH_NAME)
                        select new Section_Subject
                        {
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                        };
            ViewBag.BRANCH_NAME = BRANCH_NAME;
            ViewBag.ddl_Branch = new SelectList(db.DEPARTMENTs.ToList(), "ID", "BRANCH_NAME");
            ViewBag.DDLSelected = Branch_id;
                //query = query.Where(x => x.SECTION_NUMBER != "");
                return View(query);
        }
        [HttpPost]
        public ActionResult updatedata(FormCollection collection)
        {
            string[] date = { "M", "T", "W", "H", "F", "S" };
            var Mname = "";
            var Tname = "";
            var Wname = "";
            var Hname = "";
            var Fname = "";
            var Sname = "";
            for (int a = 0; a < 6;a++)
            {
                for (int b = 8; b < 22; b++)
                {
                    if (a == 0)
                    {
                        Mname = collection[date[a] + "name" + b];
                    }
                    else if(a == 1)
                    {
                        Tname = collection[date[a] + "name" + b];
                    }
                    else if (a == 2)
                    {
                        Wname = collection[date[a] + "name" + b];
                    }
                    else if (a == 3)
                    {
                        Hname = collection[date[a] + "name" + b];
                    }
                    else if (a == 4)
                    {
                        Fname = collection[date[a] + "name" + b];
                    }
                    else if (a == 5)
                    {
                        Sname = collection[date[a] + "name" + b];
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}