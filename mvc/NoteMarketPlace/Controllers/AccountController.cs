﻿using NoteMarketPlace.Models;
using NoteMarketPlace.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{

    public class AccountController : Controller
    {
        protected NoteMarketPlaceEntities db;
        public AccountController()
        {
            db = new NoteMarketPlaceEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [Route("LOGIN")]
        public ActionResult Login()
        {
            
            HttpCookie userInfo = Request.Cookies["userInfo"];
            string username = string.Empty; string password = string.Empty;

            if (userInfo != null)
            {
                username = userInfo["username"].ToString();
                password = userInfo["password"].ToString();
                Userlogin ucredintial = new Userlogin()
                {
                    emailAddress = username,
                    password = password
                };
                UserLoginRemember ucredintials = new UserLoginRemember()
                {
                    userlogins = ucredintial,
                    rememberMe = false
                };
                return View(ucredintials);
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("LOGIN")]
        public ActionResult Login(UserLoginRemember userloginremembers)
        {
            //bool active = db.Users.Any(m => m.IsActive == true);
            //if (!(active))
            //{
            //    return View("Login");
            //}
            if (ModelState.IsValid)
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                var isActiveornote = db.Users.Where(m => m.Password == userloginremembers.userlogins.password && m.EmailID == userloginremembers.userlogins.emailAddress && m.IsActive==false).SingleOrDefault();
                if (isActiveornote != null)
                {
                    return new HttpStatusCodeResult(404, "user not found");
                }
                bool Isvld = db.Users.Any(m => m.Password == userloginremembers.userlogins.password && m.EmailID == userloginremembers.userlogins.emailAddress);
                if (Isvld)
                {
                    var user = db.Users.Where(m => m.EmailID == userloginremembers.userlogins.emailAddress).SingleOrDefault();
                    bool profile = db.UserProfiles.Any(m => m.UserID == user.id);
                    Session["username"] = userloginremembers.userlogins.emailAddress;
                    if (userloginremembers.rememberMe)
                    {
                        if ((user.IsEmailVerified != null && user.IsEmailVerified == true) || user.RoleID == 1 || user.RoleID == 2)
                        {
                            FormsAuthentication.SetAuthCookie(userloginremembers.userlogins.emailAddress, true);
                        }
                        
                            

                        userInfo["username"] = userloginremembers.userlogins.emailAddress;
                        userInfo["password"] = userloginremembers.userlogins.password;
                        Response.Cookies.Add(userInfo);
                        userInfo.Expires = DateTime.Now.AddSeconds(300);
                    }
                    else
                    {
                        if ((user.IsEmailVerified != null && user.IsEmailVerified == true) || user.RoleID==1 || user.RoleID==2)
                        {
                            FormsAuthentication.SetAuthCookie(userloginremembers.userlogins.emailAddress, false);

                            userInfo.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(userInfo);
                        }
                        
                        
                    }
                    if (user.RoleID == 1 || user.RoleID == 2)
                    {
                        return RedirectToAction("Index","admin");
                    }
                    else if (user.IsEmailVerified !=null && user.IsEmailVerified==true)
                    {
                        if (profile)
                        {
                            return RedirectToAction("Serch_note", "Home");
                        }
                        else
                        {
                            return RedirectToAction("UserProfile", "User");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }

                }
                ModelState.AddModelError("", "invalid Username or Password");

            }

            return View();
        }
        





        [HttpGet]
        [Route("SIGNUP")]
        public ActionResult Signup()
        {
            var user = new usersignup();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SIGNUP")]
        public ActionResult Signup(usersignup usersignup)
        {
            bool exist = db.Users.Any(m => m.EmailID == usersignup.emailAddress);
            
            if (usersignup.password != usersignup.con_password || exist)
            {
                if(usersignup.password != usersignup.con_password)
                {
                    TempData["notMatched"] = "confirm password note matched with password";

                }
                if (exist)
                {
                    return RedirectToAction("Login", "Account");

                }

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = new User();
                    user.FirstName = usersignup.firstname;
                    user.LastName = usersignup.lastname;
                    user.EmailID = usersignup.emailAddress;
                    user.Password = usersignup.password;
                    user.RoleID = 3;
                    user.CreatedDate = DateTime.Now;
                    user.IsActive = true;
                    db.Users.Add(user);
                    db.SaveChanges();
                    TempData["register"] = "succesfully created";
                    buildEmailTamplate(user.id);
                }
            }
            return View(usersignup);
        }








        public ActionResult confirm(int regid)
        {
            ViewBag.regid = regid;
            return View();

        }
        public JsonResult RegisterConfirm(int regid)
        {
            User Data = db.Users.Where(m => m.id == regid).FirstOrDefault();
            Data.IsEmailVerified = true;
            Data.ModifiedDate = DateTime.Now;
            Data.ModifiedBy = Data.id;
            db.SaveChanges();
            var msg = "your Email is varified";
            return Json(msg,JsonRequestBehavior.AllowGet);
        }




        public void buildEmailTamplate(int regid)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            User userinfo = db.Users.Where(m => m.id == regid).FirstOrDefault();
            var url = "https://localhost:44308/" + "Account/confirm?regid=" + regid;
            body = body.Replace("viewbag.Confirmation", url);
            body = body.Replace("viewbag.yourname", userinfo.FirstName);
            body = body.ToString();
            buildEmailTamplate("NoteMarketPlace", body, userinfo.EmailID);
        }
        public void buildEmailTamplateforget(int regid)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "TextForget" + ".cshtml");
            User userinfo = db.Users.Where(m => m.id == regid).FirstOrDefault();
            body = body.Replace(" Viewbag.password", userinfo.Password);

            body = body.ToString();
            buildEmailTamplate("New Temporary Password has been created for you", body, userinfo.EmailID);
        }
        


        public static void buildEmailTamplate(string subject,string body,string to)
        {
            string From, To, bcc, cc, Subject, Body;
            From = "hirenpithva105@gmail.com";
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
            client.Credentials = new System.Net.NetworkCredential("EmailID", "PWD");
            try
            {
                client.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }




        [Authorize]
        public ActionResult Logout()
        {
            Session.Remove("username");
            HttpCookie isExist = Request.Cookies["userInfo"];
            if(isExist!=null)
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                userInfo["username"] = isExist["username"].ToString();
                userInfo["password"] = isExist["password"].ToString();
                Response.Cookies.Add(userInfo);

                userInfo.Expires = DateTime.Now.AddSeconds(300);
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }




        [Route("FORGET")]
        public ActionResult forget()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("FORGET")]
        public ActionResult forget(UserForget userForget)
        {
            if (ModelState.IsValid)
            {
                bool valid = db.Users.Any(m => m.EmailID == userForget.emailAddress);
                if(valid)
                {
                    string _allowedChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int length = _allowedChar.Length;
                    Random rand = new Random();
                    char[] password = new char[5];
                    for(int i = 0; i < 5; i++)
                    {
                        password[i] = _allowedChar[(int)(length*rand.NextDouble())];
                    }
                    User user = db.Users.Where(m => m.EmailID == userForget.emailAddress).FirstOrDefault();
                    
                    string pass = new string(password);
                    user.Password = pass;
                    db.Users.Attach(user);
                    db.Entry(user).Property(m => m.Password).IsModified = true;

                    db.SaveChanges();
                    buildEmailTamplateforget(user.id);
                    ViewBag.success = "letter sent";
                    return View();
                }
                else
                {
                    TempData["invalidEmail"] = "Wrong Email Adress";
                    return View();

                }
            }
            return View();
        }






        [HttpGet]
        [Authorize]
        [OutputCache(Duration =0)]
        [Route("CHANGEPASSWORD")]
        public ActionResult changePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 0)]
        [Route("CHANGEPASSWORD")]
        public ActionResult changePassword(userPassword userPasswords)
        {
            User authuser = db.Users.Where(m => m.EmailID == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {

                if (authuser.Password == userPasswords.password1)
                {
                    if (userPasswords.password2 == userPasswords.password3)
                    {
                        //User clonuser = authuser;
                        authuser.Password = userPasswords.password3;
                        db.Users.Attach(authuser);

                        db.Entry(authuser).Property(m => m.Password).IsModified = true;

                        db.SaveChanges();
                        
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        TempData["confirmchangePassword"] = "Password Is Not Mathced with confirm Password";
                        return View("changePassword");
                    }
                }
                else
                {
                    TempData["IncorrectconfirmPassword"] = "Incorrect Password";
                    return View("changePassword");
                }

            }
            return View();
            
        }


    }
        
}

