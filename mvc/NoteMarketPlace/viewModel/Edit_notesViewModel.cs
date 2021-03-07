using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class Edit_notesViewModel
    {

        [Required]
        [Display(Name = "Title*")]
        public string Title { get; set; }
        public IEnumerable<Country> countries { get; set; }
        [Display(Name = "Type")]
        public IEnumerable<NoteType> noteTypes { get; set; }
        public IEnumerable<NoteCategory> noteCategories { get; set; }
        [Display(Name = "display picture")]
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        [Display(Name = "Upload Notes*")]
        public string ImagePathForNote { get; set; }
        public HttpPostedFileBase ImageFileForNote { get; set; }
        [Display(Name = "Note Preview")]
        public string ImagePathForPreview { get; set; }

        public HttpPostedFileBase ImageFileForPreview { get; set; }
        [Required]
        public string gridRadios { get; set; }
        public string btn { get; set; }
        [Required]
        [Display(Name = "Category*")]

        public int category { get; set; }
        public int type { get; set; }
        [Display(Name = "Type")]
        public Nullable<int> NoteType { get; set; }
        [Display(Name = "Number of Pages")]

        public Nullable<int> NumberOfPages { get; set; }
        [Required]
        [Display(Name = "Description*")]

        public string Description { get; set; }
        [Display(Name = "Institution Name")]

        public string UniversityName { get; set; }
        [Display(Name = "Country")]

        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        [Display(Name = "Course Code")]

        public string CourseCode { get; set; }
        [Display(Name = "Professor/Lecturer")]

        public string Professor { get; set; }
        [Display(Name = "Selling Price")]

        public Nullable<decimal> SellingPrice { get; set; }

        public int IDofNote { get; set; }

    }
}
