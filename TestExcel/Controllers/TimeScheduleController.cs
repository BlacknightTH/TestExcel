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
            var BRANCH_NAMEs = db.BRANCHes.Select(x => x.BRANCH_NAME).First();
            var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(BRANCH_NAMEs)
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
            ViewBag.BRANCH_NAME = BRANCH_NAMEs;
            ViewBag.DDLSelected = 1;
            ViewBag.DepartDDLSelected = 1;
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "ID", "DEPARTMENT_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.DEPARTMENT_NAME == DEPART_NAMEs).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int Branch_id = int.Parse(collection["DDL_BRANCH"]);
            int Depart_id = int.Parse(collection["DDL_DEPARTMENT"]);
            int count = int.Parse(collection["Count"]);
            string temp, contain, BRANCH_NAME;
            if (count == 1)
            {
                temp = db.DEPARTMENTs.Where(x => x.ID == Depart_id).First().DEPARTMENT_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).First().BRANCH_NAME;
                int BRANCH_ID = db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).First().BRANCH_ID;
                var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
                contain = BRANCH_NAME;
                Branch_id = BRANCH_ID;
            }
            else
            {
                temp = db.DEPARTMENTs.Where(x => x.ID == Depart_id).First().DEPARTMENT_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.BRANCH_ID == Branch_id).First().BRANCH_NAME;
                var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
                string[] br = BRANCH_NAME.Split('\r');
                contain = br[0];
            }
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(contain)
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
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "ID", "DEPARTMENT_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).ToList(), "BRANCH_ID", "BRANCH_NAME");
            ViewBag.DDLSelected = Branch_id;
            ViewBag.DepartDDLSelected = Depart_id;
            //query = query.Where(x => x.SECTION_NUMBER != "");
            return View(query);
        }
        public ActionResult ClSchedule()
        {
            var CLASSROOM_NAME = db.BUILDINGs.Select(x => x.CLASSROOM_NAME).First();
           
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_CLASSROOM.Contains(CLASSROOM_NAME)
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
            ViewBag.BUILDING_NAME = 63;
            ViewBag.CLASSROOM_NAME = CLASSROOM_NAME;
            ViewBag.CDDLSelected = 1;
            ViewBag.BDDLSelected = 63;
            ViewBag.ddl_Building = new SelectList(db.BUILDINGs.Distinct().ToList(), "ID", "BUILDING_NAME");
            ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == 63).ToList(), "ID", "CLASSROOM_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult ClSchedule(FormCollection collection)
        {
            int Building_id = int.Parse(collection["DDL_BUILDING"]);
            int Classroom_id = int.Parse(collection["DDL_CLASSROOM"]);
            int count = int.Parse(collection["Count"]);
            var CLASSROOM_NAME = "";
            if (count == 1)
            {
                CLASSROOM_NAME = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id).First().CLASSROOM_NAME;
                int CLASSROOM_ID = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id).First().ID;
                Classroom_id = CLASSROOM_ID;
            }
            else
            {
                CLASSROOM_NAME = db.BUILDINGs.Where(x => x.ID == Classroom_id).First().CLASSROOM_NAME;
                //string[] br = CLASSROOM_NAME.Split('\r');
                //CLASSROOM_NAME = br[0];
            }

            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_CLASSROOM.Contains(CLASSROOM_NAME)
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
            ViewBag.BUILDING_NAME = Building_id;
            ViewBag.CLASSROOM_NAME = CLASSROOM_NAME;
            ViewBag.CDDLSelected = Classroom_id;
            ViewBag.BDDLSelected = Building_id;
            ViewBag.ddl_Building = new SelectList(db.BUILDINGs.Distinct().ToList(), "ID", "BUILDING_NAME");
            ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id).ToList(), "ID", "CLASSROOM_NAME");
            return View(query);
        }
        public ActionResult TeSchedule()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TeSchedule(FormCollection collection)
        {
            return View();
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