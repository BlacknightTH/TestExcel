using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestExcel.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using TestExcel.Utility;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace TestExcel.Controllers
{
    public class ExportExcelController : Controller
    {
        TestExcelEntities db = new TestExcelEntities();
        // GET: ExportExcel
        public ActionResult Index()
        {
            return View();
        }
        public void Export()
        {
            string FilePath = "D:\\ขบวน1-2560.xlsx";
            string FileName = Path.GetFileName(FilePath);
            Process[] excelProcsOld = Process.GetProcessesByName("EXCEL");
            try
            {
                TestExcelEntities db = new TestExcelEntities();
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
                rangeA4.Value = "ภาคการศึกษาที่ 1 ปีการศึกษา 2560";
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
                foreach (SUBJECT p in db.SUBJECTs.ToList())
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
                    foreach (SECTION r in db.SECTIONs.Where(x => x.SUBJECT_ID == p.SUBJECT_ID).ToList())
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

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();

                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //xls
                //                                                   // For xlsx, use: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
                //Response.AddHeader(String.Format("content-disposition", "attachment; filename={0}"), Path.GetFileName(FileName));
                //Response.TransmitFile(FileName);
                //Response.End();

                //workbook.SaveAs("D:\\ขบวน1-2560.xlsx");
                //workbook.Close();
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

        }
    }
}