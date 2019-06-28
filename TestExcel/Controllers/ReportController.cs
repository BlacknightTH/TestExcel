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
using System.Text;

namespace TestExcel.Controllers
{
    [adminauthen]
    public class ReportController : Controller
    {
        string[] date = { "วันจันทร์", "วันอังคาร", "วันพุธ", "วันพฤหัสบดี", "วันศุกร์", "วันเสาร์" };
        TestExcelEntities db = new TestExcelEntities();
        List<DEPARTMENT> _DEPARTMENT = new List<DEPARTMENT>();
        // GET: PdfExport
        public void SetYear()
        {
            var semesteryear = (from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                                select new SemesterYear
                                {
                                    SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                    SEMESTER = d1.SEMESTER,
                                    YEAR = d1.YEAR
                                }).OrderByDescending(x => x.YEAR).OrderByDescending(x => x.SEMESTER);
            var model = db.DEPARTMENTs.ToList();
            _DEPARTMENT = model;
            string first_Year;
            first_Year = semesteryear.FirstOrDefault().YEAR;
            ViewBag.Semester = semesteryear.FirstOrDefault().SEMESTER;
            ViewBag.ddl_Year = new SelectList(semesteryear, "YEAR", "YEAR", first_Year);
        }
        public ActionResult data(string ErrorMessage)
        {
            string m = ErrorMessage;
            SetYear();

            List<DATE> DATE = new List<DATE>();

            string FilePath = Server.MapPath("~/Content/import/fin/");
            foreach (string f in Directory.GetFiles(FilePath))
            {
                string FileName = Path.GetFileNameWithoutExtension(f);
                var split = FileName.Split(' ', '-');
                var item = new DATE();
                item.DAY = int.Parse(split[2]);
                item.MONTH = int.Parse(split[3]);
                item.YEAR = int.Parse(split[4]);
                item.EXCEL_DATE = FileName;
                DATE.Add(item);
            }
            ViewBag.DATE = DATE.OrderByDescending(x => x.DAY).OrderByDescending(x => x.MONTH).OrderByDescending(x => x.YEAR).ToList();
            return View(_DEPARTMENT);
        }
        [HttpPost]
        public ActionResult Report(FormCollection collection)
        {
            int department_id = int.Parse(collection["department"]);
            string semester = collection["semester"];
            string year = collection["year"];
            var department = "";
            if (department_id != 0)
            {
                department = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == department_id).First().DEPARTMENT_NAME.Trim();
            }
            string FilePath = @"C:\\รายการลงทะเบียนเรียน_" + semester + "-" + year + "_" + department + ".pdf";
            string FileName = Path.GetFileName(FilePath);
            PdfReport pdfReport = new PdfReport();
            try
            {
                LogFile("ดาวน์โหลดไฟล์ -> รายการลงทะเบียนเรียน");
                byte[] abytes = pdfReport.PrepareReport(department_id, semester, year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                SetYear();
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "ขออภัยไม่มีข้อมูลของ ภาคการศึกษา/ปีการศึกษา ที่เลือก";
                return View("data", _DEPARTMENT);
            }
        }
        public ActionResult PfReport(FormCollection collection)
        {
            int department_id = int.Parse(collection["department"]);
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            try
            {
                var department = "";
                if (department_id != 0)
                {
                    department = db.DEPARTMENTs.Where(x => x.DEPARTMENT_ID == department_id).First().DEPARTMENT_NAME.Trim();
                }
                string FilePath = @"C:\\ภาระการสอน_" + department + "_" + semester + "-" + year + ".pdf";
                string FileName = Path.GetFileName(FilePath);
                byte[] abytes = pdfReport.PfPrepareReport(department, semester, year);
                //return File(abytes, "application/pdf");
                LogFile("ดาวน์โหลดไฟล์ -> ภาระการสอน_" + department + "_" + semester + "-" + year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                SetYear();
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "ขออภัยไม่มีข้อมูลของ ภาคการศึกษา/ปีการศึกษา ที่เลือก";
                return View("data", _DEPARTMENT);
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
                LogFile("ดาวน์โหลดไฟล์ -> ตารางการใช้ห้องเรียน " + date[Date] + "_" + semester + "-" + year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                SetYear();
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "ขออภัยไม่มีข้อมูลของ ภาคการศึกษา/ปีการศึกษา ที่เลือก";
                return View("data", _DEPARTMENT);
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
                LogFile("ดาวน์โหลดไฟล์ -> ตารางการใช้ห้องเรียน_" + BUILDING + "_" + semester + "-" + year);
                return File(abytes, "application/pdf", FileName);
            }
            catch
            {
                SetYear();
                ViewBag.Message = "";
                ViewBag.ErrorMessage = "ขออภัยไม่มีข้อมูลของ ภาคการศึกษา/ปีการศึกษา ที่เลือก";
                return View("data", _DEPARTMENT);
            }
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase excelfile)
        {
            if (Session["status"].ToString() == "admin")
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
                        string path = Server.MapPath("~/Content/import/Upload/" + excelfile.FileName);
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
                            string Semester_Year = worksheet.Cells[4, 1].Text;
                            string[] split_semester_year = Semester_Year.Split(' ');
                            string semester = split_semester_year[1];
                            string year = split_semester_year[3];
                            //string semester = tmpstring[0];
                            //string year = tmpstring[1];
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

                            for (int row = 5; row <= totalRows; row++)
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
                                                string subject_Mid_fin = worksheet.Cells[row, 9].Text;
                                                string subject_MIDTERM_DATE = "", subject_FINAL_DATE = "", subject_MIDTERM_TIME = "", subject_FINAL_TIME = "";
                                                if (subject_Mid_fin == "Mid")
                                                {
                                                    subject_MIDTERM_DATE = worksheet.Cells[row, 10].Text;
                                                    subject_FINAL_DATE = worksheet.Cells[row + 1, 10].Text;
                                                    subject_MIDTERM_TIME = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row + 1, 11].Text;
                                                }
                                                else if(subject_Mid_fin == "Final")
                                                {
                                                    subject_FINAL_DATE = worksheet.Cells[row, 10].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row, 11].Text;
                                                }


                                                saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                            }
                                            else
                                            {
                                                string Subject_ID = B;
                                                string subject_NAME = C;
                                                string subject_CREDIT = G;
                                                string subject_Mid_fin = worksheet.Cells[row, 10].Text;
                                                string subject_MIDTERM_DATE = "", subject_FINAL_DATE = "", subject_MIDTERM_TIME = "", subject_FINAL_TIME = "";
                                                if (subject_Mid_fin == "Mid")
                                                {
                                                    subject_MIDTERM_DATE = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_DATE = worksheet.Cells[row + 1, 11].Text;
                                                    subject_MIDTERM_TIME = worksheet.Cells[row, 12].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row + 1, 12].Text;
                                                }
                                                else if (subject_Mid_fin == "Final")
                                                {
                                                    subject_FINAL_DATE = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row, 12].Text;
                                                }


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
                                                string subject_Mid_fin = worksheet.Cells[row, 9].Text;
                                                string subject_MIDTERM_DATE = "", subject_FINAL_DATE = "", subject_MIDTERM_TIME = "", subject_FINAL_TIME = "";
                                                if (subject_Mid_fin == "Mid")
                                                {
                                                    subject_MIDTERM_DATE = worksheet.Cells[row, 10].Text;
                                                    subject_FINAL_DATE = worksheet.Cells[row + 1, 10].Text;
                                                    subject_MIDTERM_TIME = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row + 1, 11].Text;
                                                }
                                                else if (subject_Mid_fin == "Final")
                                                {
                                                    subject_FINAL_DATE = worksheet.Cells[row, 10].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row, 11].Text;
                                                }

                                                saveSubject(Subject_ID, subject_NAME, subject_CREDIT, subject_MIDTERM_DATE, subject_FINAL_DATE, subject_MIDTERM_TIME, subject_FINAL_TIME, semester, year, db);
                                            }
                                            else
                                            {
                                                string Subject_ID = B;
                                                string subject_NAME = C;
                                                string subject_CREDIT = H.Trim();
                                                string subject_Mid_fin = worksheet.Cells[row, 10].Text;
                                                string subject_MIDTERM_DATE = "", subject_FINAL_DATE = "", subject_MIDTERM_TIME = "", subject_FINAL_TIME = "";
                                                if (subject_Mid_fin == "Mid")
                                                {
                                                    subject_MIDTERM_DATE = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_DATE = worksheet.Cells[row + 1, 11].Text;
                                                    subject_MIDTERM_TIME = worksheet.Cells[row, 12].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row + 1, 12].Text;
                                                }
                                                else if (subject_Mid_fin == "Final")
                                                {
                                                    subject_FINAL_DATE = worksheet.Cells[row, 11].Text;
                                                    subject_FINAL_TIME = worksheet.Cells[row, 12].Text;
                                                }

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
                                        saveProfessor(Section_Professor, db);
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
                                                saveProfessor(Section_Professor, db);
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
                                                saveProfessor(Section_Professor, db);
                                            }
                                        }
                                    }
                                }
                            }
                            Export(semester, year);
                        }
                        catch
                        {

                        }
                        SetYear();
                        ViewBag.Message = "อัปโหลดไฟล์ " + excelfile.FileName + " เสร็จสิ้น";
                        ViewBag.ErrorMessage = "";
                        LogFile("อัปโหลดไฟล์ Excel " + excelfile.FileName);

                        List<DATE> DATE = new List<DATE>();

                        string FilePath = Server.MapPath("~/Content/import/fin/");
                        foreach (string f in Directory.GetFiles(FilePath))
                        {
                            string FileName = Path.GetFileNameWithoutExtension(f);
                            var split = FileName.Split(' ', '-');
                            var item = new DATE();
                            item.DAY = int.Parse(split[2]);
                            item.MONTH = int.Parse(split[3]);
                            item.YEAR = int.Parse(split[4]);
                            item.EXCEL_DATE = FileName;
                            DATE.Add(item);
                        }
                        ViewBag.DATE = DATE.OrderByDescending(x => x.DAY).OrderByDescending(x => x.MONTH).OrderByDescending(x => x.YEAR).ToList();

                        return View("data", _DEPARTMENT);
                    }
                    else
                    {
                        SetYear();
                        ViewBag.Message = "";
                        ViewBag.ErrorMessage = "ชนิดของไฟล์ไม่ถูกต้อง กรุณาอัปโหลดไฟล์ .xlsx";

                        List<DATE> DATE = new List<DATE>();

                        string FilePath = Server.MapPath("~/Content/import/fin/");
                        foreach (string f in Directory.GetFiles(FilePath))
                        {
                            string FileName = Path.GetFileNameWithoutExtension(f);
                            var split = FileName.Split(' ', '-');
                            var item = new DATE();
                            item.DAY = int.Parse(split[2]);
                            item.MONTH = int.Parse(split[3]);
                            item.YEAR = int.Parse(split[4]);
                            item.EXCEL_DATE = FileName;
                            DATE.Add(item);
                        }
                        ViewBag.DATE = DATE.OrderByDescending(x => x.DAY).OrderByDescending(x => x.MONTH).OrderByDescending(x => x.YEAR).ToList();

                        return View("data", _DEPARTMENT);
                    }
                }
            }
            else
            {

                List<DATE> DATE = new List<DATE>();

                string FilePath = Server.MapPath("~/Content/import/fin/");
                foreach (string f in Directory.GetFiles(FilePath))
                {
                    string FileName = Path.GetFileNameWithoutExtension(f);
                    var split = FileName.Split(' ', '-');
                    var item = new DATE();
                    item.DAY = int.Parse(split[2]);
                    item.MONTH = int.Parse(split[3]);
                    item.YEAR = int.Parse(split[4]);
                    item.EXCEL_DATE = FileName;
                    DATE.Add(item);
                }
                ViewBag.DATE = DATE.OrderByDescending(x => x.DAY).OrderByDescending(x => x.MONTH).OrderByDescending(x => x.YEAR).ToList();

                return RedirectToAction("data");
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
                    for (int i = 0; i < tmp.Length; i++)
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
        //[HttpPost]
        //public ActionResult Export(FormCollection collection)
        //{
        //    var semester = collection["semester"];
        //    var year = collection["year_export"];
        //    var datetime = DateTime.Now.ToShortDateString().Replace('/', '-');
        //    string FilePath = Server.MapPath("~/Content/import/fin/ขบวน" + semester + "-" + year + " " + datetime + ".xlsx");
        //    System.IO.File.Delete(FilePath);
        //    string FileName = Path.GetFileName(FilePath);

        //    if (db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).Any() != false)
        //    {
        //        try
        //        {
        //            var package = new ExcelPackage(new FileInfo(FilePath));
        //            var workbook = package.Workbook;
        //            var worksheet = workbook.Worksheets.Add("ขบวน"+ semester + "-" + year); //read sheet 1

        //            //------------------------------------------------------//
        //            worksheet.Cells["B:K"].Style.Font.Name = "TH SarabunPSK";
        //            worksheet.Cells["B:K"].Style.Font.Size = 15;
        //            worksheet.Column(2).Style.Numberformat.Format = "@";
        //            //------------------------------------------------------//

        //            //-------------------------------------------//
        //            using (ExcelRange range = worksheet.Cells["A2:K3"])
        //            {
        //                range.Merge = true;
        //                range.Value = "ขบวนวิชาที่เปิดสอนระดับปริญญาตรี";
        //                range.Style.Font.Name = "Angsana New";
        //                range.Style.Font.Size = 20;
        //                range.Style.Font.Bold = true;
        //                range.Style.Font.UnderLine = true;
        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            }
        //            //-------------------------------------------//
        //            using (ExcelRange range = worksheet.Cells["A4:K4"])
        //            {
        //                range.Merge = true;
        //                range.Value = "ภาคการศึกษาที่ " + semester + " ปีการศึกษา " + year;
        //                range.Style.Font.Name = "Angsana New";
        //                range.Style.Font.Size = 20;
        //                range.Style.Font.Bold = true;
        //                range.Style.Font.UnderLine = true;
        //                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            }
        //            //-------------------------------------------//
        //            worksheet.Column(10).Style.Numberformat.Format = "DD MMM YY";
        //            worksheet.Column(2).Width = 12f;
        //            worksheet.Column(4).Width = 13f;
        //            worksheet.Column(5).Width = 16f;
        //            worksheet.Column(6).Width = 21f;
        //            worksheet.Column(7).Width = 30f;
        //            worksheet.Column(11).Width = 11f;
        //            int row = 5;
        //            foreach (SUBJECT p in db.SUBJECTs.Where(x => x.SEMESTER == semester && x.YEAR == year).ToList())
        //            {
        //                //-------------------------------------------//
        //                worksheet.Cells[row, 2].Value = p.SUBJECT_ID;
        //                worksheet.Cells[row, 3].Value = p.SUBJECT_NAME;
        //                worksheet.Cells[row, 7].Value = p.SUBJECT_CREDIT;
        //                //-------------------------------------------//
        //                var midtermcheck = p.SUBJECT_MIDTERM_DATE.Any();
        //                var finalcheck = p.SUBJECT_FINAL_DATE.Any();
        //                if (midtermcheck == true && finalcheck == true)
        //                {
        //                    worksheet.Cells[row, 9].Value = "Mid";
        //                    worksheet.Cells[row + 1, 9].Value = "Final";
        //                    worksheet.Cells[row, 10].Value = p.SUBJECT_MIDTERM_DATE;
        //                    worksheet.Cells[row, 11].Value = p.SUBJECT_MIDTERM_TIME;
        //                    worksheet.Cells[row + 1, 10].Value = p.SUBJECT_FINAL_DATE;
        //                    worksheet.Cells[row + 1, 11].Value = p.SUBJECT_FINAL_TIME;
        //                }
        //                else if (midtermcheck == true && finalcheck == false)
        //                {
        //                    worksheet.Cells[row, 9].Value = "Mid";
        //                    worksheet.Cells[row, 10].Value = p.SUBJECT_MIDTERM_DATE;
        //                    worksheet.Cells[row, 11].Value = p.SUBJECT_MIDTERM_TIME;
        //                }
        //                else if (midtermcheck == false && finalcheck == true)
        //                {
        //                    worksheet.Cells[row, 9].Value = "Final";
        //                    worksheet.Cells[row, 10].Value = p.SUBJECT_FINAL_DATE;
        //                    worksheet.Cells[row, 11].Value = p.SUBJECT_FINAL_TIME;
        //                }
        //                row++;
        //                foreach (SECTION r in db.SECTIONs.Where(x => x.SUBJECT_ID == p.SUBJECT_ID && x.SEMESTER == semester && x.YEAR == year).ToList())
        //                {
        //                    worksheet.Cells[row, 2].Value = r.SECTION_NUMBER;
        //                    worksheet.Cells[row, 3].Value = r.SECTION_DATE;
        //                    worksheet.Cells[row, 4].Value = Convert.ToDecimal(r.SECTION_TIME_START).ToString("0#.00") + "-" + Convert.ToDecimal(r.SECTION_TIME_END).ToString("0#.00");
        //                    worksheet.Cells[row, 5].Value = r.SECTION_CLASSROOM;
        //                    worksheet.Cells[row, 6].Value = r.SECTION_PROFESSOR_SHORTNAME;
        //                    worksheet.Cells[row, 7].Value = r.SECTION_BRANCH_NAME;
        //                    row++;
        //                }
        //                row++;
        //            }
        //            //-------------------------------------------//
        //            package.SaveAs(new FileInfo(FilePath));
        //            byte[] fileBytes = System.IO.File.ReadAllBytes(FilePath);
        //            LogFile("ดาวน์โหลดไฟล์ Excel " + FileName);
        //            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
        //        }
        //        catch
        //        {
        //            SetYear();
        //            ViewBag.Message = "";
        //            ViewBag.ErrorMessage = "ขออภัยมีข้อผิดพลาดเกิดขึ้นกรุณาติดต่อ ADMIN";
        //            return View("data", _DEPARTMENT);
        //        }
        //    }
        //    else
        //    {
        //        SetYear();
        //        ViewBag.Message = "";
        //        ViewBag.ErrorMessage = "ขออภัยไม่มีข้อมูลของ ภาคการศึกษา/ปีการศึกษา ที่เลือก";
        //        return View("data", _DEPARTMENT);
        //    }
        //}
        public void Export(string semester, string year)
        {
            var datetime = DateTime.Now.ToShortDateString().Replace('/', '-');
            string FilePath = Server.MapPath("~/Content/import/fin/ขบวน" + semester + "-" + year + " " + datetime + " ไฟล์ตั้งต้น.xlsx");
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
        [HttpPost]
        public FileResult Export(FormCollection collection)
        {
            string excelfile = collection["date"];
            string path = Server.MapPath("~/Content/import/fin/" + excelfile + ".xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            LogFile("ดาวน์โหลดไฟล์ Excel " + excelfile + ".xlsx");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelfile + ".xlsx");
        }
        public FileResult Download(string id)
        {
            string path = Server.MapPath("~/Content/import/" + id + ".pdf");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/pdf", id + ".pdf");
        }
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
        public ActionResult Log()
        {
            List<DATE> DATE = new List<DATE>();
            string line;
            try
            {
                string FilePath = Server.MapPath("~/LogFile/");
                foreach (string f in Directory.GetFiles(FilePath))
                {
                    string FileName = Path.GetFileNameWithoutExtension(f);
                    var split = FileName.Split(' ', '-');
                    var item = new DATE();
                    item.DAY = int.Parse(split[1]);
                    item.MONTH = int.Parse(split[2]);
                    item.YEAR = int.Parse(split[3]);
                    DATE.Add(item);
                }

                string Year = DATE.Select(x => x.YEAR).Distinct().OrderByDescending(x => x).First().ToString();
                string Month = DATE.OrderByDescending(x => x.MONTH).First().MONTH.ToString();
                string Day = DATE.Where(x => x.MONTH.ToString() == Month).Select(x => x.DAY).Distinct().OrderByDescending(x => x).First().ToString();
                ViewBag.Year = Year;
                ViewBag.Month = Month;
                ViewBag.Day = Day;

                string read = "";
                StreamReader sr = System.IO.File.OpenText(FilePath + "Log " + Day + "-" + Month + "-" + Year + ".txt");
                while ((line = sr.ReadLine()) != null)
                {
                    read = read + "\n" + line;
                }
                sr.Close();

                ViewBag.READ = read;
            }
            catch
            {

            }
            return View(DATE);
        }
        [HttpPost]
        public ActionResult Log(FormCollection collection)
        {
            List<DATE> DATE = new List<DATE>();
            string DAY = collection["ddl_Day"];
            string MONTH = collection["ddl_Month"];
            string YEAR = collection["ddl_Year"];
            int Count = int.Parse(collection["Count"]);
            string line;
            try
            {
                string FilePath = Server.MapPath("~/LogFile/");
                foreach (string f in Directory.GetFiles(FilePath))
                {
                    string FileName = Path.GetFileNameWithoutExtension(f);
                    var split = FileName.Split(' ', '-');
                    var item = new DATE();
                    item.DAY = int.Parse(split[1]);
                    item.MONTH = int.Parse(split[2]);
                    item.YEAR = int.Parse(split[3]);
                    DATE.Add(item);
                }

                if (Count == 1)
                {
                    ViewBag.Year = YEAR;
                    ViewBag.Month = MONTH;
                    ViewBag.Day = DATE.Where(x => x.MONTH.ToString() == MONTH).Select(x => x.DAY).Distinct().OrderByDescending(x => x).First().ToString();
                }
                else
                {
                    ViewBag.Year = YEAR;
                    ViewBag.Month = MONTH;
                    ViewBag.Day = DAY;
                }

                string read = "";
                StreamReader sr = System.IO.File.OpenText(FilePath + "Log " + ViewBag.Day + "-" + ViewBag.Month + "-" + ViewBag.Year + ".txt");
                while ((line = sr.ReadLine()) != null)
                {
                    read = read + "\n" + line;
                }
                sr.Close();

                ViewBag.READ = read;
            }
            catch
            {

            }
            return View(DATE);
        }
    }
}