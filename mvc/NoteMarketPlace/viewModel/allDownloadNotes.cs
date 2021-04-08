using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class allDownloadNotes
    {
        public IEnumerable<SelectListItem> allnotes { get; set; }
        public IEnumerable<SelectListItem> allBuyer { get; set; }
        public IEnumerable<SelectListItem> allseller { get; set; }
        public IEnumerable<downloadList> downloadLists { get; set; }

    }
    public class downloadList
    {
        public int noteid { get; set; }
        public string Title { get; set; }
        public string category { get; set; }
        public string buyer { get; set; }
        public int buyerID { get; set; }
        public int sellerID { get; set; }
        public string seller { get; set; }
        public string ispaid { get; set; }
        public int price { get; set; }
        public DateTime DownloadedDate { get; set; }
        public int downloadID { get; set; }
    }
}