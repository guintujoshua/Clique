﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clique.Models
{
    public class admissiontoexport
    {
        public string fname { get; set; }

        public string mname { get; set; }
        public string lname { get; set; }
        public string course { get; set; }
        public string lastschool { get; set; }
        public Nullable<System.DateTime> QDate { get; set; }

    }
}