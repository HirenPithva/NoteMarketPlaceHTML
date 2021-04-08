using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class systemConfigurationViewmodel
    {
        [Display(Name = "Support Email Address*")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Semail { get; set; }


        [Display(Name = "Support Phone NUmber*")]
        [Required]
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "number is not valid")]
        public string ScontactNo { get; set; }




        [Display(Name = "Email address(es)(for various event system will send Notification to these User)*")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Aemail { get; set; }





        [Display(Name = "Facebook URL")]
        public string Facebook { get; set; }
        [Display(Name = "Twitter URL")]
        public string twitter { get; set; }
        [Display(Name = "Linkedin URL")]
        public string Linkdin { get; set; }
        [Display(Name = "Default Images for Notes (if Seller do not upload)")]
        public string DefaultNoteDisplayPicture { get; set; }
        [Display(Name = "Default Profile Picture (if Seller do not upload)")]
        public string DefaultProfilePicture { get; set; }
        public HttpPostedFileBase DisplayPicture { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
    }
}