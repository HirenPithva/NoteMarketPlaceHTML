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
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        protected NoteMarketPlaceEntities db;
        public SuperAdminController() 
        {
            db = new NoteMarketPlaceEntities();
        }
        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("SystemConfiguration")]
        public ActionResult systemConfiguration()
        {
            systemConfigurationViewmodel systemdata=new systemConfigurationViewmodel ();
            systemdata.Aemail = db.SystemConfigurations.Where(m => m.Key == "Aemail").Select(m => m.Value).SingleOrDefault();
            systemdata.Semail = db.SystemConfigurations.Where(m => m.Key == "Semail").Select(m => m.Value).SingleOrDefault();
            systemdata.ScontactNo = db.SystemConfigurations.Where(m => m.Key == "ScontactNo").Select(m => m.Value).SingleOrDefault();
            systemdata.DefaultNoteDisplayPicture = db.SystemConfigurations.Where(m => m.Key == "DefaultNoteDisplayPicture").Select(m=>m.Value).SingleOrDefault();
            systemdata.DefaultProfilePicture = db.SystemConfigurations.Where(m => m.Key == "DefaultProfilePicture").Select(m => m.Value).SingleOrDefault();
            systemdata.Facebook = db.SystemConfigurations.Where(m => m.Key == "Facebook").Select(m => m.Value).SingleOrDefault();
            systemdata.twitter = db.SystemConfigurations.Where(m => m.Key == "twitter").Select(m => m.Value).SingleOrDefault();
            systemdata.Linkdin = db.SystemConfigurations.Where(m => m.Key == "Linkdin").Select(m => m.Value).SingleOrDefault();
            return View(systemdata);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("SystemConfiguration")]
        public ActionResult systemConfiguration(systemConfigurationViewmodel systemdata)
        {
            if (ModelState.IsValid)
            {
                var config = db.SystemConfigurations.ToList();
                if (systemdata.Aemail!=null){
                    var Aemail=config.Where(m => m.Key == "Aemail").FirstOrDefault();
                    Aemail.Value = systemdata.Aemail;
                }
                if (systemdata.Semail != null) { 
                    var Semail=config.Where(m => m.Key == "Semail").FirstOrDefault();
                    Semail.Value = systemdata.Semail;
                }
                if (systemdata.ScontactNo != null) { 
                    var ScontactNo=config.Where(m => m.Key == "ScontactNo").FirstOrDefault();
                    ScontactNo.Value = systemdata.ScontactNo;
                }
                if (systemdata.twitter != null) { 
                    var twitter=config.Where(m => m.Key == "twitter").FirstOrDefault();
                    twitter.Value = systemdata.twitter;
                }
                if (systemdata.Facebook != null) { 
                    var Facebook=config.Where(m => m.Key == "Facebook").FirstOrDefault();
                    Facebook.Value = systemdata.Facebook;
                }
                if (systemdata.Linkdin != null) { 
                    var Linkdin=config.Where(m => m.Key == "Linkdin").FirstOrDefault();
                    Linkdin.Value = systemdata.Linkdin;
                }
                
                if (systemdata.ProfilePicture != null)
                {
                    string path = config.Where(m => m.Key == "DefaultProfilePicture").Select(m => m.Value).FirstOrDefault();
                    path = Server.MapPath(path);
                    FileInfo ProfilePhoto = new FileInfo(path);
                    if (ProfilePhoto.Exists)
                    {
                        ProfilePhoto.Delete();
                    }
                    path = string.Empty;
                    string filename = System.IO.Path.GetFileName(systemdata.ProfilePicture.FileName);
                    path = "~/Membere/system/dfProfilePic/";
                    bool isExist = Directory.Exists(Server.MapPath(path));
                    if (!isExist)
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string uploadPth = Path.Combine(Server.MapPath(path), filename);
                    string storagepath = path + filename;
                    var DefaultProfilePicture=config.Where(m => m.Key == "DefaultProfilePicture").FirstOrDefault();
                    DefaultProfilePicture.Value = storagepath;
                    systemdata.ProfilePicture.SaveAs(uploadPth);
                }
                if (systemdata.DisplayPicture != null)
                {
                    string path = config.Where(m => m.Key == "DefaultNoteDisplayPicture").Select(m => m.Value).FirstOrDefault();
                    path = Server.MapPath(path);
                    FileInfo ProfilePhoto = new FileInfo(path);
                    if (ProfilePhoto.Exists)
                    {
                        ProfilePhoto.Delete();
                    }
                    path = string.Empty;
                    string filename = System.IO.Path.GetFileName(systemdata.DisplayPicture.FileName);
                    path = "~/Membere/system/dfBookPic/";
                    bool isExist = Directory.Exists(Server.MapPath(path));
                    if (!isExist)
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string uploadPth = Path.Combine(Server.MapPath(path), filename);
                    string storagepath = path + filename;
                    var DefaultNoteDisplayPicture=config.Where(m => m.Key == "DefaultNoteDisplayPicture").FirstOrDefault();
                    DefaultNoteDisplayPicture.Value = storagepath;
                    systemdata.DisplayPicture.SaveAs(uploadPth);
                }
                db.SaveChanges();

            }
            return View();
        }







        [Authorize(Roles = "Super Admin")]
        [OutputCache(Duration = 0)]
        [Route("Administrator")]
        public ActionResult administrator(string sortOrder, string sortBy, string searchtext, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;

            var adminlist = db.Users.Where(m => m.RoleID == 2).ToList();
            List<administratorViewmodel> adminListForAdmin = new List<administratorViewmodel>();
            foreach (var item in adminlist)
            {
                administratorViewmodel obj = new administratorViewmodel();
                obj.user = item;
                var profile = db.UserProfiles.Where(m => m.UserID == item.id).FirstOrDefault();
                if (profile != null)
                {
                    obj.phoneNO = profile.PhoneNumber;
                }
                else
                {
                    obj.phoneNO = "NA";
                }
                adminListForAdmin.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                adminListForAdmin = adminListForAdmin.Where(m => m.user.FirstName.ToLower().Contains(searchtext) ||
                                                             m.user.LastName.ToLower().Contains(searchtext) ||
                                                             m.phoneNO.ToString().Contains(searchtext) ||
                                                             m.user.CreatedDate.ToString().Contains(searchtext) ||
                                                             m.user.EmailID.ToLower().Contains(searchtext)).ToList();
            }
            adminListForAdmin=sorting(adminListForAdmin, sortBy, sortOrder);
            ViewBag.TotalPage = Math.Ceiling(adminListForAdmin.Count() / 5.0);
            adminListForAdmin = adminListForAdmin.Skip((currentPage - 1) * 5).Take(5).ToList();
            if(adminListForAdmin.Count()==0 && currentPage > 1)
            {
                currentPage = currentPage - 1;
                ViewBag.currentPage = currentPage;
                adminListForAdmin = adminListForAdmin.Skip((currentPage - 1) * 5).Take(5).ToList();
            }

            return View(adminListForAdmin); 
        }
        public List<administratorViewmodel> sorting(List<administratorViewmodel> adminListForAdmin,string sortBy,string sortOrder)
        {
            switch (sortBy) 
            {
                case "FIRSTNAME":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.user.FirstName).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.FirstName).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.FirstName).ToList();
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
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.user.LastName).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.LastName).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.LastName).ToList();
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
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.user.EmailID).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.EmailID).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.EmailID).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "PHONENO":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.phoneNO).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.phoneNO).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.phoneNO).ToList();
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
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
               case "ACTIVE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderBy(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                            default:
                                {
                                    adminListForAdmin = adminListForAdmin.OrderByDescending(m => m.user.CreatedDate).ToList();
                                    break;
                                }
                        }
                        break;
                    }
            }

            return adminListForAdmin;
        }
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("AddAdministrator")]
        public ActionResult Addadministrator(int memberID=0)
        {
            var adminnistratorDetails = new addAdministratorViewModel();
            if (memberID != 0)
            {

                var user = db.Users.Where(m => m.id == memberID).FirstOrDefault();
                adminnistratorDetails.memberID = memberID;
                adminnistratorDetails.FirstName = user.FirstName;
                adminnistratorDetails.Lastname = user.LastName;
                adminnistratorDetails.EmailID = user.EmailID;
                adminnistratorDetails.phoneNUmber = db.UserProfiles.Where(m => m.UserID == user.id).Select(m => m.PhoneNumber).FirstOrDefault();
                adminnistratorDetails.PhonCode = db.UserProfiles.Where(m => m.UserID == user.id).Select(m => m.PhoneNumber_code).FirstOrDefault().ToString();
            }
            List<SelectListItem> codes=new List<SelectListItem>();
            var country = db.Countries.Where(m=>m.IsActive==true).ToList();
            foreach(var item in country)
            {
                SelectListItem obj = new SelectListItem() { Text = item.CountryCode, Value = item.CountryCode };
                codes.Add(obj);
            }
            adminnistratorDetails.allCode = codes;
            return View(adminnistratorDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("AddAdministrator")]
        public ActionResult Addadministrator(addAdministratorViewModel adminDetails)
        {
            if (ModelState.IsValid)
            {
                if (adminDetails.memberID != null)
                {
                    var admin = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();

                    var oldUser = db.Users.Where(m => m.id == adminDetails.memberID).SingleOrDefault();
                    var oldprofile= db.UserProfiles.Where(m => m.UserID== adminDetails.memberID).SingleOrDefault();
                    oldUser.FirstName = adminDetails.FirstName;
                    oldUser.LastName = adminDetails.Lastname;
                    oldUser.EmailID = adminDetails.EmailID;
                    oldUser.ModifiedBy = admin.id;
                    oldUser.ModifiedDate = DateTime.Now;
                    db.Users.Attach(oldUser);
                    db.Entry(oldUser).State = EntityState.Modified;
                    db.SaveChanges();
                    if (adminDetails.phoneNUmber != null)
                    {
                        oldprofile.PhoneNumber = adminDetails.phoneNUmber;
                        oldprofile.PhoneNumber_code = int.Parse(adminDetails.PhonCode);
                        oldprofile.ModifiedBy = admin.id;
                        oldprofile.ModifiedDate = DateTime.Now;
                        db.UserProfiles.Attach(oldprofile);
                        db.Entry(oldprofile).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                }
                else
                {
                    var admin = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
                    var newUser = new User();
                    newUser.FirstName = adminDetails.FirstName;
                    newUser.LastName = adminDetails.Lastname;
                    newUser.EmailID = adminDetails.EmailID;
                    newUser.IsActive = true;
                    newUser.CreatedDate = DateTime.Now;
                    newUser.CreatedBy = admin.id;
                    newUser.Password = adminDetails.FirstName + "@123";
                    newUser.RoleID = 2;
                    newUser.IsEmailVerified = true;
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    var userprofile = new UserProfile();
                    userprofile.UserID = newUser.id;
                    userprofile.AddressLine1 = "NA";
                    userprofile.AddressLine2 = "NA";
                    userprofile.State = "NA";
                    userprofile.City = "NA";
                    userprofile.ZipCode = "NA";
                    userprofile.Country = int.Parse(db.Countries.Take(1).Select(m => m.CountryCode).SingleOrDefault());
                    if (adminDetails.phoneNUmber != null)
                    {
                        userprofile.PhoneNumber = adminDetails.phoneNUmber;
                        userprofile.PhoneNumber_code = int.Parse(adminDetails.PhonCode);
                    }
                    db.UserProfiles.Add(userprofile);
                    db.SaveChanges();

                }
                return RedirectToAction("administrator");
            }
            List<SelectListItem> codes = new List<SelectListItem>();
            var country = db.Countries.Where(m => m.IsActive == true).ToList();
            foreach (var item in country)
            {
                SelectListItem obj = new SelectListItem() { Text = item.CountryCode, Value = item.CountryCode };
                codes.Add(obj);
            }
            adminDetails.allCode = codes;
            return View(adminDetails);
        }
        public ActionResult DeactivateAdmin(FormCollection formData)
        {
            var memberID = int.Parse(formData["memberID"]);
            var admin = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();

            var olduser = db.Users.Where(m => m.id == memberID).SingleOrDefault();
            olduser.IsActive = false;
            olduser.ModifiedBy = admin.id;
            olduser.ModifiedDate = DateTime.Now;
            db.Users.Attach(olduser);
            db.Entry(olduser).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("administrator", new { sortOrder, sortBy, searchtext, currentPage });
        }






        [Authorize(Roles = "Super Admin,admin")]
        [OutputCache(Duration = 0)]
        [Route("Category")]
        public ActionResult manageCategory(string sortOrder, string sortBy, string searchtext, int currentPage = 1) 
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            var allCategory = db.NoteCategories.ToList();
            List<categoriesViewModel> categoriesforView = new List<categoriesViewModel>();
            foreach(var item in allCategory)
            {
                categoriesViewModel obj = new categoriesViewModel();
                obj.category = item.Name;
                obj.description = item.Description;
                obj.dateAdded = (DateTime)item.CreatedDate;
                obj.categoryID = item.ID;
                if (item.IsActive)
                {
                    obj.active = "YES";
                }
                else
                {
                    obj.active = "NO";
                }
                var user = db.Users.Where(m => m.id == item.CreatedBy).FirstOrDefault();
                obj.addedBy = user.FirstName + " " + user.LastName;
                categoriesforView.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                categoriesforView = categoriesforView.Where(m => m.active.ToLower().Contains(searchtext) ||
                                                             m.dateAdded.ToString().Contains(searchtext) ||
                                                             m.addedBy.ToLower().Contains(searchtext) ||
                                                             m.category.ToLower().Contains(searchtext) ||
                                                             m.description.ToLower().Contains(searchtext)).ToList();
            }
            categoriesforView=sorting(categoriesforView, sortOrder, sortBy);
            ViewBag.TotalPage = Math.Ceiling(categoriesforView.Count() / 5.0);
            categoriesforView = categoriesforView.Skip((currentPage - 1) * 5).Take(5).ToList();
            if(categoriesforView.Count()==0 && currentPage > 1)
            {
                currentPage = currentPage - 1;
                ViewBag.currentPage = currentPage;
                categoriesforView = categoriesforView.Skip((currentPage - 1) * 5).Take(5).ToList();
            }

            return View(categoriesforView);
        }
        public List<categoriesViewModel> sorting(List<categoriesViewModel> categoriesforView, string sortOrder, string sortBy)
        {
            categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
            switch (sortBy)
            {
                case "CATEGORY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DESCRIPTION":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.description).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.description).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.description).ToList();
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
                                    categoriesforView = categoriesforView.OrderBy(m => m.dateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ADDEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.addedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ACTIVE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.active).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                        break;
                    }
            }
            return categoriesforView;
        }
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("AddCategory")]
        public ActionResult AddCategory(int categoryID=0) 
        {
            if (categoryID != 0)
            {
                var category = db.NoteCategories.Where(m => m.ID == categoryID).FirstOrDefault();
                addCategory old = new addCategory();
                old.ID = category.ID;
                old.Name = category.Name;
                old.Description = category.Description;
                return View(old);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("AddCategory")]
        public ActionResult AddCategory(addCategory category) 
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
                if (category.ID == null)
                {
                    var newCategory = new NoteCategory();
                    newCategory.Name = category.Name;
                    newCategory.Description = category.Description;
                    newCategory.CreatedDate = DateTime.Now;
                    newCategory.CreatedBy = user.id;
                    newCategory.IsActive = true;
                    db.NoteCategories.Add(newCategory);
                    db.SaveChanges();
                }
                else
                {
                    var oldCategory = db.NoteCategories.Where(m => m.ID == category.ID).SingleOrDefault();
                    oldCategory.Name = category.Name;
                    oldCategory.Description = category.Description;
                    oldCategory.ModifiedBy = user.id;
                    oldCategory.ModifiedDate = DateTime.Now;
                    db.NoteCategories.Attach(oldCategory);
                    db.Entry(oldCategory).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("manageCategory");
            }
            return View();
        }
        public ActionResult deleteCategory(FormCollection formData)
        {
            var categoryID = int.Parse(formData["categoryID"]);
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var oldCategory = db.NoteCategories.Where(m => m.ID == categoryID).SingleOrDefault();
            oldCategory.IsActive = false;
            oldCategory.ModifiedBy = user.id;
            oldCategory.ModifiedDate = DateTime.Now;
            db.NoteCategories.Attach(oldCategory);
            db.Entry(oldCategory).State = EntityState.Modified;
            db.SaveChanges();
            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("manageCategory", new { sortOrder, sortBy, searchtext, currentPage });
        }






        [Authorize(Roles = "Super Admin,admin")]
        [OutputCache(Duration = 0)]
        [Route("Type")]
        public ActionResult managetype(string sortOrder, string sortBy, string searchtext, int currentPage = 1) {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            var allTypes= db.NoteTypes.ToList();
            List<manageTypeViewModel> listOfTypeForView = new List<manageTypeViewModel>();
            foreach(var item in allTypes)
            {
                manageTypeViewModel obj = new manageTypeViewModel();
                obj.Type = item.Name;
                obj.typeID = item.ID;
                obj.description = item.Description;
                obj.dateAdded = (DateTime)item.CreatedDate;
                var user = db.Users.Where(m => m.id == item.CreatedBy).SingleOrDefault();
                obj.addedBy = user.FirstName + " " + user.LastName;
                if (item.IsActive)
                {
                    obj.active = "YES";
                }
                else
                {
                    obj.active = "NO";
                }
                listOfTypeForView.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                listOfTypeForView = listOfTypeForView.Where(m => m.Type.ToLower().Contains(searchtext) ||
                                                             m.description.ToLower().Contains(searchtext) ||
                                                             m.dateAdded.ToString().Contains(searchtext) ||
                                                             m.addedBy.ToLower().Contains(searchtext) ||
                                                             m.active.ToLower().Contains(searchtext)).ToList();
            }
            listOfTypeForView=sorting(listOfTypeForView,sortOrder,sortBy);
            ViewBag.TotalPage = Math.Ceiling(listOfTypeForView.Count() / 5.0);
            listOfTypeForView = listOfTypeForView.Skip((currentPage - 1) * 5).Take(5).ToList();
            if(listOfTypeForView.Count()==0 && currentPage > 1)
            {
                currentPage = currentPage - 1;
                ViewBag.currentPage = currentPage;
                listOfTypeForView = listOfTypeForView.Skip((currentPage - 1) * 5).Take(5).ToList();

            }
            return View(listOfTypeForView);
        }
        public List<manageTypeViewModel> sorting(List<manageTypeViewModel> categoriesforView, string sortOrder, string sortBy)
        {
            categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
            switch (sortBy)
            {
                case "TYPE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.Type).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.Type).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.Type).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "DESCRIPTION":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.description).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.description).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.description).ToList();
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
                                    categoriesforView = categoriesforView.OrderBy(m => m.dateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ADDEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.addedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ACTIVE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.active).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                        break;
                    }
            }
            return categoriesforView;
        }
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("AddType")]
        public ActionResult AddType(int typeID=0)
        {
            if (typeID != 0)
            {
                var oldType = db.NoteTypes.Where(m => m.ID == typeID).FirstOrDefault();
                addType type = new addType();
                type.ID = typeID;
                type.Name = oldType.Name;
                type.Description = oldType.Description;
                return View(type);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("AddType")]
        public ActionResult AddType(addType type)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
                if (type.ID != null)
                {
                    var oldType = db.NoteTypes.Where(m => m.ID == type.ID).SingleOrDefault();
                    oldType.Name = type.Name;
                    oldType.Description = type.Description;
                    oldType.ModifiedBy = user.id;
                    oldType.ModifiedDate = DateTime.Now;
                    db.NoteTypes.Attach(oldType);
                    db.Entry(oldType).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var newType = new NoteType();
                    newType.Name = type.Name;
                    newType.Description = type.Description;
                    newType.CreatedBy = user.id;
                    newType.CreatedDate = DateTime.Now;
                    newType.IsActive = true;
                    db.NoteTypes.Add(newType);
                    db.SaveChanges();
                }
                return RedirectToAction("managetype");
            }
            return View();
        }
        public ActionResult deleteType(FormCollection formData) 
        {

            var typeID = int.Parse(formData["typeID"]);
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
            var oldType = db.NoteTypes.Where(m => m.ID == typeID).SingleOrDefault();
            oldType.IsActive = false;
            oldType.ModifiedBy = user.id;
            oldType.ModifiedDate = DateTime.Now;
            db.NoteTypes.Attach(oldType);
            db.Entry(oldType).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("managetype", new { sortOrder, sortBy, searchtext, currentPage });
        }






        [Authorize(Roles = "Super Admin,admin")]
        [OutputCache(Duration = 0)]
        [Route("Country")]
        public ActionResult ManageCountry(string sortOrder, string sortBy, string searchtext, int currentPage = 1)
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;
            var allCountries = db.Countries.ToList();
            List<CountriesViewModel> countryListForView = new List<CountriesViewModel>();
            foreach (var item in allCountries)
            {
                CountriesViewModel obj = new CountriesViewModel();
                obj.country = item.Name;
                obj.dateAdded = (DateTime)item.CreatedDate;
                obj.countryCode = item.CountryCode;
                var user = db.Users.Where(m => m.id == item.CreatedBy).SingleOrDefault();
                obj.addedBy = user.FirstName + " " + user.LastName;
                obj.countryID = item.ID;
                if (item.IsActive)
                {
                    obj.active = "YES";
                }
                else
                {
                    obj.active = "NO";
                }
                countryListForView.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                countryListForView = countryListForView.Where(m => m.active.ToLower().Contains(searchtext) ||
                                                             m.addedBy.ToLower().Contains(searchtext) ||
                                                             m.country.ToLower().Contains(searchtext) ||
                                                             m.countryCode.ToString().Contains(searchtext) ||
                                                             m.dateAdded.ToString().Contains(searchtext)).ToList();
            }
            countryListForView = sorting(countryListForView,sortOrder,sortBy);
            ViewBag.TotalPage = Math.Ceiling(countryListForView.Count() / 5.0);
            countryListForView = countryListForView.Skip((currentPage - 1) * 5).Take(5).ToList();

            return View(countryListForView);
        }
        public List<CountriesViewModel> sorting(List<CountriesViewModel> categoriesforView, string sortOrder, string sortBy)
        {
            categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
            switch (sortBy)
            {
                case "COUNTRY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.country).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.country).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.country).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "COUNTRYCODE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.countryCode).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.countryCode).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.countryCode).ToList();
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
                                    categoriesforView = categoriesforView.OrderBy(m => m.dateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ADDEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.addedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.addedBy).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "ACTIVE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    categoriesforView = categoriesforView.OrderBy(m => m.active).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                            default:
                                {
                                    categoriesforView = categoriesforView.OrderByDescending(m => m.active).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        categoriesforView = categoriesforView.OrderByDescending(m => m.dateAdded).ToList();
                        break;
                    }
            }
            return categoriesforView;
        }
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("AddCountry")]
        public ActionResult addcountry(int countryID=0)
        {
            if (countryID != 0)
            {
                var oldCountry = db.Countries.Where(m => m.ID == countryID).SingleOrDefault();
                AddCountriesModel countryDetails = new AddCountriesModel();
                countryDetails.ID = oldCountry.ID;
                countryDetails.Name = oldCountry.Name;
                countryDetails.Code = oldCountry.CountryCode;
                return View(countryDetails);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("AddCountry")]
        public ActionResult addcountry(AddCountriesModel country)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();
                if (country.ID != null)
                {
                    var oldCountry = db.Countries.Where(m => m.ID == country.ID).SingleOrDefault();
                    oldCountry.Name = country.Name;
                    oldCountry.CountryCode = country.Code;
                    oldCountry.ModifiedBy = user.id;
                    oldCountry.ModifiedDate = DateTime.Now;
                    db.Countries.Attach(oldCountry);
                    db.Entry(oldCountry).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var newCountry = new Country();
                    newCountry.Name = country.Name;
                    newCountry.CountryCode = country.Code;
                    newCountry.CreatedBy = user.id;
                    newCountry.CreatedDate = DateTime.Now;
                    newCountry.IsActive = true;
                    db.Countries.Add(newCountry);
                    db.SaveChanges();
                }
                return RedirectToAction("ManageCountry");
            }

            return View();
        }
        [HttpPost]
        public ActionResult deleteCountry(FormCollection formData)
        {
            var countryID = int.Parse(formData["countryID"]);
            var user = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).SingleOrDefault();

            var oldCountry = db.Countries.Where(m => m.ID == countryID).SingleOrDefault();
            oldCountry.IsActive = false;
            oldCountry.ModifiedBy = user.id;
            oldCountry.ModifiedDate = DateTime.Now;
            db.Countries.Attach(oldCountry);
            db.Entry(oldCountry).State = EntityState.Modified;
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("ManageCountry", new { sortOrder, sortBy, searchtext, currentPage });
        }





        [Authorize(Roles = "Super Admin,admin")]
        [OutputCache(Duration = 0)]
        [Route("SpamReport")]
        public ActionResult spamReport(string sortOrder, string sortBy, string searchtext, int currentPage = 1) 
        {
            ViewBag.sortOrder = sortOrder;
            ViewBag.sortBy = sortBy;
            ViewBag.searchtext = searchtext;
            ViewBag.currentPage = currentPage;

            List<spanReportViewModel> SpamreportListforView = new List<spanReportViewModel>();
            var reportList = db.SellerNotesReportedIssues.ToList();
            foreach (var item in reportList)
            {
                spanReportViewModel obj = new spanReportViewModel();
                obj.Title = db.SellerNotes.Where(m => m.id == item.NoteID).Select(m=>m.Title).SingleOrDefault();
                var user = db.Users.Where(m => m.id == item.ReportedByID).SingleOrDefault();
                obj.ReportedBy = user.FirstName + " " + user.LastName;
                obj.remark = item.Remarks;
                obj.noteID = item.NoteID;
                obj.category= db.SellerNotes.Where(m => m.id == item.NoteID).Select(m => m.NoteCategory.Name).SingleOrDefault();
                obj.DateAdded = (DateTime)item.CreatedDate;
                obj.reportID = item.id;
                SpamreportListforView.Add(obj);
            }
            if (searchtext != null)
            {
                searchtext = searchtext.ToLower();
                SpamreportListforView = SpamreportListforView.Where(m => m.category.ToLower().Contains(searchtext) ||
                                                                     m.DateAdded.ToString().Contains(searchtext) ||
                                                                     m.remark.ToLower().Contains(searchtext) ||
                                                                     m.ReportedBy.ToLower().Contains(searchtext) ||
                                                                     m.Title.ToLower().Contains(searchtext)).ToList();
            }
            SpamreportListforView = sorting(SpamreportListforView,sortOrder,sortBy);
            ViewBag.TotalPage = Math.Ceiling(SpamreportListforView.Count() / 5.0);
            SpamreportListforView = SpamreportListforView.Skip((currentPage - 1) * 5).Take(5).ToList();
            return View(SpamreportListforView);
        }
        public List<spanReportViewModel> sorting(List<spanReportViewModel> SpamreportListforView,string sortOrder, string sortBy)
        {
            SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.DateAdded).ToList();
            switch (sortBy) 
            {
                case "TITLE":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderBy(m => m.Title).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.Title).ToList();
                                    break;
                                }
                            default:
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.Title).ToList();
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
                                    SpamreportListforView = SpamreportListforView.OrderBy(m => m.category).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.category).ToList();
                                    break;
                                }
                            default:
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.category).ToList();
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
                                    SpamreportListforView = SpamreportListforView.OrderBy(m => m.remark).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.remark).ToList();
                                    break;
                                }
                            default:
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.remark).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "REPORTEDBY":
                    {
                        switch (sortOrder)
                        {
                            case "asc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderBy(m => m.ReportedBy).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.ReportedBy).ToList();
                                    break;
                                }
                            default:
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.ReportedBy).ToList();
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
                                    SpamreportListforView = SpamreportListforView.OrderBy(m => m.DateAdded).ToList();
                                    break;
                                }
                            case "dsc":
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.DateAdded).ToList();
                                    break;
                                }
                            default:
                                {
                                    SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.DateAdded).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        SpamreportListforView = SpamreportListforView.OrderByDescending(m => m.DateAdded).ToList();
                        break;
                    }
            }

            return SpamreportListforView;
        }
        [HttpPost]
        public ActionResult DeleteReportedIssus(FormCollection formData)
        {
            var ReportedID = int.Parse(formData["ReportedID"]);
            var ReportedIssues = db.SellerNotesReportedIssues.Where(m => m.id == ReportedID).SingleOrDefault();
            db.SellerNotesReportedIssues.Remove(ReportedIssues);
            db.SaveChanges();

            string sortOrder = formData["sortOrder"];
            string sortBy = formData["sortBy"];
            string searchtext = formData["searchtext"];
            int currentPage = int.Parse(formData["currentPage"]);
            return RedirectToAction("spamReport", new { sortOrder, sortBy, searchtext, currentPage });
        }






        [Authorize(Roles = "Super Admin,admin")]
        [HttpGet]
        [OutputCache(Duration = 0)]
        [Route("AdminProfile")]
        public ActionResult AdminProfile()
        {

            var MemberID = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).Select(m=>m.id).SingleOrDefault();
            var OldUser = db.Users.Where(m => m.id == MemberID).SingleOrDefault();
            var oldprofile = db.UserProfiles.Where(m => m.UserID == MemberID).SingleOrDefault();
            adminProfileViewmodal adminDetails = new adminProfileViewmodal();
            adminDetails.firstname = OldUser.FirstName;
            adminDetails.Lastname = OldUser.LastName;
            adminDetails.EmailID = OldUser.EmailID;
            adminDetails.allCode = db.Countries.Where(m => m.IsActive == true).ToList();
            adminDetails.memverID = MemberID;
            if (oldprofile != null)
            {
                if (oldprofile.PhoneNumber != null && oldprofile.PhoneNumber_code != null)
                {
                    adminDetails.phoneNUmber = oldprofile.PhoneNumber;
                    adminDetails.PhonCode = (int)oldprofile.PhoneNumber_code;
                }
                if (oldprofile.ProfilePicture != null)
                {
                    adminDetails.ImgPath = oldprofile.ProfilePicture;
                    FileInfo profilePic = new FileInfo(Server.MapPath(oldprofile.ProfilePicture));
                    adminDetails.ImgName = profilePic.Name;
                }
            }
            
            return View(adminDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("AdminProfile")]
        public ActionResult AdminProfile(adminProfileViewmodal admin)
        {
            if (ModelState.IsValid)
            {
                var olduser = db.Users.Where(m => m.id == admin.memverID).SingleOrDefault();
                var oldprofile = db.UserProfiles.Where(m => m.UserID == admin.memverID).SingleOrDefault();
                olduser.FirstName = admin.firstname;
                olduser.LastName = admin.Lastname;
                oldprofile.SecondryEmailAddress = admin.SecondoryEmailID;
                oldprofile.PhoneNumber_code = admin.PhonCode;
                oldprofile.PhoneNumber = admin.phoneNUmber;
                if (admin.Img != null)
                {
                    if (oldprofile.ProfilePicture != null)
                    {
                        var profilepic = new FileInfo(Server.MapPath(oldprofile.ProfilePicture));
                        if (profilepic.Exists)
                        {
                            profilepic.Delete();
                        }
                    }
                    string filename = System.IO.Path.GetFileName(admin.Img.FileName);
                    string filepath = "~/Membere/" + admin.memverID + "/";
                    bool Exists = Directory.Exists(Server.MapPath(filepath));
                    if (!Exists)
                    {
                        Directory.CreateDirectory(Server.MapPath(filepath));
                    }
                    oldprofile.ProfilePicture = filepath + filename;
                    admin.ImgPath = oldprofile.ProfilePicture;
                    admin.ImgName = filename;
                    string UploadPath = Path.Combine(Server.MapPath(filepath), filename);
                    admin.Img.SaveAs(UploadPath);
                }
                db.Users.Attach(olduser);
                db.Entry(olduser).State = EntityState.Modified;
                db.SaveChanges();

                db.UserProfiles.Attach(oldprofile);
                db.Entry(oldprofile).State = EntityState.Modified;
                db.SaveChanges();

            }
            admin.allCode = db.Countries.Where(m => m.IsActive == true).ToList();

            return View(admin);
        }
    }
}