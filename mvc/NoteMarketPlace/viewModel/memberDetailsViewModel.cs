using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class memberDetailsViewModel
    {
        public IEnumerable<notedetailswithList> notes { get; set; }
        public UserProfile profile { get; set; }
        public User basicInfo { get; set; }
        public string img { get; set; }
    }
    public class notedetailswithList
    {
        public int noteID { get; set; }
        public string status { get; set; }
        public string category { get; set; }
        public string Title { get; set; }
        public int downloadNote { get; set; }
        public int totalEarning { get; set; }
        public DateTime dateAdded { get; set; }
        public Nullable<DateTime> publishedDate { get; set; }
    }
}