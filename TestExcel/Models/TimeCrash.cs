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

        public int SECTION_ID_Last { get; set; }
        public string SUBJECT_ID_Last { get; set; }
        public string SECTION_NUMBER_Last { get; set; }
        public string SUBJECT_NAME_Last { get; set; }
        public string SECTION_BRANCH_NAME_Last { get; set; }
        public string SECTION_DATE_Last { get; set; }
        public Nullable<double> SECTION_TIME_START_Last { get; set; }
        public Nullable<double> SECTION_TIME_END_Last { get; set; }
        public string SECTION_CLASSROOM_Last { get; set; }

        public string SEMESTER { get; set; }
        public string YEAR { get; set; }
    }
}