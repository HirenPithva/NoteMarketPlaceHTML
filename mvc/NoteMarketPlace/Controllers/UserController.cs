using NoteMarketPlace.Models;
using NoteMarketPlace.viewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoteMarketPlace.Controllers
{
    public class UserController : Controller
    {
        protected NoteMarketPlaceEntities db;
        public UserController()
        {
            db = new NoteMarketPlaceEntities();
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult UserProfile()
        {
            var loginUser = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            var user = new UserProfileViewModel();
            var userprofile = db.UserProfiles.Where(m => m.UserID == loginUser.id).SingleOrDefault();
            if (userprofile != null)
            {
                ViewBag.editMode = "userprofile already exist";
                user.FirstName = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.FirstName).FirstOrDefault();
                user.LastName = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.LastName).FirstOrDefault();
                user.Email = System.Web.HttpContext.Current.User.Identity.Name;
                user.genderList = db.genders.ToList();
                user.countryList = db.Countries.ToList();
                user.Address1 = userprofile.AddressLine1;
                user.Address2 = userprofile.AddressLine2;
                if (userprofile.ProfilePicture!=null)
                {
                    user.profilepicName = Path.GetFileNameWithoutExtension(Server.MapPath(userprofile.ProfilePicture));
                    user.profilepicPath = userprofile.ProfilePicture;
                }
                    
                ViewBag.birthdate= userprofile.DOB;
                user.BirthDate = userprofile.DOB;
                user.city = userprofile.City;
                user.college = userprofile.College;
                user.countryCode = userprofile.PhoneNumber_code;
                user.gender = userprofile.Gender;
                user.PhonNumber = userprofile.PhoneNumber;
                
                user.state = userprofile.State;
                user.University = userprofile.University;
                user.usrCountry = userprofile.Country;
                user.zipCode = userprofile.ZipCode;
            }
            else
            {
                user.FirstName = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.FirstName).FirstOrDefault();
                user.LastName = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.LastName).FirstOrDefault();
                user.Email = System.Web.HttpContext.Current.User.Identity.Name;
                user.genderList = db.genders.ToList();
                user.countryList = db.Countries.ToList();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult UserProfile(UserProfileViewModel user)
        {
            user.genderList = db.genders.ToList();
            user.countryList = db.Countries.ToList();
            if (ModelState.IsValid)
            {
                User usrname = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();

                var userprofile = db.UserProfiles.Where(m => m.UserID == usrname.id).SingleOrDefault();
                if (userprofile != null)
                {
                    usrname.FirstName = user.FirstName;
                    usrname.LastName = user.LastName;
                    db.Users.Attach(usrname);
                    db.Entry(usrname).Property(m => m.FirstName).IsModified = true;
                    db.Entry(usrname).Property(m => m.LastName).IsModified = true;
                    db.SaveChanges();
                    userprofile.AddressLine1 = user.Address1;
                    userprofile.AddressLine2 = user.Address2;
                    userprofile.City = user.city;
                    userprofile.College = user.college;
                    userprofile.Country = user.usrCountry;
                    userprofile.ModifiedDate = DateTime.Now;
                    userprofile.ModifiedBy = usrname.id;
                    userprofile.DOB = user.BirthDate;
                    userprofile.Gender = user.gender;
                    userprofile.PhoneNumber = user.PhonNumber;
                    userprofile.PhoneNumber_code = user.countryCode;
                    if (user.profilepicture != null)
                    {
                        string filename = System.IO.Path.GetFileName(user.profilepicture.FileName);
                        filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + filename;
                        string FilePath = "~/Membere/" + usrname.id + "/";
                        bool exist = Directory.Exists(Server.MapPath(FilePath));
                        if (!exist)
                        {
                            Directory.CreateDirectory(Server.MapPath(FilePath));
                        }
                        userprofile.ProfilePicture = FilePath + filename;
                        user.profilepicPath = Path.Combine(Server.MapPath(FilePath), (filename));
                        user.profilepicture.SaveAs(user.profilepicPath);
                    }
                    else
                    {
                        userprofile.ProfilePicture = user.profilepicPath;
                    }
                    userprofile.State = user.state;
                    userprofile.University = user.University;
                    userprofile.ZipCode = user.zipCode;
                    db.UserProfiles.Attach(userprofile);
                    db.Entry(userprofile).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Serch_note", "Home");
                }
                else
                {
                    usrname.FirstName = user.FirstName;
                    usrname.LastName = user.LastName;
                    db.Users.Attach(usrname);
                    db.Entry(usrname).Property(m => m.FirstName).IsModified = true;
                    db.Entry(usrname).Property(m => m.LastName).IsModified = true;
                    db.SaveChanges();
                    UserProfile profile = new UserProfile();
                    profile.UserID = usrname.id;
                    profile.DOB = user.BirthDate;
                    profile.Gender = user.gender;
                    if ((user.PhonNumber == null && user.countryCode == null) || (user.PhonNumber != null && user.countryCode != null))
                    {
                        profile.PhoneNumber_code = user.countryCode;
                        profile.PhoneNumber = user.PhonNumber;
                    }

                    if (user.profilepicture != null)
                    {
                        string filename = System.IO.Path.GetFileName(user.profilepicture.FileName);
                        filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-" + filename;
                        string FilePath = "~/Membere/" + usrname.id + "/";
                        bool exist = Directory.Exists(Server.MapPath(FilePath));
                        if (!exist)
                        {
                            Directory.CreateDirectory(Server.MapPath(FilePath));
                        }
                        profile.ProfilePicture = FilePath + filename;
                        user.profilepicPath = Path.Combine(Server.MapPath(FilePath), (filename));
                        user.profilepicture.SaveAs(user.profilepicPath);
                    }
                    

                    profile.AddressLine1 = user.Address1;
                    profile.AddressLine2 = user.Address2;
                    profile.City = user.city;
                    profile.State = user.state;
                    profile.ZipCode = user.zipCode;
                    profile.Country = user.usrCountry;
                    profile.University = user.University;
                    profile.College = user.college;
                    profile.CreatedDate = DateTime.Now;
                    profile.CreatedBy = usrname.id;
                    db.UserProfiles.Add(profile);
                    db.SaveChanges();
                    return RedirectToAction("Serch_note", "Home");
                }
            }
            return View(user);
        }






        [Authorize]
        public ActionResult MyDonwloads(string sortOrder,string sortBy,string searchtext,int currentPage=1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            if (TempData.ContainsKey("addReview"))
            {
                ViewBag.ReviewMessage = TempData["addReview"] as string;
                TempData.Remove("addReview");

            }
            if (TempData.ContainsKey("Reload"))
            {
                ViewBag.reload = TempData["reload"];
                TempData.Remove("Reload");

            }
            List<MyDownloadViewModel> listOfDownload = new List<MyDownloadViewModel>();
            var Buser = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var listofdownloadTB = db.Downloads.Where(m => m.Downloader == Buser.id && m.IsSellerHasAllowedDownload==true).ToList();
            foreach(var item in listofdownloadTB)
            {
                MyDownloadViewModel obj = new MyDownloadViewModel();
                obj.NoteTitle = item.NoteTitle;
                obj.isNoteDownloaded = item.IsAttechmentDownloaded;
                obj.downloadedTime = item.AttechmentDownloadedDate;
                obj.Category = item.NoteCategory;
                obj.price = item.PurchasedPrice;
                obj.selltype = item.IsPaid;
                obj.noteid = item.NoteID;
                obj.isAlreadyReported = db.SellerNotesReportedIssues.Any(m => m.AgainstDownloadID == item.id);
                obj.downloadIDofTB = item.id;
                obj.sellerName = db.Users.Where(m => m.id == item.Seller).Select(m => m.EmailID).FirstOrDefault();
                listOfDownload.Add(obj);
            }
            if (searchtext != null)
            {
                if(searchtext.ToLower() == "free")
                {
                    listOfDownload = listOfDownload.Where(m => m.selltype == false).ToList();
                }
                else if (searchtext.ToLower() == "paid")
                {
                    listOfDownload = listOfDownload.Where(m => m.selltype == true).ToList();
                }
                else
                {
                    listOfDownload = listOfDownload.Where(m => m.NoteTitle.ToLower().Contains(searchtext.ToLower()) ||
                                 m.Category.ToLower().Contains(searchtext.ToLower()) || m.sellerName.ToLower().Contains(searchtext.ToLower()) ||
                                 m.price.ToString().Contains(searchtext) || m.downloadedTime.ToString().Contains(searchtext)).ToList();

                }

            }

            listOfDownload=sorting(sortOrder, sortBy, listOfDownload);

            ViewBag.TotalPage = Math.Ceiling(listOfDownload.Count() / 10.0);
            listOfDownload = listOfDownload.Skip((currentPage - 1) * 10).Take(10).ToList();
            return View(listOfDownload);
        }

        public ActionResult DownloadAttechment(int NoteID)
        {
            SellerNote notedetail = db.SellerNotes.Where(m => m.id == NoteID).SingleOrDefault();
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var attechment = db.SellerNotesAttechments.Where(m => m.NoteID == notedetail.id).SingleOrDefault();
            if (TempData.ContainsKey("returnFile"))
            {
                TempData.Remove("returnFile");
                FileDownload obj = new FileDownload();
                var ListOfFile = obj.GetFile(attechment.FilePath).ToList();
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchiv = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < ListOfFile.Count(); i++)
                        {
                            zipArchiv.CreateEntryFromFile(ListOfFile[i].FilePath, ListOfFile[i].FileName);
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                }
            }
            var alreadyDownloadedUser = db.Downloads.Where(m => m.NoteID == notedetail.id && m.Downloader == user.id && (m.IsPaid == false || (m.IsPaid == true && m.IsAttechmentDownloaded == true))).SingleOrDefault();
            if (alreadyDownloadedUser != null)
            {
                alreadyDownloadedUser.ModifiedBy = user.id;
                alreadyDownloadedUser.ModifiedDate = DateTime.Now;
                db.Downloads.Attach(alreadyDownloadedUser);
                db.Entry(alreadyDownloadedUser).State = EntityState.Modified;

                db.SaveChanges();
                FileDownload obj = new FileDownload();
                var ListOfFile = obj.GetFile(attechment.FilePath).ToList();
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchiv = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < ListOfFile.Count(); i++)
                        {
                            zipArchiv.CreateEntryFromFile(ListOfFile[i].FilePath, ListOfFile[i].FileName);
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                }
            }

            var AllowedpaidUserForDownload = db.Downloads.Where(m => m.NoteID == notedetail.id && m.Downloader == user.id && m.IsPaid == true && m.IsAttechmentDownloaded == false && m.IsSellerHasAllowedDownload == true).SingleOrDefault();
            if (AllowedpaidUserForDownload != null)
            {
                AllowedpaidUserForDownload.PurchasedPrice = notedetail.SellingPrice;
                AllowedpaidUserForDownload.AttechmentPath = attechment.FilePath;
                AllowedpaidUserForDownload.IsAttechmentDownloaded = true;
                AllowedpaidUserForDownload.AttechmentDownloadedDate = DateTime.Now;
                AllowedpaidUserForDownload.ModifiedBy = user.id;
                AllowedpaidUserForDownload.ModifiedDate = DateTime.Now;
                db.Downloads.Attach(AllowedpaidUserForDownload);
                db.Entry(AllowedpaidUserForDownload).State = EntityState.Modified;
                 db.SaveChanges();
            }
            TempData["Reload"] = NoteID;

            return RedirectToAction("MyDonwloads", "User");
        }
        public ActionResult downloadNote(int NoteID)
        {
            TempData["returnFile"] = "returnFile";
            return RedirectToAction("DownloadAttechment","User",new { NoteID });
        }
        public List<MyDownloadViewModel> sorting(string sortOrder, string sortBy, List<MyDownloadViewModel> listOfDownload)
        {
            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.NoteTitle).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.NoteTitle).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.NoteTitle).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTY":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.Category).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.Category).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.Category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "Seller":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.sellerName).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.sellerName).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.sellerName).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "SELLTYPE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.selltype).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.selltype).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.selltype).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PRICE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.price).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DOWNLOAD":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.downloadedTime).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.downloadedTime).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.downloadedTime).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        listOfDownload = listOfDownload.OrderByDescending(m => m.downloadedTime).ToList();
                        break;
                    }
            }
            return listOfDownload;

        }
        [HttpPost]
        public ActionResult AddReview(string Comments,string rate,string DonwloadID)
        {
            if(Comments !="" && rate!=null)
            {
                int IDofDownload = int.Parse(DonwloadID);
                var reviewRecored = db.SellerNotesReviews.Where(m => m.AgainstDownloadsID == IDofDownload).SingleOrDefault();

                if(reviewRecored != null)
                {
                    reviewRecored.Ratings = int.Parse(rate); 
                    reviewRecored.Comments = Comments;
                    reviewRecored.ModifiedBy = reviewRecored.ReviewedByID;
                    reviewRecored.ModifiedDate = DateTime.Now;
                    db.SellerNotesReviews.Attach(reviewRecored);
                    db.Entry(reviewRecored).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var downloadRecored = db.Downloads.Where(m => m.id == IDofDownload).SingleOrDefault();
                    var review = new SellerNotesReview();
                    review.NoteID = downloadRecored.NoteID;
                    review.ReviewedByID = downloadRecored.Downloader;
                    review.AgainstDownloadsID = downloadRecored.id;
                    review.Ratings = int.Parse(rate);
                    review.Comments = Comments;
                    review.CreatedDate = DateTime.Now;
                    review.CreatedBy = downloadRecored.Downloader;
                    review.IsActive = true;
                    db.SellerNotesReviews.Add(review);
                    db.SaveChanges();
                }
                

            }
            else
            {
                TempData["addReview"] = DonwloadID;
            }

            return RedirectToAction("MyDonwloads", "User");
        }
        [HttpPost]
        public ActionResult Inappropriate(string Remark,string DownloadID)
        {
            int downloadID = int.Parse(DownloadID);
            var downloadRecored = db.Downloads.Where(m => m.id == downloadID).SingleOrDefault();

            SellerNotesReportedIssue NewReport = new SellerNotesReportedIssue();
            NewReport.NoteID = downloadRecored.NoteID;
            NewReport.ReportedByID = downloadRecored.Downloader;
            NewReport.AgainstDownloadID = downloadID;
            NewReport.Remarks = Remark;
            NewReport.CreatedDate = DateTime.Now;
            NewReport.CreatedBy= downloadRecored.Downloader;
            
            db.SellerNotesReportedIssues.Add(NewReport);
            db.SaveChanges();
            buildEmailTamplateReportedIssues(NewReport);
            return RedirectToAction("MyDonwloads");
        }
        public void buildEmailTamplateReportedIssues(SellerNotesReportedIssue NewReport)
        {
            var DownloadRecored = db.Downloads.Where(m => m.id == NewReport.AgainstDownloadID).SingleOrDefault();
            var buyer = db.Users.Where(m => m.id == DownloadRecored.Downloader).Select(m => m.FirstName).SingleOrDefault();
            var seller = db.Users.Where(m => m.id == DownloadRecored.Seller).Select(m => m.FirstName).SingleOrDefault();
            var Title = DownloadRecored.NoteTitle;
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/")+ "TextReportIssues"+ ".cshtml");
            body = body.Replace("ViewBag.bn", buyer);
            body = body.Replace("ViewBag.sn", seller);
            body = body.Replace("ViewBag.nt", Title);
            body = body.ToString();
            string subject=buyer +" Reported an issue for " +Title;
            string to = db.SystemConfigurations.Where(m => m.Key == "Aemail").Select(m => m.Value).SingleOrDefault();
            string from = db.SystemConfigurations.Where(m => m.Key == "Semail").Select(m => m.Value).SingleOrDefault();
            HomeController.buildEmailTamplate(subject, body, from, to);
        }






        [HttpGet]
        [Authorize]
        public ActionResult MysoldNotes(string sortOrder, string sortBy, string searchtext, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            List<MySoldNotesViewModel> listOfnote = new List<MySoldNotesViewModel>();
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var sellerNoteList = db.Downloads.Where(m => m.Seller == user.id && m.IsSellerHasAllowedDownload==true).ToList();
            foreach(var item in sellerNoteList)
            {
                MySoldNotesViewModel obj = new MySoldNotesViewModel();
                obj.BuyerName = db.Users.Where(m => m.id == item.Downloader).Select(m => m.FirstName).SingleOrDefault();
                obj.Category = item.NoteCategory;
                obj.NoteTitle = item.NoteTitle;
                obj.noteid = item.NoteID;
                obj.downloadedTime = item.AttechmentDownloadedDate;
                obj.price = item.PurchasedPrice;
                obj.selltype = item.IsPaid;
                listOfnote.Add(obj);
            }
            if (searchtext != null)
            {
                if (searchtext.ToLower() == "free")
                {
                    listOfnote = listOfnote.Where(m => m.selltype == false).ToList();
                }
                else if (searchtext.ToLower() == "paid")
                {
                    listOfnote = listOfnote.Where(m => m.selltype == true).ToList();
                }
                else
                {
                    listOfnote = listOfnote.Where(m => m.NoteTitle.ToLower().Contains(searchtext.ToLower()) ||
                                 m.Category.ToLower().Contains(searchtext.ToLower()) || m.BuyerName.ToLower().Contains(searchtext.ToLower()) ||
                                 m.price.ToString().Contains(searchtext) || m.downloadedTime.ToString().Contains(searchtext)).ToList();

                }

            }
            listOfnote = sorting(sortOrder, sortBy, listOfnote);

            ViewBag.TotalPage = Math.Ceiling(listOfnote.Count() / 10.0);
            listOfnote = listOfnote.Skip((currentPage - 1) * 10).Take(10).ToList();
            return View(listOfnote);
        }
        public List<MySoldNotesViewModel> sorting(string sortOrder, string sortBy, List<MySoldNotesViewModel> listOfDownload)
        {
            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.NoteTitle).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.NoteTitle).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.NoteTitle).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTY":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.Category).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.Category).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.Category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "buyer":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.BuyerName).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.BuyerName).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.BuyerName).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "SELLTYPE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.selltype).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.selltype).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.selltype).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PRICE":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.price).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DOWNLOAD":
                    {
                        switch (sortOrder)
                        {
                            case "dsc":
                                {
                                    listOfDownload = listOfDownload.OrderByDescending(m => m.downloadedTime).ToList();
                                    break;
                                }
                            case "asc":
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.downloadedTime).ToList();
                                    break;
                                }
                            default:
                                {
                                    listOfDownload = listOfDownload.OrderBy(m => m.downloadedTime).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        listOfDownload = listOfDownload.OrderByDescending(m => m.downloadedTime).ToList();
                        break;
                    }
            }
            return listOfDownload;

        }
        [Authorize]
        public ActionResult ReturnSoldNote(int NoteID)
        {
            SellerNote notedetail = db.SellerNotes.Where(m => m.id == NoteID).SingleOrDefault();
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();

            var attechment = db.SellerNotesAttechments.Where(m => m.NoteID == notedetail.id).SingleOrDefault();
            if (user.id == notedetail.SellerID)
            {
                FileDownload obj = new FileDownload();
                var ListOfFile = obj.GetFile(attechment.FilePath).ToList();
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchiv = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < ListOfFile.Count(); i++)
                        {
                            zipArchiv.CreateEntryFromFile(ListOfFile[i].FilePath, ListOfFile[i].FileName);
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
                }

            }

            return null;
        }





        [HttpGet]
        public ActionResult MyRejectedNotes(string sortOrder, string sortBy, string searchtext, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;

            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var noteList = db.SellerNotes.Where(m => m.Status == 5 && m.SellerID == user.id).ToList();
            if (searchtext != null)
            {
                noteList = noteList.Where(m => m.Title.ToLower().Contains(searchtext.ToLower()) ||
                                          m.NoteCategory.Name.ToLower().Contains(searchtext.ToLower()) ||
                                          m.AdminRemarks.ToLower().Contains(searchtext.ToLower())).ToList();
            }
            noteList=sorting(sortOrder, sortBy, noteList);
            ViewBag.TotalPage = Math.Ceiling(noteList.Count() / 10.0);

            noteList = noteList.Skip((currentPage - 1) * 10).Take(10).ToList();
            return View(noteList);
        }
        public List<SellerNote> sorting(string sortOrder,string sortBy,List<SellerNote> noteList)
        {
            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {

                            case "asc":
                                {
                                    noteList = noteList.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    noteList = noteList.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    noteList = noteList.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "CATEGOTY":
                    {
                        switch (sortOrder)
                        {

                            case "asc":
                                {
                                    noteList = noteList.OrderBy(m => m.NoteCategory.Name).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    noteList = noteList.OrderByDescending(m => m.NoteCategory.Name).ToList();
                                    break;
                                }
                            default:
                                {
                                    noteList = noteList.OrderBy(m => m.NoteCategory.Name).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "REMARK":
                    {
                        switch (sortOrder)
                        {

                            case "asc":
                                {
                                    noteList = noteList.OrderBy(m => m.AdminRemarks).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    noteList = noteList.OrderByDescending(m => m.AdminRemarks).ToList();
                                    break;
                                }
                            default:
                                {
                                    noteList = noteList.OrderBy(m => m.AdminRemarks).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        noteList = noteList.OrderByDescending(m => m.ModifiedDate).ToList();
                        break;
                    }
            }
            return noteList;
        }
        public ActionResult Correction(int NoteID)
        {
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            var addNotesCategoriesTypeCountry= db.SellerNotes.Where(m => m.id == NoteID).SingleOrDefault();
            var noteDetail = new SellerNote();
            noteDetail.Title = addNotesCategoriesTypeCountry.Title;
            noteDetail.Category = addNotesCategoriesTypeCountry.Category;
            noteDetail.Country = addNotesCategoriesTypeCountry.Country;
            noteDetail.Course = addNotesCategoriesTypeCountry.Course;
            noteDetail.CourseCode = addNotesCategoriesTypeCountry.CourseCode;
            noteDetail.SellerID = user;
            noteDetail.CreatedBy = user;
            noteDetail.CreatedDate = DateTime.Now;
            noteDetail.Description = addNotesCategoriesTypeCountry.Description;
            noteDetail.Status = 1;
            noteDetail.IsPaid = addNotesCategoriesTypeCountry.IsPaid;
            noteDetail.SellingPrice = addNotesCategoriesTypeCountry.SellingPrice;
            noteDetail.NumberOfPages = addNotesCategoriesTypeCountry.NumberOfPages;
            noteDetail.UniversityName = addNotesCategoriesTypeCountry.UniversityName;
            noteDetail.Professor = addNotesCategoriesTypeCountry.Professor;
            //noteDetail.DisplayPicture = addNotesCategoriesTypeCountry.DisplayPicture;
            //noteDetail.NotesPreview = addNotesCategoriesTypeCountry.NotesPreview;
            db.SellerNotes.Add(noteDetail);
            db.SaveChanges();
            string FilePath = "~/Membere/" + user + "/" + noteDetail.id + "/";
            Directory.CreateDirectory(Server.MapPath(FilePath));
            if (addNotesCategoriesTypeCountry.DisplayPicture != null)
            {
                FileInfo IMGFile = new FileInfo(Server.MapPath(addNotesCategoriesTypeCountry.DisplayPicture));
                string UploadPathIMG = Path.Combine(Server.MapPath(FilePath), IMGFile.Name);
                noteDetail.DisplayPicture = FilePath + IMGFile.Name;
                IMGFile.CopyTo(UploadPathIMG, true);

            }
            if (addNotesCategoriesTypeCountry.NotesPreview != null)
            {
                FileInfo priviewFile = new FileInfo(Server.MapPath(addNotesCategoriesTypeCountry.NotesPreview));
                string uploadPathPreview = Path.Combine(Server.MapPath(FilePath), priviewFile.Name);
                noteDetail.NotesPreview = FilePath + priviewFile.Name;
                priviewFile.CopyTo(uploadPathPreview, true);

            }
            db.SellerNotes.Attach(noteDetail);
            db.Entry(noteDetail).Property(m => m.DisplayPicture).IsModified = true;
            db.Entry(noteDetail).Property(m => m.NotesPreview).IsModified = true;
            db.SaveChanges();
            

            var oldNote= db.SellerNotesAttechments.Where(m => m.NoteID == addNotesCategoriesTypeCountry.id).SingleOrDefault();
            var notesAttechment = new SellerNotesAttechment();

            string AttachmentPath= "~/Membere/" + user + "/" + noteDetail.id + "/Attachment/";
            string UploadPathAttachment = Server.MapPath(AttachmentPath);
            bool isPathExists = Directory.Exists(UploadPathAttachment);
            if (!isPathExists)
            {
                Directory.CreateDirectory(UploadPathAttachment);
            }
            
            string[] oldAttachmentFiles = Directory.GetFiles(Server.MapPath(oldNote.FilePath));
            foreach(string oldFile in oldAttachmentFiles)
            {
                FileInfo oldFileinfo = new FileInfo(oldFile);
                string destination = Path.Combine(UploadPathAttachment, oldFileinfo.Name);
                oldFileinfo.CopyTo(destination,true);
            }
            notesAttechment.NoteID = noteDetail.id;
            notesAttechment.FileName = oldNote.FileName;
            notesAttechment.FilePath = AttachmentPath;
            notesAttechment.CreatedBy = user;
            notesAttechment.CreatedDate = DateTime.Now;
            notesAttechment.IsActive = true;
            db.SellerNotesAttechments.Add(notesAttechment);
            db.SaveChanges();

            
            return RedirectToAction("MyRejectedNotes", "User");
        }



        [Authorize]
        public FileContentResult UserPhoto()
        {
            int userid = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m=>m.id).SingleOrDefault();
            string userImgPath = db.UserProfiles.Where(m => m.UserID == userid).Select(m => m.ProfilePicture).SingleOrDefault();
            if (userImgPath != null)
            {
                var fullPath = Server.MapPath(userImgPath);
                byte[] ImgData = null;
                FileInfo imgInfo = new FileInfo(fullPath);
                long imgLength = imgInfo.Length;
                FileStream fileStream = new FileStream(fullPath,FileMode.Open,FileAccess.Read);
                BinaryReader BinaryImgReader = new BinaryReader(fileStream);
                ImgData = BinaryImgReader.ReadBytes((int)imgLength);
                return File(ImgData, "image/jpeg");
            }
            else
            {
                string path = db.SystemConfigurations.Where(m => m.Key == "DefaultProfilePicture").Select(m => m.Value).SingleOrDefault();
                byte[] imgData = null;
                string fullPath = Server.MapPath(path);
                FileInfo ImgInfo = new FileInfo(fullPath);
                long ImgLength = ImgInfo.Length;
                FileStream fs = new FileStream(fullPath,FileMode.Open,FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imgData = br.ReadBytes((int)ImgLength);
                return File(imgData, "image/jpeg");
            }
        }
    }
}