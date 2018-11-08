using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestExcel.Data;
using TestExcel.Models;

namespace TestExcel.Report
{
    public class Notification
    {
        int notification = 0;
        int tmp;
        string[] date = { "M", "T", "W", "H", "F", "S" };
        TestExcelEntities db = new TestExcelEntities();
        List<Section_Subject> _section_subject = new List<Section_Subject>();
        public int GetNotification()
        {
            var semesteryear = from d1 in db.SECTIONs.Select(x => new { x.SEMESTER, x.YEAR }).Distinct()
                               select new SemesterYear
                               {
                                   SEMESTER_YEAR = d1.SEMESTER + "/" + d1.YEAR,
                                   SEMESTER = d1.SEMESTER,
                                   YEAR = d1.YEAR
                               };


            foreach (var i in db.BRANCHes)
            {
                foreach (var j in semesteryear)
                {
                    _section_subject = GetModel(i.BRANCH_NAME, j.SEMESTER, j.YEAR);

                    foreach (var m in db.BUILDINGs)
                    {
                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 8; l <= 21; l++)
                            {
                                var WhereTimeDate = _section_subject.Where(x => x.SECTION_TIME_START == l && x.SECTION_DATE == date[k] && x.SECTION_CLASSROOM == m.CLASSROOM_NAME && x.SECTION_NUMBER != " ").Count();
                                if (WhereTimeDate > 1)
                                {
                                    tmp = WhereTimeDate / 2;
                                    notification += tmp;
                                }
                            }
                        }
                    }
                }
            }
            return notification;
        }

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
    }
}