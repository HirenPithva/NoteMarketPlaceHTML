using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.viewModel
{
    public class rejectednotesViewModel
    {
        public IEnumerable<SelectListItem> seller { get; set; }
        public IEnumerable<rejectedlist> rejectednotes { get; set; }
    }
    public class rejectedlist
    {
        public int noteid { get; set; }
        public DateTime modifiedDate { get; set; }
        public string remark { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string RejectedBy { get; set; }
        public string seller { get; set; }
        public int sellerID { get; set; }
    }
}