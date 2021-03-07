using NoteMarketPlace.Models;
using NoteMarketPlace.viewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    
    public class HomeController : Controller
    {
        protected NoteMarketPlaceEntities db;
        public HomeController()
        {
            db = new NoteMarketPlaceEntities();
        }
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {


            return View();
        }
        [AllowAnonymous]
        public ActionResult Serch_note()
        {
            
            var unr = (from m in db.SellerNotes select m.UniversityName).Distinct().ToList();
            var universitys = unr.Select(x => new SelectListItem() { Text = x, Value = x }).ToList();
            

            var cors = (from m in db.SellerNotes
                        select m.Course).Distinct().ToList();
            var courses = cors.Select(x => new SelectListItem() { Value = x, Text = x }).ToList();



            var ratings = new SelectList(new[]
            {
                new{ ID=1,Name=1},
                new{ ID=2,Name=2},
                new{ ID=3,Name=3},
                new{ ID=4,Name=4},
                new{ ID=5,Name=5},
            }, "ID", "Name");
            SearchNoteUniConCouCateType searchComponent = new SearchNoteUniConCouCateType()
            {
                sellerNotes = db.SellerNotes.ToList(),
                noteCategory = db.NoteCategories,
                noteType = db.NoteTypes,
                country = db.Countries,
                university = universitys,
                Cource = courses,
                rating = ratings
            };
            return View(searchComponent);
        }
        [AllowAnonymous]
        public ActionResult FAQ()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult contact_us()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult contact_us(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                buildEmailTamplateContactUs(contactUs);
                return RedirectToAction("Index");
            }
            return View();
        }

        public void buildEmailTamplateContactUs(ContactUs contactUs)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "TextContactUs" + ".cshtml");
            string Subject = contactUs.Subject;
            string cmnt = contactUs.CmntQus;
            string fullname = contactUs.fullName;
            body = body.Replace("viewbag.body", cmnt);
            body = body.Replace("viewBag.fullname", fullname);
            body = body.ToString();
            SystemConfiguration mailto = db.SystemConfigurations.Where(m => m.Key == "Semail").FirstOrDefault();
            string to = mailto.Value;
            SystemConfiguration mailfrom = db.SystemConfigurations.Where(m => m.Key == "Aemail").FirstOrDefault();
            string from = mailfrom.Value;
            buildEmailTamplate(Subject, body, to, from);
        }

        public static void buildEmailTamplate(string subject, string body, string from, string to)
        {

            string From, To, bcc, cc, Subject, Body;
            From = from.Trim();
            To = to.Trim();
            bcc = "";
            cc = "";
            Subject = subject;
            StringBuilder sb = new StringBuilder();
            sb.Append(body);
            Body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From);
            mail.To.Add(new MailAddress(To));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.Bcc.Add(new MailAddress(cc));
            }
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SendMail(mail);
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("emailID", "Password");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult Add_note()
        {
            ViewBag.initial = "initial";
            var noteDetails = new AddNotesCategoriesTypeCountry()
            {

                countries = db.Countries.ToList(),
                noteTypes = db.NoteTypes.ToList(),
                noteCategories = db.NoteCategories.ToList()

            };
            return View(noteDetails);
        }
        [HttpPost]
        public ActionResult Add_note(AddNotesCategoriesTypeCountry addNotesCategoriesTypeCountry)
        {
            if (ModelState.IsValid)
            {
                
                User user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
                string FileName = string.Empty; string FileExtension = string.Empty; string UploadPath = string.Empty; string fileUploadpath=string.Empty;
                FileExtension = Path.GetExtension(addNotesCategoriesTypeCountry.ImageFileForNote.FileName);
                if (FileExtension != ".pdf")
                {
                    ViewBag.initial = "initial";


                    addNotesCategoriesTypeCountry.countries = db.Countries.ToList();
                    addNotesCategoriesTypeCountry.noteTypes = db.NoteTypes.ToList();
                    addNotesCategoriesTypeCountry.noteCategories = db.NoteCategories.ToList();

                    
                    return View(addNotesCategoriesTypeCountry);
                }
                SellerNote noteDetail = new SellerNote();
                noteDetail.SellerID = user.id;
                noteDetail.Status = 1;
                noteDetail.Title = addNotesCategoriesTypeCountry.Title;
                noteDetail.Category = addNotesCategoriesTypeCountry.category;
                noteDetail.Country = addNotesCategoriesTypeCountry.Country;
                noteDetail.Course = addNotesCategoriesTypeCountry.Course;
                noteDetail.CourseCode = addNotesCategoriesTypeCountry.CourseCode;
                noteDetail.CreatedBy = user.id;
                noteDetail.CreatedDate = DateTime.Now;
                noteDetail.Description = addNotesCategoriesTypeCountry.Description;

                

                noteDetail.IsActive = true;
                if (addNotesCategoriesTypeCountry.gridRadios == "paid")
                {

                    noteDetail.IsPaid = true;
                    noteDetail.SellingPrice = addNotesCategoriesTypeCountry.SellingPrice;
                }
                else
                {
                    noteDetail.IsPaid = false;
                    noteDetail.SellingPrice = 0;

                }
                
                

                noteDetail.NumberOfPages = addNotesCategoriesTypeCountry.NumberOfPages;
                noteDetail.UniversityName = addNotesCategoriesTypeCountry.UniversityName;
                noteDetail.Professor = addNotesCategoriesTypeCountry.Professor;
                db.SellerNotes.Add(noteDetail);
                db.SaveChanges();
                void createPath(string Filepath)
                {
                    bool pathExist = Directory.Exists(Server.MapPath(Filepath));
                    if (!pathExist)
                    {
                        Directory.CreateDirectory(Server.MapPath(Filepath));
                    }
                }
                if (addNotesCategoriesTypeCountry.ImageFile != null)
                {
                    FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                    FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFile.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName ;
                    UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                    createPath(UploadPath);
                    fileUploadpath = Path.Combine(Server.MapPath(UploadPath),(FileName));
                    noteDetail.DisplayPicture = UploadPath + FileName;
                    addNotesCategoriesTypeCountry.ImageFile.SaveAs(fileUploadpath);
                }
                
                if (addNotesCategoriesTypeCountry.ImageFileForPreview != null)
                {

                    FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                    FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFileForPreview.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName;
                    UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                    createPath(UploadPath);
                    fileUploadpath = Path.Combine(Server.MapPath(UploadPath), (FileName));
                    noteDetail.NotesPreview = UploadPath + FileName;
                    addNotesCategoriesTypeCountry.ImageFileForPreview.SaveAs(fileUploadpath);
                }
                db.SellerNotes.Attach(noteDetail);
                db.Entry(noteDetail).Property(m => m.DisplayPicture).IsModified = true;
                db.Entry(noteDetail).Property(m => m.NotesPreview).IsModified = true;
                db.SaveChanges();
                //attechments
                noteDetail = db.SellerNotes.Where(m => m.id == noteDetail.id).SingleOrDefault();
                SellerNotesAttechment notesAttechment = new SellerNotesAttechment();

                
                //newway
                FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFileForNote.FileName);
                FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName;
                UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                createPath(UploadPath);
                fileUploadpath = Path.Combine(Server.MapPath(UploadPath), (FileName));
                notesAttechment.FilePath = UploadPath + FileName;
                addNotesCategoriesTypeCountry.ImageFileForNote.SaveAs(fileUploadpath);


                notesAttechment.NoteID = noteDetail.id;
                notesAttechment.IsActive = true;
                notesAttechment.FileName = FileName;
                notesAttechment.CreatedBy = user.id;
                notesAttechment.CreatedDate = DateTime.Now;
                db.SellerNotesAttechments.Add(notesAttechment);
                db.SaveChanges();
                return RedirectToAction("Deshboard");
            }
            else
            {
                ViewBag.initial = "initial";
                addNotesCategoriesTypeCountry.countries = db.Countries.ToList();
                addNotesCategoriesTypeCountry.noteTypes = db.NoteTypes.ToList();
                addNotesCategoriesTypeCountry.noteCategories = db.NoteCategories.ToList();

            }

            return View(addNotesCategoriesTypeCountry);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Edit_notes(int noteID)
        {
            var notedetails = db.SellerNotes.Where(m => m.id == noteID).SingleOrDefault();
            var noteAttechment = db.SellerNotesAttechments.Where(m => m.NoteID == noteID).SingleOrDefault();

            Edit_notesViewModel editNoteDetails = new Edit_notesViewModel();
            editNoteDetails.category = notedetails.Category;
            editNoteDetails.Country = notedetails.Country;
            editNoteDetails.Course = notedetails.Course;
            editNoteDetails.CourseCode = notedetails.CourseCode;
            editNoteDetails.Description = notedetails.Description;
            editNoteDetails.gridRadios=notedetails.IsPaid == true ? "paid" : "free";
            editNoteDetails.ImagePath = notedetails.DisplayPicture;
            editNoteDetails.ImagePathForPreview = notedetails.NotesPreview;
            editNoteDetails.ImagePathForNote = noteAttechment.FilePath;
            editNoteDetails.NoteType = notedetails.NoteType;
            editNoteDetails.NumberOfPages = notedetails.NumberOfPages;
            editNoteDetails.Professor = notedetails.Professor;
            editNoteDetails.SellingPrice = notedetails.SellingPrice;
            editNoteDetails.Title = notedetails.Title;
            editNoteDetails.UniversityName = notedetails.UniversityName;
            editNoteDetails.countries = db.Countries.ToList();
            editNoteDetails.noteTypes = db.NoteTypes.ToList();
            editNoteDetails.noteCategories = db.NoteCategories.ToList();
            editNoteDetails.IDofNote = noteID;
            return View("Edit_notes", editNoteDetails);
        }
        [HttpPost]
        public ActionResult Edit_notes(Edit_notesViewModel addNotesCategoriesTypeCountry) 
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
                string FileName = string.Empty; string FileExtension = string.Empty; string UploadPath = string.Empty; string fileUploadpath = string.Empty;
                if (addNotesCategoriesTypeCountry.ImageFileForNote != null)
                {
                    FileExtension = Path.GetExtension(addNotesCategoriesTypeCountry.ImageFileForNote.FileName);

                    if (FileExtension != ".pdf")
                    {
                        ViewBag.initial = "initial";


                        addNotesCategoriesTypeCountry.countries = db.Countries.ToList();
                        addNotesCategoriesTypeCountry.noteTypes = db.NoteTypes.ToList();
                        addNotesCategoriesTypeCountry.noteCategories = db.NoteCategories.ToList();

                        
                        return View(addNotesCategoriesTypeCountry);
                    }
                }
                var oldNote = db.SellerNotes.Where(m => m.id == addNotesCategoriesTypeCountry.IDofNote).SingleOrDefault();
                SellerNote noteDetail = new SellerNote();
                noteDetail.SellerID = oldNote.SellerID;
                if(addNotesCategoriesTypeCountry.btn== "btnPublish")
                {
                    noteDetail.Status = 2;
                }
                if (addNotesCategoriesTypeCountry.btn == "btnCancel" || addNotesCategoriesTypeCountry.btn == "btnSave")
                {
                    noteDetail.Status = 1;
                }
                noteDetail.Title = addNotesCategoriesTypeCountry.Title;
                noteDetail.Category = addNotesCategoriesTypeCountry.category;
                noteDetail.Country = addNotesCategoriesTypeCountry.Country;
                noteDetail.Course = addNotesCategoriesTypeCountry.Course;
                noteDetail.CourseCode = addNotesCategoriesTypeCountry.CourseCode;
                noteDetail.CreatedBy = oldNote.CreatedBy;
                noteDetail.CreatedDate = oldNote.CreatedDate;
                noteDetail.ModifiedBy = user.id;
                noteDetail.ModifiedDate = DateTime.Now;
                noteDetail.Description = addNotesCategoriesTypeCountry.Description;

                if(oldNote.IsPaid == false && addNotesCategoriesTypeCountry.gridRadios== "paid")
                {
                    if(addNotesCategoriesTypeCountry.SellingPrice == 0 || addNotesCategoriesTypeCountry.ImageFileForPreview == null)
                    {
                        if(addNotesCategoriesTypeCountry.SellingPrice == 0)
                        {
                            ViewBag.price = "Plase fill  price box";
                        }
                        if (addNotesCategoriesTypeCountry.ImageFileForPreview == null)
                        {
                            ViewBag.preview = "Plase Attach Priview File";
                        }


                        addNotesCategoriesTypeCountry.countries = db.Countries.ToList();
                        addNotesCategoriesTypeCountry.noteTypes = db.NoteTypes.ToList();
                        addNotesCategoriesTypeCountry.noteCategories = db.NoteCategories.ToList();

                        
                        return View(addNotesCategoriesTypeCountry);

                    }
                }
                noteDetail.IsActive = true;

                if (addNotesCategoriesTypeCountry.gridRadios == "paid")
                {

                    noteDetail.IsPaid = true;

                    noteDetail.SellingPrice = addNotesCategoriesTypeCountry.SellingPrice;
                    

                }
                else
                {
                    noteDetail.IsPaid = false;
                    noteDetail.SellingPrice = 0;
                   
                }

                noteDetail.NumberOfPages = addNotesCategoriesTypeCountry.NumberOfPages;
                noteDetail.UniversityName = addNotesCategoriesTypeCountry.UniversityName;
                noteDetail.Professor = addNotesCategoriesTypeCountry.Professor;
                noteDetail.id = addNotesCategoriesTypeCountry.IDofNote;

                void createPath(string Filepath)
                {
                    bool pathExist = Directory.Exists(Server.MapPath(Filepath));
                    if (!pathExist)
                    {
                        Directory.CreateDirectory(Server.MapPath(Filepath));
                    }
                }
                if (addNotesCategoriesTypeCountry.ImageFile != null)
                {
                    FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                    FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFile.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName;
                    UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                    createPath(UploadPath);
                    fileUploadpath = Path.Combine(Server.MapPath(UploadPath), (FileName));
                    noteDetail.DisplayPicture = UploadPath + FileName;
                    addNotesCategoriesTypeCountry.ImageFile.SaveAs(fileUploadpath);
                }
                else
                {
                    noteDetail.DisplayPicture = addNotesCategoriesTypeCountry.ImagePath;
                }


                if (addNotesCategoriesTypeCountry.ImageFileForPreview != null)
                {

                    FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                    FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFileForPreview.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName;
                    UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                    createPath(UploadPath);
                    fileUploadpath = Path.Combine(Server.MapPath(UploadPath), (FileName));
                    noteDetail.NotesPreview = UploadPath + FileName;
                    addNotesCategoriesTypeCountry.ImageFileForPreview.SaveAs(fileUploadpath);
                }
                else
                {
                    noteDetail.NotesPreview = addNotesCategoriesTypeCountry.ImagePathForPreview;
                }
               
                SellerNotesAttechment notesAttechment = new SellerNotesAttechment();
                var oldAttechments = db.SellerNotesAttechments.Where(m => m.NoteID == noteDetail.id).SingleOrDefault();
                if (addNotesCategoriesTypeCountry.ImageFileForNote != null)
                {
                    FileName = string.Empty; FileExtension = string.Empty; UploadPath = string.Empty;
                    FileName = System.IO.Path.GetFileName(addNotesCategoriesTypeCountry.ImageFileForNote.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + FileName;
                    UploadPath = "~/Membere/" + user.id + "/" + noteDetail.id + "/";
                    createPath(UploadPath);
                    fileUploadpath = Path.Combine(Server.MapPath(UploadPath), (FileName));
                    notesAttechment.FilePath = UploadPath + FileName;
                    addNotesCategoriesTypeCountry.ImageFileForNote.SaveAs(fileUploadpath);

                }
                else
                {
                    notesAttechment.FileName = oldAttechments.FileName;
                    notesAttechment.FilePath = oldAttechments.FilePath;

                }
                notesAttechment.IsActive = true;
                notesAttechment.CreatedBy = oldAttechments.CreatedBy;
                notesAttechment.CreatedDate = oldAttechments.CreatedDate;
                notesAttechment.ModifiedBy = user.id;
                notesAttechment.ModifiedDate = DateTime.Now;
                db.SellerNotesAttechments.Remove(oldAttechments);
                db.SellerNotes.Remove(oldNote);
                db.SaveChanges();
                notesAttechment.NoteID = noteDetail.id;
                db.SellerNotes.Add(noteDetail);
                //db.SaveChanges();


                //attechments
                db.SellerNotesAttechments.Add(notesAttechment);
                db.SaveChanges();
                return RedirectToAction("Deshboard");
            }

            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Delete_notes(int noteID, string SortOrderpub, string SortBypub, int pageOfpublished , string SortOrder, string SortBy, int pageOfDraft , string SerchText, string publishedSerchText)
        {
            var oldNote = db.SellerNotes.Where(m => m.id == noteID).SingleOrDefault();

            var oldAttechments = db.SellerNotesAttechments.Where(m => m.NoteID == oldNote.id).SingleOrDefault();
            db.SellerNotesAttechments.Remove(oldAttechments);
            db.SellerNotes.Remove(oldNote);
            db.SaveChanges();
            SerchText = string.Empty;
            publishedSerchText = string.Empty;
            return RedirectToAction("Deshboard","Home", new { SerchText, publishedSerchText, SortOrder, SortBy, SortOrderpub, SortBypub, pageOfDraft, pageOfpublished });
        }
        [HttpGet]
        public ActionResult Note_details(int idForNoteDetails)
        {
            SellerNote noteDetails = db.SellerNotes.Where(m => m.id == idForNoteDetails).SingleOrDefault();
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
             var notAllowedpaidUserForDownload = db.Downloads.Where(m => m.NoteID == noteDetails.id && m.Downloader == user.id && m.IsPaid == true && m.IsSellerHasAllowedDownload == false).SingleOrDefault();
            if (notAllowedpaidUserForDownload != null)
            {
                ViewBag.visited = "already visited paid note doesn't download user";

            }
            return View(noteDetails);
        }
       
        [HttpPost]
        public ActionResult Note_details(SellerNote notedetail)
        {
            if (!(User.Identity.IsAuthenticated))
            {
                return RedirectToAction("Login","Account");
            }
            var detailRelatedToNote = db.SellerNotes.Where(m => m.id == notedetail.id).SingleOrDefault();
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var attechment = db.SellerNotesAttechments.Where(m => m.NoteID == notedetail.id).SingleOrDefault();
            var alreadyDownloadedUser = db.Downloads.Where(m => m.NoteID == detailRelatedToNote.id && m.Downloader == user.id && (m.IsPaid==false || (m.IsPaid == true && m.IsAttechmentDownloaded == true))).SingleOrDefault();
            if (alreadyDownloadedUser != null)
            {
                alreadyDownloadedUser.ModifiedBy = user.id;
                alreadyDownloadedUser.ModifiedDate = DateTime.Now;
                alreadyDownloadedUser.AttechmentDownloadedDate = DateTime.Now;
                db.Downloads.Attach(alreadyDownloadedUser);
                db.Entry(alreadyDownloadedUser).Property(m => m.ModifiedDate).IsModified = true;
                db.Entry(alreadyDownloadedUser).Property(m => m.ModifiedBy).IsModified = true;
                db.Entry(alreadyDownloadedUser).Property(m => m.AttechmentDownloadedDate).IsModified = true;

                db.SaveChanges();

                //byte[] fileBytes = System.IO.File.ReadAllBytes(alreadyDownloadedUser.AttechmentPath);
                //string fileName = attechment.FileName;
                return File(alreadyDownloadedUser.AttechmentPath, "application/pdf", attechment.FileName);

            }                                                                                                                                                       //but alloowed to dowenload
            var AllowedpaidUserForDownload = db.Downloads.Where(m => m.NoteID == detailRelatedToNote.id && m.Downloader == user.id && m.IsPaid == true && m.IsAttechmentDownloaded == false && m.IsSellerHasAllowedDownload==true).SingleOrDefault();
            if (AllowedpaidUserForDownload != null)
            {
                AllowedpaidUserForDownload.PurchasedPrice = detailRelatedToNote.SellingPrice;
                AllowedpaidUserForDownload.AttechmentPath = attechment.FilePath;
                AllowedpaidUserForDownload.IsAttechmentDownloaded = true;
                AllowedpaidUserForDownload.AttechmentDownloadedDate = DateTime.Now;
                AllowedpaidUserForDownload.ModifiedBy = user.id;
                AllowedpaidUserForDownload.ModifiedDate = DateTime.Now;
                db.Downloads.Attach(AllowedpaidUserForDownload);
                db.Entry(AllowedpaidUserForDownload).Property(m => m.PurchasedPrice).IsModified = true;
                db.Entry(AllowedpaidUserForDownload).Property(m => m.AttechmentPath).IsModified = true;
                db.Entry(AllowedpaidUserForDownload).Property(m => m.IsAttechmentDownloaded).IsModified = true;
                db.Entry(AllowedpaidUserForDownload).Property(m => m.AttechmentDownloadedDate).IsModified = true;
                db.Entry(AllowedpaidUserForDownload).Property(m => m.ModifiedDate).IsModified = true;
                db.Entry(AllowedpaidUserForDownload).Property(m => m.ModifiedBy).IsModified = true;
                db.SaveChanges();
                //byte[] fileBytes = System.IO.File.ReadAllBytes(AllowedpaidUserForDownload.AttechmentPath);
                //string fileName = attechment.FileName;
                return File(AllowedpaidUserForDownload.AttechmentPath, "application/pdf", attechment.FileName);

            }
            var notAllowedpaidUserForDownload = db.Downloads.Where(m => m.NoteID == detailRelatedToNote.id && m.Downloader == user.id && m.IsPaid == true &&  m.IsSellerHasAllowedDownload == false).SingleOrDefault();
            if (notAllowedpaidUserForDownload != null)
            {
                var id = notAllowedpaidUserForDownload.NoteID;
                return View("Serch_note");
            }
                Download downloadEntry = new Download();
            downloadEntry.NoteID = detailRelatedToNote.id;
            downloadEntry.Seller = detailRelatedToNote.SellerID;
            downloadEntry.Downloader = user.id;
            downloadEntry.NoteTitle = detailRelatedToNote.Title;
            downloadEntry.NoteCategory = detailRelatedToNote.NoteCategory.Name;
            downloadEntry.CreatedDate = DateTime.Now;
            downloadEntry.CreatedBy = user.id;
            if (detailRelatedToNote.IsPaid == true)
            {
                if (downloadEntry.IsSellerHasAllowedDownload == true)
                {
                    downloadEntry.PurchasedPrice = detailRelatedToNote.SellingPrice;
                    downloadEntry.AttechmentPath = attechment.FilePath;
                    downloadEntry.IsAttechmentDownloaded = true;
                    downloadEntry.AttechmentDownloadedDate = DateTime.Now;
                    db.Downloads.Add(downloadEntry);
                    db.SaveChanges();
                    //byte[] fileBytes = System.IO.File.ReadAllBytes(downloadEntry.AttechmentPath);
                    //string fileName = attechment.FileName;
                    return File(downloadEntry.AttechmentPath, "application/pdf", attechment.FileName);

                }
            }
            else
            {
                downloadEntry.PurchasedPrice = 0;
                downloadEntry.IsSellerHasAllowedDownload = true;
                downloadEntry.AttechmentPath = attechment.FilePath;
                //string path = AppDomain.CurrentDomain.BaseDirectory + "FolderName/";
                downloadEntry.IsAttechmentDownloaded = true;
                downloadEntry.AttechmentDownloadedDate = DateTime.Now;
                db.Downloads.Add(downloadEntry);
                db.SaveChanges();
                //byte[] fileBytes = System.IO.File.ReadAllBytes(downloadEntry.AttechmentPath);
                //string fileName = attechment.FileName;
                return File(downloadEntry.AttechmentPath, "application/pdf", attechment.FileName);

            }
            downloadEntry.IsSellerHasAllowedDownload = false;
            downloadEntry.IsPaid = true;
            downloadEntry.PurchasedPrice = detailRelatedToNote.SellingPrice;
            downloadEntry.IsAttechmentDownloaded = false;
            db.Downloads.Add(downloadEntry);
            db.SaveChanges();
            ViewBag.visited = "already visited paid note doesn't download user";
            ViewBag.buyerName = user.FirstName;
            var sellerName = db.Users.Where(m => m.id == detailRelatedToNote.SellerID).FirstOrDefault();
            
            
            ViewBag.sellerFullName = (sellerName.FirstName + " " + sellerName.LastName).ToString();

            buildEmailTamplateDownloadRequest(downloadEntry);
            ViewBag.forpopupModel = "mail has been sent";
            return View(detailRelatedToNote);
        }
        public void buildEmailTamplateDownloadRequest(Download downloadEntry)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "TextDownloadRequest" + ".cshtml");
            SystemConfiguration mailto = db.SystemConfigurations.Where(m => m.Key == "Semail").FirstOrDefault();
            string from = mailto.Value;
            var sellerName = db.Users.Where(m => m.id == downloadEntry.Seller).SingleOrDefault();
            var downloaderName = db.Users.Where(m => m.id == downloadEntry.Downloader).SingleOrDefault();
            string to = sellerName.EmailID; 
            body = body.Replace("ViewBag.sellerName", sellerName.FirstName);
            body = body.Replace("ViewBag.byureName", downloaderName.FirstName);
            body = body.ToString();
            string Subject = downloaderName.FirstName + " wants to purchase your notes";

            buildEmailTamplate(Subject, body, from,to);

        }

        [HttpGet]
        [Authorize]
        public ActionResult ByuerRequest(string SortOrder,string SortBy,string searchtext,int pagenumber=1)
        {
            ViewBag.sortorder = SortOrder;
            ViewBag.sortby = SortBy;
            ViewBag.pageNumber = pagenumber;
            var User = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var ReqForDonwloadNoteList = db.Downloads.Where(m => m.Seller == User.id && m.IsSellerHasAllowedDownload==false).ToList();
            List<BuyerRequestViewModel> requestList = new List<BuyerRequestViewModel>();
            foreach(Download item in ReqForDonwloadNoteList)
            {
                BuyerRequestViewModel obj = new BuyerRequestViewModel();
                obj.emailID = db.Users.Where(m => m.id == item.Downloader).Select(m=>m.EmailID).SingleOrDefault();
                obj.phoneNumber = db.UserProfiles.Where(m => m.UserID == item.Downloader).Select(m => m.PhoneNumber).SingleOrDefault();
                obj.phoneNumber = "+"+(db.UserProfiles.Where(m => m.UserID == item.Downloader).Select(m => m.PhoneNumber_code).SingleOrDefault())+" " + obj.phoneNumber;
                var dprice= item.PurchasedPrice;
                obj.price = (int)dprice;
                obj.Title = item.NoteTitle;
                obj.DownloadedTime = (DateTime)(item.CreatedDate);
                obj.category = item.NoteCategory;
                obj.sellType = item.IsPaid;
                obj.noteID = item.NoteID;
                obj.buyer = item.Downloader;
                requestList.Add(obj);
            }
            ViewBag.Totalpage = (double)(Math.Ceiling(requestList.Count() / 10.0));
            if (searchtext != null)
            {
                requestList = requestList.Where(m => m.Title.ToLower().Contains(searchtext.ToLower()) || 
                                (m.price.ToString().ToLower()).Contains(searchtext.ToLower()) || m.phoneNumber.Contains(searchtext.ToLower()) ||
                                m.category.ToLower().Contains(searchtext.ToLower()) || m.emailID.ToLower().Contains(searchtext.ToLower()) ||
                                m.DownloadedTime.ToString("dd MMM yyyy, hh:mm:ss").ToLower().Contains(searchtext.ToLower())).ToList();

            }
            requestList=sortingAlgo(requestList, SortOrder, SortBy);
            requestList = requestList.Skip((pagenumber - 1) * 10).Take(10).ToList();
            
                    return View(requestList);
        }
        public List<BuyerRequestViewModel> sortingAlgo(List<BuyerRequestViewModel> requestList,string SortOrder,string SortBy)
        {
            switch (SortBy)
            {
                case "DATE":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.DownloadedTime).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.DownloadedTime).ToList();
                                    break;
                                }
                            default:
                                {
                                    requestList = requestList.OrderByDescending(m => m.DownloadedTime).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "NOTETITLE":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    requestList = requestList.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTY":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    requestList = requestList.OrderBy(m => m.category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "BUYER":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.emailID).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.emailID).ToList();
                                    break;
                                }
                            default:
                                {
                                    requestList = requestList.OrderBy(m => m.emailID).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PHONENO":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.phoneNumber).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.phoneNumber).ToList();
                                    break;
                                }
                            default:
                                
                                {
                                    requestList = requestList.OrderBy(m => m.phoneNumber).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PRICE":
                    {
                        switch (SortOrder)
                        {
                            case "dsc":
                                {
                                    requestList = requestList.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    requestList = requestList.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            default:
                                
                                {
                                    requestList = requestList.OrderBy(m => m.price).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        requestList = requestList.OrderByDescending(m => m.DownloadedTime).ToList();
                        break;
                    }
                    
            }
            return requestList;
        }
        [Authorize]
        public ActionResult RequestApproved(int noteID, int buyerID)
        {
            var sellerID = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            var updateRow = db.Downloads.Where(m => m.NoteID == noteID && m.Seller == sellerID && m.Downloader == buyerID).SingleOrDefault();
            if (updateRow.IsSellerHasAllowedDownload == false)
            {
                updateRow.IsSellerHasAllowedDownload = true;
                updateRow.ModifiedDate = DateTime.Now;
                updateRow.ModifiedBy = sellerID;
                db.Downloads.Attach(updateRow);
                db.Entry(updateRow).Property(m => m.IsSellerHasAllowedDownload).IsModified = true;
                db.Entry(updateRow).Property(m => m.ModifiedBy).IsModified = true;
                db.Entry(updateRow).Property(m => m.ModifiedDate).IsModified = true;
                db.SaveChanges();
            }
               
            buildEmailTamplateApprovedRequest(updateRow);
            return RedirectToAction( "ByuerRequest", "Home");
        }
        public void buildEmailTamplateApprovedRequest(Download updateRow)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "TextapprovedRequest" + ".cshtml");
            string from = db.SystemConfigurations.Where(m => m.Key == "Semail").Select(m => m.Value).FirstOrDefault();
            string to = db.Users.Where(m => m.id == updateRow.Downloader).Select(m => m.EmailID).FirstOrDefault();
            string sellername = db.Users.Where(m => m.id == updateRow.Seller).Select(m => m.FirstName).FirstOrDefault();
            string Buyerrname = db.Users.Where(m => m.id == updateRow.Downloader).Select(m => m.FirstName).FirstOrDefault();

            body = body.Replace("ViewBag.Slrnm", sellername);
            body = body.Replace("ViewBag.Byrnm", Buyerrname);
            body = body.ToString();
            string Subject = sellername + " Allows you to download a note";
            buildEmailTamplate(Subject, body, from, to);


        }
        [HttpGet]
        [Authorize]
        public ActionResult Deshboard(string SerchText, string publishedSerchText, string SortOrder, string SortBy, string SortOrderpub, string SortBypub, int pageOfDraft = 1, int pageOfpublished = 1)

        {
            ViewBag.sortordr = SortOrder;
            ViewBag.sortby = SortBy;
            ViewBag.pagenumber = pageOfDraft;
            ViewBag.sortordrpub = SortOrderpub;
            ViewBag.sortbypub = SortBypub;
            ViewBag.pagenumberpub = pageOfpublished;
            User user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();

            ViewBag.TpageForReview = (double)Math.Ceiling(db.SellerNotes.Where(m => m.SellerID == user.id && (m.Status == 1 || m.Status == 2 || m.Status == 3)).Count() / 5.0);
            ViewBag.TpageForpublished = (double)Math.Ceiling(db.SellerNotes.Where(m => m.SellerID == user.id && m.Status == 4).Count() / 5.0);
            var userNoteRelateddetails = new SellerNoteDownload();

            userNoteRelateddetails.sellernotes = db.SellerNotes.OrderByDescending(m => m.CreatedDate).Where(m => m.SellerID == user.id && (m.Status == 1 || m.Status == 2 || m.Status == 3)).ToList();
            userNoteRelateddetails.sellernotes2 = db.SellerNotes.OrderByDescending(m => m.CreatedDate).Where(m => m.SellerID == user.id && m.Status == 4).ToList();
            userNoteRelateddetails.categories = db.NoteCategories.ToList();
            userNoteRelateddetails.Request = db.Downloads.Where(m => m.Seller == user.id && m.IsSellerHasAllowedDownload == false).Count();
            userNoteRelateddetails.rejected = db.SellerNotes.Where(m => m.SellerID == user.id && m.Status == 5).Count();
            userNoteRelateddetails.NoOfDownload = db.Downloads.Where(m => m.Seller == user.id && m.IsAttechmentDownloaded == true).Count();
            var earning= db.Downloads.Where(m => m.Seller == user.id && m.IsSellerHasAllowedDownload == true && m.IsAttechmentDownloaded == true).Sum(m => m.PurchasedPrice);
            if(earning != null)
            {
                userNoteRelateddetails.totalEarning =(int)earning;

            }
            else
            {
                userNoteRelateddetails.totalEarning = 0;
            }
            var TotalNote = db.Downloads.Where(m => m.Seller == user.id).Select(m => m.NoteID).Distinct().ToList();
            var NoOfnotesold = 0;
            foreach(var id in TotalNote)
            {
                var isNoteSold = db.Downloads.Where(m => m.NoteID == id && m.IsAttechmentDownloaded == true).SingleOrDefault();
                if (isNoteSold != null)
                {
                    NoOfnotesold++;
                }
            }
            //userNoteRelateddetails.notesold = db.Downloads.Where(m => m.Seller == user.id).Select(m => m.NoteID).Count();
            userNoteRelateddetails.notesold = NoOfnotesold;
            var finallist = userNoteRelateddetails.sellernotes;
            var finallist2 = userNoteRelateddetails.sellernotes2;
            if (SerchText != null)
            {
                finallist = finallist.Where(m => m.Title.Contains(SerchText) || m.NoteStatu.value.Contains(SerchText) || m.NoteCategory.Name.Contains(SerchText)).ToList();

            }

            if (publishedSerchText != null)
            {

                finallist2 = finallist2.Where(m => m.Title.Contains(publishedSerchText) || m.NoteStatu.value.Contains(publishedSerchText) || (m.SellingPrice).ToString() == (publishedSerchText)).ToList();

            }
            finallist = shortingFirst(SortOrder, SortBy, finallist);
            finallist2 = shortingsecond(SortOrderpub, SortBypub, finallist2);
            finallist = pagingfirst(finallist, pageOfDraft);
            finallist2 = pagingsecond(finallist2, pageOfpublished);
            userNoteRelateddetails.sellernotes = finallist;
            userNoteRelateddetails.sellernotes2 = finallist2;

            return View(userNoteRelateddetails);
        }
        public IEnumerable<SellerNote> shortingFirst(string SortOrder, string SortBy, IEnumerable<SellerNote> finallist)
        {
            switch (SortBy)
            {
                case "ADDED DATE":
                    {
                        switch (SortOrder)
                        {
                            case "asc":
                                {
                                    finallist = finallist.OrderBy(m => m.CreatedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    finallist = finallist.OrderByDescending(m => m.CreatedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    finallist = finallist.OrderByDescending(m => m.CreatedDate).ToList();

                                    break;
                                }
                        }
                        break;
                    }

                case "TITLE":
                    {
                        switch (SortOrder)
                        {
                            case "asc":
                                {
                                    finallist = finallist.OrderBy(m => m.Title).ToList();

                                    break;
                                }
                            case "dsc":
                                {
                                    finallist = finallist.OrderByDescending(m => m.Title).ToList();

                                    break;
                                }
                            default:
                                {
                                    finallist = finallist.OrderBy(m => m.Title).ToList();

                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTY":
                    {
                        switch (SortOrder)
                        {
                            case "asc":
                                {
                                    finallist = finallist.OrderBy(m => m.NoteCategory.Name).ToList();

                                    break;
                                }
                            case "dsc":
                                {
                                    finallist = finallist.OrderByDescending(m => m.NoteCategory.Name).ToList();

                                    break;
                                }
                            default:
                                {
                                    finallist = finallist.OrderBy(m => m.NoteCategory.Name).ToList();

                                    break;
                                }
                        }
                        break;
                    }
                case "STATUS":
                    {
                        switch (SortOrder)
                        {
                            case "asc":
                                {
                                    finallist = finallist.OrderBy(m => m.NoteStatu.value).ToList();

                                    break;
                                }
                            case "dsc":
                                {
                                    finallist = finallist.OrderByDescending(m => m.NoteStatu.value).ToList();

                                    break;
                                }
                            default:
                                {
                                    finallist = finallist.OrderBy(m => m.NoteStatu.value).ToList();

                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        finallist = finallist.OrderByDescending(m => m.CreatedDate).ToList();

                        break;
                    };
            }
            return finallist;

        }
        public IEnumerable<SellerNote> shortingsecond(string SortOrderpub, string SortBypub, IEnumerable<SellerNote> finallist2)
        {
            switch (SortBypub)
            {
                case "ADDEDDATEForPublished":
                    {
                        switch (SortOrderpub)
                        {
                            case "asc":
                                {
                                    finallist2 = finallist2.OrderBy(m => m.CreatedDate).ToList();

                                    break;
                                }
                            case "dsc":
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.CreatedDate).ToList();

                                    break;
                                }
                            default:
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.CreatedDate).ToList();

                                    break;
                                }
                        }
                        break;
                    }

                case "TITLEForPublished":
                    {
                        switch (SortOrderpub)
                        {
                            case "asc":
                                {
                                    finallist2 = finallist2.OrderBy(m => m.Title).ToList();


                                    break;
                                }
                            case "dsc":
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.Title).ToList();

                                    break;
                                }
                            default:
                                {
                                    finallist2 = finallist2.OrderBy(m => m.Title).ToList();


                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTYForPublished":
                    {
                        switch (SortOrderpub)
                        {
                            case "asc":
                                {
                                    finallist2 = finallist2.OrderBy(m => m.NoteCategory.Name).ToList();


                                    break;
                                }
                            case "dsc":
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.NoteCategory.Name).ToList();


                                    break;
                                }
                            default:
                                {
                                    finallist2 = finallist2.OrderBy(m => m.NoteCategory.Name).ToList();


                                    break;
                                }
                        }
                        break;
                    }
                case "SELLTYPEForPublished":
                    {
                        switch (SortOrderpub)
                        {
                            case "asc":
                                {
                                    finallist2 = finallist2.OrderBy(m => m.IsPaid).ToList();


                                    break;
                                }
                            case "dsc":
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.IsPaid).ToList();


                                    break;
                                }
                            default:
                                {
                                    finallist2 = finallist2.OrderBy(m => m.IsPaid).ToList();


                                    break;
                                }
                        }
                        break;
                    }
                case "PRICEForPublished":
                    {
                        switch (SortOrderpub)
                        {
                            case "asc":
                                {
                                    finallist2 = finallist2.OrderBy(m => m.SellingPrice).ToList();


                                    break;
                                }
                            case "dsc":
                                {
                                    finallist2 = finallist2.OrderByDescending(m => m.SellingPrice).ToList();


                                    break;
                                }
                            default:
                                {
                                    finallist2 = finallist2.OrderBy(m => m.SellingPrice).ToList();


                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        finallist2 = finallist2.OrderByDescending(m => m.CreatedDate).ToList();

                        break;
                    };
            }
            return finallist2;
        }
        public IEnumerable<SellerNote> pagingfirst(IEnumerable<SellerNote> finallist, int pageOfDraft)
        {
            return finallist.Skip((pageOfDraft - 1) * 5).Take(5).ToList();
        }
        public IEnumerable<SellerNote> pagingsecond(IEnumerable<SellerNote> finallist2, int pageOfpublished)
        {
            return finallist2.Skip((pageOfpublished - 1) * 5).Take(5).ToList();
        }

        

    }
}
