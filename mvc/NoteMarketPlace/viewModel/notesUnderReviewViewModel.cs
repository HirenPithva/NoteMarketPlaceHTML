using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class underReview
    {
        public IEnumerable<SelectListItem> allTHESeller { get; set; }
        public IEnumerable<notesUnderReviewViewModel> allTHENotes { get; set; }
    }
    public class notesUnderReviewViewModel
    {
        public int noteid { get; set; }
        public string Title { get; set; }
        public string category { get; set; }
        public string seller { get; set; }
        public DateTime dateAdded { get; set; } 
        public string status { get; set; }
    }
}