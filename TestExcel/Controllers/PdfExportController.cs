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

namespace TestExcel.Controllers
{
    public class PdfExportController : Controller
    {
        // GET: PdfExport
        public ActionResult Report(TestExcelEntities db)
        {
            PdfReport pdfReport = new PdfReport();
            byte[] abytes = pdfReport.PrepareReport(db);
            return File(abytes, "application/pdf");
        }
    }
}