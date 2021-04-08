using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class adminProfileViewmodal
    {
        [Required]
        [Display(Name = "First Name*")]
        public string firstname { get; set; }
        public int memverID { get; set; }
        [Required]
        [Display(Name = "Last Name*")]
        public string Lastname { get; set; }
        [Display(Name = "Email*")]
        public string EmailID { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Secondary Email")]
        public string SecondoryEmailID { get; set; }
        public string ImgPath { get; set; }
        public string ImgName { get; set; }
        [Display(Name ="Profile Picture")]
        public HttpPostedFileBase Img { get; set; }
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "number is not valid")]
        [Display(Name = "phone Number")]
        public string phoneNUmber { get; set; }
        public Nullable<int> PhonCode { get; set; }
        public IEnumerable<Country> allCode { get; set; }

    }
}