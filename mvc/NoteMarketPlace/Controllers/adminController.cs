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
                    Rlist.noreID = notes.id;
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
                SingleOBj.sellerID = user.id;
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










        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult allPublishedNotes(string sortOrder, string sortBy, string searchtext, string searchseller, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.searchseller = searchseller;
            ViewBag.currentPage = currentPage;


            //seller filter
            var allSeller = new List<SelectListItem>();
            var sellerList = db.SellerNotes.Select(m => m.SellerID).Distinct().ToList();
            foreach (var item in sellerList)
            {
                SelectListItem sellerObj = new SelectListItem()
                { Value = item.ToString(), Text = db.Users.Where(m => m.id == item).Select(m => m.FirstName).FirstOrDefault() };
                allSeller.Add(sellerObj);
            }
            List<SellerNote> allNotes;

            if (searchseller != null && searchseller != "")
            {
                int sellerID = int.Parse(searchseller);
                allNotes = db.SellerNotes.Where(m => m.SellerID == sellerID && (m.Status == 4)).ToList();

            }
            else
            {
                allNotes = db.SellerNotes.Where(m=>m.Status == 4).ToList();
            }

            //noteList
            List<notelist> PBnotes = new List<notelist>();
            foreach (var item in allNotes)
            {
                notelist obj = new notelist();
                var AdminWhoApproved = db.Users.Where(m => m.id == item.ActionBy).SingleOrDefault();
                string adminFullName = AdminWhoApproved.FirstName + " " + AdminWhoApproved.LastName;
                obj.approvedBy = adminFullName;
                if (item.IsPaid)
                {
                    obj.isPaid = "Paid";
                    obj.price = (int)item.SellingPrice;

                }
                else
                {
                    obj.isPaid = "Free";
                    obj.price = 0;
                }
                obj.noOfDownloads = db.Downloads.Where(m => m.NoteID == item.id).Count();
                obj.category = item.NoteCategory.Name;
                obj.noteid = item.id;
                obj.publishedDate = (DateTime)item.PublishedDate;
                var seller = db.Users.Where(m => m.id == item.SellerID).SingleOrDefault();
                obj.seller = seller.FirstName + " " + seller.LastName;
                obj.sellerID = seller.id;
                obj.Title = item.Title;
                PBnotes.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                PBnotes = PBnotes.Where(m => m.Title.ToLower().Contains(searchtext) || m.seller.ToLower().Contains(searchtext) ||
                                         m.publishedDate.ToString().Contains(searchtext) || m.price.ToString().Contains(searchtext) ||
                                         m.noOfDownloads.ToString().Contains(searchtext) || m.isPaid.ToLower().Contains(searchtext) ||
                                         m.category.ToLower().Contains(searchtext) || m.approvedBy.ToLower().Contains(searchtext)).ToList();
            }
            PBnotes=sorting( sortOrder, sortBy,PBnotes);
            ViewBag.TotalPage = Math.Ceiling(PBnotes.Count() / 5.0);
            publishedNotes NotesAlongSeller = new publishedNotes();
            NotesAlongSeller.allTHESeller = allSeller.ToList();
            NotesAlongSeller.allTHENotes = PBnotes.Skip((currentPage - 1) * 5).Take(5).ToList();
            return View(NotesAlongSeller);
        }
        public List<notelist> sorting(string sortOrder, string sortBy, List<notelist> PBnotes)
        {
            PBnotes = PBnotes.OrderByDescending(m => m.publishedDate).ToList();
            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    PBnotes = PBnotes.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.Title).ToList();
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
                                    PBnotes = PBnotes.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "SELLTYPE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    PBnotes = PBnotes.OrderBy(m => m.isPaid).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.isPaid).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.isPaid).ToList();
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
                                    PBnotes = PBnotes.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.price).ToList();
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
                                    PBnotes = PBnotes.OrderBy(m => m.seller).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PUBLISHEDDATE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    PBnotes = PBnotes.OrderBy(m => m.publishedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.publishedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.publishedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "APPROVEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    PBnotes = PBnotes.OrderBy(m => m.approvedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.approvedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.approvedBy).ToList();
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
                                    PBnotes = PBnotes.OrderBy(m => m.noOfDownloads).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.noOfDownloads).ToList();
                                    break;
                                }
                            default:
                                {
                                    PBnotes = PBnotes.OrderByDescending(m => m.noOfDownloads).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        PBnotes = PBnotes.OrderByDescending(m => m.publishedDate).ToList();
                        break;
                    }
            }

            return PBnotes;
        }
        public ActionResult UnpublishedNote(FormCollection formData)
        {
            int id = int.Parse(formData["noteID"]);
            var note = db.SellerNotes.Where(m => m.id == id).SingleOrDefault();
            note.Status = 6;
            note.ActionBy = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            note.ModifiedBy = note.ActionBy;
            note.ModifiedDate = DateTime.Now;
            note.AdminRemarks = formData["Remark"];

            db.SellerNotes.Attach(note);
            db.Entry(note).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            string searchseller = formData["searchseller"];
            int currentPage = int.Parse(formData["currentPage"]);
            BuildEmailTamplateForUnpublished(note);
            return RedirectToAction("allPublishedNotes", new { sortOrder, sortBy, searchtext, searchseller, currentPage });
        }
        public ActionResult downloadNOte(int noteID)
        {
            var attechment = db.SellerNotesAttechments.Where(m => m.NoteID == noteID).SingleOrDefault();

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
        public void BuildEmailTamplateForUnpublished(SellerNote note)
        {

            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "TextUnpublishedNote" + ".cshtml");
            var user = db.Users.Where(m => m.id == note.SellerID).SingleOrDefault();
            body = body.Replace("viewbag.r",note.AdminRemarks);
            body = body.Replace("viewbag.s", user.FirstName);
            body = body.Replace("viewbag.t", note.Title);
            body = body.ToString();
            string Subject = "Sorry! We need to remove your notes from our portal";
            string from = db.SystemConfigurations.Where(m => m.Key == "Semail").Select(m => m.Value).SingleOrDefault();
            string to = user.EmailID;
            HomeController.buildEmailTamplate(Subject,body,from,to);
        }





        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult allDownloadNotes(string sortOrder, string sortBy, string searchtext, string searchseller, string searchbyuer, string searchnote, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.searchseller = searchseller;
            ViewBag.searchbyuer = searchbyuer;
            ViewBag.searchnote = searchnote;
            ViewBag.currentPage = currentPage;

            



            //notes
            List<SelectListItem> notesInDownloadTB = new List<SelectListItem>();

            List<int> noteIDs = db.Downloads.Select(m => m.NoteID).Distinct().ToList();

            foreach (var item in noteIDs)
            {
                SelectListItem obj = new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = db.SellerNotes.Where(m => m.id == item).Select(m => m.Title).FirstOrDefault()
                };
                notesInDownloadTB.Add(obj);

            }




            //seller
            List<SelectListItem> sellerInDownloadTB = new List<SelectListItem>();

            List<int> sellerIDs = db.Downloads.Select(m => m.Seller).Distinct().ToList();

            foreach (var item in sellerIDs)
            {
                var seller = db.Users.Where(m => m.id == item).SingleOrDefault();
                SelectListItem obj = new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = seller.FirstName + " " + seller.LastName
                };
                sellerInDownloadTB.Add(obj);

            }



            //buyer
            List<SelectListItem> buyerInDownloadTB = new List<SelectListItem>();

            List<int> buyerIDs = db.Downloads.Select(m => m.Downloader).Distinct().ToList();

            foreach (var item in buyerIDs)
            {
                var buyer = db.Users.Where(m => m.id == item).SingleOrDefault();
                SelectListItem obj = new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = buyer.FirstName + " " + buyer.LastName
                };
                buyerInDownloadTB.Add(obj);

            }

            //DownloadTB list
            
            var alldownloads = db.Downloads.ToList();
            if(searchnote!=null && searchnote !="")
            {
                var noteID = int.Parse(searchnote);
                alldownloads = alldownloads.Where(m => m.NoteID == noteID).ToList();
            }
            if(searchbyuer != null && searchbyuer != "" )
            {
                var buyerID = int.Parse(searchbyuer);
                alldownloads = alldownloads.Where(m => m.Downloader == buyerID).ToList();
            }
            if(searchseller != null && searchseller != "" )
            {
                var sellerID = int.Parse(searchseller);
                alldownloads = alldownloads.Where(m => m.Seller == sellerID).ToList();
            }

            //listOFDownloads
            List<downloadList> downloads = new List<downloadList>();
            foreach(var item in alldownloads)
            {
                downloadList obj = new downloadList();
                obj.downloadID = item.id;
                obj.Title = item.NoteTitle;
                if (item.IsPaid)
                {
                    obj.ispaid = "Paid";
                    obj.price = (int)item.PurchasedPrice;
                }
                else
                {
                    obj.ispaid = "Free";
                    obj.price = 0;
                }
                obj.DownloadedDate = (DateTime)item.AttechmentDownloadedDate;
                obj.category = item.NoteCategory;
                var buyer = db.Users.Where(m => m.id == item.Downloader).SingleOrDefault();
                obj.buyer = buyer.FirstName + " " + buyer.LastName;
                var seller = db.Users.Where(m => m.id == item.Seller).SingleOrDefault();
                obj.seller = seller.FirstName + " " + seller.LastName;
                obj.buyerID = buyer.id;
                obj.sellerID = seller.id;
                obj.noteid = item.NoteID;
                downloads.Add(obj);
            }
            if (searchtext != null && searchtext !="")
            {
                searchtext = searchtext.ToLower();
                downloads = downloads.Where(m => m.category.ToLower().Contains(searchtext) || m.buyer.ToLower().Contains(searchtext) ||
                                            m.seller.ToLower().Contains(searchtext) || m.Title.ToLower().Contains(searchtext) ||
                                            m.price.ToString().Contains(searchtext) || m.ispaid.ToLower().Contains(searchtext )).ToList();
            }
            downloads=sorting(downloads, sortOrder,sortBy);
            ViewBag.TotalPage = Math.Ceiling(downloads.Count()/5.0);
            allDownloadNotes AllDownloadNotes = new allDownloadNotes();
            AllDownloadNotes.allBuyer = buyerInDownloadTB;
            AllDownloadNotes.allseller = sellerInDownloadTB;
            AllDownloadNotes.allnotes = notesInDownloadTB;
            AllDownloadNotes.downloadLists = downloads.Skip((currentPage-1)*5).Take(5).ToList();

            return View(AllDownloadNotes);
        }
        public List<downloadList> sorting(List<downloadList> downloads, string sortOrder, string sortBy)
        {
            downloads = downloads.OrderByDescending(m => m.DownloadedDate).ToList();

            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    downloads = downloads.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.Title).ToList();
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
                                    downloads = downloads.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.category).ToList();
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
                                    downloads = downloads.OrderBy(m => m.seller).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "BUYER":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    downloads = downloads.OrderBy(m => m.buyer).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.buyer).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.buyer).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "SELLTYPE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    downloads = downloads.OrderBy(m => m.ispaid).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.ispaid).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.ispaid).ToList();
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
                                    downloads = downloads.OrderBy(m => m.price).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.price).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.price).ToList();
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
                                    downloads = downloads.OrderBy(m => m.DownloadedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    downloads = downloads.OrderByDescending(m => m.DownloadedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    downloads = downloads.OrderByDescending(m => m.DownloadedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        downloads = downloads.OrderByDescending(m => m.DownloadedDate).ToList();
                        break;
                    }
            }
            return downloads;
        }






        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult rejectednotes(string sortOrder, string sortBy, string searchtext, string searchseller, int currentPage = 1) 
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.searchseller = searchseller;
            ViewBag.currentPage = currentPage;

            //seller filter
            var allSeller = new List<SelectListItem>();
            var sellerList = db.SellerNotes.Select(m => m.SellerID).Distinct().ToList();
            foreach (var item in sellerList)
            {
                SelectListItem sellerObj = new SelectListItem()
                { Value = item.ToString(), Text = db.Users.Where(m => m.id == item).Select(m => m.FirstName).FirstOrDefault() };
                allSeller.Add(sellerObj);
            }
            List<SellerNote> allNotes;

            if (searchseller != null && searchseller != "")
            {
                int sellerID = int.Parse(searchseller);
                allNotes = db.SellerNotes.Where(m => m.SellerID == sellerID && (m.Status == 5)).ToList();

            }
            else
            {
                allNotes = db.SellerNotes.Where(m => m.Status == 5).ToList();
            }

            //rejectedNotes
            List<rejectedlist> rejectednotesList = new List<rejectedlist>();
            foreach(var item in allNotes)
            {
                var obj = new rejectedlist();
                obj.title = item.Title;
                obj.remark = item.AdminRemarks;
                var rejectedByadmin = db.Users.Where(m => m.id == item.ActionBy).SingleOrDefault();
                var noteseller = db.Users.Where(m => m.id == item.SellerID).SingleOrDefault();
                obj.sellerID = noteseller.id;
                obj.seller = noteseller.FirstName + " " + noteseller.LastName;
                obj.RejectedBy = rejectedByadmin.FirstName + " " + rejectedByadmin.LastName;
                obj.modifiedDate = (DateTime)item.ModifiedDate;
                obj.noteid = item.id;
                obj.category = item.NoteCategory.Name;
                rejectednotesList.Add(obj);
            }

            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                rejectednotesList = rejectednotesList.Where(m => m.title.ToLower().Contains(searchtext) || m.seller.ToLower().Contains(searchtext) ||
                                                              m.remark.ToLower().Contains(searchtext) || m.RejectedBy.ToLower().Contains(searchtext) ||
                                                              m.modifiedDate.ToString().Contains(searchtext) || m.category.ToLower().Contains(searchtext)).ToList();
            }

            rejectednotesList=sorting(sortOrder,sortBy, rejectednotesList);

            ViewBag.TotalPage = Math.Ceiling(rejectednotesList.Count() / 5.0);
            rejectednotesViewModel rejectedNotesListForView = new rejectednotesViewModel();
            rejectedNotesListForView.seller = allSeller;
            rejectedNotesListForView.rejectednotes = rejectednotesList.Skip((currentPage - 1) * 5).Take(5).ToList();
            if (currentPage > 1 && rejectedNotesListForView.rejectednotes.Count()==0)
            {
                currentPage = currentPage - 1;
                ViewBag.currentPage = currentPage;
                rejectedNotesListForView.rejectednotes = rejectednotesList.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            return View(rejectedNotesListForView);   
        }
        public List<rejectedlist> sorting(string sortOrder, string sortBy, List<rejectedlist> rejectednotesList)
        {
            rejectednotesList = rejectednotesList.OrderByDescending(m => m.modifiedDate).ToList();
            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.title).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.title).ToList();
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
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.category).ToList();
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
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.seller).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.seller).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DATEADDED":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.modifiedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.modifiedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.modifiedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "REJECTEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.RejectedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.RejectedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.RejectedBy).ToList();
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
                                    rejectednotesList = rejectednotesList.OrderBy(m => m.remark).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.remark).ToList();
                                    break;
                                }
                            default:
                                {
                                    rejectednotesList = rejectednotesList.OrderByDescending(m => m.remark).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        rejectednotesList = rejectednotesList.OrderByDescending(m => m.modifiedDate).ToList();
                        break;
                    }
            }
            return rejectednotesList;
        }
        public ActionResult publishedNotes(FormCollection formData)
        {
            int noteID =int.Parse(formData["noteID"]);
            var note = db.SellerNotes.Where(m => m.id == noteID).SingleOrDefault();
            var userID= db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m => m.id).SingleOrDefault();
            note.ModifiedBy = userID;
            note.ModifiedDate = DateTime.Now;
            note.ActionBy = userID;
            note.Status = 4;
            note.PublishedDate = DateTime.Now;
            db.SellerNotes.Attach(note);
            db.Entry(note).State = EntityState.Modified;
            db.SaveChanges();
            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            string searchseller = formData["searchseller"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("rejectednotes", new { sortOrder, sortBy, searchtext, searchseller, currentPage });
        }












        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult allmember(string sortOrder, string sortBy, string searchtext, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            var allRegistredusers = db.Users.Where(m=>m.RoleID==3).ToList();
            List<memberViewmodel> membersList = new List<memberViewmodel>();
            foreach(var item in allRegistredusers)
            {
                memberViewmodel obj = new memberViewmodel();
                obj.first = item.FirstName;
                obj.last = item.LastName;
                obj.memberID = item.id;
                obj.email = item.EmailID;
                obj.joiningDate = (DateTime)item.CreatedDate;
                obj.underreview = db.SellerNotes.Where(m => m.SellerID == item.id && m.Status == 2 && m.Status == 3).Count();
                obj.publishednotes = db.SellerNotes.Where(m => m.SellerID == item.id && m.Status == 4).Count();
                obj.downloadnotes = db.Downloads.Where(m => m.Downloader == item.id).Count();
                var expense = db.Downloads.Where(m => m.Downloader == item.id && m.IsPaid == true).ToList();
                if (expense.Count() != 0)
                {
                    obj.expenses = (int)expense.Select(m => m.PurchasedPrice).Sum();

                }
                else
                {
                    obj.expenses = 0;
                }
                var earning = db.Downloads.Where(m => m.Seller == item.id && m.IsPaid == true).ToList();
                if (earning.Count() != 0)
                {
                    obj.earnings = (int)earning.Select(m => m.PurchasedPrice).Sum();

                }
                else
                {
                    obj.earnings = 0;

                }
                membersList.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                membersList = membersList.Where(m => m.first.ToLower().Contains(searchtext) || m.last.ToLower().Contains(searchtext) ||
                                             m.email.ToLower().Contains(searchtext) || m.downloadnotes.ToString().Contains(searchtext) ||
                                             m.underreview.ToString().Contains(searchtext) || m.publishednotes.ToString().Contains(searchtext) ||
                                             m.joiningDate.ToString().Contains(searchtext) || m.expenses.ToString().Contains(searchtext) ||
                                             m.earnings.ToString().Contains(searchtext)).ToList();
            }
            membersList = sorting(sortOrder, sortBy, membersList);
            ViewBag.TotalPage = Math.Ceiling(membersList.Count() / 5.0);
            membersList = membersList.Skip((currentPage - 1) * 5).Take(5).ToList();
            if(membersList.Count()==0 && currentPage > 1)
            {
                currentPage = currentPage - 1;
                ViewBag.currentPage = currentPage;
                membersList = membersList.Skip((currentPage - 1) * 5).Take(5).ToList();
            }
            return View(membersList);
            
        }
        public List<memberViewmodel> sorting(string sortOrder, string sortBy, List<memberViewmodel> membersList)
        {
            membersList = membersList.OrderByDescending(m => m.joiningDate).ToList();
            switch (sortBy)
            {
                case "FIRSTNAME":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.first).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.first).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.first).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "LASTNAME":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.last).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.last).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.last).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "EMAIL":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.email).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.email).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.email).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "JOININGDATE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.joiningDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.joiningDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.joiningDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "UNDERREVIEWNOTES":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.underreview).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.underreview).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.underreview).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PUBLISHEDNOTES":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.publishednotes).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.publishednotes).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.publishednotes).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DOWNLOADEDNOTES":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.downloadnotes).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.downloadnotes).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.downloadnotes).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "TOTAlEXPENSES":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.expenses).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.expenses).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.expenses).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "TOTALEARNINGS":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    membersList = membersList.OrderBy(m => m.earnings).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    membersList = membersList.OrderByDescending(m => m.earnings).ToList();
                                    break;
                                }
                            default:
                                {
                                    membersList = membersList.OrderByDescending(m => m.earnings).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        membersList = membersList.OrderByDescending(m => m.joiningDate).ToList();

                        break;
                    }
            }
            return membersList;
        }
        public ActionResult Deactivate(FormCollection formData) 
        {
            var memberID = int.Parse(formData["memberID"]);
            var notes = db.SellerNotes.Where(m => m.SellerID == memberID).ToList();
            var admin = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m=>m.id).SingleOrDefault();
            foreach(var item in notes)
            {
                item.Status = 6;
                item.ModifiedDate = DateTime.Now;
                item.ModifiedBy = admin;
                item.ActionBy = admin;

                db.SellerNotes.Attach(item);
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            var user = db.Users.Where(m => m.id == memberID).SingleOrDefault();
            user.IsActive = false;
            user.ModifiedBy = admin;
            user.ModifiedDate = DateTime.Now;
            db.Users.Attach(user);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("allmember", new { sortOrder, sortBy, searchtext, currentPage });
        }










        [Authorize(Roles = "Super Admin,admin")]
        public ActionResult memberDetails(int memberID, string sortOrder, string sortBy, int currentPage = 1) 
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.currentPage = currentPage;

            var noteList = db.SellerNotes.Where(m => m.SellerID == memberID && (m.Status == 2 || m.Status == 3 || m.Status == 4 || m.Status == 5)).ToList();
            List<notedetailswithList> notelistForView = new List<notedetailswithList>();
            foreach (var item in noteList)
            {
                notedetailswithList obj = new notedetailswithList();
                obj.Title = item.Title;
                obj.category = item.NoteCategory.Name;
                obj.status = item.NoteStatu.value;
                var earning = db.Downloads.Where(m => m.Seller == memberID && m.NoteID==item.id && m.IsPaid == true).ToList();
                if (earning.Count() != 0)
                {
                    obj.totalEarning = (int)earning.Select(m => m.PurchasedPrice).Sum();
                }
                else
                {
                    obj.totalEarning = 0;
                }
                obj.downloadNote = db.Downloads.Where(m => m.Downloader == item.id).Count();
                obj.noteID = item.id;
                obj.dateAdded =(DateTime) item.CreatedDate;
                if (item.Status == 4)
                {
                    obj.publishedDate = item.PublishedDate;
                }
                else
                {
                    obj.publishedDate = null;
                }

                notelistForView.Add(obj);
            }
            notelistForView = sorting(sortOrder, sortBy, notelistForView);
            var member = new memberDetailsViewModel();
            ViewBag.TotalPage = Math.Ceiling(notelistForView.Count() / 5.0);
            member.notes = notelistForView.Skip((currentPage-1)*5).Take(5).ToList();
            member.profile = db.UserProfiles.Where(m => m.UserID == memberID).SingleOrDefault();
            member.basicInfo = db.Users.Where(m => m.id == memberID).SingleOrDefault();
            if (member.profile != null && member.profile.ProfilePicture!=null)
            {
                member.img = member.profile.ProfilePicture;
            }
            else
            {
                member.img = db.SystemConfigurations.Where(m => m.Key == "DefaultProfilePicture").Select(m => m.Value).SingleOrDefault();
            }
                return View(member);
        }
        public List<notedetailswithList> sorting(string sortOrder,string sortBy, List<notedetailswithList> notelistForView)
        {
            notelistForView = notelistForView.OrderByDescending(m => m.publishedDate).ToList();

            switch (sortBy)
            {
                case "NOTETITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    notelistForView = notelistForView.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.Title).ToList();
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
                                    notelistForView = notelistForView.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "STATUS":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    notelistForView = notelistForView.OrderBy(m => m.status).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.status).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.status).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "TOTALEARNINGS":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    notelistForView = notelistForView.OrderBy(m => m.totalEarning).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.totalEarning).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.totalEarning).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DATEADDED":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    notelistForView = notelistForView.OrderBy(m => m.dateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PUBLISHDATE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    notelistForView = notelistForView.OrderBy(m => m.publishedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.publishedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    notelistForView = notelistForView.OrderByDescending(m => m.publishedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        notelistForView = notelistForView.OrderByDescending(m => m.publishedDate).ToList();
                        break;
                    }
            }
            
            return notelistForView;
        }
    }
}