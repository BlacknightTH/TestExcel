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
                        for (int row = 4; row < range.Rows.Count; row++)
                        {
                            string B = ((Excel.Range)range.Cells[row, 2]).Text;
                            var ee = db.SUBJECTs.SqlQuery("SELECT * FROM SUBJECT WHERE SUBJECT_ID = '"+ B +"'").Any();
                            if (B.Length > 4 && ee != true)
                            {
                                string subject_ID = B;
                                string subject_NAME = ((Excel.Range)range.Cells[row, 3]).Text;
                                string subject_CREDIT = ((Excel.Range)range.Cells[row, 7]).Text;
                                string subject_MID = ((Excel.Range)range.Cells[row, 10]).Text;
                                string subject_FINAL = ((Excel.Range)range.Cells[row+1, 10]).Text;
                                string subject_TIMEMID = ((Excel.Range)range.Cells[row, 11]).Text;
                                string subject_TIMEFINAL = ((Excel.Range)range.Cells[row+1, 11]).Text;

                                saveSubject(subject_ID, subject_NAME, subject_CREDIT, subject_MID, subject_FINAL, subject_TIMEMID, subject_TIMEFINAL, db);
                            }
                        }

                        for (int row = 4; row < range.Rows.Count; row++)
                        {
                            string B = ((Excel.Range)range.Cells[row, 2]).Text;
                            if (B.Length > 4)
                            {
                                tmp = B;
                                
                            }
                            else if(B.Length > 0 && B.Length <= 4)
                            {

                            }
                            else
                            {

                            }
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
    }
}