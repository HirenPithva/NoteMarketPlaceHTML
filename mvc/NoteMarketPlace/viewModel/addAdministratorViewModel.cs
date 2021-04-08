using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class addAdministratorViewModel
    {
        public Nullable<int> memberID { get; set; }
        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name*")]
        public string Lastname { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email*")]
        [Required]
        public string EmailID { get; set; }
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "number is not valid")]
        [Display(Name = "phone Number")]
        public string phoneNUmber { get; set; }
        public string PhonCode { get; set; }
        public IEnumerable<SelectListItem> allCode { get; set; }
    }
}