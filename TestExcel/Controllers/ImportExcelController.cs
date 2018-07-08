using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TestExcel.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace TestExcel.Controllers
{
    public class ImportExcelController : Controller
    {
        // GET: ImportExcel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Success()
        {
            TestExcelEntities db = new TestExcelEntities();
            var model = db.SUBJECTs.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a excel file<br>";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/import/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    //Read data from excel file
                    Process[] excelProcsOld = Process.GetProcessesByName("EXCEL");
                    try
                    {
                        Excel.Application excelapplication = new Excel.Application();
                        Excel.Workbook workbook = excelapplication.Workbooks.Open(path);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Excel.Range range = worksheet.UsedRange;
                        TestExcelEntities db = new TestExcelEntities();
                        string tmp = "";
                        string tmp2 = "";
                        for (int row = 4; row < range.Rows.Count; row++)
                        {
                            string B = ((Excel.Range)range.Cells[row, 2]).Text;
                            string C = ((Excel.Range)range.Cells[row, 3]).Text;
                            string D = ((Excel.Range)range.Cells[row, 4]).Text;
                            string E = ((Excel.Range)range.Cells[row, 5]).Text;
                            string F = ((Excel.Range)range.Cells[row, 6]).Text;
                            string G = ((Excel.Range)range.Cells[row, 7]).Text;

                            //
                            if (B.Length > 4)
                            {
                                tmp = B;
                                var CheckSubject = db.SUBJECTs.SqlQuery("SELECT * FROM SUBJECT WHERE SUBJECT_ID = '" + B + "'").Any();
                                if (CheckSubject != true)
                                {
                                    if (G.Length != 0)
                                    {
                                        string Subject_ID = B;
                                        string subject_NAME = C;
                                        string subject_CREDIT = G;
                                        string subject_MID = ((Excel.Range)range.Cells[row, 10]).Text;
                                        string subject_FINAL = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                        string subject_TIMEMID = ((Excel.Range)range.Cells[row, 11]).Text;
                                        string subject_TIMEFINAL = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                        saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MID, subject_FINAL, subject_TIMEMID, subject_TIMEFINAL, db);
                                    }
                                    else
                                    {
                                        string Subject_ID = B;
                                        string subject_NAME = C;
                                        string subject_CREDIT = ((Excel.Range)range.Cells[row, 8]).Text;
                                        string subject_MID = ((Excel.Range)range.Cells[row, 10]).Text;
                                        string subject_FINAL = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                        string subject_TIMEMID = ((Excel.Range)range.Cells[row, 11]).Text;
                                        string subject_TIMEFINAL = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                        saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MID, subject_FINAL, subject_TIMEMID, subject_TIMEFINAL, db);
                                    }
                                }
                            }
                            else if (B.Length <= 4)
                            {
                                tmp2 = B;

                                if (B.Length != 0)
                                {
                                    if (G.LastOrDefault().ToString() == ",")
                                    {
                                        G = G + ((Excel.Range)range.Cells[row + 1, 7]).Text;
                                    }
                                    var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                                    "SECTION_NUMBER = '" + B + "' and SECTION_DATE = '" + C + "' and SECTION_TIME = '" + D + "' and SECTION_CLASSROOM = '" + E + "' " +
                                    " and SECTION_TEACHER = '" + F + "' and SECTION_FACULTY = '" + G + "'").Any();
                                    if (CheckSection == false)
                                    {
                                        string Subject_ID = tmp;
                                        string Section_Number = B;
                                        string Section_Date = C;
                                        string Section_Time = D;
                                        string Section_Classroom = E;
                                        string Section_Teacher = F;
                                        string Section_Faculty = G;
                                        saveSection(Subject_ID, Section_Number, Section_Date, Section_Time, Section_Classroom, Section_Teacher, Section_Faculty, db);
                                    }
                                }
                                else
                                {
                                    if (C.Length != 0 && D.Length != 0 && G.Length != 0)
                                    {
                                        if (tmp2 != "")
                                        {
                                            if (G.LastOrDefault().ToString() == ",")
                                            {
                                                G = G + ((Excel.Range)range.Cells[row + 1, 7]).Text;
                                            }
                                            var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                                        "SECTION_NUMBER = '" + tmp2 + "' and SECTION_DATE = '" + C + "' and SECTION_TIME = '" + D + "' and SECTION_CLASSROOM = '" + E + "' " +
                                        " and SECTION_TEACHER = '" + F + "' and SECTION_FACULTY = '" + G + "'").Any();
                                            if (CheckSection == false)
                                            {
                                                string Subject_ID = tmp;
                                                string Section_Number = tmp2;
                                                string Section_Date = ((Excel.Range)range.Cells[row, 3]).Text;
                                                string Section_Time = ((Excel.Range)range.Cells[row, 4]).Text;
                                                string Section_Classroom = ((Excel.Range)range.Cells[row, 5]).Text;
                                                string Section_Teacher = ((Excel.Range)range.Cells[row, 6]).Text;
                                                string Section_Faculty = ((Excel.Range)range.Cells[row, 7]).Text;
                                                saveSection(Subject_ID, Section_Number, Section_Date, Section_Time, Section_Classroom, Section_Teacher, Section_Faculty, db);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //for (int row = 4; row < range.Rows.Count; row++)
                        //{
                        //    string B = ((Excel.Range)range.Cells[row, 2]).Text;
                        //    string C = ((Excel.Range)range.Cells[row, 3]).Text;
                        //    string D = ((Excel.Range)range.Cells[row, 4]).Text;
                        //    string E = ((Excel.Range)range.Cells[row, 5]).Text;
                        //    string F = ((Excel.Range)range.Cells[row, 6]).Text;
                        //    string G = ((Excel.Range)range.Cells[row, 7]).Text;
                        //    if (B.Length > 4)
                        //    {
                        //        tmp = B;

                        //    }
                        //    else if (B.Length > 0 && B.Length <= 4)
                        //    {
                        //        var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                        //            "SECTION_NUMBER = '"+ B +"' and SECTION_DATE = '"+ C +"' and SECTION_TIME = '"+ D + "' and SECTION_CLASSROOM = '"+ E +"' " +
                        //            " and SECTION_TEACHER = '"+ F +"' and SECTION_FACULTY = '"+ G +"'").Any();

                        //        if(CheckSection == false)
                        //        {
                        //            tmp2 = ((Excel.Range)range.Cells[row, 2]).Text;
                        //            string Subject_ID = tmp;
                        //            string Section_Number = ((Excel.Range)range.Cells[row, 2]).Text;
                        //            string Section_Date = ((Excel.Range)range.Cells[row, 3]).Text;
                        //            string Section_Time = ((Excel.Range)range.Cells[row, 4]).Text;
                        //            string Section_Classroom = ((Excel.Range)range.Cells[row, 5]).Text;
                        //            string Section_Teacher = ((Excel.Range)range.Cells[row, 6]).Text;
                        //            string Section_Faculty = ((Excel.Range)range.Cells[row, 7]).Text;
                        //            saveSection(Subject_ID, Section_Number, Section_Date, Section_Time, Section_Classroom, Section_Teacher, Section_Faculty, db);
                        //         }
                        //        else
                        //        {

                        //        }



                        //    }
                        //    else if (B.Length == 0) 
                        //    {
                        //        string C = ((Excel.Range)range.Cells[row, 3]).Text;
                        //        string D = ((Excel.Range)range.Cells[row, 4]).Text;
                        //        string E = ((Excel.Range)range.Cells[row, 5]).Text;
                        //        string F = ((Excel.Range)range.Cells[row, 6]).Text;
                        //        string G = ((Excel.Range)range.Cells[row, 7]).Text;
                        //        var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                        //            "SECTION_NUMBER = '" + tmp2 + "' and SECTION_DATE = '" + C + "' and SECTION_TIME = '" + D + "' and SECTION_CLASSROOM = '" + E + "' " +
                        //            " and SECTION_TEACHER = '" + F + "' and SECTION_FACULTY = '" + G + "'").Any();
                        //        if (C.Length != 0)
                        //        {
                        //            if (CheckSection == false)
                        //            {
                        //                string Subject_ID = tmp;
                        //                string Section_Number = tmp2;
                        //                string Section_Date = ((Excel.Range)range.Cells[row, 3]).Text;
                        //                string Section_Time = ((Excel.Range)range.Cells[row, 4]).Text;
                        //                string Section_Classroom = ((Excel.Range)range.Cells[row, 5]).Text;
                        //                string Section_Teacher = ((Excel.Range)range.Cells[row, 6]).Text;
                        //                string Section_Faculty = ((Excel.Range)range.Cells[row, 7]).Text;
                        //                saveSection(Subject_ID, Section_Number, Section_Date, Section_Time, Section_Classroom, Section_Teacher, Section_Faculty, db);
                        //            }
                        //            else
                        //            {

                        //            }
                        //        }
                        //    }
                        //}
                    }
                    catch
                    {

                    }
                    finally
                    {
                        //Compare the EXCEL ID and Kill it 
                        Process[] excelProcsNew = Process.GetProcessesByName("EXCEL");
                        foreach (Process procNew in excelProcsNew)
                        {
                            int exist = 0;
                            foreach (Process procOld in excelProcsOld)
                            {
                                if (procNew.Id == procOld.Id)
                                {
                                    exist++;
                                }
                            }
                            if (exist == 0)
                            {
                                procNew.Kill();
                            }
                        }
                    }
                    return RedirectToAction("Success");
                }
                else
                {
                    ViewBag.Error = "File type is incorrect<br>";
                    return View("Index");
                }
            }
        }

        public void saveSubject(string subject_ID, string subject_NAME, string subject_CREDIT, string subject_MID,
                                string subject_FINAL, string subject_TIMEMID, string subject_TIMEFINAL, TestExcelEntities db)
        {
            try
            {
                //Check exists
                var item = new SUBJECT();
                item.SUBJECT_ID = subject_ID;
                item.SUBJECT_NAME = subject_NAME;
                item.SUBJECT_CREDIT = subject_CREDIT;
                item.SUBJECT_MID = subject_MID;
                item.SUBJECT_FINAL = subject_FINAL;
                item.SUBJECT_TIMEMID = subject_TIMEMID;
                item.SUBJECT_TIMEFINAL = subject_TIMEFINAL;
                db.SUBJECTs.Add(item);
                db.SaveChanges();

            }
            catch
            {

            }
        }

        public void saveSection(string SUBJECT_ID, string SECTION_NUMBER, string SECTION_DATE, string SECTION_TIME,
                                string SECTION_CLASSROOM, string SECTION_TEACHER, string SECTION_FACULTY, TestExcelEntities db)
        {
            try
            {
                //Check exists
                var item = new SECTION();
                item.SUBJECT_ID = SUBJECT_ID;
                item.SECTION_NUMBER = SECTION_NUMBER;
                item.SECTION_DATE = SECTION_DATE;
                item.SECTION_TIME = SECTION_TIME;
                item.SECTION_CLASSROOM = SECTION_CLASSROOM;
                item.SECTION_TEACHER = SECTION_TEACHER;
                item.SECTION_FACULTY = SECTION_FACULTY;
                db.SECTIONs.Add(item);
                db.SaveChanges();

            }
            catch
            {

            }
        }
    }
}