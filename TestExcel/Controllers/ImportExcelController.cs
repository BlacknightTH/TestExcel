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
            if(excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a excel file<br>";
                return View("Index");
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
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
                        bool dd = false;
                        for (int row = 4; row < range.Rows.Count; row++)
                        {
                            string B = ((Excel.Range)range.Cells[row, 2]).Text;
                            string credit = ((Excel.Range)range.Cells[row, 7]).Text;
                            //string C = ((Excel.Range)range.Cells[row, 3]).Text;
                            //string D = ((Excel.Range)range.Cells[row, 4]).Text;
                            //string E = ((Excel.Range)range.Cells[row, 5]).Text;
                            //string F = ((Excel.Range)range.Cells[row, 6]).Text;
                            //string G = ((Excel.Range)range.Cells[row, 7]).Text;
                            //string H = ((Excel.Range)range.Cells[row, 8]).Text;
                            //string I = ((Excel.Range)range.Cells[row, 9]).Text;
                            //string J = ((Excel.Range)range.Cells[row, 10]).Text;
                            //string K = ((Excel.Range)range.Cells[row, 11]).Text;
                            var ee = db.SUBJECTs.SqlQuery("SELECT * FROM SUBJECT WHERE SUBJECT_ID = '" + B + "'").Any();
                            if (B.Length < 4)
                            { 
                            dd = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' AND SECTION_NUMBER = '" + B + "'").Any();
                            }
                            if (B.Length > 4 && ee != true)
                            {
                                if (credit != "")
                                {
                                    tmp = B;
                                    string subject_ID = B;
                                    string subject_NAME = ((Excel.Range)range.Cells[row, 3]).Text;
                                    string subject_CREDIT = credit;
                                    string subject_MID = ((Excel.Range)range.Cells[row, 10]).Text;
                                    string subject_FINAL = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                    string subject_TIMEMID = ((Excel.Range)range.Cells[row, 11]).Text;
                                    string subject_TIMEFINAL = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                    saveSubject(subject_ID, subject_NAME, subject_CREDIT, subject_MID, subject_FINAL, subject_TIMEMID, subject_TIMEFINAL, db);
                                }
                                else
                                {
                                    tmp = B;
                                    string subject_ID = B;
                                    string subject_NAME = ((Excel.Range)range.Cells[row, 3]).Text;
                                    string subject_CREDIT = ((Excel.Range)range.Cells[row, 8]).Text;
                                    string subject_MID = ((Excel.Range)range.Cells[row, 10]).Text;
                                    string subject_FINAL = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                    string subject_TIMEMID = ((Excel.Range)range.Cells[row, 11]).Text;
                                    string subject_TIMEFINAL = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                    saveSubject(subject_ID, subject_NAME, subject_CREDIT, subject_MID, subject_FINAL, subject_TIMEMID, subject_TIMEFINAL, db);
                                }
                            }
                            else if(B.Length <= 4 && dd != true)
                            {
                                string subject_ID = tmp;
                                string Section_NUMBER = B;
                                string Section_DATE = ((Excel.Range)range.Cells[row, 3]).Text;
                                string Section_TIME = ((Excel.Range)range.Cells[row, 4]).Text;
                                string Section_CLASSROOM = ((Excel.Range)range.Cells[row, 5]).Text;
                                string Section_TEACHER = ((Excel.Range)range.Cells[row, 6]).Text;
                                string Section_FACULTY = ((Excel.Range)range.Cells[row, 7]).Text;

                                saveSection(subject_ID, Section_NUMBER, Section_DATE, Section_TIME, Section_CLASSROOM, Section_TEACHER, Section_FACULTY, db);
                            }

                            //saveSubject(subject_ID, subject_NAME, subject_CREDIT,
                            //    subject_SECTION, subject_DAY, subject_TIME, subject_CLASSROOM,
                            //    subject_TEACHER, subject_SECT, subject_YEAR, db);
                        }
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

        public bool saveSubject(string subject_ID, string subject_NAME, string subject_CREDIT, string subject_MID,
                                string subject_FINAL, string subject_TIMEMID, string subject_TIMEFINAL, TestExcelEntities db)
        {
            var result = false;
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
                result = true;
            }
            catch
            {

            }
            return result;
        }

        public bool saveSection(string subject_ID, string Section_NUMBER, string Section_DATE, string Section_TIME, 
                                string Section_CLASSROOM, string Section_TEACHER, string Section_FACULTY, TestExcelEntities db)
        {
            var result = false;
            try
            {
                //Check exists
                var item = new SECTION();
                item.SUBJECT_ID = subject_ID;
                item.SECTION_NUMBER = Section_NUMBER;
                item.SECTION_DATE = Section_DATE;
                item.SECTION_TIME = Section_TIME;
                item.SECTION_CLASSROOM = Section_CLASSROOM;
                item.SECTION_TEACHER = Section_TEACHER;
                item.SECTION_FACULTY = Section_FACULTY;
                db.SECTIONs.Add(item);
                db.SaveChanges();
                result = true;
            }
            catch
            {

            }
            return result;
        }
    }
}