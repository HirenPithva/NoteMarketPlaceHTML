using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "Email*")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:MM/dd/yyyy}")]
        public Nullable<DateTime> BirthDate { get; set; }
        public IEnumerable<gender> genderList { get; set; }
        
        [Display(Name ="Gender")]

        public Nullable<int> gender { get; set; }
        
        public IEnumerable<Country> countryList { get; set; }
        

        public Nullable<int> countryCode { get; set; }
        [Display(Name ="Phone Number")]
        [RegularExpression("^[0-9]{0,10}$", ErrorMessage = "number is not valid")]
        public string PhonNumber { get; set; }
        
        [Display(Name ="Profile Picture")]
        public string profilepicPath { get; set; }
        public HttpPostedFileBase profilepicture { get; set; }

        [Display(Name ="Address Line 1*")]
        [Required]
        public string Address1 { get; set; }
        
        [Display(Name ="Address Line 2")]
        public string Address2 { get; set; }
        
        [Display(Name ="City*")]
        [Required]
        public string city { get; set; }
        
        [Display(Name ="State*")]
        [Required]
        public string state { get; set; }
        
        [Display(Name ="ZipCode*")]
        [Required]
        public string zipCode { get; set; }
        
        [Display(Name ="Country*")]
        [Required]
        public int usrCountry { get; set; }
        
        [Display(Name ="University")]
        public string University { get; set; }
        
        [Display(Name ="College")]
        public string college  { get; set; }

        public string profilepicName { get; set; }
    }
}