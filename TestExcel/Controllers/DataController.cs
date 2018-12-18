using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using TestExcel.Utility;
using Newtonsoft.Json;
using TestExcel.Data;
using TestExcel.Report;
using TestExcel.Models;

namespace TestExcel.Controllers
{
    public class DataController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        string[] date = { "M", "T", "W", "H", "F", "S" };
        List<Section_Subject> _section_subject = new List<Section_Subject>();
        List<TimeCrash> _TimeCrash = new List<TimeCrash>();
        List<SemesterYear> SemesterYear = new List<SemesterYear>();
        // GET: Data
        #region Section
        public ActionResult Section()
        {
            var model = db.SECTIONs.ToList();
            SemesterYear = GetSemesterYear();
            ViewBag.ddl_Year = new SelectList(SemesterYear.OrderBy(x => x.YEAR), "YEAR", "YEAR", SemesterYear.OrderBy(x => x.YEAR).FirstOrDefault().YEAR);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveSection(FormCollection collection)
        {
            int SECTION_ID = int.Parse(collection["SECTION_ID"]);
            string SUBJECT_ID = collection["SUBJECT_ID"];
            string SECTION_NUMBER = collection["SECTION_NUMBER"];
            string SECTION_DATE = collection["SECTION_DATE"];
            double? SECTION_TIME_START = double.Parse(collection["SECTION_TIME_START"]);
            double? SECTION_TIME_END = double.Parse(collection["SECTION_TIME_END"]);
            string SECTION_PROFESSOR_SHORTNAME = collection["SECTION_PROFESSOR_SHORTNAME"];
            string SECTION_CLASSROOM = collection["SECTION_CLASSROOM"];
            string SECTION_BRANCH_NAME = collection["SECTION_BRANCH_NAME"];
            string SEMESTER = collection["SEMESTER"];
            string YEAR = collection["YEAR"];
            if (ModelState.IsValid && SECTION_DATE != "" && SUBJECT_ID != "" && SECTION_CLASSROOM != "" && SECTION_BRANCH_NAME != "")
            {
                if (SECTION_ID > 0)
                {
                    //Edit
                    var edit = db.SECTIONs.Where(x => x.SECTION_ID == SECTION_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.SECTION_ID = SECTION_ID;
                        edit.SUBJECT_ID = SUBJECT_ID;
                        edit.SECTION_NUMBER = SECTION_NUMBER;
                        edit.SECTION_DATE = SECTION_DATE;
                        edit.SECTION_TIME_START = SECTION_TIME_START;
                        edit.SECTION_TIME_END = SECTION_TIME_END;
                        edit.SECTION_PROFESSOR_SHORTNAME = SECTION_PROFESSOR_SHORTNAME;
                        edit.SECTION_CLASSROOM = SECTION_CLASSROOM;
                        edit.SECTION_BRANCH_NAME = SECTION_BRANCH_NAME;
                        edit.SEMESTER = SEMESTER;
                        edit.YEAR = YEAR;
                    }
                }
                else
                {
                    //Add
                    var item = new SECTION();
                    item.SECTION_ID = SECTION_ID;
                    item.SUBJECT_ID = SUBJECT_ID;
                    item.SECTION_NUMBER = SECTION_NUMBER;
                    item.SECTION_DATE = SECTION_DATE;
                    item.SECTION_TIME_START = SECTION_TIME_START;
                    item.SECTION_TIME_END = SECTION_TIME_END;
                    item.SECTION_PROFESSOR_SHORTNAME = SECTION_PROFESSOR_SHORTNAME;
                    item.SECTION_CLASSROOM = SECTION_CLASSROOM;
                    item.SECTION_BRANCH_NAME = SECTION_BRANCH_NAME;
                    item.SEMESTER = SEMESTER;
                    item.YEAR = YEAR;
                    db.SECTIONs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Section");
        }
        [HttpPost]
        public ActionResult DeleteSection(FormCollection collection)
        {
            int SECTION_ID = int.Parse(collection["Del_SECTION_ID"]);
            var del = db.SECTIONs.Where(x => x.SECTION_ID == SECTION_ID).FirstOrDefault();
            if (del != null)
            {
                db.SECTIONs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Section");
        }
        #endregion
        #region Subject
        public ActionResult Subject()
        {
            var model = db.SUBJECTs.ToList();
            SemesterYear = GetSemesterYear();
            ViewBag.ddl_Year = new SelectList(SemesterYear.OrderBy(x => x.YEAR), "YEAR", "YEAR", SemesterYear.OrderBy(x => x.YEAR).FirstOrDefault().YEAR);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveSubject(FormCollection collection)
        {
            int ID = int.Parse(collection["ID"]);
            string SUBJECT_ID = collection["SUBJECT_ID"];
            string SUBJECT_NAME = collection["SUBJECT_NAME"];
            string SUBJECT_CREDIT = collection["SUBJECT_CREDIT"];
            string SUBJECT_MIDTERM_TIME = collection["SUBJECT_MIDTERM_TIME"];
            string SUBJECT_MIDTERM_DATE = collection["SUBJECT_MIDTERM_DATE"];
            string SUBJECT_FINAL_TIME = collection["SUBJECT_FINAL_TIME"];
            string SUBJECT_FINAL_DATE = collection["SUBJECT_FINAL_DATE"];
            string SEMESTER = collection["SEMESTER"];
            string YEAR = collection["YEAR"];
            if (ModelState.IsValid && SUBJECT_NAME != "" && SUBJECT_ID != "" && SUBJECT_CREDIT != "")
            {
                if (ID > 0)
                {
                    //Edit
                    var edit = db.SUBJECTs.Where(x => x.ID == ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.SUBJECT_ID = SUBJECT_ID;
                        edit.SUBJECT_NAME = SUBJECT_NAME;
                        edit.SUBJECT_CREDIT = SUBJECT_CREDIT;
                        edit.SUBJECT_MIDTERM_TIME = SUBJECT_MIDTERM_TIME;
                        edit.SUBJECT_MIDTERM_DATE = SUBJECT_MIDTERM_DATE;
                        edit.SUBJECT_FINAL_TIME = SUBJECT_FINAL_TIME;
                        edit.SUBJECT_FINAL_DATE = SUBJECT_FINAL_DATE;
                        edit.SEMESTER = SEMESTER;
                        edit.YEAR = YEAR;
                    }
                }
                else
                {
                    //Add
                    var item = new SUBJECT();
                    item.SUBJECT_ID = SUBJECT_ID;
                    item.SUBJECT_NAME = SUBJECT_NAME;
                    item.SUBJECT_CREDIT = SUBJECT_CREDIT;
                    item.SUBJECT_MIDTERM_TIME = SUBJECT_MIDTERM_TIME;
                    item.SUBJECT_MIDTERM_DATE = SUBJECT_MIDTERM_DATE;
                    item.SUBJECT_FINAL_TIME = SUBJECT_FINAL_TIME;
                    item.SUBJECT_FINAL_DATE = SUBJECT_FINAL_DATE;
                    item.SEMESTER = SEMESTER;
                    item.YEAR = YEAR;
                    db.SUBJECTs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Subject");
        }
        [HttpPost]
        public ActionResult DeleteSubject(FormCollection collection)
        {
            int ID = int.Parse(collection["Del_ID"]);
            var del = db.SUBJECTs.Where(x => x.ID == ID).FirstOrDefault();
            if (del != null)
            {
                db.SUBJECTs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Subject");
        }
        #endregion
        //#region Member
        //public ActionResult Member()
        //{
        //    var model = db.USERs.ToList();
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult SaveMember(FormCollection collection)
        //{
        //    int ID = int.Parse(collection["ID"]);
        //    string USER_USERNAME = collection["USER_USERNAME"];
        //    string USER_PASSWORD = collection["USER_PASSWORD"];
        //    string USER_EMAIL = collection["USER_EMAIL"];
        //    string USER_FIRSTNAME = collection["USER_FIRSTNAME"];
        //    string USER_LASTNAME = collection["USER_LASTNAME"];
        //    string USER_STATUS = collection["USER_STATUS"];
        //    if (ModelState.IsValid && USER_USERNAME != "" && USER_PASSWORD != "")
        //    {
        //        if (ID > 0)
        //        {
        //            //Edit
        //            var edit = db.USERs.Where(x => x.ID == ID).FirstOrDefault();
        //            if (edit != null)
        //            {
        //                edit.USER_USERNAME = USER_USERNAME;
        //                edit.USER_PASSWORD = USER_PASSWORD;
        //                edit.USER_EMAIL = USER_EMAIL;
        //                edit.USER_FIRSTNAME = USER_FIRSTNAME;
        //                edit.USER_LASTNAME = USER_LASTNAME;
        //                edit.USER_STATUS = USER_STATUS;
        //            }
        //        }
        //        else
        //        {
        //            //Add
        //            var item = new USER();
        //            item.USER_USERNAME = USER_USERNAME;
        //            item.USER_PASSWORD = USER_PASSWORD;
        //            item.USER_EMAIL = USER_EMAIL;
        //            item.USER_FIRSTNAME = USER_FIRSTNAME;
        //            item.USER_LASTNAME = USER_LASTNAME;
        //            item.USER_STATUS = USER_STATUS;
        //            db.USERs.Add(item);
        //        }
        //        db.SaveChanges();

        //    }
        //    return RedirectToAction("Member");
        //}
        //[HttpPost]
        //public ActionResult DeleteMember(FormCollection collection)
        //{
        //    int ID = int.Parse(collection["Del_ID"]);
        //    var del = db.USERs.Where(x => x.ID == ID).FirstOrDefault();
        //    if (del != null)
        //    {
        //        db.USERs.Remove(del);
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //    return RedirectToAction("Member");
        //}
        //#endregion
        #region Professor
        public ActionResult Professor()
        {
            var model = db.PROFESSORs.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveProfessor(FormCollection collection)
        {
            int PROFESSOR_ID = int.Parse(collection["PROFESSOR_ID"]);
            string PROFESSOR_NAME = collection["PROFESSOR_NAME"];
            string PROFESSOR_SHORTNAME = collection["PROFESSOR_SHORTNAME"];
            string PROFESSOR_STATUS = collection["PROFESSOR_STATUS"];
            string DEPARTMENT_NAME = collection["DEPARTMENT_NAME"];
            if (ModelState.IsValid && PROFESSOR_SHORTNAME != "")
            {
                if (PROFESSOR_ID > 0)
                {
                    //Edit
                    var edit = db.PROFESSORs.Where(x => x.PROFESSOR_ID == PROFESSOR_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.PROFESSOR_NAME = PROFESSOR_NAME;
                        edit.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
                        edit.PROFESSOR_STATUS = PROFESSOR_STATUS;
                        edit.DEPARTMENT_NAME = DEPARTMENT_NAME;
                    }
                }
                else
                {
                    //Add
                    var item = new PROFESSOR();
                    item.PROFESSOR_NAME = PROFESSOR_NAME;
                    item.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
                    item.PROFESSOR_STATUS = PROFESSOR_STATUS;
                    item.DEPARTMENT_NAME = DEPARTMENT_NAME;
                    db.PROFESSORs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Professor");
        }
        [HttpPost]
        public ActionResult DeleteProfessor(FormCollection collection)
        {
            int PROFESSOR_ID = int.Parse(collection["Del_PROFESSOR_ID"]);
            var del = db.PROFESSORs.Where(x => x.PROFESSOR_ID == PROFESSOR_ID).FirstOrDefault();
            if (del != null)
            {
                db.PROFESSORs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Professor");
        }
        #endregion
        #region Course
        public ActionResult Course()
        {
            var model = db.COURSEs.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveCourse(FormCollection collection)
        {
            int COURSE_ID = int.Parse(collection["COURSE_ID"]);
            string COURSE_NAME = collection["COURSE_NAME"];
            string DEPARTMENT_NAME = collection["DEPARTMENT_NAME"];
            string COURSE_THAI_NAME = collection["COURSE_THAI_NAME"];
            if (ModelState.IsValid && COURSE_NAME != "")
            {
                if (COURSE_ID > 0)
                {
                    //Edit
                    var edit = db.COURSEs.Where(x => x.COURSE_ID == COURSE_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.DEPARTMENT_NAME_ID = int.Parse(DEPARTMENT_NAME);
                        edit.COURSE_NAME = COURSE_NAME;
                        edit.COURSE_THAI_NAME = COURSE_THAI_NAME;
                    }
                }
                else
                {
                    //Add
                    var item = new COURSE();
                    item.DEPARTMENT_NAME_ID = int.Parse(DEPARTMENT_NAME);
                    item.COURSE_NAME = COURSE_NAME;
                    item.COURSE_THAI_NAME = COURSE_THAI_NAME;
                    db.COURSEs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Course");
        }
        [HttpPost]
        public ActionResult DeleteCourse(FormCollection collection)
        {
            int COURSE_ID = int.Parse(collection["Del_COURSE_ID"]);
            var del = db.COURSEs.Where(x => x.COURSE_ID == COURSE_ID).FirstOrDefault();
            if (del != null)
            {
                db.COURSEs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Course");
        }
        #endregion
        #region Building
        public ActionResult Building()
        {
            var model = db.BUILDINGs.OrderBy(x => x.BUILDING_ID).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveBuilding(FormCollection collection)
        {
            int BUILDING_ID = int.Parse(collection["BUILDING_ID"]);
            string BUILDING_NAME = collection["BUILDING_NAME"];
            string CLASSROOM_NAME = collection["CLASSROOM_NAME"];
            if (ModelState.IsValid && CLASSROOM_NAME != "")
            {
                if(BUILDING_NAME == "")
                {
                    BUILDING_NAME = "0";
                }
                if (BUILDING_ID > 0)
                {
                    //Edit
                    var edit = db.BUILDINGs.Where(x => x.BUILDING_ID == BUILDING_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.BUILDING_NAME = BUILDING_NAME;
                        edit.CLASSROOM_NAME = CLASSROOM_NAME;
                    }
                }
                else
                {
                    //Add
                    var item = new BUILDING();
                    item.BUILDING_NAME = BUILDING_NAME;
                    item.CLASSROOM_NAME = CLASSROOM_NAME;
                    db.BUILDINGs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Building");
        }
        [HttpPost]
        public ActionResult DeleteBuilding(FormCollection collection)
        {
            int BUILDING_ID = int.Parse(collection["Del_BUILDING_ID"]);
            var del = db.BUILDINGs.Where(x => x.BUILDING_ID == BUILDING_ID).FirstOrDefault();
            if (del != null)
            {
                db.BUILDINGs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Building");
        }
        #endregion
        #region Branch
        public ActionResult Branch()
        {
            var model = db.BRANCHes.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveBranch(FormCollection collection)
        {
            int BranchId = int.Parse(collection["BRANCH_ID"]);
            string BranchName = collection["BRANCH_NAME"];
            string CourseName = collection["DEPARTMENT_NAME"];
            if (ModelState.IsValid && BranchName != "" && CourseName != "")
            {
                if(BranchId > 0)
                {
                    //Edit
                    var edit = db.BRANCHes.Where(x => x.BRANCH_ID == BranchId).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.BRANCH_NAME = BranchName;
                        edit.COURSE_NAME = CourseName;
                    }
                }
                else
                {
                    //Add
                    var item = new BRANCH();
                    item.BRANCH_NAME = BranchName;
                    item.COURSE_NAME = CourseName;
                    db.BRANCHes.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Branch");
        }
        [HttpPost]
        public ActionResult DeleteBranch(FormCollection collection)
        {
            int BranchId = int.Parse(collection["Del_BRANCH_ID"]);
            var del = db.BRANCHes.Where(x => x.BRANCH_ID == BranchId).FirstOrDefault();
            if (del != null)
            {
                db.BRANCHes.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Branch");
        }
        #endregion
        public JsonResult GetNotifications()
        {
            var list = SetNotification();
            return new JsonResult {Data = list.ToList() ,JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }
        [HttpPost]
        public JsonResult Warning(string data, string semester, string year)
        {
            var list = GetWarning(data,semester,year);
            return new JsonResult { Data = list.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public List<TimeCrash> GetWarning(string data,string semester,string year)
        {
            var section = db.SECTIONs;
            List<Section_Subject> _Section_Subject = new List<Section_Subject>();
            var split = data.Split(',');
            for (int i = 0; i < split.Length - 1; i++)
            {
                int sp = int.Parse(split[i]);
                var have = db.SECTIONs.Where(x => x.SECTION_ID == sp).First();
                var query = (from e1 in db.SECTIONs
                            join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                            where e1.SECTION_ID != have.SECTION_ID && e1.SECTION_CLASSROOM == have.SECTION_CLASSROOM && e1.SECTION_DATE == have.SECTION_DATE && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                            }).OrderBy(x => x.SECTION_TIME_START).ToList();

                    var model = query.Where(x => (x.SECTION_TIME_START <= have.SECTION_TIME_START && x.SECTION_TIME_START < have.SECTION_TIME_END && x.SECTION_TIME_END > have.SECTION_TIME_START) && x.SUBJECT_ID != have.SUBJECT_ID && x.SECTION_NUMBER != "");
                if (model.Count() > 0)
                {
                    foreach (var im in model)
                    {
                        var e = section.Where(x => x.SECTION_ID == im.SECTION_ID).First();
                        e.CRASH = "3";
                        var item = new TimeCrash();
                        item.SECTION_ID_First = im.SECTION_ID;
                        item.SUBJECT_ID_First = im.SUBJECT_ID;
                        item.SUBJECT_NAME_First = im.SUBJECT_NAME;
                        item.SECTION_NUMBER_First = im.SECTION_NUMBER;
                        item.SECTION_DATE_First = im.SECTION_DATE;
                        item.SECTION_TIME_START_First = im.SECTION_TIME_START;
                        item.SECTION_TIME_END_First = im.SECTION_TIME_END;
                        item.SECTION_CLASSROOM_First = im.SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_First = im.SECTION_BRANCH_NAME;
                        item.SEMESTER = semester;
                        item.YEAR = year;
                        _TimeCrash.Add(item);
                    }
                }
            }
            var TimeCrash = _TimeCrash.ToList();
            return TimeCrash;
        }
        public List<TimeCrash> SetNotification()
        {
            var section = db.SECTIONs;
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            foreach (var i in semesteryear.ToList())
            {
                   _section_subject = GetModel(i.SEMESTER, i.YEAR);
                //foreach (var k in _section_subject)
                //{
                //    var model = db.SECTIONs.Where(x => x.SECTION_ID == k.SECTION_ID).First();
                //    if (k.SUBJECT_CREDIT.Contains("-0-"))
                //    {
                //        model.CRASH = "1";
                //    }
                //    else
                //    {
                //        model.CRASH = "2";
                //    }
                //}
                foreach (var j in db.SECTIONs.Where(x => x.SEMESTER == i.SEMESTER && x.YEAR == i.YEAR).OrderBy(x => x.SECTION_TIME_START))
                {
                    //T = _section_subject.Where(x => x.SECTION_ID == j.SECTION_ID).First();
                    //if (T.SUBJECT_CREDIT.Contains("-0-") && j.CRASH == "3")
                    //{
                    //    j.CRASH = "1";
                    //}
                    //else if(!T.SUBJECT_CREDIT.Contains("-0-") && j.CRASH == "3")
                    //{
                    //    j.CRASH = "2";
                    //}
                    var WhereTimeDate = _section_subject.Where(x => (x.SECTION_TIME_START <= j.SECTION_TIME_START && x.SECTION_TIME_START < j.SECTION_TIME_END && x.SECTION_TIME_END > j.SECTION_TIME_START) && x.SECTION_DATE == j.SECTION_DATE && x.SECTION_ID != j.SECTION_ID && x.SECTION_BRANCH_NAME != j.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == j.SECTION_CLASSROOM && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม") && x.SECTION_NUMBER != "").OrderBy(x => x.SECTION_TIME_START).ToList();
                    if (WhereTimeDate.Count() > 0)
                    {
                        foreach (var im in WhereTimeDate)
                        {
                            var e = section.Where(x => x.SECTION_ID == im.SECTION_ID).First();
                                e.CRASH = "3";
                            var item = new TimeCrash();
                            item.SECTION_ID_First = im.SECTION_ID;
                            item.SUBJECT_ID_First = im.SUBJECT_ID;
                            item.SUBJECT_NAME_First = im.SUBJECT_NAME;
                            item.SECTION_NUMBER_First = im.SECTION_NUMBER;
                            item.SECTION_DATE_First = im.SECTION_DATE;
                            item.SECTION_TIME_START_First = im.SECTION_TIME_START;
                            item.SECTION_TIME_END_First = im.SECTION_TIME_END;
                            item.SECTION_CLASSROOM_First = im.SECTION_CLASSROOM;
                            item.SECTION_BRANCH_NAME_First = im.SECTION_BRANCH_NAME;
                            item.SEMESTER = i.SEMESTER;
                            item.YEAR = i.YEAR;
                            item.SUBJECT_ID_Last = WhereTimeDate.LastOrDefault().SUBJECT_ID;
                            _TimeCrash.Add(item);
                        }
                    }
                    //foreach(var im in WhereTimeDate)
                    //{
                    //    var e = section.Where(x => x.SECTION_ID == im.SECTION_ID).First();
                    //    e.CRASH = "3";
                    //}
                    //if (WhereTimeDate.Count() > 1)
                    //{
                    //    var Firsttimestart = WhereTimeDate.FirstOrDefault().SECTION_TIME_START;
                    //    var Firsttimeend = WhereTimeDate.FirstOrDefault().SECTION_TIME_END;
                    //    var Lasttimestart = WhereTimeDate.LastOrDefault().SECTION_TIME_START;
                    //    var Lasttimeend = WhereTimeDate.LastOrDefault().SECTION_TIME_END;

                    //    //if (Firsttimestart <= Lasttimestart && Firsttimestart < Lasttimeend && Firsttimeend > Lasttimestart)
                    //    if (Lasttimestart < Firsttimeend || Firsttimestart == Lasttimestart)
                    //    {
                    //        var item = new TimeCrash();
                    //        item.SECTION_ID_First = WhereTimeDate.FirstOrDefault().SECTION_ID;
                    //        item.SUBJECT_ID_First = WhereTimeDate.FirstOrDefault().SUBJECT_ID;
                    //        item.SUBJECT_NAME_First = WhereTimeDate.FirstOrDefault().SUBJECT_NAME;
                    //        item.SECTION_NUMBER_First = WhereTimeDate.FirstOrDefault().SECTION_NUMBER;
                    //        item.SECTION_DATE_First = WhereTimeDate.FirstOrDefault().SECTION_DATE;
                    //        item.SECTION_TIME_START_First = WhereTimeDate.FirstOrDefault().SECTION_TIME_START;
                    //        item.SECTION_TIME_END_First = WhereTimeDate.FirstOrDefault().SECTION_TIME_END;
                    //        item.SECTION_CLASSROOM_First = WhereTimeDate.FirstOrDefault().SECTION_CLASSROOM;
                    //        item.SECTION_BRANCH_NAME_First = WhereTimeDate.FirstOrDefault().SECTION_BRANCH_NAME;

                    //        item.SECTION_ID_Last = WhereTimeDate.LastOrDefault().SECTION_ID;
                    //        item.SUBJECT_ID_Last = WhereTimeDate.LastOrDefault().SUBJECT_ID;
                    //        item.SUBJECT_NAME_Last = WhereTimeDate.LastOrDefault().SUBJECT_NAME;
                    //        item.SECTION_NUMBER_Last = WhereTimeDate.LastOrDefault().SECTION_NUMBER;
                    //        item.SECTION_DATE_Last = WhereTimeDate.LastOrDefault().SECTION_DATE;
                    //        item.SECTION_TIME_START_Last = WhereTimeDate.LastOrDefault().SECTION_TIME_START;
                    //        item.SECTION_TIME_END_Last = WhereTimeDate.LastOrDefault().SECTION_TIME_END;
                    //        item.SECTION_CLASSROOM_Last = WhereTimeDate.LastOrDefault().SECTION_CLASSROOM;
                    //        item.SECTION_BRANCH_NAME_Last = WhereTimeDate.LastOrDefault().SECTION_BRANCH_NAME;
                    //        item.SEMESTER = i.SEMESTER;
                    //        item.YEAR = i.YEAR;
                    //        _TimeCrash.Add(item);
                    //    }
                    //}
                }
            }
            db.SaveChanges();
            var TimeCrash = _TimeCrash.OrderByDescending(x => x.YEAR).OrderByDescending(y => y.SEMESTER).ToList();
            return TimeCrash;
        }
        public List<SemesterYear> GetSemesterYear()
        {
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            return semesteryear.ToList();
        }
        public List<Section_Subject> GetModel(string semester, string year)
        {
            List<Section_Subject> section_subject = new List<Section_Subject>();
            var Section = db.SECTIONs;
            SECTION a = new SECTION();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where  e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
            foreach (var i in query)
            {
                if (i.SUBJECT_CREDIT.Contains("-0-") && (i.CRASH == "3" || i.CRASH == null))
                {
                    a = Section.Where(x => x.SECTION_ID == i.SECTION_ID).First();
                    a.CRASH = "1";
                }
                else if (!i.SUBJECT_CREDIT.Contains("-0-") && (i.CRASH == "3" || i.CRASH == null))
                {
                    a = Section.Where(x => x.SECTION_ID == i.SECTION_ID).First();
                    a.CRASH = "2";
                }
            }
            section_subject = query.OrderBy(x => x.SECTION_ID).ToList();
            return section_subject;
        }
    }
}