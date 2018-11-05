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

namespace TestExcel.Controllers
{
    public class PdfExportController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: PdfExport
        public ActionResult Index()
        {
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
            return File(abytes, "application/pdf");
        }
        public ActionResult TeReport(FormCollection collection)
        {
            int Date = int.Parse(collection["DDL_DATE"]);
            string semester = collection["semester"];
            string year = collection["year"];
            PdfReport pdfReport = new PdfReport();
            byte[] abytes = pdfReport.TePrepareReport(Date,semester, year);
            return File(abytes, "application/pdf");
        }
    }
}