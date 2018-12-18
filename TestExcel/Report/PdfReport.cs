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
        string[] date = { "M", "T", "W", "H", "F", "S" ,"SUN"};
        string[] date_thai = { "จันทร์", "อังคาร", "พุธ", "พฤหัสบดี", "ศุกร์", "เสาร์" };
        string textcreadit = "", tmp;
        int _totalColumn = 5, _totalColumn2 = 15, _totalColumn3 = 19;
        Document _document;
        Font _fontStyle, THSarabunfnt;
        int i;
        // สร้าง BaseFont 
        BaseFont bf = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/fonts/THSarabun.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        PdfPTable _pdfTable = new PdfPTable(5);
        PdfPTable _pdfTable2 = new PdfPTable(15);
        PdfPTable _pdfTable3 = new PdfPTable(19);
        PdfPCell _pdfPCell;
        Chunk _chunk;
        MemoryStream _memoryStream = new MemoryStream();
        TestExcelEntities db = new TestExcelEntities();
        List<Section_Subject> _section_subject = new List<Section_Subject>();
        List<Building_Classroom> _building_classroom = new List<Building_Classroom>();
        List<Department_Branch> _department_branch = new List<Department_Branch>();
        List<Section_Professor> _professor = new List<Section_Professor>();
        string Semester, Year, Branch_Name, department_name, day, BUILDING;
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
        public List<Section_Subject> PGetData(string professor,string semester, string year)
        {
            List<Section_Subject> section_subject = new List<Section_Subject>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e3.PROFESSOR_SHORTNAME
                        where e1.SECTION_PROFESSOR_SHORTNAME.Contains(professor) && e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
                        select new Section_Subject
                        {
                            SECTION_ID = e1.SECTION_ID,
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
            section_subject = query.OrderBy(x => x.YEAR).ToList();
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
        public List<Building_Classroom> GetCLModel(string semester, string year)
        {
            List<Building_Classroom> Building_subject = new List<Building_Classroom>();
            var query = from e1 in db.SECTIONs
                        join e2 in db.SUBJECTs on e1.SUBJECT_ID equals e2.SUBJECT_ID
                        join e3 in db.BUILDINGs on e1.SECTION_CLASSROOM equals e3.CLASSROOM_NAME into d
                        from e3 in d.DefaultIfEmpty()
                        where e1.SEMESTER.Contains(semester) && e2.SEMESTER.Contains(semester) && e1.YEAR.Contains(year) && e2.YEAR.Contains(year)
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
                THSarabunfnt = new Font(bf, 12, 0);
            }
            else
            {
                THSarabunfnt = new Font(bf, 14, 0);
            }
            //THSarabunfnt = new Font(bf, 8, 0);
            for (int b = 8; b <= 21; b++)
            {
                var WhereTimeDate = _building_classroom.Where(x => x.SECTION_TIME_START == b && x.SECTION_CLASSROOM == classroom);
                var check = WhereTimeDate.LastOrDefault();
                var check1 = _building_classroom.Where(x => x.SECTION_TIME_START <= b && x.SECTION_TIME_END > b && x.SECTION_CLASSROOM == classroom).OrderBy(x => x.SECTION_TIME_START).LastOrDefault();
                if (check != null)
                {
                    var trigger = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == classroom).Count();
                    if (trigger == 1)
                    {
                        var TIME_START = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_START.ToString())).ToString());
                        var TIME_END = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_END.ToString())).ToString());
                        var TIME = TIME_END - TIME_START;
                        tmp = Tetemp(TIME);

                        string SECTION_NUMBER = "";
                        if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
                        {
                            SECTION_NUMBER = "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
                        }
                        //_pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER + tmp + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME, THSarabunfnt));
                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "/\n" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                        _pdfPCell.Colspan = TIME;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable2.AddCell(_pdfPCell);

                    }
                    else if (trigger == 2)
                    {
                        var first = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == classroom).First();
                        var second = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == classroom).Last();
                        int tmp_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_START.ToString())).ToString());
                        int tmp_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_END.ToString())).ToString());

                        int tmpl_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_END.ToString())).ToString());
                        int tmpl_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_START.ToString())).ToString());
                        if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                        {
                        }
                        else if (tmpl_first == tmpl_last)
                        {
                            string SECTION_NUMBER = "";
                            if (first.SECTION_BRANCH_NAME != "")
                            {
                                SECTION_NUMBER = "/" + first.SECTION_NUMBER;
                            }
                            var TIME = tmp_last - tmp_first;
                            tmp = Tetemp(TIME);
                            _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + SECTION_NUMBER + "/\n" + first.SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                            //_pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + "/" + first.SECTION_NUMBER + tmp + second.SECTION_PROFESSOR_SHORTNAME, THSarabunfnt));
                            _pdfPCell.Colspan = TIME;
                            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _pdfPCell.BackgroundColor = BaseColor.WHITE;
                            _pdfTable2.AddCell(_pdfPCell);
                        }
                        else if (tmpl_first != tmpl_last)
                        {
                            var TIME_START = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_START.ToString()));
                            var TIME_END = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_END.ToString()));
                            var TIME = TIME_END - TIME_START;
                            string SECTION_NUMBER = "";
                            if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
                            {
                                SECTION_NUMBER = "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
                            }
                            _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "/\n" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                            //_pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER + tmp + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME, THSarabunfnt));
                            _pdfPCell.Colspan = int.Parse(TIME.ToString());
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
                        _pdfPCell = new PdfPCell(new Phrase("\n\n", THSarabunfnt));
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
            _document.SetMargins(5f, 5f, 10f, 5f);
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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 63", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 10, 1);
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
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "63").OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                THSarabunfnt = new Font(bf, 15, 0);
                if (aa.NUMBER_SEATS == "ป.โท" || aa.NUMBER_SEATS == "lab-LE" || aa.NUMBER_SEATS.Contains("Draw"))
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + ")" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + " ที่นั่ง)" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }

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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 63 (อาคารสีเทา ตึกใหม่)", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "632").OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                THSarabunfnt = new Font(bf, 15, 0);
                if (aa.NUMBER_SEATS == "ป.โท" || aa.NUMBER_SEATS == "lab-LE" || aa.NUMBER_SEATS.Contains("Draw"))
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + ")" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + " ที่นั่ง)" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }

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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 62", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "62").OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                THSarabunfnt = new Font(bf, 15, 0);
                if (aa.NUMBER_SEATS == "ป.โท" || aa.NUMBER_SEATS == "lab-LE" || aa.NUMBER_SEATS.Contains("Draw"))
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + ")" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + " ที่นั่ง)" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }

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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 42", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "42").OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                THSarabunfnt = new Font(bf, 15, 0);
                if (aa.NUMBER_SEATS == "ป.โท" || aa.NUMBER_SEATS == "lab-LE" || aa.NUMBER_SEATS.Contains("Draw"))
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + ")" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + " ที่นั่ง)" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }

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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 65", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
            foreach (var aa in db.BUILDINGs.Where(x => x.BUILDING_NAME == "65").OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                THSarabunfnt = new Font(bf, 15, 0);
                if (aa.NUMBER_SEATS == "ป.โท" || aa.NUMBER_SEATS == "lab-LE" || aa.NUMBER_SEATS.Contains("Draw"))
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + ")" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(aa.CLASSROOM_NAME + "(" + aa.NUMBER_SEATS + " ที่นั่ง)" + "\n", THSarabunfnt));
                    _pdfPCell.PaddingBottom = 8f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
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
            THSarabunfnt = new Font(bf, 18, 1);
            _pdfPCell = new PdfPCell(new Phrase("ตารางการใช้ห้องเรียน", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();


            _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
            _pdfPCell.Colspan = _totalColumn2;
            _pdfPCell.PaddingTop = -24f;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable2.AddCell(_pdfPCell);
            _pdfTable2.CompleteRow();

            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 69 (ด้านหลัง SHOP-IP)", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
            THSarabunfnt = new Font(bf, 18, 1);
            _chunk = new Chunk("ห้องเรียนอาคาร 64 (ด้านหลังปรับอากาศ)", THSarabunfnt);
            _chunk.SetUnderline(1, -3);
            _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
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
        public byte[] PfPrepareReport(string department, string semester, string year)
        {
            Semester = semester;
            Year = year;
            #region T
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4.Rotate());
            _document.SetMargins(0f, 0f, 10f, 5f);
            _pdfTable3.WidthPercentage = 93;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfTable3.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();

            _pdfTable3.SetWidths(new float[] { 15f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,20f, 20f, 20f, 20f, 20f, 20f,
                                                22f, 30f, 32f, 38f, 20f});
            #endregion
            if (department == "")
            {
                _professor = (from e1 in db.SECTIONs
                              join e2 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e2.PROFESSOR_SHORTNAME
                              //where e1.SEMESTER.Contains(Semester) && e1.YEAR.Contains(Year)
                              where (e2.DEPARTMENT_NAME == null || e2.DEPARTMENT_NAME == "") && e1.SEMESTER.Contains(Semester) && e1.YEAR.Contains(Year)
                              select new Section_Professor
                              {
                                  PROFESSOR_ID = e2.PROFESSOR_ID,
                                  SECTION_PROFESSOR_SHORTNAME = e2.PROFESSOR_SHORTNAME
                              }).ToList();
            }
            else
            {
                _professor = (from e1 in db.SECTIONs
                              join e2 in db.PROFESSORs on e1.SECTION_PROFESSOR_SHORTNAME equals e2.PROFESSOR_SHORTNAME
                              //where e1.SEMESTER.Contains(Semester) && e1.YEAR.Contains(Year)
                              where e2.DEPARTMENT_NAME == department && e1.SEMESTER.Contains(Semester) && e1.YEAR.Contains(Year)
                              select new Section_Professor
                              {
                                  PROFESSOR_ID = e2.PROFESSOR_ID,
                                  SECTION_PROFESSOR_SHORTNAME = e2.PROFESSOR_SHORTNAME
                              }).ToList();
            }
            var professor = _professor.OrderBy(x => x.SECTION_PROFESSOR_SHORTNAME);

            #region Query
            foreach (var item in professor.Select(x => new { x.PROFESSOR_ID, x.SECTION_PROFESSOR_SHORTNAME }).Distinct())
            {
                _section_subject = PGetData(item.SECTION_PROFESSOR_SHORTNAME,Semester, Year);
                #region Header
                THSarabunfnt = new Font(bf, 18, 1);
                _pdfPCell = new PdfPCell(new Phrase("\nภาระการสอน", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.PaddingBottom = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("วิทยาลัยเทคโนโลยีอุตสาหกรรม", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.PaddingBottom = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่..............................ปีการศึกษา..............................", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("                      " + Semester + "                               " + Year, THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.PaddingTop = -24f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                THSarabunfnt = new Font(bf, 12, 1);
                _pdfPCell = new PdfPCell(new Phrase("\n\n", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("\n\n\n", THSarabunfnt));
                _pdfPCell.PaddingBottom = 20f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);
                for (int b = 8; b < 20; b++)
                {
                    string s = "";
                    if (b < 10)
                    {
                        s = "0";
                    }
                    _pdfPCell = new PdfPCell(new Phrase(s + b.ToString(), THSarabunfnt));
                    _pdfPCell.PaddingTop = 10f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable3.AddCell(_pdfPCell);
                }

                _pdfPCell = new PdfPCell(new Phrase("20      21", THSarabunfnt));
                _pdfPCell.PaddingTop = 10f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("รวมจำนวน"+ "\n\n" + "หน่วยกิต", THSarabunfnt));
                _pdfPCell.PaddingTop = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("รวมหน่วย ชม." + "\n\n" + "อ้างอิง(ในเวลา)", THSarabunfnt));
                _pdfPCell.PaddingTop = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("รวมหน่วย ชม." + "\n\n" + "อ้างอิง(นอกเวลา)", THSarabunfnt));
                _pdfPCell.PaddingTop = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("รวมหน่วย ชม." + "\n\n" + "อ้างอิง ใน/นอก เวลา", THSarabunfnt));
                _pdfPCell.PaddingTop = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase("หมายเหตุ", THSarabunfnt));
                _pdfPCell.PaddingTop = 5f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable3.AddCell(_pdfPCell);

                _pdfTable3.CompleteRow();
                #endregion
                #region Body
                for (int c = 0; c < 7; c++)
                {
                    THSarabunfnt = new Font(bf, 14, 1);
                    _pdfPCell = new PdfPCell(new Phrase(date[c], THSarabunfnt));
                    _pdfPCell.PaddingTop = 5f;
                    _pdfPCell.PaddingBottom = 20f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable3.AddCell(_pdfPCell);

                    for (int d = 8; d <= 20; d++)
                    {
                        var WhereTimeDate = _section_subject.Where(x => x.SECTION_TIME_START == d && x.SECTION_DATE == date[c]);
                        var check = WhereTimeDate.LastOrDefault();
                        var check1 = _section_subject.Where(x => x.SECTION_TIME_START <= d && x.SECTION_TIME_END > d && x.SECTION_DATE == date[c]).OrderBy(x => x.SECTION_TIME_START).LastOrDefault();
                        if (check != null)
                        {
                            var trigger = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_DATE == date[c]).Count();
                            if (trigger == 1)
                            {
                                string SECTION_NUMBER = "";
                                if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
                                {
                                    SECTION_NUMBER = "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
                                }
                                var TIME_START = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_START.ToString())).ToString());
                                var TIME_END = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_END.ToString())).ToString());
                                var TIME = TIME_END - TIME_START;
                                tmp = temp(TIME);

                                if (TIME == 1)
                                {
                                    THSarabunfnt = new Font(bf, 10, 1);
                                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_BRANCH_NAME + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_CLASSROOM, THSarabunfnt));
                                }
                                else
                                {
                                    THSarabunfnt = new Font(bf, 12, 1);
                                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_BRANCH_NAME + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_CLASSROOM, THSarabunfnt));
                                }
                                _pdfPCell.Colspan = TIME;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfTable3.AddCell(_pdfPCell);

                            }
                            else if (trigger >= 2)
                            {
                                var first = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).First();
                                var second = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).Last();
                                int tmp_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_START.ToString())).ToString());
                                int tmp_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_END.ToString())).ToString());

                                int tmpl_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_END.ToString())).ToString());
                                int tmpl_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_START.ToString())).ToString());

                                if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                                {

                                }
                                else if (tmpl_first == tmpl_last)
                                {
                                    string SECTION_NUMBER = "";
                                    if (first.SECTION_BRANCH_NAME != "")
                                    {
                                        SECTION_NUMBER = "/" + first.SECTION_NUMBER;
                                    }
                                    var TIME = tmp_last - tmp_first;
                                    tmp = temp(TIME);
                                    THSarabunfnt = new Font(bf, 12, 1);
                                    _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + SECTION_NUMBER + "\n/" + first.SECTION_BRANCH_NAME + "/" + first.SECTION_CLASSROOM, THSarabunfnt));
                                    _pdfPCell.Colspan = TIME;
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable3.AddCell(_pdfPCell);
                                }
                                else if (tmpl_first != tmpl_last)
                                {
                                    var TIME_START = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_START.ToString()));
                                    var TIME_END = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_END.ToString()));
                                    var TIME = TIME_END - TIME_START;

                                    string SECTION_NUMBER = "";
                                    if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
                                    {
                                        SECTION_NUMBER = "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
                                    }
                                    if (TIME == 1)
                                    {
                                        THSarabunfnt = new Font(bf, 10, 1);
                                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_BRANCH_NAME + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_CLASSROOM, THSarabunfnt));
                                    }
                                    else
                                    {
                                        THSarabunfnt = new Font(bf, 12, 1);
                                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_BRANCH_NAME + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_CLASSROOM, THSarabunfnt));
                                    }
                                    _pdfPCell.Colspan = int.Parse(TIME.ToString());
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable3.AddCell(_pdfPCell);
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
                                _pdfTable3.AddCell(_pdfPCell);
                            }
                            else
                            {
                            }
                        }
                    }
                    if(c == 6)
                    {
                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 10;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 10;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 10;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 10;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 10;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);
                    }
                    else
                    {
                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 8;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 8;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 8;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 8;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);

                        _pdfPCell = new PdfPCell(new Phrase("", THSarabunfnt));
                        _pdfPCell.Border = 8;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfTable3.AddCell(_pdfPCell);
                    }
                    _pdfTable3.CompleteRow();
                }
                THSarabunfnt = new Font(bf, 16, 1);
                _pdfPCell = new PdfPCell(new Phrase("\n\n", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("หัวหน้าภาควิชา................................................................................อาจารย์................................................................................", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();

                _pdfPCell = new PdfPCell(new Phrase("                                                                                                                                              " + item.SECTION_PROFESSOR_SHORTNAME, THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn3;
                _pdfPCell.PaddingTop = -24f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable3.AddCell(_pdfPCell);
                _pdfTable3.CompleteRow();
                #endregion
                _document.Add(_pdfTable3);
                _pdfTable3 = new PdfPTable(19);
                _pdfTable3.WidthPercentage = 93;
                _pdfTable3.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfTable3.SetWidths(new float[] { 15f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,20f, 20f, 20f, 20f, 20f, 20f,
                                                22f, 30f, 32f, 38f, 20f});
                _document.NewPage();
            }
            #endregion
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
                        join e2 in db.COURSEs on e1.COURSE_NAME equals e2.COURSE_NAME
                        select new Department_Branch
                        {
                            BRANCH_NAME = e1.BRANCH_NAME,
                            COURSE_NAME = e1.COURSE_NAME,
                            COURSE_THAI_NAME = e2.COURSE_THAI_NAME
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

            foreach (var q in _department_branch.Where(x => x.COURSE_NAME == department_name).ToList())
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

                    THSarabunfnt = new Font(bf, 16, 1);
                    _pdfPCell = new PdfPCell(new Phrase("รายการลงทะเบียนเรียน", THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();


                    _pdfPCell = new PdfPCell(new Phrase("ภาคการศึกษาที่ " + Semester + "/" + Year, THSarabunfnt));
                    _pdfPCell.Colspan = _totalColumn;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.Border = 0;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfPCell.ExtraParagraphSpace = 0;
                    _pdfTable.AddCell(_pdfPCell);
                    _pdfTable.CompleteRow();
                    if (department_name == "MDET" || department_name == "EnET" || department_name == "TDET" || department_name == "InET" || department_name == "PnET")
                    {

                        _pdfPCell = new PdfPCell(new Phrase("นักศึกษาสาขาวิชา" + q.COURSE_THAI_NAME, THSarabunfnt));
                        _pdfPCell.Colspan = _totalColumn;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.Border = 0;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfPCell.ExtraParagraphSpace = 0;
                        _pdfTable.AddCell(_pdfPCell);
                        _pdfTable.CompleteRow();

                        if (q.COURSE_NAME == "MDET")
                        {
                            if (Branch_Name.Contains("(M)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาสร้างเครื่องจักรกล)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาออกแบบผลิตภัณฑ์เครื่องกล)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                        }
                        if (q.COURSE_NAME == "EnET")
                        {
                            if (Branch_Name.Contains("(T)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาโทรคมนาคม)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else if (Branch_Name.Contains("(I)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาเครื่องมือวัดและระบบอัตโนมัติ)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else if (Branch_Name.Contains("(C)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาคอมพิวเตอร์)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชากระจายเสียงวิทยุและโทรทัศน์)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                        }
                        if (q.COURSE_NAME == "TDET")
                        {
                            if (Branch_Name.Contains("(P)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาแม่พิมพ์พลาสติก)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาแม่พิมพ์โลหะ)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                        }
                        if (q.COURSE_NAME == "InET")
                        {
                            if (Branch_Name.Contains("(M)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาการจัดการกระบวนการผลิต)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชากระบวนการผลิตเครื่องเรือน)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                        }
                        if (q.COURSE_NAME == "PnET")
                        {
                            if (Branch_Name.Contains("(PE)"))
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาวิศวกรรมอิเล็กทรอนิกส์กำลัง)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                            else
                            {

                                _pdfPCell = new PdfPCell(new Phrase("(แขนงวิชาวิศวกรรมควบคุม)  " + Branch_Name, THSarabunfnt));
                                _pdfPCell.Colspan = _totalColumn;
                                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                _pdfPCell.Border = 0;
                                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                _pdfPCell.ExtraParagraphSpace = 0;
                                _pdfTable.AddCell(_pdfPCell);
                                _pdfTable.CompleteRow();
                            }
                        }
                    }
                    else
                    {
                        THSarabunfnt = new Font(bf, 16, 1);
                        _pdfPCell = new PdfPCell(new Phrase("นักศึกษาสาขาวิชา" + q.COURSE_THAI_NAME + " " + Branch_Name, THSarabunfnt));
                        _pdfPCell.Colspan = _totalColumn;
                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _pdfPCell.Border = 0;
                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                        _pdfPCell.ExtraParagraphSpace = 0;
                        _pdfTable.AddCell(_pdfPCell);
                        _pdfTable.CompleteRow();
                    }

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
                        string[] e = p.SUBJECT_CREDIT.Trim().Split('(', ')');
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
                                    var TIME_START = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_START.ToString())).ToString());
                                    var TIME_END = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_END.ToString())).ToString());
                                    var TIME = TIME_END - TIME_START;
                                    tmp = temp(TIME);

                                    THSarabunfnt = new Font(bf, 14, 0);
                                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                                    _pdfPCell.Colspan = TIME;
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable2.AddCell(_pdfPCell);

                                }
                                else if (trigger == 2)
                                {
                                    var first = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).FirstOrDefault();
                                    var second = _section_subject.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME).LastOrDefault();
                                    int tmp_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_START.ToString())).ToString());
                                int tmp_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_END.ToString())).ToString());

                                int tmpl_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_END.ToString())).ToString());
                                int tmpl_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_START.ToString())).ToString());
                                    if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                                    {

                                    }
                                    else if (tmpl_first == tmpl_last)
                                    {
                                        var TIME = tmp_last - tmp_first;
                                        tmp = temp(TIME);
                                        THSarabunfnt = new Font(bf, 14, 0);
                                        _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + "/" + first.SECTION_NUMBER + "\n/" + first.SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                                        _pdfPCell.Colspan = TIME;
                                        _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                        _pdfTable2.AddCell(_pdfPCell);
                                    }
                                    else if (tmpl_first != tmpl_last)
                                    {
                                        var TIME_START = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_START.ToString()));
                                        var TIME_END = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_END.ToString()));
                                        var TIME = TIME_END - TIME_START;

                                        THSarabunfnt = new Font(bf, 14, 0);
                                        _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SUBJECT_ID + "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER + "\n/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                                        _pdfPCell.Colspan = int.Parse(TIME.ToString());
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
        public byte[] ClPrepareReport(string Building, string semester, string year)
        {
            Semester = semester;
            Year = year;
            BUILDING = Building;

            #region T
            string tableHeader = "";
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4.Rotate());
            _document.SetMargins(0, 0, 20f, 20f);
            _pdfTable2.WidthPercentage = 93;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();

            _pdfTable2.SetWidths(new float[] { 30f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});
            #endregion

            _building_classroom = GetCLModel(Semester, Year);

            foreach (var Item in db.BUILDINGs.Where(x => x.BUILDING_NAME == BUILDING).OrderBy(x => x.CLASSROOM_NAME).ToList())
            {
                #region Header
                THSarabunfnt = new Font(bf, 30, 0);
                _chunk = new Chunk("ตารางการใช้ห้องเรียน ห้อง " + Item.CLASSROOM_NAME + " ภาคการศึกษาที่ " + semester + "-" + year, THSarabunfnt);
                _chunk.SetUnderline(1, -3);
                _pdfPCell = new PdfPCell(new Phrase(new Chunk(_chunk)));
                _pdfPCell.Colspan = _totalColumn2;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable2.AddCell(_pdfPCell);
                _pdfTable2.CompleteRow();

                THSarabunfnt = new Font(bf, 16, 0);
                _pdfPCell = new PdfPCell(new Phrase(" \n", THSarabunfnt));
                _pdfPCell.Colspan = _totalColumn2;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.Border = 0;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfPCell.ExtraParagraphSpace = 0;
                _pdfTable2.AddCell(_pdfPCell);
                _pdfTable2.CompleteRow();
                #endregion
                #region Table Header
                THSarabunfnt = new Font(bf, 24, 0);
                _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
                _pdfPCell.PaddingBottom = 10f;
                _pdfPCell.PaddingTop = 15f;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);

                for (int b = 8; b <= 21; b++)
                {
                    if (b < 10)
                    {
                        tableHeader = "0" + b + ".00";
                    }
                    else
                    {
                        tableHeader = b + ".00";
                    }
                    THSarabunfnt = new Font(bf, 24, 0);
                    _pdfPCell = new PdfPCell(new Phrase(tableHeader, THSarabunfnt));
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                _pdfTable2.CompleteRow();
                #endregion

                #region Table Body
                for (int c = 0; c < 6; c++)
                {
                    THSarabunfnt = new Font(bf, 24, 0);
                    _pdfPCell = new PdfPCell(new Phrase(" " + date_thai[c], THSarabunfnt));
                    _pdfPCell.PaddingBottom = 17f;
                    _pdfPCell.PaddingTop = 17f;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);

                    for (int d = 8; d <= 21; d++)
                    {
                        THSarabunfnt = new Font(bf, 20, 0);
                        var WhereTimeDate = _building_classroom.Where(x => x.SECTION_CLASSROOM == Item.CLASSROOM_NAME && x.SECTION_TIME_START == d && x.SECTION_DATE == date[c]);
                        var check = WhereTimeDate.LastOrDefault();
                        var check1 = _building_classroom.Where(x => x.SECTION_TIME_START <= d && x.SECTION_TIME_END > d && x.SECTION_DATE == date[c] && x.SECTION_CLASSROOM == Item.CLASSROOM_NAME).OrderBy(x => x.SECTION_TIME_START).LastOrDefault();
                        if (check != null)
                        {
                            THSarabunfnt = new Font(bf, 22, 0);
                            var trigger = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_DATE == date[c] && x.SECTION_CLASSROOM == Item.CLASSROOM_NAME).Count();
                            if (trigger == 1)
                            {

                                var TIME_START = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_START.ToString())).ToString());
                                var TIME_END = int.Parse(Math.Floor(decimal.Parse(check.SECTION_TIME_END.ToString())).ToString());
                                var TIME = TIME_END - TIME_START;

                                ClBody(TIME, WhereTimeDate);
                                THSarabunfnt = new Font(bf, 22, 0);
                            }
                            else if (trigger == 2)
                            {
                                var first = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == Item.CLASSROOM_NAME).FirstOrDefault();
                                var second = _building_classroom.Where(x => x.SUBJECT_ID == check.SUBJECT_ID && x.SECTION_DATE == date[c] && x.SECTION_BRANCH_NAME == check.SECTION_BRANCH_NAME && x.SECTION_CLASSROOM == Item.CLASSROOM_NAME).LastOrDefault();
                                int tmp_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_START.ToString())).ToString());
                                int tmp_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_END.ToString())).ToString());

                                int tmpl_first = int.Parse(Math.Floor(decimal.Parse(first.SECTION_TIME_END.ToString())).ToString());
                                int tmpl_last = int.Parse(Math.Floor(decimal.Parse(second.SECTION_TIME_START.ToString())).ToString());
                                if (tmpl_first == tmpl_last && check.SECTION_NUMBER == "")
                                {

                                }
                                else if (tmpl_first == tmpl_last)
                                {
                                    var TIME = tmp_last - tmp_first;
                                    string SECTION_NUMBER = "";
                                    if (first.SECTION_BRANCH_NAME != "")
                                    {
                                        SECTION_NUMBER = "/" + first.SECTION_NUMBER;
                                    }
                                    _pdfPCell = new PdfPCell(new Phrase(first.SUBJECT_ID + SECTION_NUMBER + "/" + first.SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                                    _pdfPCell.Colspan = TIME;
                                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                                    _pdfTable2.AddCell(_pdfPCell);
                                }
                                else if (tmpl_first != tmpl_last)
                                {
                                    var TIME_START = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_START.ToString()));
                                    var TIME_END = Math.Floor(decimal.Parse(WhereTimeDate.Last().SECTION_TIME_END.ToString()));
                                    var TIME = TIME_END - TIME_START;

                                    ClBody(int.Parse(TIME.ToString()), WhereTimeDate);
                                }
                            }
                        }
                        else
                        {
                            if (check1 == null)
                            {
                                _pdfPCell = new PdfPCell(new Phrase(" ", THSarabunfnt));
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
                    _pdfTable2.CompleteRow();
                }
                #endregion

                _document.Add(_pdfTable2);
                _document.NewPage();
                _pdfTable2 = new PdfPTable(15);
                _pdfTable2.WidthPercentage = 93;
                _pdfTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfTable2.SetWidths(new float[] { 30f, 20f, 20f, 20f, 20f, 20f, 20f, 20f,
                                                20f, 20f, 20f, 20f, 20f, 20f, 20f});
            }
            _document.Close();
            return _memoryStream.ToArray();
        }
        public void ClBody(int TIME, IEnumerable<Building_Classroom> WhereTimeDate)
        {
            string SECTION_NUMBER = "";
            if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
            {
                SECTION_NUMBER = "/" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
            }
            if (TIME == 1)
            {
                THSarabunfnt = new Font(bf, 14, 0);
                _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.First().SUBJECT_ID + SECTION_NUMBER + "/" + WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                _pdfPCell.Colspan = TIME;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            else if (TIME == 2)
            {
                string SECTION_NUMBER2 = "";
                if (WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER != "")
                {
                    SECTION_NUMBER2 = "/\n" + WhereTimeDate.OrderBy(x => x.SECTION_TIME_START).FirstOrDefault().SECTION_NUMBER;
                }
                _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.First().SUBJECT_ID + SECTION_NUMBER2 + "/" + WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                _pdfPCell.Colspan = TIME;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
            else if (TIME == 3)
            {
                if (WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Length <= 3)
                {
                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.First().SUBJECT_ID + SECTION_NUMBER + "/" + WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                    _pdfPCell.Colspan = TIME;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
                else
                {
                    _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.First().SUBJECT_ID + SECTION_NUMBER + "/\n" + WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                    _pdfPCell.Colspan = TIME;
                    _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _pdfPCell.BackgroundColor = BaseColor.WHITE;
                    _pdfTable2.AddCell(_pdfPCell);
                }
            }
            else
            {
                _pdfPCell = new PdfPCell(new Phrase(WhereTimeDate.First().SUBJECT_ID + SECTION_NUMBER + "/" + WhereTimeDate.First().SECTION_PROFESSOR_SHORTNAME.Replace('/', ','), THSarabunfnt));
                _pdfPCell.Colspan = TIME;
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable2.AddCell(_pdfPCell);
            }
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
    }
}