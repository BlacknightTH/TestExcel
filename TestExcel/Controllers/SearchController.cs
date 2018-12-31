using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Models;
using TestExcel.Utility;

namespace TestExcel.Controllers
{
    [adminauthen]
    public class SearchController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        public JsonResult GetSearch(string search)
        {
            var semesteryear = (from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                                select new SemesterYear
                                {
                                    SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                    SEMESTER = d1.SEMESTER,
                                    YEAR = d1.YEAR
                                }).OrderByDescending(x => x.YEAR).OrderByDescending(x => x.SEMESTER).ToList();
            var YEAR = semesteryear.First().YEAR;
            var SEMESTER = semesteryear.First().SEMESTER;
            var asearch = db.SUBJECTs.Where(x => x.SUBJECT_ID.Contains(search) && x.YEAR == YEAR && x.SEMESTER == SEMESTER).ToList();
            return new JsonResult { Data = asearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchProfessor(string search)
        {
            var asearch = db.PROFESSORs.Where(x => x.PROFESSOR_SHORTNAME.Contains(search)).ToList();
            return new JsonResult { Data = asearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchBranch(string search)
        {
            var asearch = db.BRANCHes.Where(x => x.COURSE_NAME.Contains(search)).ToList();
            return new JsonResult { Data = asearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetSearchDepartment(string search)
        {
            var asearch = db.DEPARTMENTs.Where(x => x.DEPARTMENT_NAME.Contains(search)).ToList();
            return new JsonResult { Data = asearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //public List<TimeCrash> SetNotification()
        //{
        //    return 
        //}
    }
}