using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestExcel.Data;
using TestExcel.Models;

namespace TestExcel.Report
{
    public class PdfReport
    {
        #region Declaration
        int k = 0; int l = 0; int m = 0; int n = 0;
        string[] date = { "M", "T", "W", "H", "F", "S" };
        //string[] building = { "63", "62", "W", "H", "F", "S" };
        string textcreadit = "", tmp;
        string tab = "  ";
        int _totalColumn = 5, _totalColumn2 = 15;
        Document _document;
        Font _fontStyle, THSarabunfnt;
        int i;
        // สร้าง BaseFont 
        BaseFont bf = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/fonts/THSarabun.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        PdfPTable _pdfTable = new PdfPTable(5);
        PdfPTable _pdfTable2 = new PdfPTable(15);
        PdfPCell _pdfPCell;
        MemoryStream _memoryStream = new MemoryStream();
        TestExcelEntities db = new TestExcelEntities();
        List<Section_Subject> _section_subject = new List<Section_Subject>();
        List<Building_Classroom> _building_classroom = new List<Building_Classroom>();
        List<Department_Branch> _department_branch = new List<Department_Branch>();
        string Semester, Year, Branch_Name, department_name, day;
        #endregion

        public List<Section_Subject> GetModel(string Branch_Name, string semester, string year)
        {
            List<Section_Subject> section_subject = new List<Section_Subject>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        where e1.SECTION_BRANCH_NAME.Contains(Branch_Name) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
                        select new Section_Subject
                        {
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            section_subject = query.ToList();
            return section_subject;
        }
        public List<Building_Classroom> GetTEModel(string Building, string semester, string year, string day)
        {
            List<Building_Classroom> Building_subject = new List<Building_Classroom>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME
                        where e3.BUILDING_NAME.Contains(Building) && e1.SECTION_DATE.Contains(day) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
                        select new Building_Classroom
                        {
                            SUBJECT_ID = e1.SUBJECT_ID,
                            SUBJECT_NAME = e2.SUBJECT_NAME,
                            SUBJECT_CREDIT = e2.SUBJECT_CREDIT,
                            SECTION_NUMBER = e1.SECTION_NUMBER,
                            SECTION_BRANCH_NAME = e1.SECTION_BRANCH_NAME,
                            SECTION_CLASSROOM = e1.SECTION_CLASSROOM,
                            SECTION_DATE = e1.SECTION_DATE,
                            SECTION_PROFESSOR_SHORTNAME = e1.SECTION_PROFESSOR_SHORTNAME,
                            SECTION_TIME_START = e1.SECTION_TIME_START,
                            SECTION_TIME_END = e1.SECTION_TIME_END,
                            BUILDING_NAME = e3.BUILDING_NAME,
                            SEMESTER = e1.SEMESTER,
                            YEAR = e1.YEAR
                        };
            Building_subject = query.ToList();
            return Building_subject;
        }
        public void B_Body(string classroom, string building)
        {
            if (building == "42" || building == "65")
            {
                THSarabunfnt = new Font(bf, 8, 0);
            }
            else
            {
                THSarabunfnt = new Font(bf, 9, 0);
            }
            //THSarabunfnt = new Font(bf, 8, 0);
            for (int b = 8; b <= 21; b++)
            {
                var WhereTimeDate = _building_classroom.Where(x => x.SECTION_TIME_START == b && x.SECTION_CLASSROOM.Contains(classroom));
                var check = WhereTimeDate.LastOrDefault();
                var check1 = _building_classroom.Where(x => x.SECTION_TIME_START <= b && x.SECTION_TIME_END > b && x.SECTION_CLASSROOM.Contains(classroom)).OrderBy(x => x.SECTION_TIME_START).LastOrDefault();
                if (check != null)
                {
                    var trigger = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM.Contains(classroom)).Count();
                    if (trigger == 1)
                    {
                        var tmp_TIME_START = check.SECTION_TIME_START;
                        var tmp_TIME_END = check.SECTION_TIME_END;
                        var TIME_START = int.Parse(tmp_TIME_START.ToString());
                        var TIME_END = int.Parse(tmp_TIME_END.ToString());
                        var TIME = TIME_END - TIME_START;
                        tmp = Tetemp(TIME);
                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SUBJECT_ID + tab + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                        _pdfPCell.Colspan = TIME;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable2.AddCell(_pdfPCell);

                    }
                    else if (trigger == 2)
                    {
                        var first = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM.Contains(classroom)).First();
                        var second = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM.Contains(classroom)).Last();
                        int tmp_first = int.Parse(first.SECTION_TIME_START.ToString());
                        int tmp_last = int.Parse(second.SECTION_TIME_END.ToString());

                        int tmpl_first = int.Parse(first.SECTION_TIME_END.ToString());
                        int tmpl_last = int.Parse(second.SECTION_TIME_START.ToString());
                        if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                        {
                        }
                        else if (tmpl_first == tmpl_last)
                        {
                            var TIME = tmp_last - tmp_first;
                            tmp = Tetemp(TIME);
                            _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + tab + second.SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                            _pdfPCell.Colspan = TIME;
                            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _pdfPCell.BackgroundColor = BaseColor.WHITE;
                            _pdfTable2.AddCell(_pdfPCell);
                        }
                        else if (tmpl_first != tmpl_last)
                        {
                            var tmp_TIME_START = WhereTimeDate.Last().SECTION_TIME_START;
                            var tmp_TIME_END = WhereTimeDate.Last().SECTION_TIME_END;
                            var TIME_START = int.Parse(tmp_TIME_START.ToString());
                            var TIME_END = int.Parse(tmp_TIME_END.ToString());
                            var TIME = TIME_END - TIME_START;
                            tmp = Tetemp(TIME);

                            _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SUBJECT_ID + tab + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                            _pdfPCell.Colspan = TIME;
                            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _pdfPCell.BackgroundColor = BaseColor.WHITE;
                            _pdfTable2.AddCell(_pdfPCell);
                        }

                    }
                }
                else
                {
                    if (check1 == null)
                    {
                        _pdfPCell = new PdfPCell(new Phrase("\n\n\n", THSarabunfnt));
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable2.AddCell(_pdfPCell);
                    }
                    else
                    {

                    }
                }
            }
        }
        public byte[] TePrepareReport(int Date, string semester, string year)
        {
            day = date[Date];
            Semester = semester;
            Year = year;

            #region T
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4.Rotate());
            _document.SetMargins(10f, 10f, 10f, 20f);
            _pdfTable2.WidthPercentage = 93;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();

            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});
            #endregion
            #region B_63
            _building_classroom = GetTEModel("63", Semester, Year, day);
            #region header B_63
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่        " + Semester + "        ปีการศึกษา        " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 63", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_63 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_63 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "63").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "63");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);

            _document.NewPage();
            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            #region B_63_2
            _building_classroom = GetTEModel("632", Semester, Year, day);
            #region header B_63_2
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่    " + Semester + "    ปีการศึกษา    " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 63 (อาคารสีเทา ตึกใหม่)", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_63_2 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_63_2 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "632").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "632");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);
            _document.NewPage();
            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            #region B_62
            _building_classroom = GetTEModel("62", Semester, Year, day);
            #region header B_62
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่    " + Semester + "    ปีการศึกษา    " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 62", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_62 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_62 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "62").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "62");
                _pdfTable2.CompleteRow();
            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);
            _document.NewPage();
            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            #region B_42
            _building_classroom = GetTEModel("42", Semester, Year, day);
            #region header B_42
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่    " + Semester + "    ปีการศึกษา    " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 42", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_42 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_42 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "42").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "42");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);
            _document.NewPage();
            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            #region B_65
            _building_classroom = GetTEModel("65", Semester, Year, day);
            #region header B_65
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่    " + Semester + "    ปีการศึกษา    " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 21, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 65", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_65 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_65 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "65").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "65");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);
            _document.NewPage();
            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            #region B_69
            _building_classroom = GetTEModel("69", Semester, Year, day);
            #region header B_69
            THSarabunfnt = new Font(bf, 20, 0);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่    " + Semester + "    ปีการศึกษา    " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 69 (ด้านหลัง SHOP-IP)", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_69 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_69 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "69").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "69");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);

            _pdfTable2 = new PdfPTable(15);
            _pdfTable2.WidthPercentage = 93;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});

            _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            #region B_64
            _building_classroom = GetTEModel("64", Semester, Year, day);
            #region header B_64
            THSarabunfnt = new Font(bf, 22, 4);
            _pdfPCell = new PdfPCell(new Phrase("ห้องเรียนอาคาร 64 (ด้านหลังปรับอากาศ)", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 0);
            _pdfPCell = new PdfPCell(new Phrase("\n", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();
            #endregion
            #region B_64 Table header
            THSarabunfnt = new Font(bf, 16, 0);
            _pdfPCell = new PdfPCell(new Phrase("อาคาร/ห้อง", THSarabunfnt));
            _pdfPCell.PaddingBottom = 9f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfTable2.AddCell(_pdfPCell);

            for (int b = 8; b <= 21; b++)
            {
                _pdfPCell = new PdfPCell(new Phrase(" " + b.ToString() + ":00", THSarabunfnt));
                _pdfPCell.PaddingBottom = 9f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            _pdfTable2.CompleteRow();
            #endregion
            #region B_64 Body
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "64").ToList())
            {
                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 8f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                B_Body(aa.CLASSROOM_NAME, "64");
                _pdfTable2.CompleteRow();

            }
            #endregion
            #endregion
            _document.Add(_pdfTable2);

            _document.Close();
            return _memoryStream.ToArray();
        }

        public byte[] PrepareReport(string Department_name, string semester, string year)
        {
            Semester = semester;
            Year = year;
            department_name = Department_name;
            //_db = db;
            var model = from e1 in db.BRANCHes
                        join e2 in db.DEPARTMENTs on e1.DEPARTMENT_NAME equals e2.DEPARTMENT_NAME
                        select new Department_Branch
                        {
                            BRANCH_NAME = e1.BRANCH_NAME,
                            DEPARTMENT_NAME = e1.DEPARTMENT_NAME,
                            DEPARTMENT_THAI_NAME = e2.DEPARTMENT_THAI_NAME
                        };
            _department_branch = model.ToList();


            #region T1
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 90;
            _pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 20f, 32f, 140f, 32f, 20f });
            #endregion

            #region T2
            _pdfTable2.WidthPercentage = 90;
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfTable2.SetWidths(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});
            #endregion

            foreach (var q in _department_branch.Where(x => x.DEPARTMENT_NAME == department_name).ToList())
            {
                Branch_Name = q.BRANCH_NAME;
                _section_subject = GetModel(Branch_Name, Semester, Year);
                if (_section_subject.Count() != 0)
                {


                    #region Header
                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    _pdfPCell = new PdfPCell(new Phrase("รายการลงทะเบียนเรียน", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่ " + Semester + "/" + Year, THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase(Branch_Name, THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();
                    #endregion


                    #region T1 Table header
                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase("ลำดับ", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase("รหัสวิชา", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase("ชื่อวิชา", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase("หน่วยกิต", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase("ตอน", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    #endregion

                    #region Table Body
                    THSarabunfnt = new Font(bf, 16, 0);
                    int number = 1;
                    foreach (var p in _section_subject.Where(x => x.SECTION_NUMBER != ""))
                    {
                        string[] e = p.SUBJECT_CREDIT.Split('(', ')');
                        if (e[0] != "")
                        {
                            k += int.Parse(e[0]);
                            string[] ee = e[1].Split('-');
                            l += int.Parse(ee[0]);
                            m += int.Parse(ee[1]);
                            n += int.Parse(ee[2]);

                            textcreadit = k + "(" + l + "-" + m + "-" + n + ")";
                        }
                        else
                        {
                            textcreadit = "";
                        }

                        _pdfPCell = new PdfPCell(new Phrase(number++.ToString(), THSarabunfnt));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(p.SUBJECT_ID, THSarabunfnt));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(" " + p.SUBJECT_NAME, THSarabunfnt));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(p.SUBJECT_CREDIT, THSarabunfnt));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(p.SECTION_NUMBER, THSarabunfnt));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);
                        _pdfTable.CompleteRow();
                        i = number;
                    }
                    for (int j = i; j < 12; j++)
                    {
                        _pdfPCell = new PdfPCell(new Phrase(" "));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(" "));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(" "));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(" "));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase(" "));
                        _pdfPCell.Border = 12;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable.AddCell(_pdfPCell);
                        _pdfTable.CompleteRow();
                    }
                    #region spacetable
                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 14;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 14;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 14;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 14;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 14;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();
                    #endregion

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 0;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 0;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase("รวม", THSarabunfnt));
                    _pdfPCell.Border = 0;
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.PaddingRight = 15f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(textcreadit, THSarabunfnt));
                    _pdfPCell.Border = 14;
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);

                    _pdfPCell = new PdfPCell(new Phrase(" "));
                    _pdfPCell.Border = 0;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();

                    #endregion

                    _pdfTable.HeaderRows = 3;
                    _document.Add(_pdfTable);
                    _pdfTable = new PdfPTable(5);
                    _pdfTable.WidthPercentage = 90;
                    _pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfTable.SetWidths(new float[] { 20f, 32f, 140f, 32f, 20f });
                    k = 0; l = 0; m = 0; n = 0;

                    THSarabunfnt = new Font(bf, 16, 0);
                    _pdfPCell = new PdfPCell(new Phrase("\n\n", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn2;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable2.AddCell(_pdfPCell);
                    _pdfTable2.CompleteRow();

                    #region T2 Table header
                    _pdfPCell = new PdfPCell(new Phrase("D/T", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 10f;
                    _pdfPCell.PaddingTop = 5f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);

                    THSarabunfnt = new Font(bf, 16, 0);
                    for (int b = 8; b <= 21; b++)
                    {
                        _pdfPCell = new PdfPCell(new Phrase(b.ToString(), THSarabunfnt));
                        _pdfPCell.PaddingBottom = 10f;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable2.AddCell(_pdfPCell);
                    }
                    _pdfTable2.CompleteRow();
                    for (int c = 0; c < 6; c++)
                    {
                        THSarabunfnt = new Font(bf, 16, 0);
                        _pdfPCell = new PdfPCell(new Phrase(date[c] + "\n\n", THSarabunfnt));
                        _pdfPCell.PaddingTop = 10f;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable2.AddCell(_pdfPCell);

                        for (int d = 8; d <= 21; d++)
                        {
                            var WhereTimeDate = _section_subject.Where(x => x.SECTION_TIME_START == d && x.SECTION_DATE == date[c]);
                            var check = WhereTimeDate.LastOrDefault();
                            var check1 = _section_subject.Where(x => x.SECTION_TIME_START <= d && x.SECTION_TIME_END > d && x.SECTION_DATE == date[c]).OrderBy(x => x.SECTION_TIME_START).LastOrDefault();
                            if (check != null)
                            {
                                var trigger = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_DATE == date[c]).Count();
                                if (trigger == 1)
                                {
                                    var tmp_TIME_START = check.SECTION_TIME_START;
                                    var tmp_TIME_END = check.SECTION_TIME_END;
                                    var TIME_START = int.Parse(tmp_TIME_START.ToString());
                                    var TIME_END = int.Parse(tmp_TIME_END.ToString());
                                    var TIME = TIME_END - TIME_START;
                                    tmp = temp(TIME);

                                    THSarabunfnt = new Font(bf, 10, 0);
                                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SUBJECT_ID + "\n" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                                    _pdfPCell.Colspan = TIME;
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable2.AddCell(_pdfPCell);

                                }
                                else if (trigger == 2)
                                {
                                    var first = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).First();
                                    var second = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).Last();
                                    int tmp_first = int.Parse(first.SECTION_TIME_START.ToString());
                                    int tmp_last = int.Parse(second.SECTION_TIME_END.ToString());

                                    int tmpl_first = int.Parse(first.SECTION_TIME_END.ToString());
                                    int tmpl_last = int.Parse(second.SECTION_TIME_START.ToString());
                                    if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                                    {

                                    }
                                    else if (tmpl_first == tmpl_last)
                                    {
                                        var TIME = tmp_last - tmp_first;
                                        tmp = temp(TIME);
                                        THSarabunfnt = new Font(bf, 10, 0);
                                        _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + "\n" + second.SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                                        _pdfPCell.Colspan = TIME;
                                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                        _pdfTable2.AddCell(_pdfPCell);
                                    }
                                    else if (tmpl_first != tmpl_last)
                                    {
                                        var tmp_TIME_START = WhereTimeDate.Last().SECTION_TIME_START;
                                        var tmp_TIME_END = WhereTimeDate.Last().SECTION_TIME_END;
                                        var TIME_START = int.Parse(tmp_TIME_START.ToString());
                                        var TIME_END = int.Parse(tmp_TIME_END.ToString());
                                        var TIME = TIME_END - TIME_START;
                                        tmp = temp(TIME);

                                        THSarabunfnt = new Font(bf, 10, 0);
                                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SUBJECT_ID + "\n" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).LastOrDefault().SECTION_PROFESSOR_SHORTNAME + tmp, THSarabunfnt));
                                        _pdfPCell.Colspan = TIME;
                                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                        _pdfTable2.AddCell(_pdfPCell);
                                    }
                                }
                            }
                            else
                            {
                                if (check1 == null)
                                {
                                    THSarabunfnt = new Font(bf, 12, 0);
                                    _pdfPCell = new PdfPCell(new Phrase("\n\n\n", THSarabunfnt));
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable2.AddCell(_pdfPCell);
                                }
                                else
                                {
                                }
                            }
                            //_pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                            //_pdfPCell.PaddingBottom = 10f;
                            //_pdfPCell.PaddingTop = 5f;
                            //_pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //_pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //_pdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_pdfTable2.AddCell(_pdfPCell);
                        }
                        _pdfTable2.CompleteRow();
                    }
                    _document.Add(_pdfTable2);
                    _pdfTable2 = new PdfPTable(15);
                    _pdfTable2.WidthPercentage = 90;
                    _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfTable2.SetWidths(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f });
                    #endregion
                    _document.NewPage();
                }
            }

            _document.Close();
            return _memoryStream.ToArray();

        }
        public string temp(int TIME)
        {
            string Temp = "";
            if (TIME == 1)
            {
                Temp = " \n /--------" + "--------/ \n";
            }
            else if (TIME == 2)
            {
                Temp = " \n /----------" + "----------/ \n";
            }
            else if (TIME == 3)
            {
                Temp = " \n /---------------" + "---------------/ \n";
            }
            else if (TIME == 4)
            {
                Temp = " \n /--------------------" + "--------------------/ \n";
            }
            else if (TIME == 5)
            {
                Temp = " \n /--------------------------" + "--------------------------/ \n";
            }
            else if (TIME == 6)
            {
                Temp = " \n /--------------------------------" + "--------------------------------/ \n";
            }
            else if (TIME == 7)
            {
                Temp = " \n /--------------------------------------" + "--------------------------------------/ \n";
            }
            return Temp;
        }
        public string Tetemp(int TIME)
        {
            string Temp = "";
            if (TIME == 1)
            {
                Temp = " \n /----------------" + "----------------/ \n";
            }
            else if (TIME == 2)
            {
                Temp = " \n /--------------------" + "--------------------/ \n";
            }
            else if (TIME == 3)
            {
                Temp = " \n /------------------------------" + "------------------------------/ \n";
            }
            else if (TIME == 4)
            {
                Temp = " \n /----------------------------------------" + "----------------------------------------/ \n";
            }
            else if (TIME == 5)
            {
                Temp = " \n /-----------------------------------------" + "-----------------------------------------/ \n";
            }
            else if (TIME == 6)
            {
                Temp = " \n /-----------------------------------------------" + "-----------------------------------------------/ \n";
            }
            else if (TIME == 7)
            {
                Temp = " \n /-----------------------------------------------------" + "-----------------------------------------------------/ \n";
            }
            return Temp;
        }
    }
}