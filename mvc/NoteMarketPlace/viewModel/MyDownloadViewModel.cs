using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class MyDownloadViewModel
    {
        public bool isAlreadyReported { get; set; }
        public int noteid { get; set; }
        public int downloadIDofTB { get; set; }
        public bool isNoteDownloaded { get; set; } 
        public string NoteTitle { get; set; }
        public string Category { get; set; }
        public string sellerName { get; set; }
        public bool selltype { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<DateTime> downloadedTime { get; set; }
    }
}