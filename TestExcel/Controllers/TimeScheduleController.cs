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
                            YEAR = e1.YEAR
                        };
            section_subject = query.ToList();
            return section_subject;
        }
        public ActionResult DSchedule(string BR_NAME, string BR_Semester, string BR_Year,string Message)
        {
            string select_Department, select_branch;
            int select_Departmentid;
            int select_branchid;
            if (BR_NAME == null && BR_Semester == null && BR_Year == null)
            {
                BR_Semester = "1";
                BR_Year = "2560";
                select_branchid = 1;
                select_branch = db.BRANCHes.First().BRANCH_NAME;
                select_Department = db.DEPARTMENTs.First().DEPARTMENT_NAME;
                select_Departmentid = 1;
            }
            else
            {
                select_Department = db.BRANCHes.Where(x => x.BRANCH_NAME == BR_NAME).First().DEPARTMENT_NAME;
                select_Departmentid = db.DEPARTMENTs.Where(x => x.DEPARTMENT_NAME == select_Department).First().DEPARTMENT_ID;
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

            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", BR_Year);
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_ID", "DEPARTMENT_NAME");
            ViewBag.ddl_Branch = new SelectList(db.BRANCHes.Where(x => x.DEPARTMENT_NAME == select_Department).ToList(), "BRANCH_ID", "BRANCH_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult Dschedule(FormCollection collection)
        {
            int Branch_id = int.Parse(collection["DDL_BRANCH"]);
            int Depart_id = int.Parse(collection["DDL_DEPARTMENT"]);
            int count = int.Parse(collection["Count"]);
            string ddl_Year = collection["ddl_Year"];
            string ddl_Semester = collection["ddl_Semester"];
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
            var query = GetData(contain, ddl_Semester, ddl_Year);
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


            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", ddl_Year);
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
            ViewBag.Semester = "1";
            ViewBag.Year = "2560";
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

            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building_id.ToString()) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
            ViewBag.Semester = semester;
            ViewBag.Year = year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            var BUILDING = db.BUILDINGs.Where(x => x.BUILDING_NAME == Building_id.ToString()).ToList();
            var tupleData = new Tuple<IEnumerable<Building_Classroom>, IEnumerable<BUILDING>>(query, BUILDING);
            return View(tupleData);
        }
        public ActionResult TeSchedule(string id,string classroom,string SUBJECTid, string BR_Semester, string BR_Year, string Message,string color)
        {
            SUBJECT SUBJECT = new SUBJECT();
            IQueryable<Section_Subject> query;
            if (SUBJECTid == null && BR_Semester == null && BR_Year == null)
            {
                BR_Semester = "1";
                BR_Year = "2560";
                SUBJECT = db.SUBJECTs.First();
                color = "";
            }
            else
            {
                SUBJECT = db.SUBJECTs.Where(x => x.SUBJECT_ID == SUBJECTid).First();
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
            if (classroom != null)
            {
               query = from e1 in db.SECTIONs
                            join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                            where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SECTION_CLASSROOM.Contains(classroom) && e1.SEMESTER.Contains(BR_Semester) && e2.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year) && e2.YEAR.Contains(BR_Year)
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
                                YEAR = e1.YEAR
                            };
            }
            else
            {
               query = from e1 in db.SECTIONs
                            join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                            where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SEMESTER.Contains(BR_Semester) && e2.SEMESTER.Contains(BR_Semester) && e1.YEAR.Contains(BR_Year) && e2.YEAR.Contains(BR_Year)
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
                                YEAR = e1.YEAR
                            };
            }
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            if (id != "" && color != null)
            {
                ViewBag.color = "#ff0000";
                ViewBag.Number = id;
            }
            else
            {
                ViewBag.Number = "";
            }
            ViewBag.SUBJECT_NAME = SUBJECT.SUBJECT_NAME;
            ViewBag.SUBJECT = SUBJECT.SUBJECT_ID;
            ViewBag.Semester = BR_Semester;
            ViewBag.Year = BR_Year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            ViewBag.ddl_Subject = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_ID", SUBJECT.SUBJECT_ID);
            ViewBag.ddl_Subject_Name = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_NAME", SUBJECT.SUBJECT_ID);
            return View(query);
        }
        [HttpPost]
        public ActionResult TeSchedule(FormCollection collection)
        {
            var DDL_SUBJECT = collection["SUBJECT"];
            var ddl_Semester = collection["ddl_Semester"];
            var ddl_Year = collection["ddl_Year"];

            var SUBJECT = db.SUBJECTs.Where(x => x.SUBJECT_ID == DDL_SUBJECT).First();

            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SUBJECT_ID.Contains(SUBJECT.SUBJECT_ID) && e1.SEMESTER.Contains(ddl_Semester) && e2.SEMESTER.Contains(ddl_Semester) && e1.YEAR.Contains(ddl_Year) && e2.YEAR.Contains(ddl_Year)
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
                            YEAR = e1.YEAR
                        };
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            ViewBag.SUBJECT_NAME = SUBJECT.SUBJECT_NAME;
            ViewBag.SUBJECT = DDL_SUBJECT;
            ViewBag.Semester = ddl_Semester;
            ViewBag.Year = ddl_Year;
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR");
            ViewBag.ddl_Subject = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_ID", DDL_SUBJECT);
            ViewBag.ddl_Subject_Name = new SelectList(db.SUBJECTs.ToList(), "SUBJECT_ID", "SUBJECT_NAME", DDL_SUBJECT);
            return View(query);
        }
        [HttpPost]
        public ActionResult singleupdatedata(FormCollection collection)
        {
            var Message = "0";
            var FIRST_SECTION_ID = collection["FIRST_SECTION_ID"];
            var SECOND_SECTION_ID = collection["SECOND_SECTION_ID"];
            var SUBJECTid = collection["SUBJECTid"];
            var BR_NAME = collection["BR_NAME"];
            var Semester = collection["Semester"];
            var Year = collection["Year"];

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
            else
            {
                return Redirect("/TimeSchedule/TeSchedule?SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
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
            var SearchId = collection["searchId"];
            var split = SearchId.Split(',');
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
            else
            {
                return Redirect("/TimeSchedule/TeSchedule?SUBJECTid=" + SUBJECTid + "&BR_SEMESTER=" + Semester + "&BR_YEAR=" + Year + "&Message=" + Message);
            }
        }
    }
}