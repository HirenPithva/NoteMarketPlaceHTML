using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ContactUs
    {
        [Required]
        [Display(Name ="Full Name*")]
        public string fullName { get; set; }
        [Required]
        [Display(Name = "Email Address*")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Subject*")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Comments/Questions*")]
        public string CmntQus { get; set; }
    }
}