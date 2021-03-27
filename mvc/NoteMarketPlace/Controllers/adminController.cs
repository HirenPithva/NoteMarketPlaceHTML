using NoteMarketPlace.viewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class adminController : Controller
    {
        // GET: admin
        protected NoteMarketPlaceEntities db;
        public adminController()
        {
            db = new NoteMarketPlaceEntities();
        }




        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult Index(string sortOrder, string sortBy, string searchtext, string searchmonth, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.searchmonth = searchmonth;
            ViewBag.currentPage = currentPage;
            List<publishedList> allpubNoteList = new List<publishedList>();

            var Listmonth = new List<SelectListItem>();

            AdminDashboard obj = new AdminDashboard();

            DateTime currentDate = DateTime.Now;

            //Dropdown month menu
            SelectListItem listOFitem = new SelectListItem() { Text = currentDate.Month.ToString(), Value="1"};
            Listmonth.Add(listOFitem);
            for (int m = 1; m < 6; m++)
            {
                int month = currentDate.Month - m;
                if (month < 1)
                {
                    month=month + 12;
                }
                SelectListItem ls = new SelectListItem() { Text = month.ToString(), Value = (m + 1).ToString() };
                Listmonth.Add(ls);
            }
            
            obj.monthList = Listmonth;

            //month filter
            DateTime lastMonth;
            DateTime? durationMonth=null;
            if (searchmonth != null)
            {
                int selectedMonth = int.Parse(searchmonth)-1;

                if (currentDate.Month - selectedMonth < 1)
                {
                    var year = currentDate.Year - 1;
                    selectedMonth = currentDate.Month - selectedMonth + 12;
                    
                    lastMonth = new DateTime(year, selectedMonth, 1);
                    if (selectedMonth == 12)
                    {
                        durationMonth = new DateTime(currentDate.Year,1,1);
                    }
                    else
                    {
                        durationMonth = new DateTime(year, selectedMonth+1, 1);
                    }
                }
                else
                {
                    selectedMonth = currentDate.Month - selectedMonth;
                    lastMonth = new DateTime(currentDate.Year, selectedMonth, 1);
                    durationMonth = new DateTime(currentDate.Year, selectedMonth+1,1);
                }
                
            }
            else
            {
                //lastMonth = currentDate.AddMonths(-1);
                lastMonth = new DateTime(currentDate.Year,currentDate.Month,1);
            }
            
            //last 7 days filter
            TimeSpan ts = new TimeSpan(7,0,0,0);
            currentDate = currentDate.Subtract(ts);

            var users = db.Users.ToList();
            var allsellernotes = db.SellerNotes.ToList();
            var allDownloads = db.Downloads.ToList();

            int noOfDownload = 0;
            int noOFRegistration = 0;

            foreach (var notes in allsellernotes)
            {
                var MonthResult=0;
                var upper = 0;
                var lower = 0;
                if (notes.Status == 4)
                {
                    if (searchmonth != null)
                    {
                        upper = DateTime.Compare((DateTime)notes.PublishedDate,lastMonth);
                        lower = DateTime.Compare((DateTime)notes.PublishedDate, (DateTime)durationMonth);
                    }
                    else
                    {
                        MonthResult = DateTime.Compare((DateTime)notes.PublishedDate, lastMonth);
                    }
                }
                var Rlist = new publishedList();
                if ((MonthResult > 0)||(upper>0 && lower<0))
                {

                    var attachment = db.SellerNotesAttechments.Where(m => m.NoteID == notes.id).SingleOrDefault();
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath(attachment.FilePath));
                    FileInfo[] allFile = dir.GetFiles();
                    long size = 0;
                    foreach (var file in allFile)
                    {
                        size = size + file.Length;
                    }
                    long kb = (long)Decimal.Truncate(size / 1024);
                    long mb = (long)Decimal.Truncate(kb / 1024);
                    if (mb != 0)
                    {
                        Rlist.AttachmentSize = mb + " MB";
                    }
                    else
                    {
                        Rlist.AttachmentSize = kb + " KB";
                    }

                    Rlist.Title = notes.Title;

                    Rlist.category = notes.NoteCategory.Name;

                    if (notes.IsPaid)
                    {
                        Rlist.price = (int)notes.SellingPrice;
                        Rlist.ispaid = "Paid";
                    }
                    else
                    {
                        Rlist.price = 0;
                        Rlist.ispaid = "Free";
                    }

                    var seller = db.Users.Where(m => m.id == notes.SellerID).SingleOrDefault();
                    Rlist.fullname = seller.FirstName + " " + seller.LastName;

                    Rlist.publishDate = (DateTime)notes.PublishedDate;

                    Rlist.NoDownloads = db.Downloads.Where(m => m.NoteID == notes.id).Count();
                    allpubNoteList.Add(Rlist);
                }
                
            }
            foreach(var DownloadEntry in allDownloads)
            {
                var result = DateTime.Compare((DateTime)DownloadEntry.CreatedDate, currentDate);
                if (result > 0)
                {
                    noOfDownload++;
                }
            }
            foreach (var user in users)
            {
                var result = DateTime.Compare((DateTime)user.CreatedDate, currentDate);
                if (result > 0)
                {
                    noOFRegistration++;
                }
            }

            obj.registration = noOFRegistration;
            obj.Downloaded = noOfDownload;

            obj.InReview = db.SellerNotes.Where(m => m.Status == 3).Count();
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                allpubNoteList = allpubNoteList.Where(m => m.Title.ToLower().Contains(searchtext) || m.price.ToString().Contains(searchtext)
                                                     || m.publishDate.ToString().Contains(searchtext) || m.ispaid.ToLower().Contains(searchtext)
                                                     || m.category.ToLower().Contains(searchtext) || m.fullname.Contains(searchtext)).ToList();
            }
            allpubNoteList = sorting(sortOrder,sortBy,allpubNoteList);
            ViewBag.TotalPage = Math.Ceiling(allpubNoteList.Count()/5.0);

            obj.publishedLists = allpubNoteList.Skip((currentPage - 1) * 5).Take(5).ToList();

            return View(obj);
        }
        public List<publishedList> sorting(string sortOrder,string sortBy, List<publishedList> allpubNoteList)
        {
            allpubNoteList = allpubNoteList.OrderByDescending(m => m.publishDate).ToList();

            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "CATEGORY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.category).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "ATTACHMENT":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.AttachmentSize).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.AttachmentSize).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.AttachmentSize).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "TYPE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.ispaid).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.ispaid).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.ispaid).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "PRICE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.price).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "PUBLISHER":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.fullname).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.fullname).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.fullname).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "DATE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.publishDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.publishDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.publishDate).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "DOWNLOADS":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    allpubNoteList = allpubNoteList.OrderBy(m => m.NoDownloads).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.NoDownloads).ToList();
                                    break;
                                }
                            default:
                                {
                                    allpubNoteList = allpubNoteList.OrderByDescending(m => m.NoDownloads).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                default:
                    {
                        allpubNoteList = allpubNoteList.OrderByDescending(m => m.publishDate).ToList();
                        break;
                    }  
            }
            return allpubNoteList;
        }








        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult NotesUnderReview(string sortOrder, string sortBy, string searchtext, string searchseller, int currentPage = 1)
        
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.searchseller = searchseller;
            ViewBag.currentPage = currentPage;
            underReview Notes = new underReview();
            var obj = new List<notesUnderReviewViewModel>();
            var allSeller = new List<SelectListItem>();
            var sellerList = db.SellerNotes.Select(m => m.SellerID).Distinct().ToList();
            foreach(var item in sellerList)
            {
                SelectListItem sellerObj = new SelectListItem()
                { Value = item.ToString(), Text = db.Users.Where(m => m.id == item).Select(m => m.FirstName).FirstOrDefault() };
                allSeller.Add(sellerObj);
            }
            List<SellerNote> allNotes;
            
            if (searchseller != null && searchseller != "")
            {
                int sellerID = int.Parse(searchseller);
                allNotes = db.SellerNotes.Where(m =>m.SellerID== sellerID && (m.Status == 2 || m.Status == 3)).ToList();

            }
            else
            {
                allNotes = db.SellerNotes.Where(m => m.Status == 2 || m.Status == 3).ToList();
            }
            foreach(var item in allNotes)
            {
                var SingleOBj = new notesUnderReviewViewModel();
                var user = db.Users.Where(m => m.id == item.SellerID).SingleOrDefault();
                SingleOBj.seller = user.FirstName + " " + user.LastName;
                SingleOBj.Title = item.Title;
                SingleOBj.status = item.NoteStatu.value;
                SingleOBj.dateAdded = (DateTime)item.CreatedDate;
                SingleOBj.category = item.NoteCategory.Name;
                SingleOBj.noteid = item.id;
                obj.Add(SingleOBj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToString();
                obj = obj.Where(m => m.Title.ToLower().Contains(searchtext) || m.status.ToLower().Contains(searchtext) ||
                                   m.seller.ToLower().Contains(searchtext) || m.dateAdded.ToString().Contains(searchtext)).ToList();
            }
            obj = sorting(obj, sortOrder, sortBy);
            ViewBag.TotalPage = (int)Math.Ceiling(obj.Count() / 5.0);
            Notes.allTHENotes = obj.Skip((currentPage - 1) * 5).Take(5).ToList();
             
            Notes.allTHESeller = allSeller.ToList();
            return View(Notes);
        }
        public List<notesUnderReviewViewModel> sorting(List<notesUnderReviewViewModel> obj, string sortOrder, string sortBy)
        {
            obj = obj.OrderByDescending(m => m.dateAdded).ToList();

            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    obj = obj.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    obj = obj.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "CATEGORY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    obj = obj.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    obj = obj.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(m => m.category).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "SELLER":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    obj = obj.OrderBy(m => m.seller).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    obj = obj.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "DATE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    obj = obj.OrderBy(m => m.dateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    obj = obj.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                case "STUTS":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    obj = obj.OrderBy(m => m.status).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    obj = obj.OrderByDescending(m => m.status).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(m => m.status).ToList();
                                    break;
                                }

                        }
                        break;
                    }
                default:
                    {
                        obj = obj.OrderByDescending(m => m.dateAdded).ToList();
                        break;
                    }
            }
            return obj;
        }
        public ActionResult memberDetails(int NoteID)
        {
            return View();
        }
        public ActionResult StatusInReview(FormCollection formData)
        {
            var id = int.Parse(formData["noteID"]);
            var note = db.SellerNotes.Where(m => m.id == id).SingleOrDefault();
            note.Status = 3;
            note.ActionBy= db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            note.ModifiedDate = DateTime.Now;
            note.ModifiedBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            db.SellerNotes.Attach(note);
            db.Entry(note).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            string searchseller = formData["searchseller"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("NotesUnderReview",new { sortOrder, sortBy, searchtext, searchseller, currentPage });
        }
        public ActionResult StatusApprove(FormCollection formData)
        {

            var id = int.Parse(formData["noteID"]);
            var note = db.SellerNotes.Where(m => m.id == id).SingleOrDefault();
            note.Status = 4;
            note.ActionBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            note.ModifiedDate = DateTime.Now;
            note.PublishedDate = DateTime.Now;
            note.ModifiedBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            db.SellerNotes.Attach(note);
            db.Entry(note).State = EntityState.Modified;
            db.SaveChanges();
            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            string searchseller = formData["searchseller"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("NotesUnderReview", new { sortOrder, sortBy, searchtext, searchseller, currentPage });
        }
        public ActionResult StatusRejected(FormCollection formData)
        {
            var id = int.Parse(formData["noteID"]);
            var note = db.SellerNotes.Where(m => m.id == id).SingleOrDefault();
            note.Status = 5;
            note.ActionBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            note.AdminRemarks = formData["Remark"];
            note.ModifiedDate = DateTime.Now;
            note.ModifiedBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            db.SellerNotes.Attach(note);
            db.Entry(note).State = EntityState.Modified;
            db.SaveChanges();
            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            string searchseller = formData["searchseller"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("NotesUnderReview", new { sortOrder, sortBy, searchtext, searchseller, currentPage });
        }
    }
}