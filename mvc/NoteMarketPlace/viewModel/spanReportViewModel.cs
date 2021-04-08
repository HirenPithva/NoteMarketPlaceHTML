using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.viewModel
{
    public class spanReportViewModel
    {
        public int reportID { get; set; }
        public int noteID { get; set; }
        public string ReportedBy { get; set; }
        public string Title { get; set; }
        public string category { get; set; }
        public DateTime DateAdded { get; set; }
        public string remark { get; set; }
    }
}