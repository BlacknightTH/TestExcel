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
    public class TimeScheduleController : Controller
    {
        List<Department_Branch> _department_branch = new List<Department_Branch>();
        TestExcelEntities db = new TestExcelEntities();
        int CheckMessage;
        // GET: TimeSchedule
        public List<Section_Subject> GetData(string Branch_Name, string semester, string year)
        {
            List<Section_Subject> section_subject = new List<Section_Subject>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(Branch_Name) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
                        select new Section_Subject
                        {
                            SECTION_ID = e1.SECTION_ID,
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
                            YEAR = e1.YEAR,
                            CRASH = e1.CRASH
                        };
            section_subject = query.OrderBy(x => x.YEAR).ToList();
            return section_subject;
        }
        public List<Section_Subject> PGetData(string Professor,string semester, string year)
        {
            List<Section_Subject> section_subject = new List<Section_Subject>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_PROFESSOR_SHORTNAME.Contains(Professor) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
                        select new Section_Subject
                        {
                            SECTION_ID = e1.SECTION_ID,
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
                            YEAR = e1.YEAR,
                            CRASH = e1.CRASH
                        };
            section_subject = query.OrderBy(x => x.YEAR).ToList();
            return section_subject;
        }
        public List<Building_Classroom> TEGetData(string Building, string semester, string year)
        {
            List<Building_Classroom> Building_subject = new List<Building_Classroom>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                            YEAR = e1.YEAR,
                            CRASH = e1.CRASH
                        };
            Building_subject = query.ToList();
            return Building_subject;
        }
        public ActionResult DSchedule(string BR_NAME, string BR_Semester, string BR_Year,string Message)
        {
            string select_Department, select_branch;
            int select_Departmentid;
            int select_branchid;
            if (BR_NAME == null && BR_Semester == null && BR_Year == null)
            {
                var t = db.SUBJECTs.OrderBy(x => x.YEAR).First();
                BR_Semester = t.SEMESTER;
                BR_Year = t.YEAR;
                select_branchid = 1;
                select_branch = db.BRANCHes.First().BRANCH_NAME;
                select_Department = db.COURSEs.First().COURSE_NAME;
                select_Departmentid = 1;
            }
            else
            {
                select_Department = db.BRANCHes.Where(x => x.BRANCH_NAME == BR_NAME).First().COURSE_NAME;
                select_Departmentid = db.COURSEs.Where(x => x.COURSE_NAME == select_Department).First().COURSE_ID;
                select_branch = BR_NAME;
                select_branchid = db.BRANCHes.Where(x => x.BRANCH_NAME == select_branch).First().BRANCH_ID;
            }
            if(Message != null)
            {
                CheckMessage = int.Parse(Message);
            }
            var query = GetData(select_branch, BR_Semester, BR_Year);
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.BRANCH_NAME = select_branch;
            ViewBag.DDLSelected = select_branchid;
            ViewBag.DepartDDLSelected = select_Departmentid;
            ViewBag.Semester = BR_Semester;
            ViewBag.Year = BR_Year;
            if (CheckMessage == 1)
            {
                ViewBag.Message = "Save Success";
                ViewBag.ErrorMessage = "";
            }
            else if (CheckMessage == 2)
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "Error";
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "";
            }
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", BR_Year);
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == BR_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", BR_Semester);
            ViewBag.ddl_Department = new SelectList(db.COURSEs.ToList(), "COURSE_ID", "COURSE_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.COURSE_NAME == select_Department).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult DSchedule(FormCollection collection)
        {
            int Branch_id = int.Parse(collection["DDL_BRANCH"]);
            int Depart_id = int.Parse(collection["DDL_DEPARTMENT"]);
            int count = int.Parse(collection["Count"]);
            string ddl_Year = collection["ddl_Year"];
            string ddl_Semester = collection["ddl_Semester"];
            string temp, contain, BRANCH_NAME;
            if (count == 1)
            {
                temp = db.COURSEs.Where(x => x.COURSE_ID == Depart_id).First().COURSE_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.COURSE_NAME == temp).First().BRANCH_NAME;
                int BRANCH_ID = db.BRANCHes.Where(x => x.COURSE_NAME == temp).First().BRANCH_ID;
                var COURSE_NAMEs = db.COURSEs.Select(x => x.COURSE_NAME).First();
                contain = BRANCH_NAME;
                Branch_id = BRANCH_ID;
            }
            else
            {
                temp = db.COURSEs.Where(x => x.COURSE_ID == Depart_id).First().COURSE_NAME;
                BRANCH_NAME = db.BRANCHes.Where(x => x.BRANCH_ID == Branch_id).First().BRANCH_NAME;
                var COURSE_NAMEs = db.COURSEs.Select(x => x.COURSE_NAME).First();
                string[] br = BRANCH_NAME.Split('\r');
                contain = br[0];
            }
            var query = GetData(contain, ddl_Semester, ddl_Year);
            if (query.Count == 0)
            {
                ddl_Semester = db.SUBJECTs.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.SEMESTER).First().SEMESTER;
                query = GetData(contain, ddl_Semester, ddl_Year);
            }
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
            ViewBag.Semester = ddl_Semester;
            ViewBag.Year = ddl_Year;
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", ddl_Year);
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", ddl_Semester);
            ViewBag.ddl_Department = new SelectList(db.COURSEs.ToList(), "COURSE_ID", "COURSE_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.COURSE_NAME == temp).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        public ActionResult PSchedule(string BR_Professor,string BR_Semester, string BR_Year, string Message)
        {
            if (BR_Professor == null && BR_Semester == null && BR_Year == null)
            {
                var t = db.SUBJECTs.OrderBy(x => x.YEAR).First();
                BR_Semester = t.SEMESTER;
                BR_Year = t.YEAR;
                BR_Professor = db.PROFESSORs.First().PROFESSOR_SHORTNAME;
            }
            if (Message != null)
            {
                CheckMessage = int.Parse(Message);
            }
            var query = PGetData(BR_Professor, BR_Semester, BR_Year);
            var professor = (from e1 in db.SECTIONs
                             join e2 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e2.PROFESSOR_SHORTNAME
                             where e1.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year)
                             select new Section_Professor
                             {
                                 PROFESSOR_ID = e2.PROFESSOR_ID,
                                 SECTION_PROFESSOR_SHORTNAME = e2.PROFESSOR_SHORTNAME
                             }).OrderBy(x => x.SECTION_PROFESSOR_SHORTNAME);
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.PROFESSOR_NAME = query.First().SECTION_PROFESSOR_SHORTNAME;
            ViewBag.Semester = BR_Semester;
            ViewBag.Year = BR_Year;
            if (CheckMessage == 1)
            {
                ViewBag.Message = "Save Success";
                ViewBag.ErrorMessage = "";
            }
            else if (CheckMessage == 2)
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "Error";
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "";
            }
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            ViewBag.ddl_Professor = new SelectList(professor.Select(x => new { x.PROFESSOR_ID, x.SECTION_PROFESSOR_SHORTNAME }).Distinct(), "SECTION_PROFESSOR_SHORTNAME", "SECTION_PROFESSOR_SHORTNAME", query.First().SECTION_PROFESSOR_SHORTNAME);
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", BR_Year);
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == BR_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", BR_Semester);
            return View(query);
        }
        [HttpPost]
        public ActionResult PSchedule(FormCollection collection)
        {
            string ddl_Professor = collection["ddl_Professor"];
            string ddl_Year = collection["ddl_Year"];
            string ddl_Semester = collection["ddl_Semester"];

            var query = PGetData(ddl_Professor, ddl_Semester, ddl_Year);
            //if (query.Count == 0)
            //{
            //    ddl_Semester = db.SUBJECTs.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.SEMESTER).First().SEMESTER;
            //    query = GetData(ddl_Professor, ddl_Semester, ddl_Year);
            //}
            var professor = (from e1 in db.SECTIONs
                             join e2 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e2.PROFESSOR_SHORTNAME
                             where e1.SEMESTER.Contains(ddl_Semester) && e1.YEAR.Contains(ddl_Year)
                             select new Section_Professor
                             {
                                 PROFESSOR_ID = e2.PROFESSOR_ID,
                                 SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME
                             }).OrderBy(x => x.SECTION_PROFESSOR_SHORTNAME);
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.PROFESSOR_NAME = ddl_Professor;
            ViewBag.Semester = ddl_Semester;
            ViewBag.Year = ddl_Year;
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            ViewBag.ddl_Professor = new SelectList(professor.Select(x => new { x.PROFESSOR_ID, x.SECTION_PROFESSOR_SHORTNAME }).Distinct(), "SECTION_PROFESSOR_SHORTNAME", "SECTION_PROFESSOR_SHORTNAME", ddl_Professor);
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", ddl_Year);
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", ddl_Semester);
            return View(query);
        }
        public ActionResult ClSchedule()
        {
            var Building = "63";
            var t = db.SUBJECTs.OrderBy(x => x.YEAR).First();
            var YEAR = t.YEAR;
            var SEMESTER = t.SEMESTER;
            var query = TEGetData(Building, SEMESTER, YEAR);
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
            ViewBag.Semester = SEMESTER;
            ViewBag.Year = YEAR;
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == YEAR).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", SEMESTER);
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == "63").ToList();
            string BuildingName = "";
            foreach (var a in BUILDING)
            {
                BuildingName += a.CLASSROOM_NAME + " ";
            }
            ViewBag.PassName = BuildingName;
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        [HttpPost]
        public ActionResult ClSchedule(FormCollection collection)
        {
            int Building_id = int.Parse(collection["DDL_BUILDING"]);
            int Date = int.Parse(collection["DDL_DATE"]);
            string semester = collection["ddl_Semester"];
            string year = collection["ddl_Year"];
            List<Building_Classroom> query = new List<Building_Classroom>();

            query = TEGetData(Building_id.ToString(),semester,year);
            if(query.Count == 0)
            {
                semester = db.SUBJECTs.Where(x => x.YEAR == year).OrderBy(x => x.SEMESTER).First().SEMESTER;
                query = TEGetData(Building_id.ToString(), semester, year);
            }
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
            ViewBag.Semester = semester;
            ViewBag.Year = year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id.ToString()).ToList();
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", semester);
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        public ActionResult TeSchedule(string id,string classroom,string SUBJECTid, string BR_Semester, string BR_Year, string Message)
        {
            SUBJECT SUBJECT = new SUBJECT();
            SECTION SECTION = new SECTION();
            var tmp = "";
            var tmp2 = "";
            IQueryable<Section_Subject> query;
            if (SUBJECTid == null && BR_Semester == null && BR_Year == null)
            {
                var g = db.SUBJECTs.OrderBy(x => x.YEAR).First();
                BR_Semester = g.SEMESTER;
                BR_Year = g.YEAR;
                SUBJECT = db.SUBJECTs.Where(x => x.SEMESTER == BR_Semester && x.YEAR == BR_Year).First();
                SECTION = db.SECTIONs.Where(x => x.SUBJECT_ID == SUBJECT.SUBJECT_ID && x.SEMESTER == BR_Semester && x.YEAR == BR_Year).First();
                tmp = SUBJECT.SUBJECT_ID;
                tmp2 = SECTION.SECTION_CLASSROOM;
            }
            else
            {
                SUBJECT = db.SUBJECTs.Where(x => x.SEMESTER == BR_Semester && x.SUBJECT_ID == SUBJECTid && x.YEAR == BR_Year).First();
                SECTION = db.SECTIONs.Where(x => x.SUBJECT_ID == SUBJECT.SUBJECT_ID && x.SEMESTER == BR_Semester && x.YEAR == BR_Year).First();
                tmp = SUBJECTid;
                tmp2 = classroom;
            }
            if (Message != null)
            {
                CheckMessage = int.Parse(Message);
            }
            if (CheckMessage == 1)
            {
                ViewBag.Message = "Save Success";
                ViewBag.ErrorMessage = "";
            }
            else if (CheckMessage == 2)
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "Error";
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "";
            }
                query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SUBJECT_ID.Contains(tmp) && e1.SEMESTER.Contains(BR_Semester) && e2.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year) && e2.YEAR.Contains(BR_Year)
                        select new Section_Subject
                        {
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                        };
                ViewBag.DDL_building = query.Select(x => x.SECTION_CLASSROOM).Distinct();
                ViewBag.CLASSROOM = tmp2;

                query = from e1 in db.SECTIONs
                            join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                            where e1.SUBJECT_ID.Contains(tmp) && e1.SECTION_CLASSROOM == tmp2 && e1.SEMESTER.Contains(BR_Semester) && e2.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year) && e2.YEAR.Contains(BR_Year)
                            select new Section_Subject
                            {
                                SECTION_ID = e1.SECTION_ID,
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
                                YEAR = e1.YEAR,
                                CRASH = e1.CRASH
                            };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            if (id != "")
            {
                ViewBag.Number = id;
            }
            else
            {
                ViewBag.Number = "";
            }
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            int t = query.Count();
            ViewBag.SUBJECT_NAME = SUBJECT.SUBJECT_NAME;
            ViewBag.SUBJECT = tmp;
            ViewBag.Semester = BR_Semester;
            ViewBag.Year = BR_Year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == BR_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", BR_Semester);
            ViewBag.ddl_Subject = new SelectList(db.SUBJECTs.Where(x => x.SEMESTER == BR_Semester && x.YEAR == BR_Year).ToList(), "SUBJECT_ID", "SUBJECT_ID", SUBJECT.SUBJECT_ID);
            ViewBag.ddl_Subject_Name = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_NAME", SUBJECT.SUBJECT_ID);
            return View(query);
        }
        [HttpPost]
        public ActionResult TeSchedule(FormCollection collection)
        {
            SUBJECT SUBJECT = new SUBJECT();
            SECTION SECTION = new SECTION();
            var DDL_SUBJECT = collection["SUBJECT"];
            var ddl_Semester = collection["ddl_Semester"];
            var ddl_Year = collection["ddl_Year"];
            var ddl_classroom = collection["ddl_classroom"];
            SUBJECT = db.SUBJECTs.Where(x => x.SUBJECT_ID == DDL_SUBJECT).First();

            var query = from e1 in db.SECTIONs
                    join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                    where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SEMESTER.Contains(ddl_Semester) && e2.SEMESTER.Contains(ddl_Semester) && e1.YEAR.Contains(ddl_Year) && e2.YEAR.Contains(ddl_Year)
                        select new Section_Subject
                    {
                        SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                    };
            if (query.Count() == 0)
            {
                ddl_Semester = db.SUBJECTs.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.SEMESTER).First().SEMESTER;
                query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SEMESTER.Contains(ddl_Semester) && e2.SEMESTER.Contains(ddl_Semester) && e1.YEAR.Contains(ddl_Year) && e2.YEAR.Contains(ddl_Year)
                        select new Section_Subject
                        {
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                        };
                ddl_classroom = query.FirstOrDefault().SECTION_CLASSROOM;
            }
            else
            {
                foreach (var i in query)
                {
                    if (i.SECTION_CLASSROOM == ddl_classroom)
                    {
                        ddl_classroom = i.SECTION_CLASSROOM;
                    }
                }
            }
            ViewBag.DDL_building = query.Select(x => x.SECTION_CLASSROOM).Distinct();
            ViewBag.CLASSROOM = ddl_classroom;
            query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SECTION_CLASSROOM == ddl_classroom && e1.SEMESTER.Contains(ddl_Semester) && e2.SEMESTER.Contains(ddl_Semester) && e1.YEAR.Contains(ddl_Year) && e2.YEAR.Contains(ddl_Year)
                        select new Section_Subject
                        {
                            SECTION_ID = e1.SECTION_ID,
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
                            YEAR = e1.YEAR,
                            CRASH = e1.CRASH
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            ViewBag.SUBJECT_NAME = SUBJECT.SUBJECT_NAME;
            ViewBag.SUBJECT = DDL_SUBJECT;
            ViewBag.Semester = ddl_Semester;
            ViewBag.Year = ddl_Year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == ddl_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", ddl_Semester);
            ViewBag.ddl_Subject = new SelectList(db.SUBJECTs.Where(x => x.SEMESTER == ddl_Semester && x.YEAR == ddl_Year).ToList(), "SUBJECT_ID", "SUBJECT_ID", DDL_SUBJECT);
            ViewBag.ddl_Subject_Name = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_NAME", DDL_SUBJECT);
            return View(query);
        }
        public ActionResult ReportSchedule(string id, string classroom, string SUBJECTid, string BR_Semester, string BR_Year, string Message)
        {
            SUBJECT SUBJECT = new SUBJECT();
            SECTION SECTION = new SECTION();
            IQueryable<Section_Subject> query;
            if (Message != null)
            {
                CheckMessage = int.Parse(Message);
            }
            if (CheckMessage == 1)
            {
                ViewBag.Message = "Save Success";
                ViewBag.ErrorMessage = "";
            }
            else if (CheckMessage == 2)
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "Error";
            }
            else
            {
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "";
            }
                query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_CLASSROOM == classroom && e1.SEMESTER.Contains(BR_Semester) && e2.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year) && e2.YEAR.Contains(BR_Year)
                        select new Section_Subject
                        {
                            SECTION_ID = e1.SECTION_ID,
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
                            YEAR = e1.YEAR,
                            CRASH = e1.CRASH
                        };
                ViewBag.CLASSROOM = " ";
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            if (id != "")
            {
                ViewBag.Number = id;
            }
            else
            {
                ViewBag.Number = "";
            }
            string ID = "";
            foreach (var i in query)
            {
                ID += i.SECTION_ID.ToString() + ",";
            }
            ViewBag.dataID = ID;
            int t = query.Count();
            ViewBag.SUBJECT_NAME = SUBJECT.SUBJECT_NAME;
            ViewBag.SUBJECT = SUBJECT.SUBJECT_ID;
            ViewBag.Semester = BR_Semester;
            ViewBag.Year = BR_Year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            ViewBag.ddl_Semester = new SelectList(semesteryear.Where(x => x.YEAR == BR_Year).OrderBy(x => x.YEAR).OrderBy(y => y.SEMESTER), "SEMESTER", "SEMESTER", BR_Semester);
            ViewBag.ddl_Subject = new SelectList(db.SUBJECTs.Where(x => x.SEMESTER == BR_Semester && x.YEAR == BR_Year).ToList(), "SUBJECT_ID", "SUBJECT_ID", SUBJECT.SUBJECT_ID);
            ViewBag.ddl_Subject_Name = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_NAME", SUBJECT.SUBJECT_ID);
            return View(query);
        }
        [HttpPost]
        public ActionResult singleupdatedata(FormCollection collection)
        {
            var Message = "0";
            var FIRST_SECTION_ID = collection["FIRST_SECTION_ID"];
            var SECOND_SECTION_ID = collection["SECOND_SECTION_ID"];
            var BR_Professor = collection["BR_Professor"];
            var SUBJECTid = collection["SUBJECTid"];
            var BR_NAME = collection["BR_NAME"];
            var Semester = collection["Semester"];
            var Year = collection["Year"];
            var SEC_ID = collection["SEC_ID2"];
            string CLASSROOM = collection["CLASSROOM"];

            if (ModelState.IsValid && FIRST_SECTION_ID != "" && SECOND_SECTION_ID == "0")
            {
                var tmp_FIRST_SECTION_ID = int.Parse(FIRST_SECTION_ID);
                var FIRST_SAVE_NUMBER = collection["FIRST_SAVE_NUMBER"];
                var FIRST_SAVE_DATE = collection["FIRST_SAVE_DATE"];
                var FIRST_SAVE_CLASSROOM = collection["FIRST_SAVE_CLASSROOM"];
                var FIRST_SAVE_TIMESTART = double.Parse(collection["FIRST_SAVE_TIMESTART"]);
                var FIRST_SAVE_TIMEEND = double.Parse(collection["FIRST_SAVE_TIMEEND"]);

                var edit = db.SECTIONs.Where(x => x.SECTION_ID == tmp_FIRST_SECTION_ID).FirstOrDefault();
                if(edit != null)
                {
                    edit.SECTION_NUMBER = FIRST_SAVE_NUMBER;
                    edit.SECTION_DATE = FIRST_SAVE_DATE;
                    edit.SECTION_CLASSROOM = FIRST_SAVE_CLASSROOM;
                    edit.SECTION_TIME_START = FIRST_SAVE_TIMESTART;
                    edit.SECTION_TIME_END = FIRST_SAVE_TIMEEND;
                }
                Message = "1";
            }
            else if (ModelState.IsValid && FIRST_SECTION_ID != "" && SECOND_SECTION_ID != "0")
            {
                var FIRST_SAVE_NUMBER = collection["FIRST_SAVE_NUMBER"];
                var FIRST_SAVE_DATE = collection["FIRST_SAVE_DATE"];
                var FIRST_SAVE_CLASSROOM = collection["FIRST_SAVE_CLASSROOM"];
                var FIRST_SAVE_TIMESTART = double.Parse(collection["FIRST_SAVE_TIMESTART"]);
                var FIRST_SAVE_TIMEEND = double.Parse(collection["FIRST_SAVE_TIMEEND"]);
                var tmp_FIRST_SECTION_ID = int.Parse(FIRST_SECTION_ID);

                var edit = db.SECTIONs.Where(x => x.SECTION_ID == tmp_FIRST_SECTION_ID).FirstOrDefault();
                if (edit != null)
                {
                    edit.SECTION_NUMBER = FIRST_SAVE_NUMBER;
                    edit.SECTION_DATE = FIRST_SAVE_DATE;
                    edit.SECTION_CLASSROOM = FIRST_SAVE_CLASSROOM;
                    edit.SECTION_TIME_START = FIRST_SAVE_TIMESTART;
                    edit.SECTION_TIME_END = FIRST_SAVE_TIMEEND;
                }

                var SECOND_SAVE_NUMBER = collection["SECOND_SAVE_NUMBER"];
                var SECOND_SAVE_DATE = collection["SECOND_SAVE_DATE"];
                var SECOND_SAVE_CLASSROOM = collection["SECOND_SAVE_CLASSROOM"];
                var SECOND_SAVE_TIMESTART = double.Parse(collection["SECOND_SAVE_TIMESTART"]);
                var SECOND_SAVE_TIMEEND = double.Parse(collection["SECOND_SAVE_TIMEEND"]);
                var tmp_SECOND_SECTION_ID = int.Parse(SECOND_SECTION_ID);

                var second_edit = db.SECTIONs.Where(x => x.SECTION_ID == tmp_SECOND_SECTION_ID).FirstOrDefault();
                if (second_edit != null)
                {
                    second_edit.SECTION_NUMBER = SECOND_SAVE_NUMBER;
                    second_edit.SECTION_DATE = SECOND_SAVE_DATE;
                    edit.SECTION_CLASSROOM = SECOND_SAVE_CLASSROOM;
                    second_edit.SECTION_TIME_START = SECOND_SAVE_TIMESTART;
                    second_edit.SECTION_TIME_END = SECOND_SAVE_TIMEEND;
                }
                Message = "1";
            }
            else
            {
                Message = "2";
            }
            db.SaveChanges();
            if (BR_NAME != null && SUBJECTid == null)
            {
                return Redirect("/TimeSchedule/DSchedule?BR_NAME=" + BR_NAME + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
            }
            else if (BR_Professor != null)
            {
                return Redirect("/TimeSchedule/PSchedule?BR_Professor=" + BR_Professor + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
            }
            else
            {
                if (SUBJECTid.Contains("_"))
                {
                    return Redirect("/TimeSchedule/ReportSchedule/" + SEC_ID + "?classroom=" + CLASSROOM + "&SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
                }
                else
                {
                    return Redirect("/TimeSchedule/TeSchedule/" + SEC_ID + "?classroom=" + CLASSROOM + "&SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
                }
            }
        }
        [HttpPost]
        public ActionResult updatedata(FormCollection collection)
        {
            var Message = "0";
            var SUBJECTid = collection["SUBJECTid"];
            var BR_NAME = collection["BR_NAME"];
            var Semester = collection["Semester"];
            var Year = collection["Year"];
            var BR_Professor = collection["BR_Professor"];
            var SearchId = collection["searchId"];
            var split = SearchId.Split(',');
            var SEC_ID = collection["SEC_ID"];
            var CLASSROOM = collection["CLASSROOM"];

            for (int i = 0; i < split.Length; i++)
            {
                var FIRST_SECTION_ID = int.Parse(collection["First_id_" + split[i]]);
                var SECOND_SECTION_ID = collection["Second_id_" + split[i]];

                if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID == null)
                {
                    var FIRST_SAVE_DATE = collection["First_date_" + split[i]];
                    var FIRST_SAVE_TIMESTART = double.Parse(collection["First_timestart_" + split[i]]);
                    var FIRST_SAVE_TIMEEND = double.Parse(collection["First_timeend_" + split[i]]);

                    var edit = db.SECTIONs.Where(x => x.SECTION_ID == FIRST_SECTION_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.SECTION_DATE = FIRST_SAVE_DATE;
                        edit.SECTION_TIME_START = FIRST_SAVE_TIMESTART;
                        edit.SECTION_TIME_END = FIRST_SAVE_TIMEEND;
                    }
                    Message = "1";
                }
                else if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID != null)
                {
                    var tmp_SECOND_SECTION_ID = int.Parse(SECOND_SECTION_ID);

                    var FIRST_SAVE_DATE = collection["First_date_" + split[i]];
                    var FIRST_SAVE_TIMESTART = double.Parse(collection["First_timestart_" + split[i]]);
                    var FIRST_SAVE_TIMEEND = double.Parse(collection["First_timeend_" + split[i]]);

                    var edit = db.SECTIONs.Where(x => x.SECTION_ID == FIRST_SECTION_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.SECTION_DATE = FIRST_SAVE_DATE;
                        edit.SECTION_TIME_START = FIRST_SAVE_TIMESTART;
                        edit.SECTION_TIME_END = FIRST_SAVE_TIMEEND;
                    }

                    var SECOND_SAVE_DATE = collection["Second_date_" + split[i]];
                    var SECOND_SAVE_TIMESTART = double.Parse(collection["Second_timestart_" + split[i]]);
                    var SECOND_SAVE_TIMEEND = double.Parse(collection["Second_timeend_" + split[i]]);

                    var second_edit = db.SECTIONs.Where(x => x.SECTION_ID == tmp_SECOND_SECTION_ID).FirstOrDefault();
                    if (second_edit != null)
                    {
                        second_edit.SECTION_DATE = SECOND_SAVE_DATE;
                        second_edit.SECTION_TIME_START = SECOND_SAVE_TIMESTART;
                        second_edit.SECTION_TIME_END = SECOND_SAVE_TIMEEND;
                    }
                    Message = "1";
                }
                else
                {
                    Message = "2";
                }
            }
            db.SaveChanges();
            if (BR_NAME != null && SUBJECTid == null)
            {
                return Redirect("/TimeSchedule/DSchedule?BR_NAME=" + BR_NAME + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
            }
            else if (BR_Professor != null)
            {
                return Redirect("/TimeSchedule/PSchedule?BR_Professor=" + BR_Professor + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
            }
            else
            {
                if (SUBJECTid.Contains("_"))
                {
                    return Redirect("/TimeSchedule/ReportSchedule/" + SEC_ID + "?classroom=" + CLASSROOM + "&SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
                }
                else
                {
                    return Redirect("/TimeSchedule/TeSchedule/" + SEC_ID + "?classroom=" + CLASSROOM + "&SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
                }
            }
        }
    }
}