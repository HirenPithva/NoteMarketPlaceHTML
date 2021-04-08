using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class addCategory
    {
        public Nullable<int> ID { get; set; }
        [Required]
        [Display(Name = "Category name*")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description*")]
        public string Description { get; set; }
    }
}