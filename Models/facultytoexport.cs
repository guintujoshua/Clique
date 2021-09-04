using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clique.Models
{
    public class facultytoexport
    {
        public string FacultyID { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public string course { get; set; }
        public string services { get; set; }

        public Nullable<System.DateTime> QDate { get; set; }
    }
}