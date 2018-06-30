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
            ViewBag.model = db.SUBJECTs.ToList();
            return View();
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
                        for (int row = 1; row < range.Rows.Count; row++)
                        {
                            string subject_ID = ((Excel.Range)range.Cells[row, 1]).Text;
                            string subject_NAME = ((Excel.Range)range.Cells[row, 2]).Text;
                            string subject_CREDIT = ((Excel.Range)range.Cells[row, 3]).Text;
                            string subject_SECTION = ((Excel.Range)range.Cells[row, 4]).Text;
                            string subject_DAY = ((Excel.Range)range.Cells[row, 5]).Text;
                            string subject_TIME = ((Excel.Range)range.Cells[row, 6]).Text;
                            string subject_CLASSROOM = ((Excel.Range)range.Cells[row, 7]).Text;
                            string subject_TEACHER = ((Excel.Range)range.Cells[row, 8]).Text;
                            string subject_SECT = ((Excel.Range)range.Cells[row, 9]).Text;
                            string subject_YEAR = ((Excel.Range)range.Cells[row, 10]).Text;

                            saveSubject(subject_ID, subject_NAME, subject_CREDIT,
                                subject_SECTION, subject_DAY, subject_TIME, subject_CLASSROOM,
                                subject_TEACHER, subject_SECT, subject_YEAR, db);
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

        //private bool ImportData(out int count)
        //{
        //    var result = false;
        //    count = 0;
        //    try
        //    {
        //        String path = Server.MapPath("/") + "\\import\\Test.xlsx";
        //        var package = new ExcelPackage(new FileInfo(path));
        //        int startColumn = 1; // in this file excel, data column start at 1, row 1
        //        int startRow = 1;
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets[1]; //read sheet 1
        //        object data = null;

        //        TestExcelEntities db = new TestExcelEntities();

        //        do
        //        {
        //            data = workSheet.Cells[startRow, startColumn].Value; //column No.
        //            object subject_ID = workSheet.Cells[startRow, startColumn].Value.ToString(); //read column subject id
        //            if (data != null && subject_ID != null)
        //            {
        //                //import db
        //                var isSuccess = saveSubject(subject_ID.ToString(), db);
        //                if(isSuccess)
        //                {
        //                    count++;
        //                }
        //            }
        //            startRow++;
        //        }
        //        while (data != null) ;
        //    }
        //    catch
        //    {

        //    }
        //    return result;
        //}

        public bool saveSubject(string subject_ID, string subject_NAME, string subject_CREDIT,
                                string subject_SECTION, string subject_DAY, string subject_TIME, string subject_CLASSROOM,
                                string subject_TEACHER, string subject_SECT, string subject_YEAR, TestExcelEntities db)
        {
            var result = false;
            try
            {
                //Check exists
                var item = new SUBJECT();
                item.SUBJECT_ID = subject_ID;
                item.SUBJECT_NAME = subject_NAME;
                item.SUBJECT_CREDIT = subject_CREDIT;
                item.SUBJECT_SECTION = subject_SECTION;
                item.SUBJECT_DAY = subject_DAY;
                item.SUBJECT_TIME = subject_TIME;
                item.SUBJECT_CLASSROOM = subject_CLASSROOM;
                item.SUBJECT_TEACHER = subject_TEACHER;
                item.SUBJECT_SECT = subject_SECT;
                item.SUBJECT_YEAR = subject_YEAR;
                db.SUBJECTs.Add(item);
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