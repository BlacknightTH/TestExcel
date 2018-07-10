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
        // GET: TimeSchedule
        public ActionResult Index()
        {
            TestExcelEntities db = new TestExcelEntities();

            //var model = db.SUBJECTs.SqlQuery("Select DISTINCT SUBJECT.SUBJECT_ID, * from SUBJECT inner join SECTION on SUBJECT.SUBJECT_ID = SECTION.SUBJECT_ID where SECTION.SECTION_FACULTY like '%EnET(I)-R21%'").ToList();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_FACULTY.Contains("EnET(I)-R21")
                        select new Section_Subject
                        {
                            ID = e1.ID,
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_FACULTY = e1.SECTION_FACULTY
                        };
            Section_Subject d = new Section_Subject();
            var e = d.SUBJECT_CREDIT.Split('(', ')');

            query = query.Where(x => x.SECTION_NUMBER != "");
            return View(query);
        }
    }
}