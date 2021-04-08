using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class publishedNotes
    {
        public IEnumerable<SelectListItem> allTHESeller { get; set; }
        public IEnumerable<notelist> allTHENotes { get; set; }
    }
    public class notelist
    {
        public int noteid { get; set; }
        public string Title { get; set; }
        public string category { get; set; }
        public string seller { get; set; }
        public int sellerID { get; set; }
        public DateTime publishedDate { get; set; }
        public string isPaid { get; set; }
        public int price { get; set; }
        public string approvedBy { get; set; }
        public int noOfDownloads { get; set; }
    }
}