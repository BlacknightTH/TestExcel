﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using TestExcel.Models;
using System.Text;
using TestExcel.Utility;
using Newtonsoft.Json;

namespace TestExcel.Controllers
{
    public class DataController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: Data
        #region Section
        public ActionResult Section()
        {
            var model = db.SECTIONs.ToList();
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
            if (ModelState.IsValid)
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
            if (ModelState.IsValid)
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
        #region Member
        public ActionResult Member()
        {
            var model = db.USERs.ToList();
            return View(model);
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
            if (ModelState.IsValid)
            {
                if (ID > 0)
                {
                    //Edit
                    var edit = db.USERs.Where(x => x.ID == ID).FirstOrDefault();
                    if (edit != null)
                    {
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
            string PROFESSOR_FIRSTNAME = collection["PROFESSOR_FIRSTNAME"];
            string PROFESSOR_LASTNAME = collection["PROFESSOR_LASTNAME"];
            string PROFESSOR_SHORTNAME = collection["PROFESSOR_SHORTNAME"];
            string PROFESSOR_STATUS = collection["PROFESSOR_STATUS"];
            string DEPARTMENT_NAME = collection["DEPARTMENT_NAME"];
            if (ModelState.IsValid)
            {
                if (PROFESSOR_ID > 0)
                {
                    //Edit
                    var edit = db.PROFESSORs.Where(x => x.PROFESSOR_ID == PROFESSOR_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.PROFESSOR_FIRSTNAME = PROFESSOR_FIRSTNAME;
                        edit.PROFESSOR_LASTNAME = PROFESSOR_LASTNAME;
                        edit.PROFESSOR_SHORTNAME = PROFESSOR_SHORTNAME;
                        edit.PROFESSOR_STATUS = PROFESSOR_STATUS;
                        edit.DEPARTMENT_NAME = DEPARTMENT_NAME;
                    }
                }
                else
                {
                    //Add
                    var item = new PROFESSOR();
                    item.PROFESSOR_FIRSTNAME = PROFESSOR_FIRSTNAME;
                    item.PROFESSOR_LASTNAME = PROFESSOR_LASTNAME;
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
            string DEPARTMENT_NAME = collection["DEPARTMENT_NAME"];
            string DEPARTMENT_THAI_NAME = collection["DEPARTMENT_THAI_NAME"];
            if (ModelState.IsValid)
            {
                if (DEPARTMENT_ID > 0)
                {
                    //Edit
                    var edit = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == DEPARTMENT_ID).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.DEPARTMENT_NAME = DEPARTMENT_NAME;
                        edit.DEPARTMENT_THAI_NAME = DEPARTMENT_THAI_NAME;
                    }
                }
                else
                {
                    //Add
                    var item = new DEPARTMENT();
                    item.DEPARTMENT_NAME = DEPARTMENT_NAME;
                    item.DEPARTMENT_THAI_NAME = DEPARTMENT_THAI_NAME;
                    db.DEPARTMENTs.Add(item);
                }
                db.SaveChanges();

            }
            return RedirectToAction("Department");
        }
        [HttpPost]
        public ActionResult DeleteDepartment(FormCollection collection)
        {
            int DEPARTMENT_ID = int.Parse(collection["Del_DEPARTMENT_ID"]);
            var del = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == DEPARTMENT_ID).FirstOrDefault();
            if (del != null)
            {
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
        #region Building
        public ActionResult Building()
        {
            var model = db.BUILDINGs.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveBuilding(FormCollection collection)
        {
            int BUILDING_ID = int.Parse(collection["BUILDING_ID"]);
            string BUILDING_NAME = collection["BUILDING_NAME"];
            string CLASSROOM_NAME = collection["CLASSROOM_NAME"];
            if (ModelState.IsValid)
            {
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
            string DepartName = collection["DEPARTMENT_NAME"];
            if (ModelState.IsValid)
            {
                if(BranchId > 0)
                {
                    //Edit
                    var edit = db.BRANCHes.Where(x => x.BRANCH_ID == BranchId).FirstOrDefault();
                    if (edit != null)
                    {
                        edit.BRANCH_NAME = BranchName;
                        edit.DEPARTMENT_NAME = DepartName;
                    }
                }
                else
                {
                    //Add
                    var item = new BRANCH();
                    item.BRANCH_NAME = BranchName;
                    item.DEPARTMENT_NAME = DepartName;
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
    }
}