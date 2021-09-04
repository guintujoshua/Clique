using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clique.Models;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Dynamic;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Diagnostics;
using System.Web.Security;

namespace Clique.Controllers
{
    [Authorize]
    public class CliqueController : Controller
    {
        // GET: Clique
        [AllowAnonymous]
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }
        [AllowAnonymous]
        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult resetqueue()
        {
            string a = "{ crudsubject :false, claimofrequestdoc :false, completionofgrade :false, documentreq :false, subjectcrediting :false, certificationverificationauthentication :false, clearance :false, crossenrollment :false, droppingcourse :true, others :false, admission :false}";
            DateTime thisday = DateTime.Today;
            QTableEntities db = new QTableEntities();
            var item = db.QTables.ToList();
            foreach (var record in item)
            {
                db.QTables.Remove(record);
            }
            db.SaveChanges();
            var createitem = db.QTables.Create();
            createitem.ID = 1;
            createitem.QNBlue = 0;
            createitem.QNGreen = 0;
            createitem.QNRed = 0;
            createitem.QNYellow = 0;
            createitem.QNWhite = 0;
            createitem.QNPink = 0;
            createitem.UID = "test";
            createitem.Service = a;
            createitem.UnitColor = "test";
            createitem.Fname = "test";
            createitem.Mname = "test";
            createitem.Lname = "test";
            createitem.Course = "test";
            createitem.LastSchool = "test";
            createitem.QDate = thisday ;
            createitem.QStatus = "test";
            createitem.ClientType = "test";
            db.QTables.Add(createitem);
            db.SaveChanges();

            return View("AdminDashboard",db.QTables.ToList());
           
        }
        [AllowAnonymous]
        public ActionResult Login(Account model)
        {
            QTableEntities db = new QTableEntities();
            AccountModelEntities dbs = new AccountModelEntities();
            try
            {
                //var A = dbs.Accounts.Where(x => x.UserName.ToString().Equals(model.UserName.ToString()) && x.Password.ToString().Equals(model.Password.ToString())).ToList().FirstOrDefault();

                var users = dbs.Accounts.Where(x => x.UserName.ToString().Equals(model.UserName.ToString())).ToList();
                var b = users.SingleOrDefault(x => x.Password.ToString().Equals(model.Password.ToString()));
                if (b.UnitType == "client")
                {
                    FormsAuthentication.SetAuthCookie(b.UserName, true);
                    return View("Client");
                }
                else if (b.UnitType == "admin")
                {
                    FormsAuthentication.SetAuthCookie(b.UserName, true);
                    return View("AdminDashboard", db.QTables.ToList());
                }
                else if (b.UnitType == "staff")
                {
                    if (b.UnitColor == "red")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("RedCounter", db.QTables.ToList());
                    }
                    else if (b.UnitColor == "pink")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("PinkCounter", db.QTables.ToList());
                    }
                    else if (b.UnitColor == "yellow")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("YellowCounter", db.QTables.ToList());
                    }
                    else if (b.UnitColor == "white")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("WhiteCounter", db.QTables.ToList());
                    }
                    else if (b.UnitColor == "green")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("GreenCounter", db.QTables.ToList());
                    }
                    else if (b.UnitColor == "blue")
                    {
                        FormsAuthentication.SetAuthCookie(b.UserName, true);
                        return View("BlueCounter", db.QTables.ToList());
                    }

                }
            }
            catch
            {
                return Content("<script type='text/javascript'>alert('Invalid Credentials');location.href='Index';</script>");
            }
            return Content("<script type='text/javascript'>alert('Invalid Credentials');location.href='Index';</script>");


        }

        // Admin
       
        public ActionResult AdminDashboard()
        {
            QTableEntities db = new QTableEntities();

            return View(db.QTables.ToList());
        }
        public ActionResult AddAccount()
        {

            return View();
        }
        public ActionResult AddAcctestount()
        {


            return View();
        }
        public ActionResult EditAccount(int id)
        {
            AccountModelEntities db = new AccountModelEntities();
           var item = db.Accounts.Where(x => x.ID == id).FirstOrDefault();


            TempData["AID"] = item.ID;
            return View(item);
        }
        public ActionResult SaveEdit(Account model)
        {
            AccountModelEntities db = new AccountModelEntities();
            model.ID=Int32.Parse(TempData["AID"].ToString());
            var item = db.Accounts.Where(x => x.ID ==model.ID ).FirstOrDefault();
            item.Fname = model.Fname;
            item.Lname = model.Lname;
            item.Mname = model.Mname;
            item.UserName = model.UserName;
            item.Password = model.Password;
            item.UnitType = model.UnitType;
            if (model.UnitType == "admin" || model.UnitType == "client")
            {
                item.UnitColor = "--";
            }
            else
            {
                item.UnitColor = model.UnitColor;
            }
            db.SaveChanges();


            return View("Accounts",db.Accounts.ToList());
        }
        

        public ActionResult UpdateTotalServed(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();
            try
            {
             

                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();

                return View("AdminDashboard", b);
            }
            catch(ArgumentException e)
            {
                return View("AdminDashboard",db.QTables.ToList());
            }
            
        }

        
        public ActionResult UpdateBlueCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "blue" && element.ClientType=="Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "blue" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("BlueCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "blue" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "blue" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("BlueCounterReports",tupleModel);
            }


        }
        public ActionResult UpdateWhiteCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "white" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "white" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("WhiteCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "white" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "white" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("WhiteCounterReports", tupleModel);
            }


        }
        public ActionResult UpdateGreenCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "green" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "green" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("GreenCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "green" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "green" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("GreenCounterReports", tupleModel);
            }
        }
        public ActionResult UpdateRedCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "red" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "red" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("RedCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "red" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "red" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("RedCounterReports", tupleModel);
            }
        }
        public ActionResult UpdatePinkCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "pink" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "pink" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("PinkCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "pink" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "pink" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("PinkCounterReports", tupleModel);
            }
        }
        public ActionResult WhiteGreenCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "white" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "white" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("WhiteCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "white" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "white" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("WhiteCounterReports", tupleModel);
            }
        }
        public ActionResult UpdateYellowCounterReports(Nullable<DateTime> Start, Nullable<DateTime> End)
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            try
            {
                var a = db.QTables.Where(x => x.QDate > Start).ToList();
                var b = a.Where(x => x.QDate < End).ToList();


                foreach (var element in b.ToList())
                {
                    if (element.UnitColor == "yellow" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "yellow" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }

                }
                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("YellowCounterReports", tupleModel);
            }
            catch
            {
                foreach (var element in db.QTables.ToList())
                {
                    if (element.UnitColor == "yellow" && element.ClientType == "Student" || element.ClientType == "Others")
                    {
                        serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                    }
                    else if (element.UnitColor == "yellow" && element.ClientType == "Faculty")
                    {
                        serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                    }
                }


                var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
                return View("YellowCounterReports", tupleModel);
            }
        }


        public ActionResult Admissionsform()
        {
            return View();
        }
      

        // Client
        public ActionResult Client()
        {
            return View();
        }

        public ActionResult Nationality()
        {
            return View();
        }

        public ActionResult Services()
        {
           var id = TempData["UID"].ToString();
            TempData["UID"] = id;
            var client = TempData["Client"].ToString();
            TempData["Client"] = client;

            return View();
        }
        public ActionResult facultyservices()
        {
            var id = TempData["FID"].ToString();
            TempData["FID"] = id;

            return View();
        }

        public ActionResult Faculty()
        {
            return View();
        }

        public ActionResult StudentCourses()
        {
            return View();
        }
        public ActionResult AdmissionReports()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }
        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            QTableEntities db = new QTableEntities();
           

            var item = db.QTables.Where(x => x.UnitColor == "pink").ToList();
            var items = item.Where(x => x.QStatus == "served").ToList();

            List<admissiontoexport> toexport = new List<admissiontoexport>();
       
            foreach (var test in items)
            {
                admissiontoexport lalagyan = new admissiontoexport();
                lalagyan.fname = test.Fname;
                lalagyan.mname = test.Mname;
                lalagyan.lname = test.Lname;
                lalagyan.lastschool = test.LastSchool;
                lalagyan.course = test.Course;
                lalagyan.QDate = test.QDate;
                toexport.Add(lalagyan);
                

            }
            gv.DataSource = toexport.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdmissionReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("AdmissionReports");
        }
        public ActionResult ExportToExcelstudent()
        {
            var gv = new GridView();
            QTableEntities db = new QTableEntities();


            var item = db.QTables.Where(x => x.ClientType == "Student").ToList();
            var items = item.Where(x => x.QStatus == "served").ToList();

            List<studenttoexport> toexport = new List<studenttoexport>();
          
            foreach (var test in items)
            {
                studenttoexport lalagyan = new studenttoexport();
                lalagyan.fname = test.Fname;
                lalagyan.mname = test.Mname;
                lalagyan.lname = test.Lname;
                lalagyan.course = test.Course;
                lalagyan.QDate = test.QDate;
                lalagyan.StudentID = test.UID;
                lalagyan.services = test.Service;
                if (test.UnitColor == "white")
                {
                    lalagyan.nationality = "Foreign";
                }
                else
                {
                    lalagyan.nationality = "Filipino";
                }
                toexport.Add(lalagyan);
               


            }
            gv.DataSource = toexport.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=StudentReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("StudentReports");
        }
        public ActionResult ExportToExcelfaculty()
        {
            var gv = new GridView();
            QTableEntities db = new QTableEntities();


            var item = db.QTables.Where(x => x.ClientType == "Faculty").ToList();
            var items = item.Where(x => x.QStatus == "served").ToList();

            List<facultytoexport> toexport = new List<facultytoexport>();

            foreach (var test in items)
            {
                facultytoexport lalagyan = new facultytoexport();
                lalagyan.FacultyID = test.UID;
                lalagyan.fname = test.Fname;
                lalagyan.mname = test.Mname;
                lalagyan.lname = test.Lname;
                lalagyan.services = test.Service;
                lalagyan.course = test.Course;
                lalagyan.QDate = test.QDate;
                toexport.Add(lalagyan);


            }
            gv.DataSource = toexport.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=FacultyReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("FacultyReports");
        }
        public ActionResult ExportToExcelOthers()
        {
            var gv = new GridView();
            QTableEntities db = new QTableEntities();


            var item = db.QTables.Where(x => x.ClientType == "Others").ToList();
            var items = item.Where(x => x.QStatus == "served").ToList();

            List<studenttoexport> toexport = new List<studenttoexport>();

            foreach (var test in items)
            {
                studenttoexport lalagyan = new studenttoexport();
                lalagyan.fname = test.Fname;
                lalagyan.mname = test.Mname;
                lalagyan.lname = test.Lname;
                lalagyan.course = test.Course;
                lalagyan.QDate = test.QDate;
                lalagyan.StudentID = test.UID;
                lalagyan.services = test.Service;
                lalagyan.nationality = "Others";
                toexport.Add(lalagyan);



            }
            gv.DataSource = toexport.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=OthersReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("OthersReports");
        }
        public ActionResult StudentReports()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }
        public ActionResult FacultyReports()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }
    
        public ActionResult OthersReports()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }
        public ActionResult FacultyCourses()
        {
            return View();
        }
     
        public ActionResult BlueCounterReports()
        {
            QTableEntities db = new QTableEntities();
        
            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if(element.UnitColor == "blue" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "blue" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);


        }
        public ActionResult GreenCounterReports()
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if (element.UnitColor == "green" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "green" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);
        }
        public ActionResult RedCounterReports()
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if (element.UnitColor == "red" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "red" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);
        }
        public ActionResult PinkCounterReports()
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if (element.UnitColor == "pink" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "pink" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);
        }
        public ActionResult WhiteCounterReports()
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if (element.UnitColor == "white" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "white" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);
        }
        public ActionResult YellowCounterReports()
        {
            QTableEntities db = new QTableEntities();

            List<Services> serv = new List<Services>();
            List<FACULTYSERVICE> serv2 = new List<FACULTYSERVICE>();
            foreach (var element in db.QTables.ToList())
            {
                if (element.UnitColor == "yellow" && element.ClientType == "Student" || element.ClientType == "Others")
                {
                    serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
                }
                else if (element.UnitColor == "yellow" && element.ClientType == "Faculty")
                {
                    serv2.Add(JsonConvert.DeserializeObject<FACULTYSERVICE>(element.Service));
                }
            }


            var tupleModel = new Tuple<List<Services>, List<FACULTYSERVICE>>(serv, serv2);
            return View(tupleModel);
        }
        // Employee
        public ActionResult PinkCounter()
        {
            /**QTableEntities db = new QTableEntities();

            return View(db.QTables.ToList());**/

            QTableEntities db = new QTableEntities();
        
            List<Services> serv = new List<Services>();
            foreach (var element in db.QTables.ToList())
            {
                serv.Add(JsonConvert.DeserializeObject<Services>(element.Service));
            }
          

            var tupleModel = new Tuple<List<QTable>, List<Services>>(db.QTables.ToList(), serv);
            return View(tupleModel);
        }



        public ActionResult Serving(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();

                return View("PinkCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("PinkCounter", db.QTables.ToList());
            }
        }
        public ActionResult Servingw(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();

                return View("WhiteCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("WhiteCounter", db.QTables.ToList());
            }
        }
        public ActionResult Servingy(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();

                return View("YellowCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("YellowCounter", db.QTables.ToList());
            }
        }
        public ActionResult Servingb(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();

                return View("BlueCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("BlueCounter", db.QTables.ToList());
            }
        }
        public ActionResult Servingg(int id)
        {
            try
            {


                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();

                return View("GreenCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("GreenCounter", db.QTables.ToList());
            }
        }
        public ActionResult Servingr(int id)
        {
            try
            {


                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "serving";
                db.SaveChanges();
                return View("RedCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("RedCounter", db.QTables.ToList());
            }
        }
        public ActionResult DoneServed(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();




                return View("PinkCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("PinkCounter", db.QTables.ToList());
            }
        }
        public ActionResult DoneServedw(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();

              
                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();


                return View("WhiteCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("WhiteCounter", db.QTables.ToList());
            }
          
        }
        public ActionResult DoneServedy(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();
                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();



                return View("YellowCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("YellowCounter", db.QTables.ToList());
            }
        }
        public ActionResult DoneServedr(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();
                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();



                return View("RedCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("RedCounter", db.QTables.ToList());
            }
        }
        public ActionResult DoneServedb(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();

             

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();



                return View("BlueCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

      
                return View("BlueCounter", db.QTables.ToList());
            }
        }
        public ActionResult DoneServedg(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "served";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();

                return View("GreenCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("GreenCounter", db.QTables.ToList());
            }
        }
        public ActionResult Notserved(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();



                return View("PinkCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("PinkCounter", db.QTables.ToList());
            }
        }
        public ActionResult Notservedr(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();

                return View("RedCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("RedCounter", db.QTables.ToList());
            }
        }
        public ActionResult Notservedw(int id)
        {

            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();



                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();


                return View("WhiteCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("WhiteCounter", db.QTables.ToList());
            }
           
        }
        public ActionResult Notservedy(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();


                return View("YellowCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("YellowCounter", db.QTables.ToList());
            }
        }

        public ActionResult Notservedb(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();
                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();

                return View("BlueCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();
                return View("BlueCounter", db.QTables.ToList());
            }
        }
        public ActionResult Notservedg(int id)
        {
            try
            {
                QTableEntities db = new QTableEntities();

                var item = db.QTables.Where(x => x.ID == id).ToList().Last();
                item.QStatus = "notserved";
                db.SaveChanges();

                backupdbEntities backup = new backupdbEntities();
                var a = backup.backuptables.Create();
                a.ID = item.ID;
                a.QNPink = item.QNPink;
                a.QNBlue = item.QNBlue;
                a.QNWhite = item.QNWhite;
                a.QNRed = item.QNRed;
                a.QNYellow = item.QNYellow;
                a.QNGreen = item.QNGreen;
                a.UnitColor = item.UnitColor;
                a.Service = item.Service;
                a.Fname = item.Fname;
                a.Mname = item.Mname;
                a.Lname = item.Lname;
                a.Course = item.Course;
                a.LastSchool = item.LastSchool;
                a.QDate = item.QDate;
                a.Qstatus = item.QStatus;
                a.ClientType = item.ClientType;
                backup.backuptables.Add(a);
                backup.SaveChanges();


                return View("GreenCounter", db.QTables.ToList());
            }
            catch
            {
                QTableEntities db = new QTableEntities();

                return View("GreenCounter", db.QTables.ToList());
            }
        }

        public ActionResult openservice(int id)
        {
            QTableEntities db = new QTableEntities();
            var item = db.QTables.Where(x => x.ID == id).ToList().Last();
            Services serv = new Services();
            serv = JsonConvert.DeserializeObject<Services>(item.Service);
            TempData["SID"] = id;
            TempData["UCOLOR"] = item.UnitColor;
            return View(serv);
        }

        public ActionResult openfacultyservice(int id)
        {
            QTableEntities db = new QTableEntities();
            var item = db.QTables.Where(x => x.ID == id).ToList().Last();
            FACULTYSERVICE serv = new FACULTYSERVICE();
            serv = JsonConvert.DeserializeObject<FACULTYSERVICE>(item.Service);
            TempData["SID"] = id;
            TempData["UCOLOR"] = item.UnitColor;
            return View(serv);
        }

        public ActionResult facultyupdateservice(FACULTYSERVICE service)
        {
            int id = Int32.Parse(TempData["SID"].ToString());
            string color = TempData["UCOLOR"].ToString();
            QTableEntities db = new QTableEntities();
            var item = db.QTables.Where(x => x.ID == id).ToList().Last();
            string serlist = JsonConvert.SerializeObject(service);
            item.Service = serlist;
            db.SaveChanges();


            if (color == "pink")
            {
                return View("PinkCounter", db.QTables.ToList());
            }
            else if (color == "white")
            {
                return View("WhiteCounter", db.QTables.ToList());
            }
            else if (color == "yellow")
            {
                return View("YellowCounter", db.QTables.ToList());
            }
            else if (color == "blue")
            {
                return View("BlueCounter", db.QTables.ToList());
            }
            else if (color == "green")
            {
                return View("GreenCounter", db.QTables.ToList());
            }
            else if (color == "red")
            {
                return View("RedCounter", db.QTables.ToList());
            }
            else
            {
                return View("PinkCounter", db.QTables.ToList());
            }


        }

        public ActionResult updateservice (Services service)
        {
            int id = Int32.Parse(TempData["SID"].ToString());
            string color = TempData["UCOLOR"].ToString();
            QTableEntities db = new QTableEntities();
            var item = db.QTables.Where(x => x.ID == id).ToList().Last();
            string serlist = JsonConvert.SerializeObject(service);
            item.Service = serlist;
            db.SaveChanges();

            if( color == "pink")
            {
                return View("PinkCounter", db.QTables.ToList());
            }else if( color == "white")
            {
                return View("WhiteCounter", db.QTables.ToList());
            }
            else if (color == "yellow")
            {
                return View("YellowCounter", db.QTables.ToList());
            }
            else if (color == "blue")
            {
                return View("BlueCounter", db.QTables.ToList());
            }
            else if (color == "green")
            {
                return View("GreenCounter", db.QTables.ToList());
            }
            else if (color == "red")
            {
                return View("RedCounter", db.QTables.ToList());
            }
            else
            {
                return View("PinkCounter", db.QTables.ToList());
            }

           
        }
        public ActionResult WhiteCounter()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }

        public ActionResult YellowCounter()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }

        public ActionResult BlueCounter()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }

        public ActionResult RedCounter()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }

        public ActionResult GreenCounter()
        {
            QTableEntities db = new QTableEntities();
            return View(db.QTables.ToList());
        }

        // View Queue
        [AllowAnonymous]
        public ActionResult Queue()
        {
            QTableEntities db = new QTableEntities();

            return View(db.QTables.ToList());
        }



       
        //mga bagong gawa

        public ActionResult Admission(Admission admission)
        {
            if(admission.course ==null || admission.fname == null || admission.mname == null || admission.lname == null || admission.lastschool == null)
            {
                return Content("<script type='text/javascript'>alert('Please fill in all the information.');location.href='admissionsform';</script>");

            }
            else
            {
                DateTime thisday = DateTime.Today;
                QTableEntities db = new QTableEntities();
                var last = db.QTables.ToList().LastOrDefault();
                Services services = new Services();
                services.admission = true;
                string serlist = JsonConvert.SerializeObject(services);
                TicketModel ticket = new TicketModel();
                QTable table = new QTable();
                table.QNPink = last.QNPink + 1;
                table.QNBlue = last.QNBlue;
                table.QNWhite = last.QNWhite;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "pink";
                table.Service = serlist;
                table.Fname = admission.fname;
                table.Mname = admission.mname;
                table.Lname = admission.lname;
                table.Course = admission.course;
                table.LastSchool = admission.lastschool;
                table.QDate = thisday.Date;
                table.QStatus = "waiting";
                table.ClientType = "Student";
                db.QTables.Add(table);
                db.SaveChanges();
                ticket.Qnumber = (int)table.QNPink;
                ticket.UnitColor = table.UnitColor + " Counter";

                return View("queueticket", ticket);

            }
           

        }
    

        public ActionResult foreign()
        {
            
            return View("IDForeign");

        }



        //latest add ons na pages

        
        public ActionResult queueticket()
        {
            return View();
        }
        public ActionResult facultyform()
        {
            return View();
        }
        public ActionResult othersform()
        {
            return View();
        }
        public ActionResult studentform()
        {
            return View();
        }
        public ActionResult OthersCourses()
        {
            return View();
        }
        [Authorize]
        public ActionResult Accounts()
        {
            AccountModelEntities db = new AccountModelEntities();
           
            return View(db.Accounts.ToList());
        }
        public ActionResult CreateNewAccount(string UnitType,string Fname, string Mname, string Lname, string UserName, string Password, string UnitColor)
        {
            AccountModelEntities db = new AccountModelEntities();
            var item = db.Accounts.ToList().LastOrDefault();
            var check = db.Accounts.ToList();
            foreach (var i in check )
            {
                if(i.UserName.ToLower() == UserName.ToLower() && i.Password.ToLower() == Password.ToLower())
                {
                    return View("AddAccount");
                }
            }
            Account Uinput = new Account();
            try
            {
                Uinput.ID = item.ID + 1;
            }catch(Exception e)
            {
                Uinput.ID = 1;
            }
        
            Uinput.UnitType = UnitType;
            Uinput.Fname = Fname;
            Uinput.Mname = Mname;
            Uinput.Lname = Lname;
            Uinput.UserName = UserName;
            Uinput.Password = Password;
            if(UnitType == "admin" || UnitType == "client")
            {
                Uinput.UnitColor = "--";
            }
            else
            {
                Uinput.UnitColor = UnitColor;
            }
           
            
            db.Accounts.Add(Uinput);
            db.SaveChanges();

            return View("Accounts", db.Accounts.ToList());
        }
        public ActionResult DeleteAccount(int id)
        {
            AccountModelEntities db = new AccountModelEntities();
            var item = db.Accounts.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                db.Accounts.Remove(item);
                db.SaveChanges();
            }
            return View("Accounts", db.Accounts.ToList());
        }


        public ActionResult ViewAccount()
        {
            return View();
        }

       
        public ActionResult ServiceCheckbox(Services services)
        {

            DateTime thisday = DateTime.Today;
            var id = TempData["UID"].ToString();
            var client = TempData["Client"].ToString();
            AUFMISSMODELDB db = new AUFMISSMODELDB();
            var item = db.AUFMISSDBs.Where(x => x.ID == id).ToList().Last();
            string serlist = JsonConvert.SerializeObject(services);
            
            QTableEntities db2 = new QTableEntities();
            var last = db2.QTables.ToList().LastOrDefault();

            TicketModel ticket = new TicketModel();
            QTable table = new QTable();
            table.QNPink = last.QNPink;
            table.QNWhite = last.QNWhite;


            table.UID = id;
            table.Service = serlist;
            table.Fname = item.Fname;
            table.Mname = item.Mname;
            table.Lname = item.Lname;
            table.Course = item.Course;
            table.QDate = thisday;
            table.QStatus = "waiting";
            table.ClientType = client;

            if (client == "Studentf")
            {
                if (services.claimofrequestdoc == false && services.clearance == false && services.completionofgrade == false && services.crossenrollment == false && services.certificationverificationauthentication == false &&
               services.crudsubject == false && services.documentreq == false && services.droppingcourse == false && services.others == false && services.subjectcrediting == false)
                {
                    TempData["UID"] = id;
                    TempData["Client"] = client;
                    return Content("<script type='text/javascript'>alert('Please select a service.');location.href='Services';</script>");
                }


                table.ClientType = "Student";
                table.QNPink = last.QNPink;
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "white";
                table.QNWhite = table.QNWhite + 1;
                ticket.Qnumber = (int)table.QNWhite;
            }
            else if (services.claimofrequestdoc == false && services.clearance == false && services.completionofgrade == false && services.crossenrollment == false && services.certificationverificationauthentication == false &&
            services.crudsubject == false && services.documentreq == false && services.droppingcourse == false && services.others == false && services.subjectcrediting == false)
            {
                
                TempData["UID"] = id;
                TempData["Client"] = client;
                return Content("<script type='text/javascript'>alert('Please select a service.');location.href='Services';</script>");

            }
            else if (services.claimofrequestdoc == true && services.clearance == false && services.completionofgrade == false && services.crossenrollment == false && services.certificationverificationauthentication == false &&
             services.crudsubject == false && services.documentreq == false && services.droppingcourse == false && services.others==false && services.subjectcrediting ==false)
            {
                table.QNPink = last.QNPink + 1;
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "pink";
                ticket.Qnumber = (int)table.QNPink;
            }
            else if (item.Course == "BSN" || item.Course == "BSBA-MM" || item.Course == "BSBA-ME" || item.Course == "BSBA-HRDM" || item.Course == "BSIT" || item.Course == "BMMA" || item.Course == "BSCS")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow + 1;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "yellow";
                ticket.Qnumber = (int)table.QNYellow;

            }
            else if (item.Course == "BSMT" || item.Course == "BSPT" || item.Course == "BSOT" || item.Course == "BSRT" || item.Course == "BSPHARMA" || item.Course == "BSCLINPHARMA")
            {
                table.QNBlue = last.QNBlue + 1;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "blue";
                ticket.Qnumber = (int)table.QNBlue;
            }
            else if (item.Course == "BSCRIM" || item.Course == "BEED" || item.Course == "BSPE" || item.Course == "BSED" || item.Course == "BSARCHI" || item.Course == "BSCvE" || item.Course == "BSCoE" || item.Course == "BSEE" || item.Course == "BSA" || item.Course == "BSMA" || item.Course == "BSTM" || item.Course == "BSHRM")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed + 1;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "red";
                ticket.Qnumber = (int)table.QNRed;
            }
            else if (item.Course == "ABCOMM" || item.Course == "BSBIO" || item.Course == "BSPSYCH" || item.Course == "ABPSYCH")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen + 1;
                table.UnitColor = "green";
                ticket.Qnumber = (int)table.QNGreen;
            }

            db2.QTables.Add(table);
            db2.SaveChanges();
            ticket.UnitColor = table.UnitColor + " Counter";
            return View("queueticket", ticket);
        }

        public ActionResult FacultyServiceCheckbox(FACULTYSERVICE services)
        {
            DateTime thisday = DateTime.Today;
            var id = TempData["FID"].ToString();
            AUFMISSMODELDB db = new AUFMISSMODELDB();
            var item = db.AUFMISSDBs.Where(x => x.ID == id).ToList().Last();
            string serlist = JsonConvert.SerializeObject(services);

            QTableEntities db2 = new QTableEntities();
            var last = db2.QTables.ToList().LastOrDefault();

            TicketModel ticket = new TicketModel();
       


            QTable table = new QTable();
            table.QNPink = last.QNPink;
            table.QNWhite = last.QNWhite;
            if (services.clearance == false && services.verification == false && services.others == false) {
                TempData["FID"] = id;
                TempData["Client"] = "Faculty";
                return Content("<script type='text/javascript'>alert('Please select a service.');location.href='facultyservices';</script>");
            }
            else if (item.Course == "BSN" || item.Course == "BSBA-MM" || item.Course == "BSBA-ME" || item.Course == "BSBA-HRDM" || item.Course == "BSIT" || item.Course == "BMMA" || item.Course == "BSCS")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow + 1;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "yellow";
                ticket.Qnumber = (int)table.QNYellow;

            }
            else if (item.Course == "BSMT" || item.Course == "BSPT" || item.Course == "BSOT" || item.Course == "BSRT" || item.Course == "BSPHARMA" || item.Course == "BSCLINPHARMA")
            {
                table.QNBlue = last.QNBlue + 1;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "blue";
                ticket.Qnumber = (int)table.QNBlue;
            }
            else if (item.Course == "BSCRIM" || item.Course == "BEED" || item.Course == "BSPE" || item.Course == "BSED" || item.Course == "BSARCHI" || item.Course == "BSCvE" || item.Course == "BSCoE" || item.Course == "BSEE" || item.Course == "BSA" || item.Course == "BSMA" || item.Course == "BSTM" || item.Course == "BSHRM")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed + 1;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen;
                table.UnitColor = "red";
                ticket.Qnumber = (int)table.QNRed;
            }
            else if (item.Course == "ABCOMM" || item.Course == "BSBIO" || item.Course == "BSPSYCH" || item.Course == "ABPSYCH")
            {
                table.QNBlue = last.QNBlue;
                table.QNRed = last.QNRed;
                table.QNYellow = last.QNYellow;
                table.QNGreen = last.QNGreen + 1;
                table.UnitColor = "green";
                ticket.Qnumber = (int)table.QNGreen;
            }

            table.UID = id;
            table.Service = serlist;
            table.Fname = item.Fname;
            table.Mname = item.Mname;
            table.Lname = item.Lname;
            table.Course = item.Course;
            table.QDate = thisday;
            table.ClientType = "Faculty";
            table.QStatus = "waiting";
            db2.QTables.Add(table);
            db2.SaveChanges();
            ticket.UnitColor = table.UnitColor+" Counter";




            return View("queueticket", ticket);
        }

        public ActionResult IDStudent()
        {
            return View();
        } public ActionResult IDOthers()
        {
            return View();
        }
        
        public ActionResult IDINPUTS(ID ids)
        {
            try
            {
               
                AUFMISSMODELDB db = new AUFMISSMODELDB();
                var item = db.AUFMISSDBs.Where(x => x.ID == ids.id).ToList();
                if(item.Count > 0)
                {
                    TempData["UID"] = ids.id.ToString();
                    TempData["Client"] = "Student";
                    return View("Services");
                }
                else
                {
                   
                    return Content("<script type='text/javascript'>alert('Invalid ID');location.href='IDStudent';</script>");

                }


            }
            catch (Exception ex)
            {
                return View("IDStudent");
            }
                
           
      
        }
        public ActionResult IDINPUTOTHER(ID ids)
        {
            try
            {
                AUFMISSMODELDB db = new AUFMISSMODELDB();
                var item = db.AUFMISSDBs.Where(x => x.ID == ids.id).ToList();
                if (item.Count > 0)
                {
                    TempData["UID"] = ids.id.ToString();
                    TempData["Client"] = "Others";
                    return View("Services");
                }
                else
                {
                    
                    return Content("<script type='text/javascript'>alert('Invalid ID');location.href='IDOthers';</script>");
                }
            }
             catch (Exception ex)
            {
                return View("IDOthers");
            }
        }

        public ActionResult IDForeign()
        {
            return View();
        }
        public ActionResult IDFaculty()
        {
            return View();
        }
        public ActionResult IDINPUTSF(ID ids)
        {
            try
            {
                AUFMISSMODELDB db = new AUFMISSMODELDB();
                var item = db.AUFMISSDBs.Where(x => x.ID == ids.id).ToList();
                if (item.Count > 0)
                {
                    TempData["UID"] = ids.id.ToString();
                    TempData["Client"] = "Studentf";
                    return View("Services");
                }
                else
                {
                    return Content("<script type='text/javascript'>alert('Invalid ID');location.href='IDForeign';</script>");

                }
            }
            catch (Exception ex)
            {
                return View("IDForeign");
            }



        }


        public ActionResult IDINPUTSFACULTY(ID ids)
        {
            try
            {
                AUFMISSMODELDB db = new AUFMISSMODELDB();
                var item = db.AUFMISSDBs.Where(x => x.ID == ids.id).ToList();
                if (item.Count > 0)
                {
                    TempData["FID"] = ids.id.ToString();
                    TempData["Client"] = "Faculty";
                    return View("facultyservices");
                }
                else
                {
                    return Content("<script type='text/javascript'>alert('Invalid ID');location.href='IDFaculty';</script>");

                }
            }
              catch (Exception ex)
            {
                return View("IDFaculty");
            }
        }

    }
}