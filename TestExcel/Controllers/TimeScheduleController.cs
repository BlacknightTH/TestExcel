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
        List<Department_Branch> _department_branch = new List<Department_Branch>();
        TestExcelEntities db = new TestExcelEntities();
        // GET: TimeSchedule
        public ActionResult Index()
        {
            var BRANCH_NAMEs = db.BRANCHes.Select(x => x.BRANCH_NAME).First();
            var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(BRANCH_NAMEs) && e1.SEMESTER.Contains("1") && e2.SEMESTER.Contains("1") && e1.YEAR.Contains("2560") && e2.YEAR.Contains("2560")
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
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };

            ViewBag.BRANCH_NAME = BRANCH_NAMEs;
            ViewBag.DDLSelected = 1;
            ViewBag.DepartDDLSelected = 1;
            ViewBag.SYDDLSelected = "1/2560";
            ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", "1/2560");
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.DEPARTMENT_NAME == DEPART_NAMEs).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int Branch_id = int.Parse(collection["DDL_BRANCH"]);
            int Depart_id = int.Parse(collection["DDL_DEPARTMENT"]);
            int count = int.Parse(collection["Count"]);
            string DDL_SEMESTERYEAR = collection["DDL_SEMESTERYEAR"];
            string[] dl = DDL_SEMESTERYEAR.Split('/');
            string semester = dl[0];
            string year = dl[1];
            string temp, contain, BRANCH_NAME;
            if (count == 1)
            {
                temp = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == Depart_id).First().DEPARTMENT_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).First().BRANCH_NAME;
                int BRANCH_ID = db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).First().BRANCH_ID;
                var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
                contain = BRANCH_NAME;
                Branch_id = BRANCH_ID;
            }
            else
            {
                temp = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == Depart_id).First().DEPARTMENT_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.BRANCH_ID == Branch_id).First().BRANCH_NAME;
                var DEPART_NAMEs = db.DEPARTMENTs.Select(x => x.DEPARTMENT_NAME).First();
                string[] br = BRANCH_NAME.Split('\r');
                contain = br[0];
            }
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(contain) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.BRANCH_NAME = BRANCH_NAME;
            ViewBag.DDLSelected = Branch_id;
            ViewBag.DepartDDLSelected = Depart_id;
            ViewBag.SYDDLSelected = DDL_SEMESTERYEAR;
            ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", DDL_SEMESTERYEAR);
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.DEPARTMENT_NAME == temp).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        public ActionResult ClSchedule()
        {
            var Building = "63";
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building) && e1.SEMESTER.Contains("1") && e2.SEMESTER.Contains("1") && e1.YEAR.Contains("2560") && e2.YEAR.Contains("2560")
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
                            BUILDING_NAME = e3.BUILDING_NAME,
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.BUILDING_NAME = Building;
            ViewBag.BDDLSelected = Building;
            ViewBag.DATE = 0;
            ViewBag.SYDDLSelected = "1/2560";
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == "63").ToList();
            string BuildingName = "";
            foreach (var a in BUILDING)
            {
                BuildingName += a.CLASSROOM_NAME + " ";
            }
            ViewBag.PassName = BuildingName;
            ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", "1/2560");
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        [HttpPost]
        public ActionResult ClSchedule(FormCollection collection)
        {
            int Building_id = int.Parse(collection["DDL_BUILDING"]);
            int Date = int.Parse(collection["DDL_DATE"]);
            string DDL_SEMESTERYEAR = collection["DDL_SEMESTERYEAR"];
            string[] dl = DDL_SEMESTERYEAR.Split('/');
            string semester = dl[0];
            string year = dl[1];
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
                            BUILDING_NAME = e3.BUILDING_NAME,
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.BUILDING_NAME = Building_id.ToString();
            ViewBag.BDDLSelected = Building_id;
            ViewBag.DATE = Date;
            ViewBag.SYDDLSelected = DDL_SEMESTERYEAR;
            ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", DDL_SEMESTERYEAR);
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id.ToString()).ToList();
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        //public ActionResult TeSchedule()
        //{
        //    var PROFESSOR_SHORTNAME = db.PROFESSORs.Select(x => x.PROFESSOR_SHORTNAME).First();

        //    var query = from e1 in db.SECTIONs
        //                join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
        //                where e1.SECTION_PROFESSOR_SHORTNAME.Contains(PROFESSOR_SHORTNAME) && e1.SEMESTER.Contains("1") && e2.SEMESTER.Contains("1") && e1.YEAR.Contains("2560") && e2.YEAR.Contains("2560")
        //                select new Section_Subject
        //                {
        //                    SUBJECT_ID = e1.SUBJECT_ID,
        //                    SUBJECT_NAME = e2.SUBJECT_NAME,
        //                    SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
        //                    SECTION_NUMBER = e1.SECTION_NUMBER,
        //                    SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
        //                    SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
        //                    SECTION_DATE = e1.SECTION_DATE,
        //                    SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
        //                    SECTION_TIME_START = e1.SECTION_TIME_START,
        //                    SECTION_TIME_END = e1.SECTION_TIME_END,
        //                    SEMESTER = e1.SEMESTER,
        //                    YEAR = e1.YEAR
        //                };
        //    var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
        //                       select new SemesterYear
        //                       {
        //                           SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
        //                           SEMESTER = d1.SEMESTER,
        //                           YEAR = d1.YEAR
        //                       };
        //    ViewBag.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
        //    ViewBag.PDDLSelected = 1;
        //    ViewBag.SYDDLSelected = "1/2560";
        //    ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", "1/2560");
        //    ViewBag.ddl_Professor = new SelectList(db.PROFESSORs.ToList(), "PROFESSOR_ID", "PROFESSOR_SHORTNAME");
        //    //ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == 63).ToList(), "ID", "CLASSROOM_NAME");
        //    return View(query);
        //}
        //[HttpPost]
        //public ActionResult TeSchedule(FormCollection collection)
        //{
        //    int Professor_id = int.Parse(collection["DDL_PROFESSOR"]);
        //    string DDL_SEMESTERYEAR = collection["DDL_SEMESTERYEAR"];
        //    string[] dl = DDL_SEMESTERYEAR.Split('/');
        //    string semester = dl[0];
        //    string year = dl[1];
        //    var PROFESSOR_SHORTNAME = db.PROFESSORs.Where(x => x.PROFESSOR_ID == Professor_id).First().PROFESSOR_SHORTNAME;

        //    var query = from e1 in db.SECTIONs
        //                join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
        //                where e1.SECTION_PROFESSOR_SHORTNAME.Contains(PROFESSOR_SHORTNAME)
        //                select new Section_Subject
        //                {
        //                    SUBJECT_ID = e1.SUBJECT_ID,
        //                    SUBJECT_NAME = e2.SUBJECT_NAME,
        //                    SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
        //                    SECTION_NUMBER = e1.SECTION_NUMBER,
        //                    SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
        //                    SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
        //                    SECTION_DATE = e1.SECTION_DATE,
        //                    SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
        //                    SECTION_TIME_START = e1.SECTION_TIME_START,
        //                    SECTION_TIME_END = e1.SECTION_TIME_END,
        //                    SEMESTER = e1.SEMESTER,
        //                    YEAR = e1.YEAR
        //                };
        //    var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
        //                       select new SemesterYear
        //                       {
        //                           SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
        //                           SEMESTER = d1.SEMESTER,
        //                           YEAR = d1.YEAR
        //                       };
        //    ViewBag.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
        //    ViewBag.PDDLSelected = Professor_id;
        //    ViewBag.SYDDLSelected = DDL_SEMESTERYEAR;
        //    ViewBag.ddl_SemesterYear = new SelectList(semesteryear.OrderBy(x => x.SEMESTER_YEAR), "SEMESTER_YEAR", "SEMESTER_YEAR", DDL_SEMESTERYEAR);
        //    ViewBag.ddl_Professor = new SelectList(db.PROFESSORs.ToList(), "PROFESSOR_ID", "PROFESSOR_SHORTNAME");
        //    //ViewBag.ddl_Classroom = new SelectList(db.BUILDINGs.Where(x => x.BUILDING_NAME == 63).ToList(), "ID", "CLASSROOM_NAME");
        //    return View(query);
        //}
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
            for (int a = 0; a < 6; a++)
            {
                for (int b = 8; b < 22; b++)
                {
                    if (a == 0)
                    {
                        Mname = collection[date[a] + "name" + b];
                    }
                    else if (a == 1)
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