using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using TestExcel.Data;
using TestExcel.Report;
using TestExcel.Models;
using TestExcel.Utility;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TestExcel.Controllers
{
    [adminauthen]
    public class ReportController : Controller
    {
        string[] date = { "วันจันทร์", "วันอังคาร", "วันพุธ", "วันพฤหัสบดี", "วันศุกร์", "วันเสาร์" };
        TestExcelEntities db = new TestExcelEntities();
        // GET: PdfExport
        public ActionResult data()
        {
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };
            string first_Year;
                first_Year = "2560";
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", first_Year);
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_NAME", "DEPARTMENT_NAME");
            return View();
        }
        [HttpPost]
        public ActionResult Report(FormCollection collection)
        {
            string department_name = collection["department_name"];
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            byte[] abytes = pdfReport.PrepareReport(department_name, semester, year);

            string FilePath = "D:" + "\\รายการลงทะเบียนเรียน_" + semester + "-" + year + "_" + department_name + ".pdf";
            string FileName = Path.GetFileName(FilePath);

            //return File(abytes, "application/pdf", FileName);
            return File(abytes, "application/pdf", FileName);
        }
        public ActionResult TeReport(FormCollection collection)
        {
            int Date = int.Parse(collection["DDL_DATE"]);
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            byte[] abytes = pdfReport.TePrepareReport(Date, semester, year);

            string FilePath = "D:\\" + "ตารางลงห้องเรียน_" + date[Date] + "_" + semester + "-" + year + ".pdf";
            string FileName = Path.GetFileName(FilePath);

            //return File(abytes, "application/pdf", FileName);
            return File(abytes, "application/pdf", FileName);
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a excel file<br>";
                return View("data");
            }
            else
            {
                if ((excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx")) && excelfile.FileName.StartsWith("ขบวน"))
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

                        string semester_year = ((Excel.Range)range.Cells[3, 1]).Text;
                        string[] split_semester_year = semester_year.Split(' ');
                        string semester = split_semester_year[1];
                        string year = split_semester_year[3];
                        for (int row = 4; row < range.Rows.Count; row++)
                        {
                            string B = ((Excel.Range)range.Cells[row, 2]).Text;
                            string C = ((Excel.Range)range.Cells[row, 3]).Text;
                            string D = ((Excel.Range)range.Cells[row, 4]).Text;
                            string E = ((Excel.Range)range.Cells[row, 5]).Text;
                            string F = ((Excel.Range)range.Cells[row, 6]).Text;
                            string G = ((Excel.Range)range.Cells[row, 7]).Text;

                            if (B.Length > 4)
                            {
                                tmp = B;
                                var CheckSubject = db.SUBJECTs.SqlQuery("SELECT * FROM SUBJECT WHERE SUBJECT_ID = '" + B + "' and SEMESTER = '" + semester + "' and YEAR = '" + year + "'").Any();
                                if (CheckSubject != true)
                                {
                                    if (G.Length != 0)
                                    {
                                        string Subject_ID = B;
                                        string subject_NAME = C;
                                        string subject_CREDIT = G;
                                        string subject_MIDTERM_DATE = ((Excel.Range)range.Cells[row, 10]).Text;
                                        string subject_FINAL_DATE = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                        string subject_MIDTERM_TIME = ((Excel.Range)range.Cells[row, 11]).Text;
                                        string subject_FINAL_TIME = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                        saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                    }
                                    else
                                    {
                                        string Subject_ID = B;
                                        string subject_NAME = C;
                                        string subject_CREDIT = ((Excel.Range)range.Cells[row, 8]).Text;
                                        string subject_MIDTERM_DATE = ((Excel.Range)range.Cells[row, 10]).Text;
                                        string subject_FINAL_DATE = ((Excel.Range)range.Cells[row + 1, 10]).Text;
                                        string subject_MIDTERM_TIME = ((Excel.Range)range.Cells[row, 11]).Text;
                                        string subject_FINAL_TIME = ((Excel.Range)range.Cells[row + 1, 11]).Text;

                                        saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                    }
                                }
                            }
                            else if (B.Length <= 4)
                            {
                                tmp2 = B;
                                string[] split_date = D.Split('-');
                                if (B.Length != 0)
                                {
                                    if (G.LastOrDefault().ToString() == ",")
                                    {
                                        G = G + ((Excel.Range)range.Cells[row + 1, 7]).Text;
                                    }
                                    var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                                    "SECTION_NUMBER = '" + B + "' and SECTION_DATE = '" + C + "' and SECTION_TIME_START = '" + split_date[0] + "' and SECTION_TIME_END = '" + split_date[1] + "' and SECTION_CLASSROOM = '" + E + "' " +
                                    " and SECTION_PROFESSOR_SHORTNAME = '" + F + "' and SECTION_BRANCH_NAME = '" + G + "' and SEMESTER = '" + semester + "' and YEAR = '" + year + "'").Any();
                                    if (CheckSection == false)
                                    {
                                        string Subject_ID = tmp;
                                        string Section_Number = B;
                                        string Section_Date = C;
                                        string Section_Start_Time = split_date[0];
                                        string Section_End_Time = split_date[1];
                                        string Section_Classroom = E;
                                        string Section_Professor = F;
                                        string Section_Branch_Name = G;
                                        saveSection(Subject_ID, Section_Number, Section_Date, Section_Start_Time, Section_End_Time, Section_Classroom, Section_Professor, Section_Branch_Name, semester, year, db);
                                        //Section_ID++;
                                    }
                                }
                                else
                                {
                                    if (C.Length != 0 && D.Length != 0 && G.Length != 0)
                                    {
                                        //if (tmp2 != "")
                                        //{
                                        if (G.LastOrDefault().ToString() == ",")
                                        {
                                            G = G + ((Excel.Range)range.Cells[row + 1, 7]).Text;
                                        }
                                        var CheckSection = db.SECTIONs.SqlQuery("SELECT * FROM SECTION WHERE SUBJECT_ID = '" + tmp + "' and " +
                                "SECTION_NUMBER = '" + tmp2 + "' and SECTION_DATE = '" + C + "' and SECTION_TIME_START = '" + split_date[0] + "' and SECTION_TIME_END = '" + split_date[1] + "' and SECTION_CLASSROOM = '" + E + "' " +
                                " and SECTION_PROFESSOR_SHORTNAME = '" + F + "' and SECTION_BRANCH_NAME = '" + G + "' and SEMESTER = '" + semester + "' and YEAR = '" + year + "'").Any();
                                        if (CheckSection == false)
                                        {
                                            string Subject_ID = tmp;
                                            string Section_Number = tmp2;
                                            string Section_Date = C;
                                            string Section_Start_Time = split_date[0];
                                            string Section_End_Time = split_date[1];
                                            string Section_Classroom = E;
                                            string Section_Professor = F;
                                            string Section_Branch_Name = G;
                                            saveSection(Subject_ID, Section_Number, Section_Date, Section_Start_Time, Section_End_Time, Section_Classroom, Section_Professor, Section_Branch_Name, semester, year, db);
                                            //Section_ID++;
                                        }
                                        //}
                                    }
                                }
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
                    return RedirectToAction("DSchedule", "TimeSchedule");
                }
                else
                {
                    ViewBag.Error = "File type is incorrect<br>";
                    return View("data");
                }
            }
        }

        public void saveSubject(string subject_ID, string subject_NAME, string subject_CREDIT, string SUBJECT_MIDTERM_DATE,
                                string SUBJECT_FINAL_DATE, string SUBJECT_MIDTERM_TIME, string SUBJECT_FINAL_TIME, string SEMESTER, string YEAR, TestExcelEntities db)
        {
            try
            {
                //Check exists
                var item = new SUBJECT();
                item.SUBJECT_ID = subject_ID;
                item.SUBJECT_NAME = subject_NAME;
                item.SUBJECT_CREDIT = subject_CREDIT;
                item.SUBJECT_MIDTERM_DATE = SUBJECT_MIDTERM_DATE;
                item.SUBJECT_FINAL_DATE = SUBJECT_FINAL_DATE;
                item.SUBJECT_MIDTERM_TIME = SUBJECT_MIDTERM_TIME;
                item.SUBJECT_FINAL_TIME = SUBJECT_FINAL_TIME;
                item.SEMESTER = SEMESTER;
                item.YEAR = YEAR;
                db.SUBJECTs.Add(item);
                db.SaveChanges();

            }
            catch
            {

            }
        }

        public void saveSection(string SUBJECT_ID, string SECTION_NUMBER, string SECTION_DATE, string SECTION_TIME_START, string SECTION_TIME_END,
                                string SECTION_CLASSROOM, string SECTION_PROFESSOR_SHORTNAME, string SECTION_BRANCH_NAME, string SEMESTER, string YEAR, TestExcelEntities db)
        {
            try
            {
                //Check exists
                var item = new SECTION();
                item.SUBJECT_ID = SUBJECT_ID;
                item.SECTION_NUMBER = SECTION_NUMBER;
                item.SECTION_DATE = SECTION_DATE;
                item.SECTION_TIME_START = double.Parse(SECTION_TIME_START);
                item.SECTION_TIME_END = double.Parse(SECTION_TIME_END);
                item.SECTION_CLASSROOM = SECTION_CLASSROOM;
                item.SECTION_PROFESSOR_SHORTNAME = SECTION_PROFESSOR_SHORTNAME;
                item.SECTION_BRANCH_NAME = SECTION_BRANCH_NAME;
                item.SEMESTER = SEMESTER;
                item.YEAR = YEAR;
                db.SECTIONs.Add(item);
                db.SaveChanges();

            }
            catch
            {

            }
        }
        [HttpPost]
        public ActionResult Export(FormCollection collection)
        {
            var semester = collection["semester"];
            var year = collection["year_export"];
            string FilePath = @"D:\ขบวน" + semester + "-" + year + ".xlsx";
            string FileName = Path.GetFileName(FilePath);
            Process[] excelProcsOld = Process.GetProcessesByName("EXCEL");
            if (db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).Any() != false)
            {
                try
                {
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    Excel.XlHAlign center = Excel.XlHAlign.xlHAlignCenter;

                    worksheet.Range["B:K"].Font.Name = "TH SarabunPSK";
                    worksheet.Range["B:K"].Font.Size = "15";
                    worksheet.Columns[2].NumberFormat = "@";
                    //-------------------------------------------//
                    var rangeA2 = worksheet.get_Range("A2", "K3");
                    rangeA2.Merge();
                    rangeA2.Value = "ขบวนวิชาที่เปิดสอนระดับปริญญาตรี";
                    rangeA2.Font.Name = "Angsana New";
                    rangeA2.Font.Size = "20";
                    rangeA2.Font.Bold = true;
                    rangeA2.Font.Underline = true;
                    rangeA2.HorizontalAlignment = center;
                    //-------------------------------------------//
                    var rangeA4 = worksheet.get_Range("A4", "K4");
                    rangeA4.Merge();
                    rangeA4.Value = "ภาคการศึกษาที่ " + semester + " ปีการศึกษา " + year;
                    rangeA4.Font.Name = "Angsana New";
                    rangeA4.Font.Size = "20";
                    rangeA4.Font.Bold = true;
                    rangeA4.Font.Underline = true;
                    rangeA4.HorizontalAlignment = center;
                    //-------------------------------------------//
                    worksheet.Columns[10].NumberFormat = "DD MMM YY";
                    //Excel.Range er = worksheet.get_Range(System.Type.Missing, "B:K");
                    //er.EntireColumn.Font.Name = "TH SarabunPSK";
                    //er.EntireColumn.Font.Size = "15";
                    int row = 5;
                    foreach (SUBJECT p in db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).ToList())
                    {
                        //-------------------------------------------//
                        var rangeheader = worksheet.get_Range("B" + row, "B2");
                        worksheet.Cells[row, 2] = p.SUBJECT_ID;
                        //-------------------------------------------//
                        var rangeheader2 = worksheet.get_Range("C" + row, "F" + row);
                        rangeheader2.Merge();
                        rangeheader2.Value = p.SUBJECT_NAME;
                        rangeheader2.NumberFormat = "@";
                        //-------------------------------------------//
                        worksheet.Cells[row, 7] = p.SUBJECT_CREDIT;

                        var midtermcheck = p.SUBJECT_MIDTERM_DATE.Any();
                        var finalcheck = p.SUBJECT_FINAL_DATE.Any();
                        if (midtermcheck == true && finalcheck == true)
                        {
                            worksheet.Cells[row, 9] = "Mid";
                            worksheet.Cells[row + 1, 9] = "Final";
                            worksheet.Cells[row, 10] = p.SUBJECT_MIDTERM_DATE;
                            worksheet.Cells[row, 11] = p.SUBJECT_MIDTERM_TIME;
                            worksheet.Cells[row + 1, 10] = p.SUBJECT_FINAL_DATE;
                            worksheet.Cells[row + 1, 11] = p.SUBJECT_FINAL_TIME;
                        }
                        else if (midtermcheck == true && finalcheck == false)
                        {
                            worksheet.Cells[row, 9] = "Mid";
                            worksheet.Cells[row, 10] = p.SUBJECT_MIDTERM_DATE;
                            worksheet.Cells[row, 11] = p.SUBJECT_MIDTERM_TIME;
                        }
                        else if (midtermcheck == false && finalcheck == true)
                        {
                            worksheet.Cells[row, 9] = "Final";
                            worksheet.Cells[row, 10] = p.SUBJECT_FINAL_DATE;
                            worksheet.Cells[row, 11] = p.SUBJECT_FINAL_TIME;
                        }
                        row++;
                        foreach (SECTION r in db.SECTIONs.Where(x => x.SUBJECT_ID == p.SUBJECT_ID && x.SEMESTER == semester && x.YEAR == year).ToList())
                        {
                            worksheet.Cells[row, 2] = r.SECTION_NUMBER;
                            worksheet.Cells[row, 3] = r.SECTION_DATE;
                            //float number = 17.3f;
                            //string aa = Convert.ToDecimal(number).ToString("#,###.00");
                            worksheet.Cells[row, 4] = Convert.ToDecimal(r.SECTION_TIME_START).ToString("0#.00") + "-" + Convert.ToDecimal(r.SECTION_TIME_END).ToString("0#.00");
                            worksheet.Cells[row, 5] = r.SECTION_CLASSROOM;
                            worksheet.Cells[row, 6] = r.SECTION_PROFESSOR_SHORTNAME;
                            worksheet.Cells[row, 7] = r.SECTION_BRANCH_NAME;
                            row++;
                        }
                        row++;
                    }

                    workbook.SaveAs(FilePath);
                    workbook.Close();

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.Flush();
                    Response.End();

                  
                    Marshal.ReleaseComObject(workbook);

                    application.Quit();
                    Marshal.FinalReleaseComObject(application);
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
                System.IO.File.Delete(FilePath);
                return View("Close");
            }
            else
            {
                return View("Close");
            }
        }
    }
}