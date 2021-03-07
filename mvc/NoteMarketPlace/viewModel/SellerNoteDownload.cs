using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class SellerNoteDownload
    {
        public IEnumerable<SellerNote> sellernotes { get; set; }
        public IEnumerable<SellerNote> sellernotes2 { get; set; }

        public IEnumerable<NoteCategory> categories { get; set; }
        public int notesold { get; set; }
        public int NoOfDownload { get; set; }
        public int totalEarning { get; set; }
        public int rejected { get; set; }
        public int Request { get; set; }
        //public int pageOfDraft { get; set; }
        //public string SerchText { get; set; }
    }
}