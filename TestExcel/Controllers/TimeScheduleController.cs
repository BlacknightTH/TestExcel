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
            var Building = "63";
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building)
                        select new Building_Classroom
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
                            BUILDING_NAME = e3.BUILDING_NAME
                        };
            ViewBag.BUILDING_NAME = Building;
            ViewBag.BDDLSelected = Building;
            ViewBag.DATE = 0;
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == "63").ToList();
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        [HttpPost]
        public ActionResult ClSchedule(FormCollection collection)
        {
            int Building_id = int.Parse(collection["DDL_BUILDING"]);
            int Date = int.Parse(collection["DDL_DATE"]);
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building_id.ToString())
                        select new Building_Classroom
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
                            BUILDING_NAME = e3.BUILDING_NAME
                        };
            ViewBag.BUILDING_NAME = Building_id.ToString();
            ViewBag.BDDLSelected = Building_id;
            ViewBag.DATE = Date;
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id.ToString()).ToList();
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        public ActionResult TeSchedule()
        {
            var PROFESSOR_SHORTNAME = db.PROFESSORs.Select(x => x.PROFESSOR_SHORTNAME).First();

            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_PROFESSOR_SHORTNAME.Contains(PROFESSOR_SHORTNAME)
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
            ViewBag.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
            ViewBag.PDDLSelected = 1;
            ViewBag.ddl_Professor = new SelectList(db.PROFESSORs.ToList(), "ID", "PROFESSOR_SHORTNAME");
            //ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == 63).ToList(), "ID", "CLASSROOM_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult TeSchedule(FormCollection collection)
        {
            int Professor_id = int.Parse(collection["DDL_PROFESSOR"]);

            var PROFESSOR_SHORTNAME = db.PROFESSORs.Where(x => x.ID == Professor_id).First().PROFESSOR_SHORTNAME;

            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_PROFESSOR_SHORTNAME.Contains(PROFESSOR_SHORTNAME)
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
            ViewBag.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
            ViewBag.PDDLSelected = Professor_id;
            ViewBag.ddl_Professor = new SelectList(db.PROFESSORs.ToList(), "ID", "PROFESSOR_SHORTNAME");
            //ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == 63).ToList(), "ID", "CLASSROOM_NAME");
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