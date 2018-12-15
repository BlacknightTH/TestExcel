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
using OfficeOpenXml;

namespace TestExcel.Controllers
{
    //[adminauthen]
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
            var model = db.DEPARTMENTs.ToList();
            string first_Year;
            first_Year = "2560";
            ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", first_Year);
            ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_NAME", "DEPARTMENT_NAME");
            return View(model);
        }
        [HttpPost]
        public ActionResult Report(FormCollection collection)
        {
            string department_name = collection["department_name"];
            string semester = collection["semester"];
            string year = collection["year"];
            string FilePath = @"C:\\รายการลงทะเบียนเรียน_" + semester + "-" + year + "_" + department_name + ".pdf";
            string FileName = Path.GetFileName(FilePath);
            PdfReport pdfReport = new PdfReport();
            try
            {
                byte[] abytes = pdfReport.PrepareReport(department_name, semester, year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                return RedirectToAction("data");
            }
        }
        public ActionResult PfReport(FormCollection collection)
        {
            string department = collection["department"];
            string semester = collection["semester"];
            string year = collection["year"];
            string FilePath = @"C:\\ภาระการสอน_" + semester + "-" + year + ".pdf";
            string FileName = Path.GetFileName(FilePath);
            PdfReport pdfReport = new PdfReport();
            try
            {
                byte[] abytes = pdfReport.PfPrepareReport(department, semester, year);
                //return File(abytes, "application/pdf");
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                return RedirectToAction("data");
            }
        }
        [HttpPost]
        public ActionResult TeReport(FormCollection collection)
        {
            int Date = int.Parse(collection["DDL_DATE"]);
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            string FilePath = @"C:\\" + "ตารางการใช้ห้องเรียน_" + date[Date] + "_" + semester + "-" + year + ".pdf";
            string FileName = Path.GetFileName(FilePath);
            try
            {
                byte[] abytes = pdfReport.TePrepareReport(Date, semester, year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                return RedirectToAction("data");
            }
        }
        [HttpPost]
        public ActionResult ClReport(FormCollection collection)
        {
            string BUILDING = collection["DDL_BUILDING"];
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            try
            {
                byte[] abytes = pdfReport.ClPrepareReport(BUILDING, semester, year);
                if (BUILDING == "632")
                {
                    BUILDING = "อาคารเรียน " + BUILDING + " (อาคารเรียนสีเทา ตึกใหม่)";
                }
                else if (BUILDING == "1")
                {
                    BUILDING = "อื่นๆ";
                }
                else
                {
                    BUILDING = "อาคารเรียน " + BUILDING;
                }
                string FilePath = @"C:\\" + "ตารางการใช้ห้องเรียน_" + BUILDING + "_" + semester + "-" + year + ".pdf";
                string FileName = Path.GetFileName(FilePath);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                return RedirectToAction("data");
            }
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select a excel file<br>";
                return RedirectToAction("data");
            }
            else
            {
                if ((excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx")))
                {
                    string path = Server.MapPath("~/Content/import/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    try
                    {
                        int FilenameLength = excelfile.FileName.Length;
                        FilenameLength = FilenameLength - 11;
                        string FileName = excelfile.FileName.Substring(FilenameLength);
                        string[] tmpstring = FileName.Split('.', '-');

                        var package = new ExcelPackage(new FileInfo(path));
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets[1]; //read sheet 1
                        int totalRows = worksheet.Dimension.End.Row;

                        string tmp = "";
                        string tmp2 = "";

                        string semester = tmpstring[0];
                        string year = tmpstring[1];
                        var check_subject_semester_year = db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year);
                        var check_section_semester_year = db.SECTIONs.Where(x => x.SEMESTER == semester && x.YEAR == year);
                        if (check_subject_semester_year.Any() == true)
                        {
                            for (int g = check_subject_semester_year.ToList().FirstOrDefault().ID; g <= check_subject_semester_year.ToList().Last().ID; g++)
                            {
                                var record = db.SUBJECTs.Find(g);
                                db.SUBJECTs.Remove(record);
                            }
                            db.SaveChanges();
                        }
                        if (check_section_semester_year.Any() == true)
                        {
                            for (int g = check_section_semester_year.ToList().FirstOrDefault().SECTION_ID; g <= check_section_semester_year.ToList().LastOrDefault().SECTION_ID; g++)
                            {
                                var record = db.SECTIONs.Where(x => x.SECTION_ID == g).First();
                                db.SECTIONs.Remove(record);
                            }
                            db.SaveChanges();
                        }

                        for (int row = 1; row < totalRows; row++)
                        {
                            string B = worksheet.Cells[row, 2].Text;
                            string C = worksheet.Cells[row, 3].Text;
                            string D = worksheet.Cells[row, 4].Text;
                            string E = worksheet.Cells[row, 5].Text;
                            string F = worksheet.Cells[row, 6].Text;
                            string G = worksheet.Cells[row, 7].Text;
                            string H = worksheet.Cells[row, 8].Text;
                            string L = worksheet.Cells[row, 12].Text;

                            if (B.Length > 4 && B.Length < 12)
                            {
                                tmp = B;
                                var CheckSubject = db.SUBJECTs.Where(x => x.SUBJECT_ID == B && x.SEMESTER == semester && x.YEAR == year).Any();
                                if (CheckSubject != true)
                                {
                                    if (G.Length != 0)
                                    {
                                        if (L.Length == 0)
                                        {
                                            string Subject_ID = B;
                                            string subject_NAME = C;
                                            string subject_CREDIT = G;
                                            string subject_MIDTERM_DATE = worksheet.Cells[row, 10].Text;
                                            string subject_FINAL_DATE = worksheet.Cells[row + 1, 10].Text;
                                            string subject_MIDTERM_TIME = worksheet.Cells[row, 11].Text;
                                            string subject_FINAL_TIME = worksheet.Cells[row + 1, 11].Text;

                                            saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                        }
                                        else
                                        {
                                            string Subject_ID = B;
                                            string subject_NAME = C;
                                            string subject_CREDIT = G;
                                            string subject_MIDTERM_DATE = worksheet.Cells[row, 11].Text;
                                            string subject_FINAL_DATE = worksheet.Cells[row + 1, 11].Text;
                                            string subject_MIDTERM_TIME = worksheet.Cells[row, 12].Text;
                                            string subject_FINAL_TIME = worksheet.Cells[row + 1, 12].Text;

                                            saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                        }
                                    }
                                    else
                                    {
                                        if (L.Length == 0)
                                        {
                                            string Subject_ID = B;
                                            string subject_NAME = C;
                                            string subject_CREDIT = H.Trim();
                                            string subject_MIDTERM_DATE = worksheet.Cells[row, 10].Text;
                                            string subject_FINAL_DATE = worksheet.Cells[row + 1, 10].Text;
                                            string subject_MIDTERM_TIME = worksheet.Cells[row, 11].Text;
                                            string subject_FINAL_TIME = worksheet.Cells[row + 1, 11].Text;

                                            saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                        }
                                        else
                                        {
                                            string Subject_ID = B;
                                            string subject_NAME = C;
                                            string subject_CREDIT = H.Trim();
                                            string subject_MIDTERM_DATE = worksheet.Cells[row, 11].Text;
                                            string subject_FINAL_DATE = worksheet.Cells[row + 1, 11].Text;
                                            string subject_MIDTERM_TIME = worksheet.Cells[row, 12].Text;
                                            string subject_FINAL_TIME = worksheet.Cells[row + 1, 12].Text;

                                            saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                        }
                                    }
                                }
                            }
                            else if (B.Length <= 4)
                            {
                                tmp2 = B;
                                if (B.Length != 0)
                                {
                                    string[] split_date = D.Split('-');
                                    if (G.LastOrDefault().ToString() == ",")
                                    {
                                        G = G + worksheet.Cells[row + 1, 7].Text;
                                    }

                                    string Subject_ID = tmp;
                                    string Section_Number = B;
                                    string Section_Date = C;
                                    string Section_Start_Time = split_date[0];
                                    string Section_End_Time = split_date[1];
                                    string Section_Classroom = E;
                                    string Section_Professor = F;
                                    string Section_Branch_Name = G;
                                    saveSection(Subject_ID, Section_Number, Section_Date, Section_Start_Time, Section_End_Time, Section_Classroom, Section_Professor, Section_Branch_Name, semester, year, db);
                                    saveProfessor(Section_Professor,db);
                                }
                                else
                                {
                                    if (D.Length != 0 && E.Length != 0 && G.Length != 0)
                                    {
                                        if (C == "M" || C == "T" || C == "W" || C == "H" || C == "F" || C == "S" || C == "SUN")
                                        {
                                            string[] split_date = D.Split('-');
                                            if (G.LastOrDefault().ToString() == ",")
                                            {
                                                G = G + worksheet.Cells[row + 1, 7].Text;
                                            }

                                            string Subject_ID = tmp;
                                            string Section_Number = tmp2;
                                            string Section_Date = C;
                                            string Section_Start_Time = split_date[0];
                                            string Section_End_Time = split_date[1];
                                            string Section_Classroom = E;
                                            string Section_Professor = F;
                                            string Section_Branch_Name = G;
                                            saveSection(Subject_ID, Section_Number, Section_Date, Section_Start_Time, Section_End_Time, Section_Classroom, Section_Professor, Section_Branch_Name, semester, year, db);
                                            saveProfessor(Section_Professor,db);
                                        }
                                        else if (D == "M" || D == "T" || D == "W" || D == "H" || D == "F" || D == "S" || D == "SUN")
                                        {
                                            string[] split_date = E.Split('-');
                                            if (H.LastOrDefault().ToString() == ",")
                                            {
                                                H = H + worksheet.Cells[row + 1, 7].Text;
                                            }

                                            string Subject_ID = tmp;
                                            string Section_Number = C;
                                            string Section_Date = D;
                                            string Section_Start_Time = split_date[0];
                                            string Section_End_Time = split_date[1];
                                            string Section_Classroom = F;
                                            string Section_Professor = G;
                                            string Section_Branch_Name = H;
                                            saveSection(Subject_ID, Section_Number, Section_Date, Section_Start_Time, Section_End_Time, Section_Classroom, Section_Professor, Section_Branch_Name, semester, year, db);
                                            saveProfessor(Section_Professor,db);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    return RedirectToAction("DSchedule", "TimeSchedule");
                }
                else
                {
                    ViewBag.Error = "File type is incorrect<br>";
                    return RedirectToAction("data");
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
        public void saveProfessor(string SECTION_PROFESSOR_SHORTNAME, TestExcelEntities db)
        {
            try
            {
            var item2 = new PROFESSOR();
            var model = db.PROFESSORs;
                if (SECTION_PROFESSOR_SHORTNAME.Contains("/"))
                {
                    string[] tmp = SECTION_PROFESSOR_SHORTNAME.Split('/');
                    for (int i = 0; i < tmp.Length ; i++) 
                    {
                        model.Where(x => x.PROFESSOR_SHORTNAME == tmp[i]);
                        if (model.Count() == 0)
                        {
                            item2.PROFESSOR_SHORTNAME = tmp[i];
                            db.PROFESSORs.Add(item2);
                        }
                    }
                }
                else
                {
                    model.Where(x => x.PROFESSOR_SHORTNAME == SECTION_PROFESSOR_SHORTNAME);
                    if (model.Count() == 0)
                    {
                        item2.PROFESSOR_SHORTNAME = SECTION_PROFESSOR_SHORTNAME;
                        db.PROFESSORs.Add(item2);
                    }
                }
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
            string FilePath = Server.MapPath("~/Content/import/fin/ขบวน" + semester + "-" + year + ".xlsx");
            System.IO.File.Delete(FilePath);
            string FileName = Path.GetFileName(FilePath);

            if (db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).Any() != false)
            {
                try
                {
                    var package = new ExcelPackage(new FileInfo(FilePath));
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.Add("ขบวน"+ semester + "-" + year); //read sheet 1
                  
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
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                }
                catch
                {
                    return RedirectToAction("data");
                }
            }
            else
            {
                //var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                //                   select new SemesterYear
                //                   {
                //                       SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                //                       SEMESTER = d1.SEMESTER,
                //                       YEAR = d1.YEAR
                //                   };
                //var first_Year = "2560";
                //ViewBag.ddl_Year = new SelectList(semesteryear.OrderBy(x => x.YEAR), "YEAR", "YEAR", first_Year);
                //ViewBag.ddl_Department = new SelectList(db.DEPARTMENTs.ToList(), "DEPARTMENT_NAME", "DEPARTMENT_NAME");
                return RedirectToAction("data");
            }
        }

        public FileResult Download(string id)
        {
            string path = Server.MapPath("~/Content/import/" + id + ".pdf");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/pdf", id);
        }
    }
}