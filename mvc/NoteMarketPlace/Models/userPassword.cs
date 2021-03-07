using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class userPassword
    {
        [Required]
        [Display(Name = "Old Password*")]
        public string password1 { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*?&])([^\s]).{6,24}$", ErrorMessage = "It must be between 6 and 24 characters long,atleast 1 lowercase, 1 special character, 1 digit and no white space allowed")]
        [Display(Name = "New Password*")]
        public string password2 { get; set; }
        [Required]
        [Display(Name = "Confirm Password*")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*?&])(?=.*[^\s]).{6,24}$", ErrorMessage = "It must be between 6 and 24 characters long,atleast 1 lowercase, 1 special character, 1 digit and no white space allowed")]

        public string password3 { get; set; }
    }
}