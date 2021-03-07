using NoteMarketPlace.newCustomeValidationAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class usersignup
    {
        [Required]
        [Display(Name ="First Name*")]
        public string firstname { get; set; }
        [Required]
        [Display(Name = "Last Name*")]
        public string lastname { get; set; }
        [Required]
        [Display(Name = "Password*")]
        public string password { get; set; }
        [Required]
        //[passwordEqualConfirmPassword]
        [Display(Name = "Confirm Password*")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*?&])([^\s]).{6,24}$", ErrorMessage = "It must be between 6 and 24 characters long,atleast 1 lowercase, 1 special character, 1 digit and no white space allowed")]

        public string con_password {get; set;}
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string emailAddress { get; set; }
    }
}