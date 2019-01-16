using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestExcel.Models
{
    public class TimeCrash
    {
        public int SECTION_ID_First { get; set; }
        public string SUBJECT_ID_First { get; set; }
        public string SECTION_NUMBER_First { get; set; }
        public string SUBJECT_NAME_First { get; set; }
        public string SECTION_BRANCH_NAME_First { get; set; }
        public string SECTION_DATE_First { get; set; }
        public Nullable<double> SECTION_TIME_START_First { get; set; }
        public Nullable<double> SECTION_TIME_END_First { get; set; }
        public string SECTION_CLASSROOM_First { get; set; }
        public string SECTION_PROFESSOR_First { get; set; }

        public int SECTION_ID_Second { get; set; }
        public string SUBJECT_ID_Second { get; set; }
        public string SECTION_NUMBER_Second { get; set; }
        public string SUBJECT_NAME_Second { get; set; }
        public string SECTION_BRANCH_NAME_Second { get; set; }
        public string SECTION_DATE_Second { get; set; }
        public Nullable<double> SECTION_TIME_START_Second { get; set; }
        public Nullable<double> SECTION_TIME_END_Second { get; set; }
        public string SECTION_CLASSROOM_Second { get; set; }
        public string SECTION_PROFESSOR_Second { get; set; }

        public int SECTION_ID_Third { get; set; }
        public string SUBJECT_ID_Third { get; set; }
        public string SECTION_NUMBER_Third { get; set; }
        public string SUBJECT_NAME_Third { get; set; }
        public string SECTION_BRANCH_NAME_Third { get; set; }
        public string SECTION_DATE_Third { get; set; }
        public Nullable<double> SECTION_TIME_START_Third { get; set; }
        public Nullable<double> SECTION_TIME_END_Third { get; set; }
        public string SECTION_CLASSROOM_Third { get; set; }
        public string SECTION_PROFESSOR_Third { get; set; }

        public string SEMESTER { get; set; }
        public string YEAR { get; set; }
        public string TIME_CRASH { get; set; }
        public string TEACHER_CRASH { get; set; }
    }
}