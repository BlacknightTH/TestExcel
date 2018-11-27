using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestExcel.Models
{
    public class Building_Classroom
    {
        public int SECTION_ID { get; set; }
        public string SUBJECT_ID { get; set; }
        public string SECTION_NUMBER { get; set; }
        public string SUBJECT_NAME { get; set; }
        public string SUBJECT_CREDIT { get; set; }
        public string SECTION_BRANCH_NAME { get; set; }
        public string SECTION_DATE { get; set; }
        public Nullable<double> SECTION_TIME_START { get; set; }
        public Nullable<double> SECTION_TIME_END { get; set; }
        public string SECTION_CLASSROOM { get; set; }
        public string SECTION_PROFESSOR_SHORTNAME { get; set; }
        public string SEMESTER { get; set; }
        public string YEAR { get; set; }
        public string BUILDING_NAME { get; set; }
        
    }
}