using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class AdminDashboard
    {
        public int registration { get; set; }
        public int Downloaded { get; set; }
        public int InReview { get; set; }
        public IEnumerable<publishedList> publishedLists { get; set; }
        public IEnumerable<SelectListItem> monthList { get; set; }
    }
    public class publishedList
    {
        public string Title { get; set; }
        public string category { get; set; }
        public string AttachmentSize { get; set; }
        public string ispaid { get; set; }
        public int price { get; set; }
        public string fullname { get; set; }
        public DateTime publishDate { get; set; } 
        public int NoDownloads { get; set; }
        public int noreID { get; set; }
    }
}