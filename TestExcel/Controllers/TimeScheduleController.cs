using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Models;
using System.Text;

namespace TestExcel.Controllers
{
    public class TimeScheduleController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();

        // GET: TimeSchedule
        public ActionResult Index()
        {
            var Faculty_Name = db.FACULTies.Select(x => x.FACULTY_NAME).First();
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_FACULTY.Contains(Faculty_Name)
                        select new Section_Subject
                        {
                            ID = e1.ID,
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_FACULTY = e1.SECTION_FACULTY,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_TEACHER = e1.SECTION_TEACHER,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END
                        };
            ViewBag.FacultyName = Faculty_Name;
            ViewBag.DDLSelected = 1;
            var rr = query.Where(x => x.SECTION_TIME_START <= 15.00 && x.SECTION_DATE == "M").Any();
            ViewBag.ddl_Faculty = new SelectList(db.FACULTies.ToList(), "ID", "FACULTY_NAME");
            return View(query);
        }
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            int Faculty_id = int.Parse(collection["DDL_FACULTY"]);
            var Faculty_Name = db.FACULTies.Where(x => x.ID == Faculty_id).First().FACULTY_NAME;
            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_FACULTY.Contains(Faculty_Name)
                        select new Section_Subject
                        {
                            ID = e1.ID,
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_FACULTY = e1.SECTION_FACULTY,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_TEACHER = e1.SECTION_TEACHER,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                            SECTION_PERIOD = e1.SECTION_TIME_END - e1.SECTION_TIME_START
                        };
            ViewBag.FacultyName = Faculty_Name;
            ViewBag.ddl_Faculty = new SelectList(db.FACULTies.ToList(), "ID", "FACULTY_NAME");
            ViewBag.DDLSelected = Faculty_id;
                //query = query.Where(x => x.SECTION_NUMBER != "");
                return View(query);
        }
    }
}