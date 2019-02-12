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
using System.IO;
using OfficeOpenXml;

namespace TestExcel.Controllers
{
    [adminauthen]
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
            try
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
                        var edit = db.SECTIONs.Find(SECTION_ID);
                        //var edit = db.SECTIONs.Where(x => x.SECTION_ID == SECTION_ID).FirstOrDefault();
                        if (edit != null)
                        {
                            LogFile("อัปเดตจาก " + edit.SUBJECT_ID + " " + edit.SECTION_NUMBER + " " + edit.SECTION_DATE + " "
                                    + edit.SECTION_TIME_START + "-" + edit.SECTION_TIME_END + " " + edit.SECTION_PROFESSOR_SHORTNAME + " "
                                    + edit.SECTION_CLASSROOM + " " + edit.SECTION_BRANCH_NAME + " " + edit.SEMESTER + " " + edit.YEAR + " เป็น "
                                    + SUBJECT_ID + " " + SECTION_NUMBER + " " + SECTION_DATE + " " + SECTION_TIME_START + "-"
                                    + SECTION_TIME_END + " " + SECTION_PROFESSOR_SHORTNAME + " " + SECTION_CLASSROOM + " " + SECTION_BRANCH_NAME + " "
                                    + SEMESTER + " " + YEAR);
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
                        LogFile("เพิ่มข้อมูล " + SUBJECT_ID + " " + SECTION_NUMBER + " " + SECTION_DATE + " "
                                    + SECTION_TIME_START + "-" + SECTION_TIME_END + " " + SECTION_PROFESSOR_SHORTNAME + " "
                                    + SECTION_CLASSROOM + " " + SECTION_BRANCH_NAME + " " + SEMESTER + " " + YEAR);
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
                        LogFile("เพิ่มข้อมูลในฐานข้อมูล SECTION " + SECTION_ID + " " + SUBJECT_ID + " " + SECTION_NUMBER + " "
                            + SECTION_DATE + " " + SECTION_TIME_START + "-" + SECTION_TIME_END + " " + SECTION_PROFESSOR_SHORTNAME
                            + " " + SECTION_CLASSROOM + " " + SECTION_BRANCH_NAME + " " + SEMESTER + " " + YEAR);
                    }
                    db.SaveChanges();

                }
                Export(SEMESTER ,YEAR);
                return RedirectToAction("Section");
            }
            catch
            {
                return RedirectToAction("Section");
            }

        }
        [HttpPost]
        public ActionResult DeleteSection(FormCollection collection)
        {
            int SECTION_ID = int.Parse(collection["Del_SECTION_ID"]);
            var del = db.SECTIONs.Where(x => x.SECTION_ID == SECTION_ID).FirstOrDefault();
            string SEMESTER = del.SEMESTER;
            string YEAR = del.YEAR;
            if (del != null)
            {
                LogFile("ลบข้อมูล " + del.SUBJECT_ID + " " + del.SECTION_NUMBER + " " + del.SECTION_DATE + " "
                                    + del.SECTION_TIME_START + "-" + del.SECTION_TIME_END + " " + del.SECTION_PROFESSOR_SHORTNAME + " "
                                    + del.SECTION_CLASSROOM + " " + del.SECTION_BRANCH_NAME + " " + del.SEMESTER + " " + del.YEAR);
                db.SECTIONs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            Export(SEMESTER, YEAR);
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
            try
            {
                if (ModelState.IsValid && SUBJECT_NAME != "" && SUBJECT_ID != "" && SUBJECT_CREDIT != "")
                {
                    if (ID > 0)
                    {
                        //Edit
                        var edit = db.SUBJECTs.Find(ID);
                        if (edit != null)
                        {
                            LogFile("อัปเดตจาก " + edit.SUBJECT_ID + " " + edit.SUBJECT_NAME + " " + edit.SUBJECT_CREDIT + " "
                                        + edit.SUBJECT_MIDTERM_TIME + " " + edit.SUBJECT_MIDTERM_DATE + " " + edit.SUBJECT_FINAL_TIME + " "
                                        + edit.SUBJECT_FINAL_DATE + " " + edit.SEMESTER + " " + edit.YEAR + " เป็น "
                                        + SUBJECT_ID + " " + SUBJECT_NAME + " " + SUBJECT_CREDIT + " " + SUBJECT_MIDTERM_TIME + " "
                                        + SUBJECT_MIDTERM_DATE + " " + SUBJECT_FINAL_TIME + " " + SUBJECT_FINAL_DATE + " "
                                        + SEMESTER + " " + YEAR);
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
                        LogFile("เพิ่มข้อมูล " + SUBJECT_ID + " " + SUBJECT_NAME + " " + SUBJECT_CREDIT + " " + SUBJECT_MIDTERM_TIME + " "
                                            + SUBJECT_MIDTERM_DATE + " " + SUBJECT_FINAL_TIME + " " + SUBJECT_FINAL_DATE + " "
                                            + SEMESTER + " " + YEAR);
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
                Export(SEMESTER, YEAR);
                return RedirectToAction("Subject");
            }
            catch
            {
                return RedirectToAction("Subject");
            }
        }
        [HttpPost]
        public ActionResult DeleteSubject(FormCollection collection)
        {
            int ID = int.Parse(collection["Del_ID"]);
            var del = db.SUBJECTs.Where(x => x.ID == ID).FirstOrDefault();
            string SEMESTER = del.SEMESTER;
            string YEAR = del.YEAR;
            if (del != null)
            {
                LogFile("ลบข้อมูล " + del.SUBJECT_ID + " " + del.SUBJECT_NAME + " " + del.SUBJECT_CREDIT + " " + del.SUBJECT_MIDTERM_TIME + " "
                                            + del.SUBJECT_MIDTERM_DATE + " " + del.SUBJECT_FINAL_TIME + " " + del.SUBJECT_FINAL_DATE + " "
                                            + del.SEMESTER + " " + del.YEAR);
                db.SUBJECTs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            Export(SEMESTER, YEAR);
            return RedirectToAction("Subject");
        }
        #endregion
        #region Member
        public ActionResult Member()
        {
            if (Session["status"].ToString() == "admin")
            {
                var model = db.USERs.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Section");
            }
        }
        [HttpPost]
        public ActionResult SaveMember(FormCollection collection)
        {
            int ID = int.Parse(collection["ID"]);
            string USER_USERNAME = collection["USER_USERNAME"];
            string USER_PASSWORD = collection["USER_PASSWORD"];
            string USER_EMAIL = collection["USER_EMAIL"];
            string USER_FIRSTNAME = collection["USER_FIRSTNAME"];
            string USER_LASTNAME = collection["USER_LASTNAME"];
            string USER_STATUS = collection["USER_STATUS"];
            if (ModelState.IsValid && USER_USERNAME != "" && USER_PASSWORD != "")
            {
                if (ID > 0)
                {
                    //Edit
                    var edit = db.USERs.Where(x => x.ID == ID).FirstOrDefault();
                    if (edit != null)
                    {
                        LogFile("อัปเดตจาก " + edit.USER_USERNAME + " " + edit.USER_PASSWORD + " " + edit.USER_EMAIL + " "
                                        + edit.USER_FIRSTNAME + " " + edit.USER_LASTNAME + " " + edit.USER_STATUS + " เป็น "
                                        + USER_USERNAME + " " + USER_PASSWORD + " " + USER_EMAIL + " " + USER_FIRSTNAME + " "
                                        + USER_LASTNAME + " " + USER_STATUS);
                        edit.USER_USERNAME = USER_USERNAME;
                        edit.USER_PASSWORD = USER_PASSWORD;
                        edit.USER_EMAIL = USER_EMAIL;
                        edit.USER_FIRSTNAME = USER_FIRSTNAME;
                        edit.USER_LASTNAME = USER_LASTNAME;
                        edit.USER_STATUS = USER_STATUS;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + USER_USERNAME + " " + USER_PASSWORD + " " + USER_EMAIL + " " + USER_FIRSTNAME + " "
                                        + USER_LASTNAME + " " + USER_STATUS);
                    var item = new USER();
                    item.USER_USERNAME = USER_USERNAME;
                    item.USER_PASSWORD = USER_PASSWORD;
                    item.USER_EMAIL = USER_EMAIL;
                    item.USER_FIRSTNAME = USER_FIRSTNAME;
                    item.USER_LASTNAME = USER_LASTNAME;
                    item.USER_STATUS = USER_STATUS;
                    db.USERs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Member");
        }
        [HttpPost]
        public ActionResult DeleteMember(FormCollection collection)
        {
            int ID = int.Parse(collection["Del_ID"]);
            var del = db.USERs.Where(x => x.ID == ID).FirstOrDefault();
            if (del != null)
            {
                LogFile("ลบข้อมูล " + del.USER_USERNAME + " " + del.USER_PASSWORD + " " + del.USER_EMAIL + " " + del.USER_FIRSTNAME + " "
                                           + del.USER_LASTNAME + " " + del.USER_STATUS);
                db.USERs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Member");
        }
        #endregion
        #region Department
        public ActionResult Department()
        {
            var model = db.DEPARTMENTs.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveDepartment(FormCollection collection)
        {
            int DEPARTMENT_ID = int.Parse(collection["DEPARTMENT_ID"]);
            string DEPARTMENT = collection["DEPARTMENT"];
            if (ModelState.IsValid && DEPARTMENT != "")
            {
                if (DEPARTMENT_ID > 0)
                {
                    //Edit
                    var edit = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == DEPARTMENT_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        LogFile("อัปเดตจาก " + edit.DEPARTMENT_NAME + " เป็น " + DEPARTMENT);
                        edit.DEPARTMENT_NAME = DEPARTMENT;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + DEPARTMENT);
                    var item = new DEPARTMENT();
                    item.DEPARTMENT_NAME = DEPARTMENT;
                    db.DEPARTMENTs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult DeleteDepartment(FormCollection collection)
        {
            int ID = int.Parse(collection["Del_ID"]);
            var del = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == ID).FirstOrDefault();
            if (del != null)
            {
                LogFile("ลบข้อมูล " + del.DEPARTMENT_NAME);
                db.DEPARTMENTs.Remove(del);
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Department");
        }
        #endregion
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
                        LogFile("อัปเดตจาก " + edit.PROFESSOR_NAME + " " + edit.PROFESSOR_SHORTNAME + " " + edit.PROFESSOR_STATUS + " "
                                           + edit.DEPARTMENT_NAME + " เป็น "
                                           + PROFESSOR_NAME + " " + PROFESSOR_SHORTNAME + " " + PROFESSOR_STATUS + " " + DEPARTMENT_NAME);
                        edit.PROFESSOR_NAME = PROFESSOR_NAME;
                        edit.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
                        edit.PROFESSOR_STATUS = PROFESSOR_STATUS;
                        edit.DEPARTMENT_NAME = DEPARTMENT_NAME;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + PROFESSOR_NAME + " " + PROFESSOR_SHORTNAME + " " + PROFESSOR_STATUS + " " + DEPARTMENT_NAME);
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
                LogFile("ลบข้อมูล " + del.PROFESSOR_NAME + " " + del.PROFESSOR_SHORTNAME + " " + del.PROFESSOR_STATUS + " " + del.DEPARTMENT_NAME);
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
            string DEPARTMENT_NAME_ID = collection["DEPARTMENT_NAME_ID"];
            string COURSE_THAI_NAME = collection["COURSE_THAI_NAME"];
            if (ModelState.IsValid && COURSE_NAME != "")
            {
                if (COURSE_ID > 0)
                {
                    //Edit
                    var edit = db.COURSEs.Where(x => x.COURSE_ID == COURSE_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        LogFile("อัปเดตจาก " + edit.DEPARTMENT_NAME_ID + " " + edit.COURSE_NAME + " " + edit.COURSE_THAI_NAME + " เป็น "
                                              + DEPARTMENT_NAME_ID + " " + COURSE_NAME + " " + COURSE_THAI_NAME);
                        edit.DEPARTMENT_NAME_ID = int.Parse(DEPARTMENT_NAME_ID);
                        edit.COURSE_NAME = COURSE_NAME;
                        edit.COURSE_THAI_NAME = COURSE_THAI_NAME;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + DEPARTMENT_NAME_ID + " " + COURSE_NAME + " " + COURSE_THAI_NAME);
                    var item = new COURSE();
                    item.DEPARTMENT_NAME_ID = int.Parse(DEPARTMENT_NAME_ID);
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
                LogFile("ลบข้อมูล " + del.DEPARTMENT_NAME_ID + " " + del.COURSE_NAME + " " + del.COURSE_THAI_NAME);
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
                if (BUILDING_NAME == "")
                {
                    BUILDING_NAME = "0";
                }
                if (BUILDING_ID > 0)
                {
                    //Edit
                    var edit = db.BUILDINGs.Where(x => x.BUILDING_ID == BUILDING_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        LogFile("อัปเดตจาก " + edit.BUILDING_NAME + " " + edit.CLASSROOM_NAME + " เป็น "
                                                 + BUILDING_NAME + " " + CLASSROOM_NAME);
                        edit.BUILDING_NAME = BUILDING_NAME;
                        edit.CLASSROOM_NAME = CLASSROOM_NAME;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + BUILDING_NAME + " " + CLASSROOM_NAME);
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
                LogFile("ลบข้อมูล " + del.BUILDING_NAME + " " + del.CLASSROOM_NAME);
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
            string CourseName = collection["COURSE_NAME"];
            if (ModelState.IsValid && BranchName != "" && CourseName != "")
            {
                if (BranchId > 0)
                {
                    //Edit
                    var edit = db.BRANCHes.Where(x => x.BRANCH_ID == BranchId).FirstOrDefault();
                    if (edit != null)
                    {
                        LogFile("อัปเดตจาก " + edit.BRANCH_NAME + " " + edit.COURSE_NAME + " เป็น "
                                                 + BranchName + " " + CourseName);
                        edit.BRANCH_NAME = BranchName;
                        edit.COURSE_NAME = CourseName;
                    }
                }
                else
                {
                    //Add
                    LogFile("เพิ่มข้อมูล " + BranchName + " " + CourseName);
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
                LogFile("ลบข้อมูล " + del.BRANCH_NAME + " " + del.COURSE_NAME);
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
        #region Notification
        public JsonResult GetNotifications()
        {
            var list = SetNotification();
            return new JsonResult { Data = list.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult Warning(FormCollection collection)
        {
            var list = GetWarning(collection);
            //var list = GetWarning(data, semester, year);
            return new JsonResult { Data = list.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult Warning_single(FormCollection collection)
        {
            var list = GetWarning_single(collection);
            //var list = GetWarning(data, semester, year);
            return new JsonResult { Data = list.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public List<TimeCrash> GetWarning(FormCollection collection)
        {
            var semester = collection["Semester"];
            var year = collection["Year"];
            var SUBJECTid = collection["SUBJECTid"];
            var SearchId = collection["searchId"];
            var section = db.SECTIONs;
            List<Section_Subject> _Section_Subject = new List<Section_Subject>();
            var split = SearchId.Split(',');
            for (int i = 0; i < split.Length; i++)
            {
                var FIRST_SECTION_ID = int.Parse(collection["First_id_" + split[i]]);
                var SECOND_SECTION_ID = collection["Second_id_" + split[i]];

                if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID == null)
                {
                    var FirstTime = double.Parse(collection["First_timestart_" + split[i]]);
                    var LastTime = double.Parse(collection["First_timeend_" + split[i]]);
                    var Classroom = collection["First_classroom_" + split[i]];
                    var Date = collection["First_date_" + split[i]];
                    var Subject_id = collection["First_subjectid_" + split[i]];
                    var First_SUBJECT_NAME = collection["First_name_" + split[i]];
                    var First_SECTION_NUMBER = collection["First_number_" + split[i]];
                    var First_BRANCH_NAME = collection["First_branch_" + split[i]];
                    var unit = (from e1 in db.SECTIONs
                                join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                                where e1.SECTION_ID == FIRST_SECTION_ID
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
                                }).OrderBy(x => x.SECTION_TIME_START).First();
                    var query = (from e1 in db.SECTIONs
                                 join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                                 where e1.SECTION_ID != FIRST_SECTION_ID && e1.SECTION_CLASSROOM == Classroom && e1.SECTION_DATE == Date && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                    var WhereTimeDate = query.Where(x => (x.SECTION_TIME_START <= FirstTime && x.SECTION_TIME_START < LastTime && x.SECTION_TIME_END > FirstTime) && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม")).OrderBy(x => x.SECTION_DATE).ToList();
                    if (WhereTimeDate.Count() > 0)
                    {
                        var item = new TimeCrash();
                        foreach (var im in WhereTimeDate)
                        {
                            if (im.SECTION_PROFESSOR_SHORTNAME.Contains(unit.SECTION_PROFESSOR_SHORTNAME))
                            {
                                item.TEACHER_CRASH = "1";
                            }
                            else
                            {
                                item.TEACHER_CRASH = "0";
                            }
                        }
                        item.SUBJECT_ID_First = Subject_id;
                        item.SUBJECT_NAME_First = First_SUBJECT_NAME;
                        item.SECTION_NUMBER_First = First_SECTION_NUMBER;
                        item.SECTION_DATE_First = Date;
                        item.SECTION_TIME_START_First = FirstTime;
                        item.SECTION_TIME_END_First = LastTime;
                        item.SECTION_CLASSROOM_First = Classroom;
                        item.SECTION_BRANCH_NAME_First = First_BRANCH_NAME;
                        item.SECTION_PROFESSOR_First = unit.SECTION_PROFESSOR_SHORTNAME;

                        item.SECTION_ID_Second = WhereTimeDate[0].SECTION_ID;
                        item.SUBJECT_ID_Second = WhereTimeDate[0].SUBJECT_ID;
                        item.SUBJECT_NAME_Second = WhereTimeDate[0].SUBJECT_NAME;
                        item.SECTION_NUMBER_Second = WhereTimeDate[0].SECTION_NUMBER;
                        item.SECTION_DATE_Second = WhereTimeDate[0].SECTION_DATE;
                        item.SECTION_TIME_START_Second = WhereTimeDate[0].SECTION_TIME_START;
                        item.SECTION_TIME_END_Second = WhereTimeDate[0].SECTION_TIME_END;
                        item.SECTION_CLASSROOM_Second = WhereTimeDate[0].SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_Second = WhereTimeDate[0].SECTION_BRANCH_NAME;
                        item.SECTION_PROFESSOR_Second = WhereTimeDate[0].SECTION_PROFESSOR_SHORTNAME;
                        item.TIME_CRASH = "2";
                        if (WhereTimeDate.Count() == 2)
                        {
                            item.SECTION_ID_Third = WhereTimeDate[1].SECTION_ID;
                            item.SUBJECT_ID_Third = WhereTimeDate[1].SUBJECT_ID;
                            item.SUBJECT_NAME_Third = WhereTimeDate[1].SUBJECT_NAME;
                            item.SECTION_NUMBER_Third = WhereTimeDate[1].SECTION_NUMBER;
                            item.SECTION_DATE_Third = WhereTimeDate[1].SECTION_DATE;
                            item.SECTION_TIME_START_Third = WhereTimeDate[1].SECTION_TIME_START;
                            item.SECTION_TIME_END_Third = WhereTimeDate[1].SECTION_TIME_END;
                            item.SECTION_CLASSROOM_Third = WhereTimeDate[1].SECTION_CLASSROOM;
                            item.SECTION_BRANCH_NAME_Third = WhereTimeDate[1].SECTION_BRANCH_NAME;
                            item.SECTION_PROFESSOR_Third = WhereTimeDate[1].SECTION_PROFESSOR_SHORTNAME;
                            item.TIME_CRASH = "3";
                        }
                        item.SEMESTER = semester;
                        item.YEAR = year;
                        _TimeCrash.Add(item);
                    }
                }
                else if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID != null)
                {
                    var FirstTime = double.Parse(collection["First_timestart_" + split[i]]);
                    var LastTime = double.Parse(collection["Second_timeend_" + split[i]]);
                    var Classroom = collection["First_classroom_" + split[i]];
                    var Date = collection["First_date_" + split[i]];
                    var Subject_id = collection["First_subjectid_" + split[i]];
                    var First_SUBJECT_NAME = collection["First_name_" + split[i]];
                    var First_SECTION_NUMBER = collection["First_number_" + split[i]];
                    var First_BRANCH_NAME = collection["First_branch_" + split[i]];
                    var unit = (from e1 in db.SECTIONs
                                join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                                where e1.SECTION_ID == FIRST_SECTION_ID
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
                                }).OrderBy(x => x.SECTION_TIME_START).First();
                    var query = (from e1 in db.SECTIONs
                                 join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                                 where e1.SECTION_ID != FIRST_SECTION_ID && e1.SECTION_CLASSROOM == Classroom && e1.SECTION_DATE == Date && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                    var WhereTimeDate = query.Where(x => (x.SECTION_TIME_START <= FirstTime && x.SECTION_TIME_START < LastTime && x.SECTION_TIME_END > FirstTime) && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม")).OrderBy(x => x.SECTION_DATE).ToList();
                    if (WhereTimeDate.Count() > 0)
                    {
                        var item = new TimeCrash();
                        foreach (var im in WhereTimeDate)
                        {
                            if (unit.SECTION_PROFESSOR_SHORTNAME == im.SECTION_PROFESSOR_SHORTNAME)
                            {
                                item.TEACHER_CRASH = "1";
                            }
                        }
                        item.SUBJECT_ID_First = Subject_id;
                        item.SUBJECT_NAME_First = First_SUBJECT_NAME;
                        item.SECTION_NUMBER_First = First_SECTION_NUMBER;
                        item.SECTION_DATE_First = Date;
                        item.SECTION_TIME_START_First = FirstTime;
                        item.SECTION_TIME_END_First = LastTime;
                        item.SECTION_CLASSROOM_First = Classroom;
                        item.SECTION_BRANCH_NAME_First = First_BRANCH_NAME;
                        item.SECTION_PROFESSOR_First = unit.SECTION_PROFESSOR_SHORTNAME;

                        item.SECTION_ID_Second = WhereTimeDate[0].SECTION_ID;
                        item.SUBJECT_ID_Second = WhereTimeDate[0].SUBJECT_ID;
                        item.SUBJECT_NAME_Second = WhereTimeDate[0].SUBJECT_NAME;
                        item.SECTION_NUMBER_Second = WhereTimeDate[0].SECTION_NUMBER;
                        item.SECTION_DATE_Second = WhereTimeDate[0].SECTION_DATE;
                        item.SECTION_TIME_START_Second = WhereTimeDate[0].SECTION_TIME_START;
                        item.SECTION_TIME_END_Second = WhereTimeDate[0].SECTION_TIME_END;
                        item.SECTION_CLASSROOM_Second = WhereTimeDate[0].SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_Second = WhereTimeDate[0].SECTION_BRANCH_NAME;
                        item.SECTION_PROFESSOR_Second = WhereTimeDate[0].SECTION_PROFESSOR_SHORTNAME;
                        item.TIME_CRASH = "2";
                        if (WhereTimeDate.Count() == 2)
                        {
                            item.SECTION_ID_Third = WhereTimeDate[1].SECTION_ID;
                            item.SUBJECT_ID_Third = WhereTimeDate[1].SUBJECT_ID;
                            item.SUBJECT_NAME_Third = WhereTimeDate[1].SUBJECT_NAME;
                            item.SECTION_NUMBER_Third = WhereTimeDate[1].SECTION_NUMBER;
                            item.SECTION_DATE_Third = WhereTimeDate[1].SECTION_DATE;
                            item.SECTION_TIME_START_Third = WhereTimeDate[1].SECTION_TIME_START;
                            item.SECTION_TIME_END_Third = WhereTimeDate[1].SECTION_TIME_END;
                            item.SECTION_CLASSROOM_Third = WhereTimeDate[1].SECTION_CLASSROOM;
                            item.SECTION_BRANCH_NAME_Third = WhereTimeDate[1].SECTION_BRANCH_NAME;
                            item.SECTION_PROFESSOR_Third = WhereTimeDate[1].SECTION_PROFESSOR_SHORTNAME;
                            item.TIME_CRASH = "3";
                        }
                        item.SEMESTER = semester;
                        item.YEAR = year;
                        _TimeCrash.Add(item);
                    }
                }
            }
            var TimeCrash = _TimeCrash.ToList();
            return TimeCrash;
        }
        public List<TimeCrash> GetWarning_single(FormCollection collection)
        {
            var semester = collection["Semester"];
            var year = collection["Year"];
            var SUBJECTid = collection["SUBJECTid"];
            var SearchId = collection["searchId"];
            var section = db.SECTIONs;
            List<Section_Subject> _Section_Subject = new List<Section_Subject>();
            var FIRST_SECTION_ID = int.Parse(collection["FIRST_SECTION_ID"]);
            var SECOND_SECTION_ID = collection["SECOND_SECTION_ID"];
            var unit = (from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_ID == FIRST_SECTION_ID
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
                        }).OrderBy(x => x.SECTION_TIME_START).First();
            if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID == "0")
            {
                var FirstTime = double.Parse(collection["FIRST_SAVE_TIMESTART"]);
                var LastTime = double.Parse(collection["FIRST_SAVE_TIMEEND"]);
                var Classroom = collection["FIRST_SAVE_CLASSROOM"];
                var Date = collection["FIRST_SAVE_DATE"];
                var Subject_id = unit.SUBJECT_ID;
                var First_SUBJECT_NAME = unit.SUBJECT_NAME;
                var First_SECTION_NUMBER = collection["FIRST_SAVE_NUMBER"];
                var First_BRANCH_NAME = collection["FIRST_SAVE_BRANCH"];
                var query = (from e1 in db.SECTIONs
                             join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                             where e1.SECTION_ID != FIRST_SECTION_ID && e1.SECTION_PROFESSOR_SHORTNAME.Contains(unit.SECTION_PROFESSOR_SHORTNAME) && e1.SECTION_DATE == Date && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                var WhereTimeDate = query.Where(x => (x.SECTION_TIME_START <= FirstTime && x.SECTION_TIME_START < LastTime && x.SECTION_TIME_END > FirstTime) && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม")).OrderBy(x => x.SECTION_DATE).ToList();
                if (WhereTimeDate.Count() > 0)
                {
                    var item = new TimeCrash();
                    foreach (var im in WhereTimeDate)
                    {
                        if (unit.SECTION_PROFESSOR_SHORTNAME == im.SECTION_PROFESSOR_SHORTNAME)
                        {
                            item.TEACHER_CRASH = "1";
                        }
                    }
                    item.SUBJECT_ID_First = Subject_id;
                    item.SUBJECT_NAME_First = First_SUBJECT_NAME;
                    item.SECTION_NUMBER_First = First_SECTION_NUMBER;
                    item.SECTION_DATE_First = Date;
                    item.SECTION_TIME_START_First = FirstTime;
                    item.SECTION_TIME_END_First = LastTime;
                    item.SECTION_CLASSROOM_First = Classroom;
                    item.SECTION_BRANCH_NAME_First = First_BRANCH_NAME;
                    item.SECTION_PROFESSOR_First = unit.SECTION_PROFESSOR_SHORTNAME;

                    item.SECTION_ID_Second = WhereTimeDate[0].SECTION_ID;
                    item.SUBJECT_ID_Second = WhereTimeDate[0].SUBJECT_ID;
                    item.SUBJECT_NAME_Second = WhereTimeDate[0].SUBJECT_NAME;
                    item.SECTION_NUMBER_Second = WhereTimeDate[0].SECTION_NUMBER;
                    item.SECTION_DATE_Second = WhereTimeDate[0].SECTION_DATE;
                    item.SECTION_TIME_START_Second = WhereTimeDate[0].SECTION_TIME_START;
                    item.SECTION_TIME_END_Second = WhereTimeDate[0].SECTION_TIME_END;
                    item.SECTION_CLASSROOM_Second = WhereTimeDate[0].SECTION_CLASSROOM;
                    item.SECTION_BRANCH_NAME_Second = WhereTimeDate[0].SECTION_BRANCH_NAME;
                    item.SECTION_PROFESSOR_Second = WhereTimeDate[0].SECTION_PROFESSOR_SHORTNAME;
                    item.TIME_CRASH = "2";
                    if (WhereTimeDate.Count() == 2)
                    {
                        item.SECTION_ID_Third = WhereTimeDate[1].SECTION_ID;
                        item.SUBJECT_ID_Third = WhereTimeDate[1].SUBJECT_ID;
                        item.SUBJECT_NAME_Third = WhereTimeDate[1].SUBJECT_NAME;
                        item.SECTION_NUMBER_Third = WhereTimeDate[1].SECTION_NUMBER;
                        item.SECTION_DATE_Third = WhereTimeDate[1].SECTION_DATE;
                        item.SECTION_TIME_START_Third = WhereTimeDate[1].SECTION_TIME_START;
                        item.SECTION_TIME_END_Third = WhereTimeDate[1].SECTION_TIME_END;
                        item.SECTION_CLASSROOM_Third = WhereTimeDate[1].SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_Third = WhereTimeDate[1].SECTION_BRANCH_NAME;
                        item.SECTION_PROFESSOR_Third = WhereTimeDate[1].SECTION_PROFESSOR_SHORTNAME;
                        item.TIME_CRASH = "3";
                    }
                    item.SEMESTER = semester;
                    item.YEAR = year;
                    _TimeCrash.Add(item);
                }
            }
            else if (ModelState.IsValid && FIRST_SECTION_ID != 0 && SECOND_SECTION_ID != "0")
            {
                var FirstTime = double.Parse(collection["FIRST_SAVE_TIMESTART"]);
                var LastTime = double.Parse(collection["SECOND_SAVE_TIMEEND"]);
                var Classroom = collection["FIRST_SAVE_CLASSROOM"];
                var Date = collection["FIRST_SAVE_DATE"];
                var Subject_id = unit.SUBJECT_ID;
                var First_SUBJECT_NAME = unit.SUBJECT_NAME;
                var First_SECTION_NUMBER = collection["FIRST_SAVE_NUMBER"];
                var First_BRANCH_NAME = collection["FIRST_SAVE_BRANCH"];

                var query = (from e1 in db.SECTIONs
                             join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                             where e1.SECTION_ID != FIRST_SECTION_ID && e1.SECTION_PROFESSOR_SHORTNAME.Contains(unit.SECTION_PROFESSOR_SHORTNAME) && e1.SECTION_DATE == Date && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                var WhereTimeDate = query.Where(x => (x.SECTION_TIME_START <= FirstTime && x.SECTION_TIME_START < LastTime && x.SECTION_TIME_END > FirstTime) && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม")).OrderBy(x => x.SECTION_DATE).ToList();
                if (WhereTimeDate.Count() > 0)
                {

                    var item = new TimeCrash();
                    foreach (var im in WhereTimeDate)
                    {
                        if (unit.SECTION_PROFESSOR_SHORTNAME == im.SECTION_PROFESSOR_SHORTNAME)
                        {
                            item.TEACHER_CRASH = "1";
                        }
                    }
                    item.SUBJECT_ID_First = Subject_id;
                    item.SUBJECT_NAME_First = First_SUBJECT_NAME;
                    item.SECTION_NUMBER_First = First_SECTION_NUMBER;
                    item.SECTION_DATE_First = Date;
                    item.SECTION_TIME_START_First = FirstTime;
                    item.SECTION_TIME_END_First = LastTime;
                    item.SECTION_CLASSROOM_First = Classroom;
                    item.SECTION_BRANCH_NAME_First = First_BRANCH_NAME;
                    item.SECTION_PROFESSOR_First = unit.SECTION_PROFESSOR_SHORTNAME;

                    item.SECTION_ID_Second = WhereTimeDate[0].SECTION_ID;
                    item.SUBJECT_ID_Second = WhereTimeDate[0].SUBJECT_ID;
                    item.SUBJECT_NAME_Second = WhereTimeDate[0].SUBJECT_NAME;
                    item.SECTION_NUMBER_Second = WhereTimeDate[0].SECTION_NUMBER;
                    item.SECTION_DATE_Second = WhereTimeDate[0].SECTION_DATE;
                    item.SECTION_TIME_START_Second = WhereTimeDate[0].SECTION_TIME_START;
                    item.SECTION_TIME_END_Second = WhereTimeDate[0].SECTION_TIME_END;
                    item.SECTION_CLASSROOM_Second = WhereTimeDate[0].SECTION_CLASSROOM;
                    item.SECTION_BRANCH_NAME_Second = WhereTimeDate[0].SECTION_BRANCH_NAME;
                    item.SECTION_PROFESSOR_Second = WhereTimeDate[0].SECTION_PROFESSOR_SHORTNAME;
                    item.TIME_CRASH = "2";
                    if (WhereTimeDate.Count() == 2)
                    {
                        item.SECTION_ID_Third = WhereTimeDate[1].SECTION_ID;
                        item.SUBJECT_ID_Third = WhereTimeDate[1].SUBJECT_ID;
                        item.SUBJECT_NAME_Third = WhereTimeDate[1].SUBJECT_NAME;
                        item.SECTION_NUMBER_Third = WhereTimeDate[1].SECTION_NUMBER;
                        item.SECTION_DATE_Third = WhereTimeDate[1].SECTION_DATE;
                        item.SECTION_TIME_START_Third = WhereTimeDate[1].SECTION_TIME_START;
                        item.SECTION_TIME_END_Third = WhereTimeDate[1].SECTION_TIME_END;
                        item.SECTION_CLASSROOM_Third = WhereTimeDate[1].SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_Third = WhereTimeDate[1].SECTION_BRANCH_NAME;
                        item.SECTION_PROFESSOR_Third = WhereTimeDate[1].SECTION_PROFESSOR_SHORTNAME;
                        item.TIME_CRASH = "3";
                    }
                    item.SEMESTER = semester;
                    item.YEAR = year;
                    _TimeCrash.Add(item);
                }
            }
            var TimeCrash = _TimeCrash.ToList();
            return TimeCrash;
        }
        public List<TimeCrash> SetNotification()
        {
            var section = db.SECTIONs;
            var semesteryear = (from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                                select new SemesterYear
                                {
                                    SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                    SEMESTER = d1.SEMESTER,
                                    YEAR = d1.YEAR
                                }).OrderByDescending(x => x.YEAR).OrderByDescending(x => x.SEMESTER).ToList();
            var YEAR = semesteryear.First().YEAR;
            var SEMESTER = semesteryear.First().SEMESTER;
            _section_subject = GetModel(semesteryear.First().SEMESTER, semesteryear.First().YEAR).ToList();

            foreach (var j in _section_subject.OrderBy(x => x.SECTION_TIME_START))
            {
                var WhereTimeDate = _section_subject.Where(x => (x.SECTION_TIME_START <= j.SECTION_TIME_START && x.SECTION_TIME_START < j.SECTION_TIME_END && x.SECTION_TIME_END > j.SECTION_TIME_START) && x.SECTION_ID != j.SECTION_ID && x.SECTION_DATE == j.SECTION_DATE && x.SECTION_BRANCH_NAME != j.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == j.SECTION_CLASSROOM && !x.SECTION_CLASSROOM.Contains("SHOP") && !x.SECTION_CLASSROOM.Contains("LAB") && !x.SECTION_CLASSROOM.Contains("สนาม") && x.SECTION_NUMBER != "").OrderBy(x => x.SECTION_TIME_START).ToList();
                if (WhereTimeDate.Count() > 0)
                {
                    foreach (var im in WhereTimeDate)
                    {
                        var e = section.Where(x => x.SECTION_ID == im.SECTION_ID).First();
                        e.CRASH = "3";
                    }
                    var item = new TimeCrash();
                    item.SECTION_ID_First = j.SECTION_ID;
                    item.SUBJECT_ID_First = j.SUBJECT_ID;
                    item.SUBJECT_NAME_First = j.SUBJECT_NAME;
                    item.SECTION_NUMBER_First = j.SECTION_NUMBER;
                    item.SECTION_DATE_First = j.SECTION_DATE;
                    item.SECTION_TIME_START_First = j.SECTION_TIME_START;
                    item.SECTION_TIME_END_First = j.SECTION_TIME_END;
                    item.SECTION_CLASSROOM_First = j.SECTION_CLASSROOM;
                    item.SECTION_BRANCH_NAME_First = j.SECTION_BRANCH_NAME;

                    item.SECTION_ID_Second = WhereTimeDate[0].SECTION_ID;
                    item.SUBJECT_ID_Second = WhereTimeDate[0].SUBJECT_ID;
                    item.SUBJECT_NAME_Second = WhereTimeDate[0].SUBJECT_NAME;
                    item.SECTION_NUMBER_Second = WhereTimeDate[0].SECTION_NUMBER;
                    item.SECTION_DATE_Second = WhereTimeDate[0].SECTION_DATE;
                    item.SECTION_TIME_START_Second = WhereTimeDate[0].SECTION_TIME_START;
                    item.SECTION_TIME_END_Second = WhereTimeDate[0].SECTION_TIME_END;
                    item.SECTION_CLASSROOM_Second = WhereTimeDate[0].SECTION_CLASSROOM;
                    item.SECTION_BRANCH_NAME_Second = WhereTimeDate[0].SECTION_BRANCH_NAME;
                    item.TIME_CRASH = "2";
                    if (WhereTimeDate.Count() == 2)
                    {
                        item.SECTION_ID_Third = WhereTimeDate[1].SECTION_ID;
                        item.SUBJECT_ID_Third = WhereTimeDate[1].SUBJECT_ID;
                        item.SUBJECT_NAME_Third = WhereTimeDate[1].SUBJECT_NAME;
                        item.SECTION_NUMBER_Third = WhereTimeDate[1].SECTION_NUMBER;
                        item.SECTION_DATE_Third = WhereTimeDate[1].SECTION_DATE;
                        item.SECTION_TIME_START_Third = WhereTimeDate[1].SECTION_TIME_START;
                        item.SECTION_TIME_END_Third = WhereTimeDate[1].SECTION_TIME_END;
                        item.SECTION_CLASSROOM_Third = WhereTimeDate[1].SECTION_CLASSROOM;
                        item.SECTION_BRANCH_NAME_Third = WhereTimeDate[1].SECTION_BRANCH_NAME;
                        item.TIME_CRASH = "3";
                    }
                    item.SEMESTER = SEMESTER;
                    item.YEAR = YEAR;
                    _TimeCrash.Add(item);
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
                        where e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
        #endregion
        public void LogFile(string Data)
        {
            try
            {
                var datetime = DateTime.Now.ToShortDateString().Replace('/', '-');
                string FilePath = Server.MapPath("~/LogFile/Log " + datetime + ".txt");
                string FileName = Path.GetFileName(FilePath);
                string data = DateTime.Now.ToString();
                string Username = Session["Username"].ToString();
                string Name = db.USERs.Where(x => x.USER_USERNAME == Username).FirstOrDefault().USER_FIRSTNAME;
                if (Name != null)
                {
                    data += " - " + Name + " - " + Data;
                }
                else
                {
                    data += " - " + Username + " - " + Data;
                }

                if (System.IO.File.Exists(FilePath))
                {
                    string read = "";

                    StreamReader sr = System.IO.File.OpenText(FilePath);
                    read = sr.ReadToEnd();
                    sr.Close();
                    System.IO.File.Delete(FilePath);
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        var byteArray = Encoding.UTF8.GetBytes(read + "\n" + data);
                        var stream = new MemoryStream(byteArray);
                        fs.Write(byteArray, 0, byteArray.Length);
                    }

                }
                else
                {
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        var byteArray = Encoding.UTF8.GetBytes(data);
                        var stream = new MemoryStream(byteArray);
                        fs.Write(byteArray, 0, byteArray.Length);
                    }
                }
            }
            catch
            {
            }
        }
        public void Export(string semester, string year)
        {
            var datetime = DateTime.Now.ToShortDateString().Replace('/', '-');
            string FilePath = Server.MapPath("~/Content/import/fin/ขบวน" + semester + "-" + year + " " + datetime + ".xlsx");
            System.IO.File.Delete(FilePath);
            string FileName = Path.GetFileName(FilePath);

            if (db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).Any() != false)
            {
                try
                {
                    var package = new ExcelPackage(new FileInfo(FilePath));
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.Add("ขบวน" + semester + "-" + year); //read sheet 1

                    //------------------------------------------------------//
                    worksheet.Cells["B:K"].Style.Font.Name = "TH SarabunPSK";
                    worksheet.Cells["B:K"].Style.Font.Size = 15;
                    worksheet.Column(2).Style.Numberformat.Format = "@";
                    //------------------------------------------------------//

                    //-------------------------------------------//
                    using (ExcelRange range = worksheet.Cells["A2:K3"])
                    {
                        range.Merge = true;
                        range.Value = "ขบวนวิชาที่เปิดสอนระดับปริญญาตรี";
                        range.Style.Font.Name = "Angsana New";
                        range.Style.Font.Size = 20;
                        range.Style.Font.Bold = true;
                        range.Style.Font.UnderLine = true;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    //-------------------------------------------//
                    using (ExcelRange range = worksheet.Cells["A4:K4"])
                    {
                        range.Merge = true;
                        range.Value = "ภาคการศึกษาที่ " + semester + " ปีการศึกษา " + year;
                        range.Style.Font.Name = "Angsana New";
                        range.Style.Font.Size = 20;
                        range.Style.Font.Bold = true;
                        range.Style.Font.UnderLine = true;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    //-------------------------------------------//
                    worksheet.Column(10).Style.Numberformat.Format = "DD MMM YY";
                    worksheet.Column(2).Width = 12f;
                    worksheet.Column(4).Width = 13f;
                    worksheet.Column(5).Width = 16f;
                    worksheet.Column(6).Width = 21f;
                    worksheet.Column(7).Width = 30f;
                    worksheet.Column(11).Width = 11f;
                    int row = 5;
                    foreach (SUBJECT p in db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).ToList())
                    {
                        //-------------------------------------------//
                        worksheet.Cells[row, 2].Value = p.SUBJECT_ID;
                        worksheet.Cells[row, 3].Value = p.SUBJECT_NAME;
                        worksheet.Cells[row, 7].Value = p.SUBJECT_CREDIT;
                        //-------------------------------------------//
                        var midtermcheck = p.SUBJECT_MIDTERM_DATE.Any();
                        var finalcheck = p.SUBJECT_FINAL_DATE.Any();
                        if (midtermcheck == true && finalcheck == true)
                        {
                            worksheet.Cells[row, 9].Value = "Mid";
                            worksheet.Cells[row + 1, 9].Value = "Final";
                            worksheet.Cells[row, 10].Value = p.SUBJECT_MIDTERM_DATE;
                            worksheet.Cells[row, 11].Value = p.SUBJECT_MIDTERM_TIME;
                            worksheet.Cells[row + 1, 10].Value = p.SUBJECT_FINAL_DATE;
                            worksheet.Cells[row + 1, 11].Value = p.SUBJECT_FINAL_TIME;
                        }
                        else if (midtermcheck == true && finalcheck == false)
                        {
                            worksheet.Cells[row, 9].Value = "Mid";
                            worksheet.Cells[row, 10].Value = p.SUBJECT_MIDTERM_DATE;
                            worksheet.Cells[row, 11].Value = p.SUBJECT_MIDTERM_TIME;
                        }
                        else if (midtermcheck == false && finalcheck == true)
                        {
                            worksheet.Cells[row, 9].Value = "Final";
                            worksheet.Cells[row, 10].Value = p.SUBJECT_FINAL_DATE;
                            worksheet.Cells[row, 11].Value = p.SUBJECT_FINAL_TIME;
                        }
                        row++;
                        foreach (SECTION r in db.SECTIONs.Where(x => x.SUBJECT_ID == p.SUBJECT_ID && x.SEMESTER == semester && x.YEAR == year).ToList())
                        {
                            worksheet.Cells[row, 2].Value = r.SECTION_NUMBER;
                            worksheet.Cells[row, 3].Value = r.SECTION_DATE;
                            worksheet.Cells[row, 4].Value = Convert.ToDecimal(r.SECTION_TIME_START).ToString("0#.00") + "-" + Convert.ToDecimal(r.SECTION_TIME_END).ToString("0#.00");
                            worksheet.Cells[row, 5].Value = r.SECTION_CLASSROOM;
                            worksheet.Cells[row, 6].Value = r.SECTION_PROFESSOR_SHORTNAME;
                            worksheet.Cells[row, 7].Value = r.SECTION_BRANCH_NAME;
                            row++;
                        }
                        row++;
                    }
                    //-------------------------------------------//
                    package.SaveAs(new FileInfo(FilePath));
                    byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
                }
                catch
                {
                }
            }
        }
    }
}